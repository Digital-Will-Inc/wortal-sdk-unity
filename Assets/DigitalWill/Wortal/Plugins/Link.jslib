mergeInto(LibraryManager.library, {

  GetLinkInterstitialId: function () {
    window.wortalGame.getAdUnitsAsync().then((adUnits) => {
      console.log(adUnits);
      for (let i = 0; i < adUnits.Length; i++) {
        if (adUnits[i].type == 'INTERSTITIAL') {
          let idStr = adUnits[i].id;
          let bufferSize = lengthBytesUTF8(idStr) + 1;
          let buffer = _malloc(bufferSize);
          stringToUTF8(idStr, buffer, bufferSize);
          return buffer;
        }
      }
    });
  },

  GetLinkRewardedId: function () {
    window.wortalGame.getAdUnitsAsync().then((adUnits) => {
      console.log(adUnits);
      for (let i = 0; i < adUnits.Length; i++) {
        if (adUnits[i].type == 'REWARDED_VIDEO') {
          let idStr = adUnits[i].id;
          let bufferSize = lengthBytesUTF8(idStr) + 1;
          let buffer = _malloc(bufferSize);
          stringToUTF8(idStr, buffer, bufferSize);
          return buffer;
        }
      }
    });
  },

  ShowInterstitialAdLink: function(type, adUnitId, description, beforeAdCallback, afterAdCallback, noBreakCallback) {
    Module.Wortal.beforeAdPointerLink = beforeAdCallback;
    Module.Wortal.afterAdPointerLink = afterAdCallback;
    Module.Wortal.noBreakPointerLink = noBreakCallback;
    showInterstitialAdLink(UTF8ToString(type), UTF8ToString(placementId));
  },

  ShowRewardedAdLink: function(adUnitId, description, beforeAdCallback, afterAdCallback, adDismissedCallback, adViewedCallback, noBreakCallback) {
    Module.Wortal.beforeAdPointerLink = beforeAdCallback;
    Module.Wortal.afterAdPointerLink = afterAdCallback;
    Module.Wortal.adDismissedPointerLink = adDismissedCallback;
    Module.Wortal.adViewedPointerLink = adViewedCallback;
    Module.Wortal.noBreakPointerLink = noShowCallback;
    showRewardedAdLink('reward', UTF8ToString(placementId));
  },

});
