let init = document.createElement('script');
init.type = 'text/javascript';

let platform = window.getWortalPlatform();
console.log('[Wortal] Platform: ' + platform);

if (platform === 'wortal') {
  init.src = 'assets/js/init-adsense.js';
}
else if (platform === 'link') {
  init.src = 'assets/js/init-link.js';
}
else if (platform === 'viber') {
  init.src = 'assets/js/init-viber.js';
}
else {
  console.log('Platform not supported.');
}

document.head.appendChild(init);
