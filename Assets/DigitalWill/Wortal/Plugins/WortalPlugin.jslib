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

  ShowInterstitialAdAdSense: function(type, name, beforeAdCallback, afterAdCallback, adBreakDoneCallback, noShowCallback) {
    Module.Wortal.beforeAdPointerAdSense = beforeAdCallback;
    Module.Wortal.afterAdPointerAdSense = afterAdCallback;
    Module.Wortal.adBreakDonePointerAdSense = adBreakDoneCallback;
    Module.Wortal.noShowPointerAdSense = noShowCallback;
    showInterstitialAdAdSense(UTF8ToString(type), UTF8ToString(name));
  },

  RequestRewardedAdAdSense: function(name, beforeAdCallback, afterAdCallback, adBreakDoneCallback, beforeRewardCallback,
                                     adDismissedCallback, adViewedCallback, noShowCallback) {
    Module.Wortal.beforeAdPointerAdSense = beforeAdCallback;
    Module.Wortal.afterAdPointerAdSense = afterAdCallback;
    Module.Wortal.adBreakDonePointerAdSense = adBreakDoneCallback;
    Module.Wortal.beforeRewardPointerAdSense = beforeRewardCallback;
    Module.Wortal.adDismissedPointerAdSense = adDismissedCallback;
    Module.Wortal.adViewedPointerAdSense = adViewedCallback;
    Module.Wortal.noShowPointerAdSense = noShowCallback;
    showRewardedAdAdSense('reward', UTF8ToString(name));
  },

  ShowRewardedAdAdSense: function () {
    triggerShowRewardedAdFn();
  },

  ShowInterstitialAdLink: function(type, placementId, beforeAdCallback, afterAdCallback, noBreakCallback) {
    Module.Wortal.beforeAdPointerLink = beforeAdCallback;
    Module.Wortal.afterAdPointerLink = afterAdCallback;
    Module.Wortal.noBreakPointerLink = noBreakCallback;
    showInterstitialAdLink(UTF8ToString(type), UTF8ToString(placementId));
  },

  ShowRewardedAdLink: function(placementId, beforeAdCallback, afterAdCallback, adDismissedCallback, adViewedCallback, noBreakCallback) {
    Module.Wortal.beforeAdPointerLink = beforeAdCallback;
    Module.Wortal.afterAdPointerLink = afterAdCallback;
    Module.Wortal.adDismissedPointerLink = adDismissedCallback;
    Module.Wortal.adViewedPointerLink = adViewedCallback;
    Module.Wortal.noBreakPointerLink = noShowCallback;
    showRewardedAdLink('reward', UTF8ToString(placementId));
  },

});
