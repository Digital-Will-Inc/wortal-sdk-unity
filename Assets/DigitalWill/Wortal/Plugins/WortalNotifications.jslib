mergeInto(LibraryManager.library, {

    NotificationsScheduleJS: function (payload, callback, errorCallback) {
        window.Wortal.notifications.scheduleAsync(JSON.parse(UTF8ToString(payload)))
            .then(result => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(result)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    NotificationsGetHistoryJS: function (callback, errorCallback) {
        window.Wortal.notifications.getHistoryAsync()
            .then(result => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(result)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    NotificationsCancelJS: function (id, callback, errorCallback) {
        window.Wortal.notifications.cancelAsync(UTF8ToString(id))
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    NotificationsCancelAllJS: function (callback, errorCallback) {
        window.Wortal.notifications.cancelAllAsync()
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    NotificationsCancelAllLabelJS: function (label, callback, errorCallback) {
        window.Wortal.notifications.cancelAllAsync(UTF8ToString(label))
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    }

});
