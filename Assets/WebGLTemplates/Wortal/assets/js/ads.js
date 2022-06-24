var showRewardedAdFn;
function showInterstitialAd(type, name) {
  adBreak({
    type: type,
    name: name,
    beforeAd: () => {
      myGameInstance.Module.asmLibraryArg._TriggerBeforeAd();
    },
    afterAd: () => {
      myGameInstance.Module.asmLibraryArg._TriggerAfterAd();
    },
    adBreakDone: (placementInfo) => {
      myGameInstance.Module.asmLibraryArg._TriggerAdBreakDone();
    },
  });
}
function showRewardedAd(name) {
  adBreak({
    type: 'reward',
    name: name,
    beforeAd: () => {
      myGameInstance.Module.asmLibraryArg._TriggerBeforeAd();
    },
    afterAd: () => {
      myGameInstance.Module.asmLibraryArg._TriggerAfterAd();
    },
    adBreakDone: (placementInfo) => {
      myGameInstance.Module.asmLibraryArg._TriggerAdBreakDone();
    },
    beforeReward: (showAdFn) => {
      showRewardedAdFn = showAdFn;
      myGameInstance.Module.asmLibraryArg._TriggerBeforeReward();
    },
    adDismissed: () => {
      myGameInstance.Module.asmLibraryArg._TriggerAdDismissed();
    },
    adViewed: () => {
      myGameInstance.Module.asmLibraryArg._TriggerAdViewed();
    },
  });
}
function triggerShowRewardedAdFn() {
  console.log("***** triggerShowRewardedAdFn *****");
  showRewardedAdFn();
}
