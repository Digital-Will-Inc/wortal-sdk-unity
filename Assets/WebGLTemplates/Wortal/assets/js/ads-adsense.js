var showRewardedAdFn;

function showInterstitialAdAdSense(type, name) {
  window.triggerWortalAd(type, "", name, {
    beforeAd: () => {
      gameInstance.Module.Wortal.TriggerBeforeAd();
    },
    afterAd: () => {
      gameInstance.Module.Wortal.TriggerAfterAd();
    },
    adBreakDone: (placementInfo) => {
      gameInstance.Module.Wortal.TriggerAdBreakDone();
    },
    noShow: () => {
      gameInstance.Module.Wortal.TriggerNoShow();
    },
  });
}

function showRewardedAdAdSense(name) {
  window.triggerWortalAd('reward', "", name, {
    beforeAd: () => {
      gameInstance.Module.Wortal.TriggerBeforeAd();
    },
    afterAd: () => {
      gameInstance.Module.Wortal.TriggerAfterAd();
    },
    adBreakDone: (placementInfo) => {
      gameInstance.Module.Wortal.TriggerAdBreakDone();
    },
    beforeReward: (showAdFn) => {
      showRewardedAdFn = showAdFn;
      gameInstance.Module.Wortal.TriggerBeforeReward();
    },
    adDismissed: () => {
      gameInstance.Module.Wortal.TriggerAdDismissed();
    },
    adViewed: () => {
      gameInstance.Module.Wortal.TriggerAdViewed();
    },
    noShow: () => {
      gameInstance.Module.Wortal.TriggerNoShow();
    },
  });
}

function triggerShowRewardedAdFn() {
  console.log("[Wortal] ShowRewardedAd");
  showRewardedAdFn();
}
