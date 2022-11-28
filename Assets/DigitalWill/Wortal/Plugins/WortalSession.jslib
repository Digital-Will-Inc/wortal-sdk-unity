mergeInto(LibraryManager.library, {

    SessionGetEntryPointDataJS: function () {
        var result = window.Wortal.session.getEntryPointData();
        var resultStr = JSON.stringify(result);
        var bufferSize = lengthBytesUTF8(resultStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(resultStr, buffer, bufferSize);
        return buffer;
    },

    SessionGetEntryPointJS: function (callback, errorCallback) {
        window.Wortal.session.getEntryPointAsync()
            .then(result => {
                var bufferSize = lengthBytesUTF8(result) + 1;
                var buffer = _malloc(bufferSize);
                stringToUTF8(result, buffer, bufferSize);
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

    SessionSetSessionDataJS: function (data) {
        dataStr = UTF8ToString(data);
        dataJson = JSON.parse(dataStr);
        console.log(dataStr);
        window.Wortal.session.setSessionData(dataJson);
    },

    SessionGetLocaleJS: function () {
        var result = window.Wortal.session.getLocale();
        var bufferSize = lengthBytesUTF8(result) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(result, buffer, bufferSize);
        return buffer;
    },

    SessionGetTrafficSourceJS: function () {
        var result = window.Wortal.session.getTrafficSource();
        var resultStr = JSON.stringify(result);
        var bufferSize = lengthBytesUTF8(resultStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(resultStr, buffer, bufferSize);
        return buffer;
    }

});
