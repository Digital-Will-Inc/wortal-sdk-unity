function showInterstitialAdLink(type, placementId) {
  window.triggerWortalLinkAd(type, placementId, {
    beforeAd: () => {
      gameInstance.Module.asmLibraryArg._TriggerBeforeAdLink();
    },
    afterAd: () => {
      gameInstance.Module.asmLibraryArg._TriggerAfterAdLink();
    },
    noBreak: () => {
      gameInstance.Module.asmLibraryArg._TriggerNoShowLink();
    },
  });
}

function showRewardedAdLink(placementId) {
  window.triggerWortalLinkAd('reward', placementId, {
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
    noBreak: () => {
      gameInstance.Module.asmLibraryArg._TriggerNoShowLink();
    },
  });
}
