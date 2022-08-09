mergeInto(LibraryManager.library, {

  ShowInterstitialAdAdSense: function (type, name, beforeAdCallback, afterAdCallback, adBreakDoneCallback, noShowCallback) {
    Module.Wortal.beforeAdPointerAdSense = beforeAdCallback;
    Module.Wortal.afterAdPointerAdSense = afterAdCallback;
    Module.Wortal.adBreakDonePointerAdSense = adBreakDoneCallback;
    Module.Wortal.noShowPointerAdSense = noShowCallback;
    showInterstitialAdAdSense(UTF8ToString(type), UTF8ToString(name));
  },

  RequestRewardedAdAdSense: function (name, beforeAdCallback, afterAdCallback, adBreakDoneCallback, beforeRewardCallback,
                                      adDismissedCallback, adViewedCallback, noShowCallback) {
    Module.Wortal.beforeAdPointerAdSense = beforeAdCallback;
    Module.Wortal.afterAdPointerAdSense = afterAdCallback;
    Module.Wortal.adBreakDonePointerAdSense = adBreakDoneCallback;
    Module.Wortal.beforeRewardPointerAdSense = beforeRewardCallback;
    Module.Wortal.adDismissedPointerAdSense = adDismissedCallback;
    Module.Wortal.adViewedPointerAdSense = adViewedCallback;
    Module.Wortal.noShowPointerAdSense = noShowCallback;
    showRewardedAdAdSense('reward', UTF8ToString(name));
  },

  ShowRewardedAdAdSense: function () {
    triggerShowRewardedAdFn();
  },

};
