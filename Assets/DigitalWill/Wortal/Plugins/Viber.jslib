mergeInto(LibraryManager.library, {

  ShowInterstitialAdViber: function(type, adUnitId, description, beforeAdCallback, afterAdCallback, noShowCallback) {
    Module.Wortal.beforeAdPointer = beforeAdCallback;
    Module.Wortal.afterAdPointer = afterAdCallback;
    Module.Wortal.noShowPointer = noShowCallback;
    showInterstitialAdViber(UTF8ToString(type), UTF8ToString(adUnitId), UTF8ToString(description));
  },

  ShowRewardedAdViber: function(adUnitId, description, beforeAdCallback, afterAdCallback, adDismissedCallback, adViewedCallback, noShowCallback) {
    Module.Wortal.beforeAdPointer = beforeAdCallback;
    Module.Wortal.afterAdPointer = afterAdCallback;
    Module.Wortal.adDismissedPointer = adDismissedCallback;
    Module.Wortal.adViewedPointer = adViewedCallback;
    Module.Wortal.noShowPointer = noShowCallback;
    showRewardedAdViber('reward', UTF8ToString(adUnitId), UTF8ToString(description));
  },

});
