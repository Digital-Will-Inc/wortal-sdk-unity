mergeInto(LibraryManager.library, {

  GetPlatform: function () {
    var platformStr = window.getWortalPlatform();
    var bufferSize = lengthBytesUTF8(platformStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(platformStr, buffer, bufferSize);
    return buffer;
  },

  GetLinkAdUnitIds: function(callback) {
    window.wortalGame.getAdUnitsAsync().then((adUnits) => {
      let iStr = adUnits[0].id;
      let iLen = lengthBytesUTF8(iStr) + 1;
      let iPtr = _malloc(iLen);
      stringToUTF8(iStr, iPtr, iLen);

      let rStr = adUnits[1].id;
      let rLen = lengthBytesUTF8(rStr) + 1;
      let rPtr = _malloc(rLen);
      stringToUTF8(rStr, rPtr, rLen);

      Module.dynCall_vii(callback, iPtr, rPtr);
    });
  },

  ShowInterstitialAd: function (type, adUnitId, description, beforeAdCallback, afterAdCallback, noShowCallback,
                                adBreakDoneCallback) {
    Module.Wortal.beforeAdPointer = beforeAdCallback;
    Module.Wortal.afterAdPointer = afterAdCallback;
    Module.Wortal.noShowPointer = noShowCallback;
    Module.Wortal.adBreakDonePointer = adBreakDoneCallback;
    showInterstitialAd(UTF8ToString(type), UTF8ToString(adUnitId), UTF8ToString(description));
  },

    RequestRewardedAd: function (adUnitId, description, beforeAdCallback, afterAdCallback, adDismissedCallback, adViewedCallback, noShowCallback,
                                 beforeRewardCallback, adBreakDoneCallback) {
        Module.Wortal.beforeAdPointer = beforeAdCallback;
        Module.Wortal.afterAdPointer = afterAdCallback;
        Module.Wortal.adDismissedPointer = adDismissedCallback;
        Module.Wortal.adViewedPointer = adViewedCallback;
        Module.Wortal.noShowPointer = noShowCallback;
        Module.Wortal.beforeRewardPointer = beforeRewardCallback;
        Module.Wortal.adBreakDonePointer = adBreakDoneCallback;
        showRewardedAd(UTF8ToString(adUnitId), UTF8ToString(description));
    },

  ShowRewardedAd: function () {
    triggerShowRewardedAdFn();
  },

});
