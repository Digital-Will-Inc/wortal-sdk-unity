mergeInto(LibraryManager.library, {

    TournamentGetCurrentJS: function (callback, errorCallback) {
        window.Wortal.tournament.getCurrentAsync()
            .then(result => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(result)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    TournamentGetAllJS: function (callback, errorCallback) {
        window.Wortal.tournament.getAllAsync()
            .then(result => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(result)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    TournamentPostScoreJS: function (score, callback, errorCallback) {
        window.Wortal.tournament.postScoreAsync(score)
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    TournamentCreateJS: function (payload, callback, errorCallback) {
        window.Wortal.tournament.createAsync(JSON.parse(UTF8ToString(payload)))
            .then(result => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(result)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    TournamentShareJS: function (payload, callback, errorCallback) {
        window.Wortal.tournament.shareAsync(JSON.parse(UTF8ToString(payload)))
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    TournamentJoinJS: function (tournamentID, callback, errorCallback) {
        window.Wortal.tournament.joinAsync(UTF8ToString(tournamentID))
            .then(() => {
                return Module.dynCall_v(callback);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    }

});
