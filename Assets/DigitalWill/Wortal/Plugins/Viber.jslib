mergeInto(LibraryManager.library, {

  ShowInterstitialAdViber: function(type, adUnitId, description, beforeAdCallback, afterAdCallback, noBreakCallback) {
    Module.Wortal.beforeAdPointerViber = beforeAdCallback;
    Module.Wortal.afterAdPointerViber = afterAdCallback;
    Module.Wortal.noBreakPointerViber = noBreakCallback;
    showInterstitialAdViber(UTF8ToString(type), UTF8ToString(adUnitId), UTF8ToString(description));
  },

  ShowRewardedAdViber: function(adUnitId, description, beforeAdCallback, afterAdCallback, adDismissedCallback, adViewedCallback, noBreakCallback) {
    Module.Wortal.beforeAdPointerViber = beforeAdCallback;
    Module.Wortal.afterAdPointerViber = afterAdCallback;
    Module.Wortal.adDismissedPointerViber = adDismissedCallback;
    Module.Wortal.adViewedPointerViber = adViewedCallback;
    Module.Wortal.noBreakPointerViber = noShowCallback;
    showRewardedAdViber('reward', UTF8ToString(adUnitId), UTF8ToString(description));
  },

});
