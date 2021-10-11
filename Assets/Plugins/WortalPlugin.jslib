mergeInto(LibraryManager.library, {

    GetBrowserLanguage: function() {
        var languageStr = navigator.language;
        var bufferSize = lengthBytesUTF8(languageStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(languageStr, buffer, bufferSize);
        return buffer;
    },

});
