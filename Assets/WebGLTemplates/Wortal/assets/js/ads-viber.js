function showInterstitialAdViber(type, adUnitId, description) {
  window.triggerWortalAd(type, adUnitId, description, {
    beforeAd: () => {
      gameInstance.Module.Wortal.TriggerBeforeAd();
    },
    afterAd: () => {
      gameInstance.Module.Wortal.TriggerAfterAd();
    },
    noBreak: () => {
      gameInstance.Module.Wortal.TriggerNoShow();
    },
  });
}

function showRewardedAdViber(adUnitId, description) {
  window.triggerWortalAd('reward', adUnitId, description, {
    beforeAd: () => {
      gameInstance.Module.Wortal.TriggerBeforeAd();
    },
    afterAd: () => {
      gameInstance.Module.Wortal.TriggerAfterAd();
    },
    adDismissed: () => {
      gameInstance.Module.Wortal.TriggerAdDismissed();
    },
    adViewed: () => {
      gameInstance.Module.Wortal.TriggerAdViewed();
    },
    noBreak: () => {
      gameInstance.Module.Wortal.TriggerNoShow();
    },
  });
}
