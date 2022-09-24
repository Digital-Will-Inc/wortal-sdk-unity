window.addEventListener("load", () => {
    let platform = window.getWortalPlatform();
    console.log('[Wortal] Platform: ' + platform);
    window.initWortal(function () {
        console.log("[Wortal] Initializing..");
        if (platform === 'link' || platform === 'viber') {
            if (window.wortalGame) {
                document.querySelector('#progress').style.display = "none"; // Use Link's loading progress bar.
                wortalGame = window.wortalGame;
                wortalGame.initializeAsync().then(() => {
                    document.getElementById("loading-cover").style.display = "none";
                    wortalGame.startGameAsync();
                });
            } else {
                document.getElementById("loading-cover").style.display = "none";
                console.error("[Wortal] Failed to find wortalGame.");
            }
        } else {
            window.triggerWortalAd("preroll", "", "Preroll", {
                adBreakDone: function () {
                    console.log("[Wortal] AdBreakDone");
                    document.getElementById("loading-cover").hidden = true;
                },
                noShow: function () {
                    console.log("[Wortal] NoShow");
                    document.getElementById("loading-cover").hidden = true;
                }
            });
        }
        console.log("[Wortal] Initialized");
    });
});
