mergeInto(LibraryManager.library, {

    GetContextIdJS: function () {
        var languageStr = window.Wortal.context.getId();
        if (languageStr === null) {
            languageStr = "";
        }
        var bufferSize = lengthBytesUTF8(languageStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(languageStr, buffer, bufferSize);
        return buffer;
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
    }

});
