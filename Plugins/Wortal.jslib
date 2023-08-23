mergeInto(LibraryManager.library, {

    OnPauseJS: function (callback) {
        window.Wortal.onPause(() => Module.dynCall_v(callback));
    },

    GetSupportedAPIsJS: function () {
        var result = window.Wortal.getSupportedAPIs();
        var resultStr = JSON.stringify(result);
        var bufferSize = lengthBytesUTF8(resultStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(resultStr, buffer, bufferSize);
        return buffer;
    },

    PerformHapticFeedbackJS: function (callback, errorCallback) {
        window.Wortal.performHapticFeedbackAsync()
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
