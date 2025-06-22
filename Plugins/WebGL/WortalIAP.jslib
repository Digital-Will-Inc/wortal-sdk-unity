mergeInto(LibraryManager.library, {

    IAPIsEnabledJS: function () {
        return window.Wortal.iap.isEnabled();
    },

    IAPGetCatalogJS: function (callback, errorCallback) {
        window.Wortal.iap.getCatalogAsync()
            .then(result => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(result)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    IAPGetPurchasesJS: function (callback, errorCallback) {
        window.Wortal.iap.getPurchasesAsync()
            .then(result => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(result)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    IAPMakePurchaseJS: function (purchase, callback, errorCallback) {
        window.Wortal.iap.makePurchaseAsync(JSON.parse(UTF8ToString(purchase)))
            .then(result => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(result)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    IAPConsumePurchaseJS: function (token, callback, errorCallback) {
        window.Wortal.iap.consumePurchaseAsync(UTF8ToString(token))
            .then(result => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    }

});
