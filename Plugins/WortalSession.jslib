mergeInto(LibraryManager.library, {

    SessionGetEntryPointDataJS: function () {
        return gameInstance.Module.allocString(JSON.stringify(window.Wortal.session.getEntryPointData()));
    },

    SessionGetEntryPointJS: function (callback, errorCallback) {
        window.Wortal.session.getEntryPointAsync()
            .then(result => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(result));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    SessionSetSessionDataJS: function (data) {
        window.Wortal.session.setSessionData(JSON.parse(UTF8ToString(data)));
    },

    SessionGetLocaleJS: function () {
        return gameInstance.Module.allocString(window.Wortal.session.getLocale());
    },

    SessionGetTrafficSourceJS: function () {
        return gameInstance.Module.allocString(JSON.stringify(window.Wortal.session.getTrafficSource()));
    },

    SessionGetPlatformJS: function () {
        return gameInstance.Module.allocString(window.Wortal.session.getPlatform());
    },

    SessionGetDeviceJS: function () {
        return gameInstance.Module.allocString(window.Wortal.session.getDevice());
    },

    SessionGetOrientationJS: function () {
        return gameInstance.Module.allocString(window.Wortal.session.getOrientation());
    },

    SessionOnOrientationChangeJS: function (callback) {
        window.Wortal.session.onOrientationChanged(result => {
            Module.dynCall_vi(callback, gameInstance.Module.allocString(result));
        });
    },

    SessionSwitchGameJS: function (gameId) {
        window.Wortal.session.switchGameAsync(UTF8ToString(gameId));
    },

    SessionHappyTimeJS: function () {
        window.Wortal.session.happyTime();
    },

    SessionGameplayStartJS: function () {
        window.Wortal.session.gameplayStart();
    },

    SessionGameplayStopJS: function () {
        window.Wortal.session.gameplayStop();
    },

    SessionIsAudioEnabledJS: function() {
        window.Wortal.session.isAudioEnabled();
    },

    SessionOnAudioStatusChangeJS: function (callback) {
        window.Wortal.session.onAudioStatusChange(isAudioEnabled => {
            Module.dynCall_vi(callback, isAudioEnabled ? 1 : 0); // 1 for true, 0 for false
        });
    },


});
