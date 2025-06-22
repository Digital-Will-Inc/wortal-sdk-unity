mergeInto(LibraryManager.library, {

    StatsGetStatsJS: function (level, payload, callback, errorCallback) {
        window.Wortal.stats.getStatsAsync(UTF8ToString(level), UTF8ToString(payload))
            .then(result => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(result)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    StatsPostStatsJS: function (level, value, payload, callback, errorCallback) {
        return window.Wortal.stats.postStatsAsync(UTF8ToString(level), value, UTF8ToString(payload))
            .then(result => {
                return Module.dynCall_vi(callback, result);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    }

});
