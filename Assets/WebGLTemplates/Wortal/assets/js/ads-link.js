function showInterstitialAdLink(type, adUnitId, description) {
  window.triggerWortalAd(type, adUnitId, description, {
    beforeAd: () => {
      gameInstance.Module.Wortal.TriggerBeforeAdLink();
    },
    afterAd: () => {
      gameInstance.Module.Wortal.TriggerAfterAdLink();
    },
    noBreak: () => {
      gameInstance.Module.Wortal.TriggerNoShowLink();
    },
  });
}

function showRewardedAdLink(adUnitId, description) {
  window.triggerWortalAd('reward', adUnitId, description, {
    beforeAd: () => {
      gameInstance.Module.Wortal.TriggerBeforeAdLink();
    },
    afterAd: () => {
      gameInstance.Module.Wortal.TriggerAfterAdLink();
    },
    adDismissed: () => {
      gameInstance.Module.Wortal.TriggerAdDismissedLink();
    },
    adViewed: () => {
      gameInstance.Module.Wortal.TriggerAdViewedLink();
    },
    noBreak: () => {
      gameInstance.Module.Wortal.TriggerNoShowLink();
    },
  });
}
