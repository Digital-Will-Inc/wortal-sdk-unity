window.addEventListener("load", () => {
  window.initWortal(function () {
    console.log("[Wortal] Setup complete.");
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
  });
});
