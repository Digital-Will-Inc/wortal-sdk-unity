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
  myGameInstance = unityInstance;
});

function unityProgress(progress) {
  if (progress === 1) {
    document.querySelector('#progress').style.display = 'none';
    return;
  }
  if (progress > 0) {
    document.querySelector('#progressBar').style.width = 100 * progress + '%';
    document.querySelector('#progressBar').style.display = 'inherit';
  }
}
