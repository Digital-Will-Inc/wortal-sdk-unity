let instance = document.createElement('script');
let ads = document.createElement('script');

instance.type = 'text/javascript';
ads.type = 'text/javascript';

if (platform === 'wortal') {
  instance.src = 'assets/js/instance-adsense.js';
  ads.src = 'assets/js/ads-adsense.js';
}
else if (platform === 'link') {
  instance.src = 'assets/js/instance-link.js';
  ads.src = 'assets/js/ads-link.js';
}
else if (platform === 'viber') {
  instance.src = 'assets/js/instance-viber.js';
  ads.src = 'assets/js/ads-viber.js';
}
else {
  console.log('Platform not supported.');
}

document.body.appendChild(instance);
document.body.appendChild(ads);
