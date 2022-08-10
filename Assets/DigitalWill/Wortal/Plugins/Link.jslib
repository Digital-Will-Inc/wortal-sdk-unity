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

  ShowInterstitialAdLink: function(type, adUnitId, description, beforeAdCallback, afterAdCallback, noBreakCallback) {
    Module.Wortal.beforeAdPointerLink = beforeAdCallback;
    Module.Wortal.afterAdPointerLink = afterAdCallback;
    Module.Wortal.noBreakPointerLink = noBreakCallback;
    showInterstitialAdLink(UTF8ToString(type), UTF8ToString(adUnitId), UTF8ToString(description));
  },

  ShowRewardedAdLink: function(adUnitId, description, beforeAdCallback, afterAdCallback, adDismissedCallback, adViewedCallback, noBreakCallback) {
    Module.Wortal.beforeAdPointerLink = beforeAdCallback;
    Module.Wortal.afterAdPointerLink = afterAdCallback;
    Module.Wortal.adDismissedPointerLink = adDismissedCallback;
    Module.Wortal.adViewedPointerLink = adViewedCallback;
    Module.Wortal.noBreakPointerLink = noShowCallback;
    showRewardedAdLink('reward', UTF8ToString(adUnitId), UTF8ToString(description));
  },

});
