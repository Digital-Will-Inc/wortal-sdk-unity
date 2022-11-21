mergeInto(LibraryManager.library, {

    ShowInterstitialJS: function (placement, description, beforeAdCallback, afterAdCallback) {
        Module.Wortal.beforeAdPointer = beforeAdCallback;
        Module.Wortal.afterAdPointer = afterAdCallback;

        window.Wortal.ads.showInterstitial(
            UTF8ToString(placement),
            UTF8ToString(description),
            gameInstance.Module.Wortal.TriggerBeforeAd,
            gameInstance.Module.Wortal.TriggerAfterAd
        );
    },

    ShowRewardedJS: function (description, beforeAdCallback, afterAdCallback, adDismissedCallback, adViewedCallback) {
        Module.Wortal.beforeAdPointer = beforeAdCallback;
        Module.Wortal.afterAdPointer = afterAdCallback;
        Module.Wortal.adDismissedPointer = adDismissedCallback;
        Module.Wortal.adViewedPointer = adViewedCallback;

        window.Wortal.ads.showRewarded(
            UTF8ToString(description),
            gameInstance.Module.Wortal.TriggerBeforeAd,
            gameInstance.Module.Wortal.TriggerAfterAd,
            gameInstance.Module.Wortal.TriggerAdDismissed,
            gameInstance.Module.Wortal.TriggerAdViewed,
        );
    }

});
