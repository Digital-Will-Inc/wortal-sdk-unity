mergeInto(LibraryManager.library, {

    PlayerGetIDJS: function () {
        return gameInstance.Module.allocString(window.Wortal.player.getID());
    },

    PlayerGetNameJS: function () {
        return gameInstance.Module.allocString(window.Wortal.player.getName());
    },

    PlayerGetPhotoJS: function () {
        return gameInstance.Module.allocString(window.Wortal.player.getPhoto());
    },

    PlayerIsFirstPlayJS: function () {
        return window.Wortal.player.isFirstPlay();
    },

    PlayerGetDataJS: function (keys, callback, errorCallback) {
        window.Wortal.player.getDataAsync(UTF8ToString(keys).split("|"))
            .then(result => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(result)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    PlayerSetDataJS: function (data, callback, errorCallback) {
        window.Wortal.player.setDataAsync(JSON.parse(UTF8ToString(data)))
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    PlayerFlushDataJS: function (callback, errorCallback) {
        window.Wortal.player.flushDataAsync()
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    PlayerGetConnectedPlayersJS: function (payload, callback, errorCallback) {
        window.Wortal.player.getConnectedPlayersAsync(JSON.parse(UTF8ToString(payload)))
            .then(result => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(result)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    PlayerGetSignedPlayerInfoJS: function (callback, errorCallback) {
        window.Wortal.player.getSignedPlayerInfoAsync()
            .then(info => {
                var id = gameInstance.Module.allocString(info.id);
                var sig = gameInstance.Module.allocString(info.signature);
                return Module.dynCall_vii(callback, id, sig);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    PlayerGetASIDJS: function () {
        return gameInstance.Module.allocString(window.Wortal.player.getASID());
    },

    PlayerGetSignedASIDJS: function (callback, errorCallback) {
        window.Wortal.player.getSignedASIDAsync()
            .then(info => {
                var id = gameInstance.Module.allocString(info.id);
                var sig = gameInstance.Module.allocString(info.signature);
                return Module.dynCall_vii(callback, id, sig);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    PlayerCanSubscribeBotJS: function (callback, errorCallback) {
        window.Wortal.player.canSubscribeBotAsync()
            .then(result => {
                return Module.dynCall_vi(callback, result);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    PlayerSubscribeBotJS: function (callback, errorCallback) {
        window.Wortal.player.subscribeBotAsync()
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    }

});
