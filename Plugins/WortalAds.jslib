mergeInto(LibraryManager.library, {

    IsAdBlockedJS: function () {
        return window.Wortal.ads.isAdBlocked();
    },

    ShowInterstitialJS: function (placement, description, beforeAdCallback, afterAdCallback, noFillCallback) {
        window.Wortal.ads.showInterstitial(
            UTF8ToString(placement),
            UTF8ToString(description),
            () => Module.dynCall_v(beforeAdCallback),
            () => Module.dynCall_v(afterAdCallback),
            () => Module.dynCall_v(noFillCallback),
        );
    },

    ShowRewardedJS: function (description, beforeAdCallback, afterAdCallback, adDismissedCallback, adViewedCallback, noFillCallback) {
        window.Wortal.ads.showRewarded(
            UTF8ToString(description),
            () => Module.dynCall_v(beforeAdCallback),
            () => Module.dynCall_v(afterAdCallback),
            () => Module.dynCall_v(adDismissedCallback),
            () => Module.dynCall_v(adViewedCallback),
            () => Module.dynCall_v(noFillCallback),
        );
    },

    ShowBannerJS: function (shouldShow, position) {
        window.Wortal.ads.showBanner(
            shouldShow,
            UTF8ToString(position),
        );
    }

});
