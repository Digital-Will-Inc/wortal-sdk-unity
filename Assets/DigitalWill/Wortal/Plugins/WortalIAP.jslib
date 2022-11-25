﻿mergeInto(LibraryManager.library, {

    IAPIsEnabledJS: function () {
        return window.Wortal.iap.isEnabled();
    },

    IAPGetCatalogJS: function (callback, errorCallback) {
        window.Wortal.iap.getCatalogAsync()
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

    IAPGetPurchasesJS: function (callback, errorCallback) {
        window.Wortal.iap.getPurchasesAsync()
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

    IAPMakePurchaseJS: function (purchase, callback, errorCallback) {
        purchaseStr = UTF8ToString(purchase);
        purchaseJson = JSON.parse(purchaseStr);
        window.Wortal.iap.makePurchaseAsync(purchaseJson)
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

    IAPConsumePurchaseJS: function (token, callback, errorCallback) {
        tokenStr = UTF8ToString(token);
        window.Wortal.iap.consumePurchaseAsync(tokenStr)
            .then(result => {
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