mergeInto(LibraryManager.library, {

  $beforeAdPointer: 0,
  $afterAdPointer: 0,
  $adBreakDonePointer: 0,
  $beforeRewardPointer: 0,
  $adDismissedPointer: 0,
  $adViewedPointer: 0,

  ShowInterstitialAdViaJS: function(type, name, beforeAdCallback, afterAdCallback, adBreakDoneCallback) {
    beforeAdPointer = beforeAdCallback;
    afterAdPointer = afterAdCallback;
    adBreakDonePointer = adBreakDoneCallback;
    showInterstitialAd(UTF8ToString(type), UTF8ToString(name));
  },

  RequestRewardedAdViaJS: function(name, beforeAdCallback, afterAdCallback, adBreakDoneCallback, beforeRewardCallback, adDismissedCallback, adViewedCallback) {
    beforeAdPointer = beforeAdCallback;
    afterAdPointer = afterAdCallback;
    adBreakDonePointer = adBreakDoneCallback;
    beforeRewardPointer = beforeRewardCallback;
    adDismissedPointer = adDismissedCallback;
    adViewedPointer = adViewedCallback;
    showRewardedAd('reward', UTF8ToString(name));
  },

  ShowRewardedAdViaJS: function() {
    triggerShowRewardedAdFn();
  },

  TriggerBeforeAd: function() {
    dynCall_v(beforeAdPointer);
  },

  TriggerAfterAd: function() {
    dynCall_v(afterAdPointer);
  },

  TriggerAdBreakDone: function() {
    dynCall_v(adBreakDonePointer);
  },

  TriggerBeforeReward: function() {
    dynCall_v(beforeRewardPointer);
  },

  TriggerAdDismissed: function() {
    dynCall_v(adDismissedPointer);
  },

  TriggerAdViewed: function() {
    dynCall_v(adViewedPointer);
  },
  
});
