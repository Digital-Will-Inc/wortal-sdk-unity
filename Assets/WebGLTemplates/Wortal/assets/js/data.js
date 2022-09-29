var gameData = {
    gameName: document.title,
    platform: window.getWortalPlatform(),
    browser: navigator.userAgent,
    country: "",
    linkInterstitialId: "",
    linkRewardedId: "",
    gameTimer: 0,
    levelTimer: 0,
    levelTimerHandle: 0,
    levelName: "",
};

function logLevelStart(level) {
    if (gameData.levelTimerHandle != null) {
        clearInterval(gameData.levelTimerHandle);
        gameData.levelTimerHandle = null;
    }
    gameData.levelName = level;
    gameData.levelTimer = 0;
    gameData.levelTimerHandle = setInterval(() => gameData.levelTimer += 1, 1000);
    _logEvent("LevelStart", {
        game: gameData.gameName,
        level: level,
    });
}

function logLevelEnd(level, score = '0', wasCompleted = true) {
    if (gameData.levelTimerHandle != null) {
        clearInterval(gameData.levelTimerHandle);
        gameData.levelTimerHandle = null;
    }
    if (gameData.levelName !== level) {
        gameData.levelTimer = 0;
    }
    _logEvent("LevelEnd", {
        game: gameData.gameName,
        level: level,
        time: gameData.levelTimer,
        score: score,
        complete: wasCompleted,
    });
    gameData.levelTimer = 0;
}

function logLevelUp(level) {
    _logEvent("LevelUp", {
        game: gameData.gameName,
        level: level,
    });
}

function logScore(score) {
    _logEvent("PostScore", {
        game: gameData.gameName,
        score: score,
    });
}

function logGameChoice(decision, choice) {
    _logEvent("GameChoice", {
        game: gameData.gameName,
        decision: decision,
        choice: choice,
    });
}

function _logGameStart(country) {
    gameData.country = country;
    _logEvent("GameStart", {
        game: gameData.gameName,
        platform: gameData.platform,
        browser: gameData.browser,
        country: gameData.country,
    });
    setInterval(function () {
        if (document.visibilityState !== "hidden") {
            gameData.gameTimer += 1;
        }
    }, 1000);
}

function _logGameEnd() {
    _logEvent("GameEnd", {
       game: gameData.gameName,
       timePlayed: gameData.gameTimer,
    });
}

function _logEvent(name, features) {
    let request = new XMLHttpRequest();
    request.open("POST", "https://wombat.digitalwill.co.jp/wortal/events");
    request.setRequestHeader("Content-Type", "application/json");
    request.send(JSON.stringify({ name, features }));
}
