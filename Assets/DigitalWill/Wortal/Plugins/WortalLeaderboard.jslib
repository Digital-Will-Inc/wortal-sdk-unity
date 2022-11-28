mergeInto(LibraryManager.library, {

    LeaderboardGetJS: function (name, callback, errorCallback) {
        var nameStr = UTF8ToString(name);
        window.Wortal.leaderboard.getLeaderboardAsync(nameStr)
            .then(res => {
                var result = res._current;
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

    LeaderboardSendEntryJS: function (name, score, details, callback, errorCallback) {
        var nameStr = UTF8ToString(name);
        var detailsStr = UTF8ToString(details);
        window.Wortal.leaderboard.sendEntryAsync(nameStr, score, detailsStr)
            .then(res => {
                var result = res._current;
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

    LeaderboardGetEntriesJS: function (name, count, offset, callback, errorCallback) {
        var nameStr = UTF8ToString(name);
        window.Wortal.leaderboard.getEntriesAsync(nameStr, count, offset)
            .then(res => {
                var result = res.map(entry => entry._current);
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

    LeaderboardGetPlayerEntryJS: function (name, callback, errorCallback) {
        var nameStr = UTF8ToString(name);
        window.Wortal.leaderboard.getPlayerEntryAsync(nameStr)
            .then(res => {
                var result = res._current;
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

    LeaderboardGetEntryCountJS: function (name, callback, errorCallback) {
        var nameStr = UTF8ToString(name);
        window.Wortal.leaderboard.getEntryCountAsync(nameStr)
            .then(result => {
                return Module.dynCall_vi(callback, result);
            })
            .catch(error => {
                var errorStr = JSON.stringify(error);
                var bufferSize = lengthBytesUTF8(errorStr) + 1;
                var buffer = _malloc(bufferSize);
                stringToUTF8(errorStr, buffer, bufferSize);
                return Module.dynCall_vi(errorCallback, buffer);
            });
    },

    LeaderboardGetConnectedPlayersEntriesJS: function (name, count, offset, callback, errorCallback) {
        var nameStr = UTF8ToString(name);
        window.Wortal.leaderboard.getConnectedPlayersEntriesAsync(nameStr, count, offset)
            .then(res => {
                var result = res.map(entry => entry._current);
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
    }

});
