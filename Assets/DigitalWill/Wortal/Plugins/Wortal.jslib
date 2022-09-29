mergeInto(LibraryManager.library, {

    ShowInterstitialAd: function (placement, description, beforeAdCallback, afterAdCallback) {
        Module.Wortal.beforeAdPointer = beforeAdCallback;
        Module.Wortal.afterAdPointer = afterAdCallback;
        showInterstitialAd(UTF8ToString(placement), UTF8ToString(description));
    },

    ShowRewardedAd: function (description, beforeAdCallback, afterAdCallback, adDismissedCallback, adViewedCallback) {
        Module.Wortal.beforeAdPointer = beforeAdCallback;
        Module.Wortal.afterAdPointer = afterAdCallback;
        Module.Wortal.adDismissedPointer = adDismissedCallback;
        Module.Wortal.adViewedPointer = adViewedCallback;
        showRewardedAd(UTF8ToString(description));
    },

    LogLevelStart: function (level) {
        logLevelStart(UTF8ToString(level));
    },

    LogLevelEnd: function (level, score) {
        logLevelEnd(UTF8ToString(level), UTF8ToString(score));
    },

    LogLevelUp: function (level) {
        logLevelUp(UTF8ToString(level));
    },

    LogScore: function (score) {
        logScore(UTF8ToString(score));
    },

    LogGameChoice: function (decision, choice) {
        logGameChoice(UTF8ToString(decision), UTF8ToString(choice));
    }
});
