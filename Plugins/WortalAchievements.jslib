mergeInto(LibraryManager.library, {

    AchievementsGetAchievementsJS: function (callback, errorCallback) {
        return window.Wortal.achievements.getAchievementsAsync()
            .then(result => {
                return Module.dynCall_vi(callback, gameInstance.Module.allocString(JSON.stringify(result)));
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    },

    AchievementsUnlockAchievementJS: function (achievementId, callback, errorCallback) {
        return window.Wortal.achievements.unlockAchievementAsync(UTF8ToString(achievementId))
            .then(result => {
                return Module.dynCall_vi(callback, result);
            })
            .catch(error => {
                return Module.dynCall_vi(errorCallback, gameInstance.Module.allocString(JSON.stringify(error)));
            });
    }

});
