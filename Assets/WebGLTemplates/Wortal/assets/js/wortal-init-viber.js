window.addEventListener("load", () => {
  window.initWortal(function () {
    if (window.wortalGame) {
      document.querySelector('#progress').style.display = "none"; // Use Viber's loading progress bar.
      wortalGame = window.wortalGame;
      wortalGame.initializeAsync().then(() => {
        document.getElementById("loading-cover").style.display = "none";
        wortalGame.startGameAsync();
      })
    } else {
      document.getElementById("loading-cover").style.display = "none";
      console.error("[Wortal] Failed to find wortalGame.");
    }
  });
});
