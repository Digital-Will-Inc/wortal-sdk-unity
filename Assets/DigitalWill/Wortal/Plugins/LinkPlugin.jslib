mergeInto(LibraryManager.library, {

  $beforeAdPointerLink: 0,
  $afterAdPointerLink: 0,
  $adDismissedPointerLink: 0,
  $adViewedPointerLink: 0,
  $noBreakPointerLink: 0,

  ShowInterstitialAdLink: function(type, placementId, beforeAdCallback, afterAdCallback, noBreakCallback) {
    beforeAdPointerLink = beforeAdCallback;
    afterAdPointerLink = afterAdCallback;
    noBreakPointerLink = noBreakCallback;
    showInterstitialAdLink(UTF8ToString(type), UTF8ToString(placementId));
  },

  ShowRewardedAdLink: function(placementId, beforeAdCallback, afterAdCallback, adDismissedCallback, adViewedCallback, noBreakCallback) {
    beforeAdPointerLink = beforeAdCallback;
    afterAdPointerLink = afterAdCallback;
    adDismissedPointerLink = adDismissedCallback;
    adViewedPointerLink = adViewedCallback;
    noBreakPointerLink = noShowCallback;
    showRewardedAdLink('reward', UTF8ToString(placementId));
  },

  TriggerBeforeAdLink: function() {
    dynCall_v(beforeAdPointerLink);
  },

  TriggerAfterAdLink: function() {
    dynCall_v(afterAdPointerLink);
  },

  TriggerAdDismissedLink: function() {
    dynCall_v(adDismissedPointerLink);
  },

  TriggerAdViewedLink: function() {
    dynCall_v(adViewedPointerLink);
  },

  TriggerNoShowLink: function() {
    dynCall_v(noBreakPointerLink);
  },

});
