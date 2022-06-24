window.addEventListener("load", () => {
  window.initWortal(function () {
    console.log("Wortal setup complete!");
    ShowPreroll(function () {
      ShowGame();
    });
  });
});

function ShowPreroll(adBreakDone, noShow) {
  window.triggerWortalAd("preroll", "Preroll", {
    adBreakDone: function () {
      if (adBreakDone) adBreakDone();
    },
    noShow: function () {
      if (noShow) {
        noShow();
      } else {
        adBreakDone();
      }
    }
  });
}

function ShowGame() {
  document.getElementById("black-cover").hidden = true;
}
