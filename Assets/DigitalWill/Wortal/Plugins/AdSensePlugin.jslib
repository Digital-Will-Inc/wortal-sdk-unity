mergeInto(LibraryManager.library, {

  $beforeAdPointerAdSense: 0,
  $afterAdPointerAdSense: 0,
  $adBreakDonePointerAdSense: 0,
  $beforeRewardPointerAdSense: 0,
  $adDismissedPointerAdSense: 0,
  $adViewedPointerAdSense: 0,
  $noShowPointerAdSense: 0,

  ShowInterstitialAdAdSense: function(type, name, beforeAdCallback, afterAdCallback, adBreakDoneCallback, noShowCallback) {
    beforeAdPointerAdSense = beforeAdCallback;
    afterAdPointerAdSense = afterAdCallback;
    adBreakDonePointerAdSense = adBreakDoneCallback;
    noShowPointerAdSense = noShowCallback;
    showInterstitialAdAdSense(UTF8ToString(type), UTF8ToString(name));
  },

  RequestRewardedAdAdSense: function(name, beforeAdCallback, afterAdCallback, adBreakDoneCallback, beforeRewardCallback,
                                   adDismissedCallback, adViewedCallback, noShowCallback) {
    beforeAdPointerAdSense = beforeAdCallback;
    afterAdPointerAdSense = afterAdCallback;
    adBreakDonePointerAdSense = adBreakDoneCallback;
    beforeRewardPointerAdSense = beforeRewardCallback;
    adDismissedPointerAdSense = adDismissedCallback;
    adViewedPointerAdSense = adViewedCallback;
    noShowPointerAdSense = noShowCallback;
    showRewardedAdAdSense('reward', UTF8ToString(name));
  },

  ShowRewardedAdAdSense: function() {
    triggerShowRewardedAdFn();
  },

  TriggerBeforeAdAdSense: function() {
    dynCall_v(beforeAdPointerAdSense);
  },

  TriggerAfterAdAdSense: function() {
    dynCall_v(afterAdPointerAdSense);
  },

  TriggerAdBreakDoneAdSense: function() {
    dynCall_v(adBreakDonePointerAdSense);
  },

  TriggerBeforeRewardAdSense: function() {
    dynCall_v(beforeRewardPointerAdSense);
  },

  TriggerAdDismissedAdSense: function() {
    dynCall_v(adDismissedPointerAdSense);
  },

  TriggerAdViewedAdSense: function() {
    dynCall_v(adViewedPointerAdSense);
  },

  TriggerNoShowAdSense: function() {
    dynCall_v(noShowPointerAdSense);
  },

});
