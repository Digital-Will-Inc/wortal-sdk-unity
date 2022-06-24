var showRewardedAdFn;

function showInterstitialAdAdSense(type, name) {
  window.triggerWortalAd(type, name, {
    beforeAd: () => {
      gameInstance.Module.asmLibraryArg._TriggerBeforeAdAdSense();
    },
    afterAd: () => {
      gameInstance.Module.asmLibraryArg._TriggerAfterAdAdSense();
    },
    adBreakDone: (placementInfo) => {
      gameInstance.Module.asmLibraryArg._TriggerAdBreakDoneAdSense();
    },
    noShow: () => {
      gameInstance.Module.asmLibraryArg._TriggerNoShowAdSense();
    },
  });
}

function showRewardedAdAdSense(name) {
  window.triggerWortalAd('reward', name, {
    beforeAd: () => {
      gameInstance.Module.asmLibraryArg._TriggerBeforeAdAdSense();
    },
    afterAd: () => {
      gameInstance.Module.asmLibraryArg._TriggerAfterAdAdSense();
    },
    adBreakDone: (placementInfo) => {
      gameInstance.Module.asmLibraryArg._TriggerAdBreakDoneAdSense();
    },
    beforeReward: (showAdFn) => {
      showRewardedAdFn = showAdFn;
      gameInstance.Module.asmLibraryArg._TriggerBeforeRewardAdSense();
    },
    adDismissed: () => {
      gameInstance.Module.asmLibraryArg._TriggerAdDismissedAdSense();
    },
    adViewed: () => {
      gameInstance.Module.asmLibraryArg._TriggerAdViewedAdSense();
    },
    noShow: () => {
      gameInstance.Module.asmLibraryArg._TriggerNoShowAdSense();
    },
  });
}

function showInterstitialAdLink(type, name) {
  window.triggerWortalLinkAd(type, name, {
    beforeAd: () => {
      gameInstance.Module.asmLibraryArg._TriggerBeforeAdLink();
    },
    afterAd: () => {
      gameInstance.Module.asmLibraryArg._TriggerAfterAdLink();
    },
    noShow: () => {
      gameInstance.Module.asmLibraryArg._TriggerNoShowLink();
    },
  });
}

function showRewardedAdLink(name) {
  window.triggerWortalLinkAd('reward', name, {
    beforeAd: () => {
      gameInstance.Module.asmLibraryArg._TriggerBeforeAdLink();
    },
    afterAd: () => {
      gameInstance.Module.asmLibraryArg._TriggerAfterAdLink();
    },
    adDismissed: () => {
      gameInstance.Module.asmLibraryArg._TriggerAdDismissedLink();
    },
    adViewed: () => {
      gameInstance.Module.asmLibraryArg._TriggerAdViewedLink();
    },
    noShow: () => {
      gameInstance.Module.asmLibraryArg._TriggerNoShowLink();
    },
  });
}

function triggerShowRewardedAdFn() {
  console.log("***** triggerShowRewardedAdFn *****");
  showRewardedAdFn();
}
