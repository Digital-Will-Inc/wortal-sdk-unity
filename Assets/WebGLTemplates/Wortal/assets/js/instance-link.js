createUnityInstance(
  document.querySelector("#unity-canvas"),
  {
    dataUrl: "Build/{{{ DATA_FILENAME }}}",
    frameworkUrl: "Build/{{{ FRAMEWORK_FILENAME }}}",
    codeUrl: "Build/{{{ CODE_FILENAME }}}",
    #if MEMORY_FILENAME
    memoryUrl: "Build/{{{ MEMORY_FILENAME }}}",
    #endif
    #if SYMBOLS_FILENAME
    symbolsUrl: "Build/{{{ SYMBOLS_FILENAME }}}",
    #endif
    streamingAssetsUrl: "StreamingAssets",
    companyName: "{{{ COMPANY_NAME }}}",
    productName: "{{{ PRODUCT_NAME }}}",
    productVersion: "{{{ PRODUCT_VERSION }}}",
  },
  unityProgress).then((unityInstance) => {
  gameInstance = unityInstance;
});

function unityProgress(progress) {
  if (progress === 1) {
    window.wortalLink.setLoadingProgress(100);
    return;
  }
  if (progress > 0) {
    if (window.wortalLink) {
      window.wortalLink.setLoadingProgress(100 * progress);
    }
  }
}
