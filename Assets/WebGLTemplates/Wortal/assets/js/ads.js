
function showInterstitialAd(placement, description) {
    window.triggerWortalAd(placement, gameData.linkInterstitialId, description, {
        beforeAd: () => {
            console.log("[Wortal] BeforeAd");
            gameInstance.Module.Wortal.TriggerBeforeAd();
        },
        afterAd: () => {
            console.log("[Wortal] AfterAd");
            gameInstance.Module.Wortal.TriggerAfterAd();
        },
        noShow: () => {
            console.log("[Wortal] NoShow");
            gameInstance.Module.Wortal.TriggerAfterAd();
        },
        noBreak: () => {
            console.log("[Wortal] NoBreak");
            gameInstance.Module.Wortal.TriggerAfterAd();
        },
        adBreakDone: (placementInfo) => {
            console.log("[Wortal] AdBreakDone");
        },
    });
}

function showRewardedAd(description) {
    window.triggerWortalAd('reward', gameData.linkRewardedId, description, {
        beforeAd: () => {
            console.log("[Wortal] BeforeAd");
            gameInstance.Module.Wortal.TriggerBeforeAd();
        },
        afterAd: () => {
            console.log("[Wortal] AfterAd");
            gameInstance.Module.Wortal.TriggerAfterAd();
        },
        adDismissed: () => {
            console.log("[Wortal] AdDismissed");
            gameInstance.Module.Wortal.TriggerAdDismissed();
        },
        adViewed: () => {
            console.log("[Wortal] AdViewed");
            gameInstance.Module.Wortal.TriggerAdViewed();
        },
        noShow: () => {
            console.log("[Wortal] NoShow");
            gameInstance.Module.Wortal.TriggerAfterAd();
        },
        noBreak: () => {
            console.log("[Wortal] NoBreak");
            gameInstance.Module.Wortal.TriggerAfterAd();
        },
        beforeReward: (showAdFn) => {
            showAdFn(); // This is necessary to trigger the rewarded ad to play on AdSense after its returned.
            console.log("[Wortal] BeforeReward");
        },
        adBreakDone: (placementInfo) => {
            console.log("[Wortal] AdBreakDone");
        },
    });
}
