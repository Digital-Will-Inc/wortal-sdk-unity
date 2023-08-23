mergeInto(LibraryManager.library, {

    LeaderboardGetJS: function (name, callback, errorCallback) {
        window.Wortal.leaderboard.getLeaderboardAsync(UTF8ToString(name))
            .then(res => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(res._current)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    LeaderboardSendEntryJS: function (name, score, details, callback, errorCallback) {
        window.Wortal.leaderboard.sendEntryAsync(UTF8ToString(name), score, UTF8ToString(details))
            .then(res => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(res._current)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    LeaderboardGetEntriesJS: function (name, count, offset, callback, errorCallback) {
        window.Wortal.leaderboard.getEntriesAsync(UTF8ToString(name), count, offset)
            .then(res => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(res.map(entry => entry._current))));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    LeaderboardGetPlayerEntryJS: function (name, callback, errorCallback) {
        window.Wortal.leaderboard.getPlayerEntryAsync(UTF8ToString(name))
            .then(res => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(res._current)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    LeaderboardGetEntryCountJS: function (name, callback, errorCallback) {
        window.Wortal.leaderboard.getEntryCountAsync(UTF8ToString(name))
            .then(result => {
                return Module.dynCall_vi(callback, result);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    LeaderboardGetConnectedPlayersEntriesJS: function (name, count, offset, callback, errorCallback) {
        window.Wortal.leaderboard.getConnectedPlayersEntriesAsync(UTF8ToString(name), count, offset)
            .then(res => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(res.map(entry => entry._current))));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    }

});
