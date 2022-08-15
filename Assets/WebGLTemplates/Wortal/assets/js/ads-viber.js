function showInterstitialAdViber(type, adUnitId, description) {
  window.triggerWortalAd(type, adUnitId, description, {
    beforeAd: () => {
      gameInstance.Module.Wortal.TriggerBeforeAdViber();
    },
    afterAd: () => {
      gameInstance.Module.Wortal.TriggerAfterAdViber();
    },
    noBreak: () => {
      gameInstance.Module.Wortal.TriggerNoShowViber();
    },
  });
}

function showRewardedAdViber(adUnitId, description) {
  window.triggerWortalAd('reward', adUnitId, description, {
    beforeAd: () => {
      gameInstance.Module.Wortal.TriggerBeforeAdViber();
    },
    afterAd: () => {
      gameInstance.Module.Wortal.TriggerAfterAdViber();
    },
    adDismissed: () => {
      gameInstance.Module.Wortal.TriggerAdDismissedViber();
    },
    adViewed: () => {
      gameInstance.Module.Wortal.TriggerAdViewedViber();
    },
    noBreak: () => {
      gameInstance.Module.Wortal.TriggerNoShowViber();
    },
  });
}
