mergeInto(LibraryManager.library, {

    ContextGetIdJS: function () {
        var idStr = window.Wortal.context.getId();
        if (idStr === null) {
            idStr = "";
        }
        var bufferSize = lengthBytesUTF8(idStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(idStr, buffer, bufferSize);
        return buffer;
    },

    ContextGetTypeJS: function () {
        var typeStr = window.Wortal.context.getType();
        if (typeStr === null) {
            typeStr = "";
        }
        var bufferSize = lengthBytesUTF8(typeStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(typeStr, buffer, bufferSize);
        return buffer;
    },

    ContextGetPlayersJS: function () {
        window.Wortal.context.getPlayersAsync()
            .then(result => {
                var resultStr = JSON.stringify(result);
                var bufferSize = lengthBytesUTF8(resultStr) + 1;
                var buffer = _malloc(bufferSize);
                stringToUTF8(resultStr, buffer, bufferSize);
                return Module.dynCall_vi(callback, buffer);
            })
            .catch(error => {
                var errorStr = JSON.stringify(error);
                var bufferSize = lengthBytesUTF8(errorStr) + 1;
                var buffer = _malloc(bufferSize);
                stringToUTF8(errorStr, buffer, bufferSize);
                return Module.dynCall_vi(errorCallback, buffer);
            });
    },

    ContextChooseJS: function (payload, callback, errorCallback) {
        payloadStr = UTF8ToString(payload);
        payloadJson = JSON.parse(payloadStr);
        window.Wortal.context.chooseAsync(payloadJson)
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                var errorStr = JSON.stringify(error);
                var bufferSize = lengthBytesUTF8(errorStr) + 1;
                var buffer = _malloc(bufferSize);
                stringToUTF8(errorStr, buffer, bufferSize);
                return Module.dynCall_vi(errorCallback, buffer);
            });
    },

    ContextShareJS: function (payload, callback, errorCallback) {
        payloadStr = UTF8ToString(payload);
        payloadJson = JSON.parse(payloadStr);
        window.Wortal.context.shareAsync(payloadJson)
            .then(result => {
                return Module.dynCall_vi(callback, result);
            })
            .catch(error => {
                var errorStr = JSON.stringify(error);
                var bufferSize = lengthBytesUTF8(errorStr) + 1;
                var buffer = _malloc(bufferSize);
                stringToUTF8(errorStr, buffer, bufferSize);
                return Module.dynCall_vi(errorCallback, buffer);
            });
    },

    ContextShareLinkJS: function (payload, callback, errorCallback) {
        payloadStr = UTF8ToString(payload);
        payloadJson = JSON.parse(payloadStr);
        window.Wortal.context.shareLinkAsync(payloadJson)
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                var errorStr = JSON.stringify(error);
                var bufferSize = lengthBytesUTF8(errorStr) + 1;
                var buffer = _malloc(bufferSize);
                stringToUTF8(errorStr, buffer, bufferSize);
                return Module.dynCall_vi(errorCallback, buffer);
            });
    },

    ContextUpdateJS: function (payload, callback, errorCallback) {
        payloadStr = UTF8ToString(payload);
        payloadJson = JSON.parse(payloadStr);
        window.Wortal.context.updateAsync(payloadJson)
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                var errorStr = JSON.stringify(error);
                var bufferSize = lengthBytesUTF8(errorStr) + 1;
                var buffer = _malloc(bufferSize);
                stringToUTF8(errorStr, buffer, bufferSize);
                return Module.dynCall_vi(errorCallback, buffer);
            });
    },

    ContextCreateJS: function (playerId, callback, errorCallback) {
        idStr = UTF8ToString(playerId);
        window.Wortal.context.createAsync(idStr)
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                var errorStr = JSON.stringify(error);
                var bufferSize = lengthBytesUTF8(errorStr) + 1;
                var buffer = _malloc(bufferSize);
                stringToUTF8(errorStr, buffer, bufferSize);
                return Module.dynCall_vi(errorCallback, buffer);
            });
    },

    ContextCreateGroupJS: function (playerIds, callback, errorCallback) {
        idsStr = UTF8ToString(playerIds);
        idsArr = idsStr.split("|");
        window.Wortal.context.createAsync(idsArr)
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                var errorStr = JSON.stringify(error);
                var bufferSize = lengthBytesUTF8(errorStr) + 1;
                var buffer = _malloc(bufferSize);
                stringToUTF8(errorStr, buffer, bufferSize);
                return Module.dynCall_vi(errorCallback, buffer);
            });
    },

    ContextSwitchJS: function (contextId, callback, errorCallback) {
        idStr = UTF8ToString(contextId);
        window.Wortal.context.switchAsync(idStr)
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                var errorStr = JSON.stringify(error);
                var bufferSize = lengthBytesUTF8(errorStr) + 1;
                var buffer = _malloc(bufferSize);
                stringToUTF8(errorStr, buffer, bufferSize);
                return Module.dynCall_vi(errorCallback, buffer);
            });
    },

    ContextIsSizeBetweenJS: function (minSize, maxSize) {
        var result = window.Wortal.context.isSizeBetween(minSize, maxSize);
        var resultStr = JSON.stringify(result);
        var bufferSize = lengthBytesUTF8(resultStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(resultStr, buffer, bufferSize);
        return buffer;
    }

});
