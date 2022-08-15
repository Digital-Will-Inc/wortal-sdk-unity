mergeInto(LibraryManager.library, {

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

  ShowInterstitialAdLink: function(type, adUnitId, description, beforeAdCallback, afterAdCallback, noShowCallback) {
    Module.Wortal.beforeAdPointer = beforeAdCallback;
    Module.Wortal.afterAdPointer = afterAdCallback;
    Module.Wortal.noShowPointer = noShowCallback;
    showInterstitialAdLink(UTF8ToString(type), UTF8ToString(adUnitId), UTF8ToString(description));
  },

  ShowRewardedAdLink: function(adUnitId, description, beforeAdCallback, afterAdCallback, adDismissedCallback, adViewedCallback, noShowCallback) {
    Module.Wortal.beforeAdPointer = beforeAdCallback;
    Module.Wortal.afterAdPointer = afterAdCallback;
    Module.Wortal.adDismissedPointer = adDismissedCallback;
    Module.Wortal.adViewedPointer = adViewedCallback;
    Module.Wortal.noShowPointer = noShowCallback;
    showRewardedAdLink('reward', UTF8ToString(adUnitId), UTF8ToString(description));
  },

});
