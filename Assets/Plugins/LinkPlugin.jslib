mergeInto(LibraryManager.library, {

  $beforeAdPointerLink: 0,
  $afterAdPointerLink: 0,
  $adDismissedPointerLink: 0,
  $adViewedPointerLink: 0,
  $noShowPointerLink: 0,

  ShowInterstitialAdLink: function(type, name, beforeAdCallback, afterAdCallback, noShowCallback) {
    beforeAdPointerLink = beforeAdCallback;
    afterAdPointerLink = afterAdCallback;
    noShowPointerLink = noShowCallback;
    showInterstitialAdLink(UTF8ToString(type), UTF8ToString(name));
  },

  ShowRewardedAdLink: function(name, beforeAdCallback, afterAdCallback, adDismissedCallback, adViewedCallback, noShowCallback) {
    beforeAdPointerLink = beforeAdCallback;
    afterAdPointerLink = afterAdCallback;
    adDismissedPointerLink = adDismissedCallback;
    adViewedPointerLink = adViewedCallback;
    noShowPointerLink = noShowCallback;
    showRewardedAdLink('reward', UTF8ToString(name));
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
    dynCall_v(noShowPointerLink);
  },

});