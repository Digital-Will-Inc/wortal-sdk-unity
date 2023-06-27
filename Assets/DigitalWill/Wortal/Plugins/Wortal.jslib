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
    }

});
