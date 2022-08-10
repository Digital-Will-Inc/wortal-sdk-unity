const container = document.querySelector('#unity-container');
const canvas = document.querySelector('#unity-canvas');
const loadingCover = document.querySelector('#loading-cover');

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
  config.devicePixelRatio = 1;
};

createUnityInstance(canvas, config, (progress) => {
  if (window.wortalGame) {
    window.wortalGame.setLoadingProgress(100 * progress);
  }
}).then((unityInstance) => {
  window.wortalGame.setLoadingProgress(100);
  gameInstance = unityInstance;
  console.log('[Wortal] Module Args loaded.');
  console.log(gameInstance.Module.Wortal);
});
