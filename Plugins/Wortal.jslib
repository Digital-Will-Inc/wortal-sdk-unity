mergeInto(LibraryManager.library, {

    IsInitializedJS: function () {
        return window.Wortal.isInitialized;
    },

    InitializeJS: function (callback, errorCallback) {
        window.Wortal.initializeAsync()
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    StartGameJS: function (callback, errorCallback) {
        window.Wortal.startGameAsync()
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    SetLoadingProgressJS: function (progress) {
        window.Wortal.setLoadingProgress(progress);
    },

    OnPauseJS: function (callback) {
        window.Wortal.onPause(() => Module.dynCall_v(callback));
    },

    GetSupportedAPIsJS: function () {
        return gameInstance.Module.allocString(JSON.stringify(window.Wortal.getSupportedAPIs()));
    },

    PerformHapticFeedbackJS: function (callback, errorCallback) {
        window.Wortal.performHapticFeedbackAsync()
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    }

});
