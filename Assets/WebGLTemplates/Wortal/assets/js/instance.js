const container = document.querySelector('#unity-container');
const canvas = document.querySelector('#unity-canvas');
const loadingBar = document.querySelector("#unity-loading-bar");
const progressBarFull = document.querySelector("#unity-progress-bar-full");
const progressValue = document.querySelector('#unity-progress-value');
const progressText = document.querySelector('#unity-progress-text');
const loaderUrl = "Build/{{{ LOADER_FILENAME }}}";

const config = {
    dataUrl: "Build/{{{ DATA_FILENAME }}}",
    frameworkUrl: "Build/{{{ FRAMEWORK_FILENAME }}}",
#if USE_WASM
    codeUrl: "Build/{{{ CODE_FILENAME }}}",
#endif
#if MEMORY_FILENAME
    memoryUrl: "Build/{{{ MEMORY_FILENAME }}}",
#endif
#if SYMBOLS_FILENAME
    symbolsUrl: "Build/{{{ SYMBOLS_FILENAME }}}",
#endif
    streamingAssetsUrl: "StreamingAssets",
    companyName: {{{ JSON.stringify(COMPANY_NAME) }}},
    productName: {{{ JSON.stringify(PRODUCT_NAME) }}},
    productVersion: {{{ JSON.stringify(PRODUCT_VERSION) }}},
    // matchWebGLToCanvasSize: false, // Uncomment this to separately control WebGL canvas render size and DOM element size.
    // devicePixelRatio: 1, // Uncomment this to override low DPI rendering on high DPI displays.
};

if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
    container.className = "unity-mobile";
    canvas.className = "unity-mobile";
    // To lower canvas resolution on mobile devices to gain some
    // performance, uncomment the following line:
    config.devicePixelRatio = 1;
};

#if BACKGROUND_FILENAME
canvas.style.background = "url('" + buildUrl + "/{{{ BACKGROUND_FILENAME.replace(/'/g, '%27') }}}') center / cover";
#endif

loadingBar.style.display = "block";

let dotCount = 0;
var dots = "";

const progressTexts = {
    // If you want to display different texts while loading, add them here and then
    // reference them in updateLoadingText().
    default: "Loading game",
};

let platform = "";
var script = document.createElement("script");
script.src = loaderUrl;
script.onload = () => {
    if (window.Window) {
        platform = window.Wortal.session.getPlatform();
        if (platform === 'link' || platform === 'viber' || platform === 'facebook') {
            // Comment this out if you want to manually initialize Wortal.
            window.Wortal.initializeAsync().catch(error => { console.error(error); });
        }
    }
    createUnityInstance(canvas, config, (progress) => {
        if (platform === 'link' || platform === 'viber' || platform === 'facebook') {
            // Comment this out if you want to manually initialize Wortal.
            window.Wortal.setLoadingProgress(100 * progress);
        } else {
            let value = Math.round(progress * 100);
            updateProgressText(value);
            progressValue.innerText = value + '%';
            progressBarFull.style.width = value + '%';
        }
    }).then((unityInstance) => {
        loadingBar.style.display = 'none';
        clearInterval(dotInterval);
        if (platform === 'link' || platform === 'viber' || platform === 'facebook') {
            // Comment this out if you want to manually initialize Wortal.
            window.Wortal.setLoadingProgress(100);
            window.Wortal.startGameAsync().catch(error => { console.error(error); });
        }
        gameInstance = unityInstance;
    }).catch(error => {
        console.error(error);
    });
}
document.body.appendChild(script);

var dotInterval = setInterval(() => {
    if (dotCount > 3) {
        dotCount = 0;
    }
    dotCount++;
    dots = new Array(dotCount % 10).join('.');
}, 500);

function updateProgressText(value) {
    // You can add different value ranges here to display different text to the player during the loading process.
    if (value < 100) {
        progressText.innerText = progressTexts.default + dots;
    }
}
