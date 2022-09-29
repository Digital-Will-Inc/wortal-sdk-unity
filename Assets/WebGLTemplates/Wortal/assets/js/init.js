window.addEventListener("load", () => {
    console.log("[Wortal] Platform: " + gameData.platform);
    window.initWortal(function () {
        console.log("[Wortal] Initializing..");
        if (gameData.platform === "link") {
            if (window.wortalGame) {
                _removeProgressBar();
                window.wortalGame.initializeAsync().then(() => {
                    _removeLoadingCover();
                    window.wortalGame.startGameAsync();
                    _getLinkAdUnitIds();
                });
            }
        } else if (gameData.platform === "viber") {
            if (window.wortalGame) {
                _removeProgressBar();
                window.wortalGame.initializeAsync().then(() => {
                    _removeLoadingCover();
                    window.wortalGame.startGameAsync();
                });
            }
        } else {
            window.triggerWortalAd("preroll", "", "Preroll", {
                adBreakDone: function () {
                    console.log("[Wortal] AdBreakDone");
                    _removeLoadingCover();
                },
                noShow: function () {
                    console.log("[Wortal] NoShow");
                    _removeLoadingCover();
                }
            });
        }
        console.log("[Wortal] Initialized");
    });

    window.addEventListener("visibilitychange", function () {
        if (window.visibilityState === "hidden") {
            _logGameEnd();
        }
    });

    _getIntlData()
        .then(response => _logGameStart(response))
        .catch(() => _logGameStart("Nulltherlands"));
});

function _getIntlData() {
    return fetch("assets/res/intl-data.json")
        .then(response => response.json())
        .then(data => _getPlayerCountry(data))
        .catch(error => console.log(error));
}

// This uses the time zone setting of the player to determine their country.
// We do this to avoid collecting any personal data on the player for GDPR compliance.
// The location is very coarse and easily spoofed so nothing here can identify the player.
function _getPlayerCountry(data) {
    if (data == null) {
        return "Nulltherlands";
    }
    const zone = Intl.DateTimeFormat().resolvedOptions().timeZone;
    const arr = zone.split("/");
    const city = arr[arr.length - 1];
    return data[city];
}

function _getLinkAdUnitIds() {
    wortalGame.getAdUnitsAsync().then((adUnits) => {
        console.log("[Wortal] Link AdUnit IDs returned: \n" + adUnits);
        gameData.linkInterstitialId = adUnits[0].id;
        gameData.linkRewardedId = adUnits[1].id;
    });
}

function _removeLoadingCover() {
    document.getElementById("loading-cover").style.display = "none";
}

function _removeProgressBar() {
    document.querySelector("#progress").style.display = "none";
}
