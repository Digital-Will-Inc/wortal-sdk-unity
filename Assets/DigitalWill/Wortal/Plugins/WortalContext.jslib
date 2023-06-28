﻿mergeInto(LibraryManager.library, {

    ContextGetIdJS: function () {
        return gameInstance.Module.allocString(window.Wortal.context.getId());
    },

    ContextGetTypeJS: function () {
        return gameInstance.Module.allocString(window.Wortal.context.getType());
    },

    ContextGetPlayersJS: function (callback, errorCallback) {
        window.Wortal.context.getPlayersAsync()
            .then(result => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(result)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    ContextChooseJS: function (payload, callback, errorCallback) {
        window.Wortal.context.chooseAsync(JSON.parse(UTF8ToString(payload)))
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    ContextShareJS: function (payload, callback, errorCallback) {
        window.Wortal.context.shareAsync(JSON.parse(UTF8ToString(payload)))
            .then(result => {
                return Module.dynCall_vi(callback, result);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    ContextShareLinkJS: function (payload, callback, errorCallback) {
        window.Wortal.context.shareLinkAsync(JSON.parse(UTF8ToString(payload)))
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    ContextUpdateJS: function (payload, callback, errorCallback) {
        window.Wortal.context.updateAsync(JSON.parse(UTF8ToString(payload)))
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    ContextCreateJS: function (playerId, callback, errorCallback) {
        window.Wortal.context.createAsync(UTF8ToString(playerId))
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    ContextCreateGroupJS: function (playerIds, callback, errorCallback) {
        window.Wortal.context.createAsync(UTF8ToString(playerIds).split("|"))
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    ContextSwitchJS: function (contextId, callback, errorCallback) {
        window.Wortal.context.switchAsync(UTF8ToString(contextId))
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    ContextIsSizeBetweenJS: function (minSize, maxSize) {
        return gameInstance.Module.allocString(JSON.stringify(window.Wortal.context.isSizeBetween(minSize, maxSize)));
    }

});
