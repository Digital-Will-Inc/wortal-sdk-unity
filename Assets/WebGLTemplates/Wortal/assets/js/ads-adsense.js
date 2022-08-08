var showRewardedAdFn;

function showInterstitialAdAdSense(type, name) {
  window.triggerWortalAd(type, "", name, {
    beforeAd: () => {
      gameInstance.Module.Wortal.TriggerBeforeAdAdSense();
    },
    afterAd: () => {
      gameInstance.Module.Wortal.TriggerAfterAdAdSense();
    },
    adBreakDone: (placementInfo) => {
      gameInstance.Module.Wortal.TriggerAdBreakDoneAdSense();
    },
    noShow: () => {
      gameInstance.Module.Wortal.TriggerNoShowAdSense();
    },
  });
}

function showRewardedAdAdSense(name) {
  window.triggerWortalAd('reward', "", name, {
    beforeAd: () => {
      gameInstance.Module.Wortal.TriggerBeforeAdAdSense();
    },
    afterAd: () => {
      gameInstance.Module.Wortal.TriggerAfterAdAdSense();
    },
    adBreakDone: (placementInfo) => {
      gameInstance.Module.Wortal.TriggerAdBreakDoneAdSense();
    },
    beforeReward: (showAdFn) => {
      showRewardedAdFn = showAdFn;
      gameInstance.Module.Wortal.TriggerBeforeRewardAdSense();
    },
    adDismissed: () => {
      gameInstance.Module.Wortal.TriggerAdDismissedAdSense();
    },
    adViewed: () => {
      gameInstance.Module.Wortal.TriggerAdViewedAdSense();
    },
    noShow: () => {
      gameInstance.Module.Wortal.TriggerNoShowAdSense();
    },
  });
}

function triggerShowRewardedAdFn() {
  console.log("[Wortal] ShowRewardedAd");
  showRewardedAdFn();
}
