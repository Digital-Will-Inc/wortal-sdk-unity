window.addEventListener("load", () => {
  window.initWortal(function () {
    console.log("Wortal setup complete!");
    setTimeout(() => {
      window.triggerWortalAd("preroll", "Load Game", {
        adBreakDone: function () {
          console.log("AdBreakDone reached.");
          // Render game on adBreakDone.
        }
      });
    }, 100);
  });
});
