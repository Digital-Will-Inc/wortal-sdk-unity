mergeInto(LibraryManager.library, {

  GetPlatform: function () {
    var platformStr = window.getWortalPlatform();
    var bufferSize = lengthBytesUTF8(platformStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(platformStr, buffer, bufferSize);
    return buffer;
  },

  GetBrowserLanguage: function () {
    var languageStr = navigator.language;
    var bufferSize = lengthBytesUTF8(languageStr) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(languageStr, buffer, bufferSize);
    return buffer;
  },

  OpenLink: function(url) {
    url = Pointer_stringify(url);
    window.open(url, '_blank');
  },

});
