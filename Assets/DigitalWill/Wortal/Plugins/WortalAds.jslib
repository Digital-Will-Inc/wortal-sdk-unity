mergeInto(LibraryManager.library, {

    ShowInterstitialJS: function (placement, description, beforeAdCallback, afterAdCallback) {
        window.Wortal.ads.showInterstitial(
            UTF8ToString(placement),
            UTF8ToString(description),
            () => Module.dynCall_v(beforeAdCallback),
            () => Module.dynCall_v(afterAdCallback),
        );
    },

    ShowRewardedJS: function (description, beforeAdCallback, afterAdCallback, adDismissedCallback, adViewedCallback) {
        window.Wortal.ads.showRewarded(
            UTF8ToString(description),
            () => Module.dynCall_v(beforeAdCallback),
            () => Module.dynCall_v(afterAdCallback),
            () => Module.dynCall_v(adDismissedCallback),
            () => Module.dynCall_v(adViewedCallback),
        );
    }

});
