window.addEventListener("load", () => {
  window.initWortalLink(function () {
    if (window.wortalLink) {
      wortalLink = window.wortalLink;
      Promise.all([showGame(), wortalLink.initializeAsync()])
        .then(() => {
          wortalLink.startGameAsync();
        })
    } else {
      showGame();
    }
  });
});

function showGame() {
  document.getElementById("loading-cover").hidden = true;
}
