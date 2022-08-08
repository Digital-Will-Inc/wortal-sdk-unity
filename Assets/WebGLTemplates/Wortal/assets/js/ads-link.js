function showInterstitialAdLink(type, placementId) {
  window.triggerWortalLinkAd(type, placementId, {
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

function showRewardedAdLink(placementId) {
  window.triggerWortalLinkAd('reward', placementId, {
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
