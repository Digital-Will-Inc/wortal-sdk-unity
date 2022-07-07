window.addEventListener("load", () => {
  window.initWortalLink(function () {
    if (window.wortalLink) {
      wortalLink = window.wortalLink;
      Promise.all([ShowGame(), wortalLink.initializeAsync()])
        .then(() => {
          wortalLink.startGameAsync();
        })
    } else {
      ShowGame();
    }
  });
});

function ShowGame() {
  document.getElementById("black-cover").hidden = true;
}
