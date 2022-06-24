var isFullscreen = false;

document.addEventListener('keydown', function(event) {
  if (event.which === 70) {
    if (!isFullscreen) {
      myGameInstance.SetFullscreen(1);
    } else {
      myGameInstance.SetFullscreen(0);
    }
    isFullscreen = !isFullscreen;
  }
});
