mergeInto(LibraryManager.library, {

  ShowInterstitialAdLink: function(type, placementId, beforeAdCallback, afterAdCallback, noBreakCallback) {
    Module.Wortal.beforeAdPointerLink = beforeAdCallback;
    Module.Wortal.afterAdPointerLink = afterAdCallback;
    Module.Wortal.noBreakPointerLink = noBreakCallback;
    showInterstitialAdLink(UTF8ToString(type), UTF8ToString(placementId));
  },

  ShowRewardedAdLink: function(placementId, beforeAdCallback, afterAdCallback, adDismissedCallback, adViewedCallback, noBreakCallback) {
    Module.Wortal.beforeAdPointerLink = beforeAdCallback;
    Module.Wortal.afterAdPointerLink = afterAdCallback;
    Module.Wortal.adDismissedPointerLink = adDismissedCallback;
    Module.Wortal.adViewedPointerLink = adViewedCallback;
    Module.Wortal.noBreakPointerLink = noShowCallback;
    showRewardedAdLink('reward', UTF8ToString(placementId));
  },

};
