mergeInto(LibraryManager.library, {

    LogLevelStartJS: function (level) {
        window.Wortal.analytics.logLevelStart(UTF8ToString(level));
    },

    LogLevelEndJS: function (level, score, wasCompleted) {
        window.Wortal.analytics.logLevelEnd(UTF8ToString(level), UTF8ToString(score), wasCompleted);
    },

    LogLevelUpJS: function (level) {
        window.Wortal.analytics.logLevelUp(UTF8ToString(level));
    },

    LogScoreJS: function (score) {
        window.Wortal.analytics.logScore(UTF8ToString(score));
    },

    LogTutorialStartJS: function (tutorial) {
        window.Wortal.analytics.logTutorialStart(UTF8ToString(tutorial));
    },

    LogTutorialEndJS: function (tutorial, wasCompleted) {
        window.Wortal.analytics.logTutorialEnd(UTF8ToString(tutorial), wasCompleted);
    },

    LogGameChoiceJS: function (decision, choice) {
        window.Wortal.analytics.logGameChoice(UTF8ToString(decision), UTF8ToString(choice));
    }

});
