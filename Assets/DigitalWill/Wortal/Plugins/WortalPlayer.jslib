mergeInto(LibraryManager.library, {

    PlayerGetIDJS: function () {
        var resultStr = window.Wortal.player.getID();
        if (resultStr === null) {
            resultStr = "";
        }
        var bufferSize = lengthBytesUTF8(resultStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(resultStr, buffer, bufferSize);
        return buffer;
    },

    PlayerGetNameJS: function () {
        var resultStr = window.Wortal.player.getName();
        if (resultStr === null) {
            resultStr = "";
        }
        var bufferSize = lengthBytesUTF8(resultStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(resultStr, buffer, bufferSize);
        return buffer;
    },

    PlayerGetPhotoJS: function () {
        var resultStr = window.Wortal.player.getPhoto();
        if (resultStr === null) {
            resultStr = "";
        }
        var bufferSize = lengthBytesUTF8(resultStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(resultStr, buffer, bufferSize);
        return buffer;
    },

    PlayerIsFirstPlayJS: function () {
        return window.Wortal.player.isFirstPlay();
    },

    PlayerGetDataJS: function (keys, callback, errorCallback) {
        keysStr = UTF8ToString(keys);
        keysArr = keysStr.split("|");
        window.Wortal.player.getDataAsync(keysArr)
            .then(result => {
                console.log(result);
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

    PlayerSetDataJS: function (data, callback, errorCallback) {
        dataStr = UTF8ToString(data);
        dataJson = JSON.parse(dataStr);
        window.Wortal.player.setDataAsync(dataJson)
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

    PlayerGetConnectedPlayersJS: function (payload, callback, errorCallback) {
        payloadStr = UTF8ToString(payload);
        payloadJson = JSON.parse(payloadStr);
        window.Wortal.player.getConnectedPlayersAsync(payloadJson)
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

    PlayerGetSignedPlayerInfoJS: function (callback, errorCallback) {
        window.Wortal.player.getSignedPlayerInfoAsync()
            .then(info => {
                var idStr = info.id;
                var idLen = lengthBytesUTF8(idStr) + 1;
                var idPtr = _malloc(idLen);
                stringToUTF8(idStr, idPtr, idLen);

                var sigStr = info.signature;
                var sigLen = lengthBytesUTF8(sigStr) + 1;
                var sigPtr = _malloc(sigLen);
                stringToUTF8(sigStr, sigPtr, sigLen);

                Module.dynCall_vii(callback, idPtr, sigPtr);
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
