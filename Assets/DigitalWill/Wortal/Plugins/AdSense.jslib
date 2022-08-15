mergeInto(LibraryManager.library, {

  ShowInterstitialAdAdSense: function (type, description, beforeAdCallback, afterAdCallback, adBreakDoneCallback, noShowCallback) {
    Module.Wortal.beforeAdPointer = beforeAdCallback;
    Module.Wortal.afterAdPointer = afterAdCallback;
    Module.Wortal.adBreakDonePointer = adBreakDoneCallback;
    Module.Wortal.noShowPointer = noShowCallback;
    showInterstitialAdAdSense(UTF8ToString(type), UTF8ToString(description));
  },

  RequestRewardedAdAdSense: function (description, beforeAdCallback, afterAdCallback, adBreakDoneCallback, beforeRewardCallback,
                                      adDismissedCallback, adViewedCallback, noShowCallback) {
    Module.Wortal.beforeAdPointer = beforeAdCallback;
    Module.Wortal.afterAdPointer = afterAdCallback;
    Module.Wortal.adBreakDonePointer = adBreakDoneCallback;
    Module.Wortal.beforeRewardPointer = beforeRewardCallback;
    Module.Wortal.adDismissedPointer = adDismissedCallback;
    Module.Wortal.adViewedPointer = adViewedCallback;
    Module.Wortal.noShowPointer = noShowCallback;
    showRewardedAdAdSense('reward', UTF8ToString(description));
  },

  ShowRewardedAdAdSense: function () {
    triggerShowRewardedAdFn();
  },

});
