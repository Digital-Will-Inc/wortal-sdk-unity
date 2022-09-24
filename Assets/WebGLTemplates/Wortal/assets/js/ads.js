var showRewardedAdFn;

function showInterstitialAd(type, adUnitId, description) {
    window.triggerWortalAd(type, adUnitId, description, {
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
        noBreak: () => {
            gameInstance.Module.Wortal.TriggerNoShow();
        },
    });
}

function showRewardedAd(adUnitId, description) {
    window.triggerWortalAd('reward', adUnitId, description, {
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
        noBreak: () => {
            gameInstance.Module.Wortal.TriggerNoShow();
        },
    });
}

function triggerShowRewardedAdFn() {
    console.log("[Wortal] ShowRewardedAd");
    showRewardedAdFn();
}
