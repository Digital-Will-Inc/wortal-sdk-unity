# Wortal Plugin

Unity plugin for WebGL games deployed to the Wortal.

### Install and Setup

- Install the Wortal Plugin Unity package.
- Set project settings via toolbar: `DigitalWill/Wortal/Set Project Settings`
- Configure plugin via toolbar: `DigitalWill/Wortal/Wortal Config`

### Platforms

As of version 4.0.0 the Wortal Plugin needs some modifications to work on different platforms, and separate builds will
need to be made. Future versions should resolve this limitation and allow one build for all platforms, but for now these steps
must be taken:

- Select `Platform` in the `WortalSettings`
- Modify the following in `Assets/WebGLTemplates/Wortal/index.html`:
    - Change `<script src="assets/js/wortal-init.js">` to desired platform (ex: `wortal-init-adsense.js`)
    - Change `<script src="assets/js/instance.js">` to desired platform (ex: `instance-link.js`)
    - Change `<div id="progress" class="progress Dark">` to have `style="display: none;"` for Link platform.
- Build the game - this is necessary to change the ad handling implementation for each platform

### Ads

Ads are called via the `Wortal` class:
```c#
// Show an interstitial ad when the player advances to the next level.
Wortal.ShowInterstitialAd(Placement.Next, "Level5Finished");

// Show a rewarded ad when the player loses and may revive to continue.
Wortal.ShowRewardedAd("ReviveAndContinue");
```

Wortal Plugin offers event callbacks to manage game flow during the ad cycle.

```c#
// Wortal.BeforeAd can be used to pause the game when an ad is ready to be shown.
Wortal.BeforeAd += OnBeforeAd;
void OnBeforeAd()
{
    Game.Pause();
}

// Both Wortal.AdDone and Wortal.AdTimedOut should be subscribed to by the caller, as these will
// signal that the ad cycle is finished and the game can now resume.

// Wortal.AdDone can be used to resume the game when an ad has finished showing.
Wortal.AdDone += OnAdDone;
void OnAdDone()
{
    Game.Resume();
}

// Wortal.AdTimedOut is called when no ad was returned, either due to error or frequency capping.
Wortal.AdTimedOut += OnAdTimedOut;
void OnAdTimedOut()
{
    Game.Resume();
}

// The player watched a rewarded ad successfully and needs to be given a reward.
Wortal.RewardedAdViewed += OnRewardedAdViewed;
void OnRewardedAdViewed()
{
    Player.Coins += CoinsEarned * RewardMultiplier;
}

// The player dismissed the rewarded ad before finishing and should not be rewarded.
Wortal.RewardedAdDismissed += OnRewardedAdDismissed;
void OnRewardedAdDismissed()
{
    Player.Coins += CoinsEarned;
}
```

### Localization

Games on the Wortal need to support localization. Wortal Plugin offers functionality to check the player's
browser language to determine what language the game should be played in.

Wortal Plugin does not offer localization features itself, only the player's preferred language.

```c#
// Event fired when the language is determined.
Wortal.LanguageSet += OnLanguageSet;
void OnLanguageSet(Language langauge)
{
    Localization.Init(language);
}

// Alternatively you can utilize the exposed properties.
if (Wortal.IsLanguageSet)
{
    Localization.Init(Wortal.Language);
}
```
