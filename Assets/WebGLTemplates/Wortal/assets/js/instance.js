const container = document.querySelector("#unity-container");
const canvas = document.querySelector("#unity-canvas");
const loadingBar = document.querySelector("#unity-loading-bar");
const progressBarFull = document.querySelector("#unity-progress-bar-full");
const progressValue = document.querySelector("#unity-progress-value");
const progressText = document.querySelector("#unity-progress-text");

// Register serviceWorker for PWA.
window.addEventListener("load", function () {
    if ("serviceWorker" in navigator) {
        navigator.serviceWorker.register("assets/js/service-worker.js");
    }
});

const loaderUrl = "Build/{{{ LOADER_FILENAME }}}";
const config = {
    dataUrl: "Build/{{{ DATA_FILENAME }}}",
    frameworkUrl: "Build/{{{ FRAMEWORK_FILENAME }}}",
#if USE_THREADS
    workerUrl: buildUrl + "/{{{ WORKER_FILENAME }}}",
#endif
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

// Mobile device style - fill the whole browser client area with the game canvas.
if (/iPhone|iPad|iPod|Android/i.test(navigator.userAgent)) {
    const meta = document.createElement("meta");
    meta.name = "viewport";
    meta.content = "width=device-width, height=device-height, initial-scale=1.0, user-scalable=no, shrink-to-fit=yes";
    document.getElementsByTagName("head")[0].appendChild(meta);
};

#if BACKGROUND_FILENAME
canvas.style.background = "url('" + buildUrl + "/{{{ BACKGROUND_FILENAME.replace(/'/g, '%27') }}}') center / cover";
#endif

//////////////////////////////////////////
// Loading bar and progress text
//////////////////////////////////////////

loadingBar.style.display = "block";

let dotCount = 0;
let dots = "";

// Displays dots for Loading game... animation.
const dotInterval = setInterval(() => {
    if (dotCount > 3) {
        dotCount = 0;
    }

    dotCount++;
    dots = new Array(dotCount % 10).join('.');
}, 500);

// If you want to display different texts while loading, add them here and then
// reference them in updateLoadingText().
const progressTexts = {
    default: "Loading game",
};

// You can add different value ranges here to display different text to the player during the loading process.
function updateProgressText(value) {
    if (value < 100) {
        progressText.innerText = progressTexts.default + dots;
    }
}

//////////////////////////////////////////
// Unity loader
//////////////////////////////////////////

// Wortal SDK has an isInitialized property that we would normally check before accessing the SDK, but
// in the jslib we use gameInstance.Module to return data to the WASM module, which may not be available
// yet even if the SDK is initialized. So we use this flag instead of Wortal.isInitialized.
window.isUnitySDKInitialized = false;

let platform = "";
const script = document.createElement("script");
script.src = loaderUrl;
script.onload = () => {
    window.Wortal.initializeAsync().then(() => {
        createUnityInstance(canvas, config, (progress) => {
            window.Wortal.setLoadingProgress(100 * progress);
            const value = Math.round(progress * 100);
            updateProgressText(value);

            progressValue.innerText = value + "%";
            progressBarFull.style.width = value + "%";
        }).then((unityInstance) => {
            loadingBar.style.display = "none";
            clearInterval(dotInterval);

            window.Wortal.setLoadingProgress(100);
            window.Wortal.startGameAsync().catch(error => {
                console.error(error);
            });

            gameInstance = unityInstance;
            window.isUnitySDKInitialized = true;
        }).catch(error => {
            console.error(error);
        });
    }).catch(error => {
        console.error(error);
    });
}

document.body.appendChild(script);
