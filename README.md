# Wortal SDK

Unity plugin for WebGL games using the Wortal SDK to deploy games on the Digital Will HTML5 Game Portal.

### Install and Setup

- Install the <b>WortalSDK</b> Unity package.
- Configure localization support via toolbar: `DigitalWill/Wortal/Wortal Config`.
- On installation the plugin sets the necessary project settings. If changed, these can be set again via toolbar:
`DigitalWill/Wortal/Set Project Settings`.

### Ads

Ads are called via the `Wortal` class:
```c#
// Show an interstitial ad when the player advances to the next level.
Wortal.ShowInterstitialAd(Placement.Next, "Level5Finished");

// Show a rewarded ad when the player loses and may revive to continue.
Wortal.ShowRewardedAd("ReviveAndContinue");
```

<b>WortalSDK</b> offers event callbacks to manage game flow during the ad cycle.

```c#
// Wortal.BeforeAd can be used to pause the game when an ad is ready to be shown.
Wortal.BeforeAd += OnBeforeAd;
void OnBeforeAd()
{
    Game.Pause();
}

// Wortal.AfterAd can be used to resume the game when an ad has finished showing.
Wortal.AfterAd += OnAfterAd;
void OnAfterAd()
{
    Game.Resume();
}

// The player watched a rewarded ad successfully and needs to be given a reward.
Wortal.AdViewed += OnAdViewed;
void OnAdViewed()
{
    Player.Coins += CoinsEarned * RewardMultiplier;
}

// The player dismissed the rewarded ad before finishing and should not be rewarded.
Wortal.AdDismissed += OndDismissed;
void OnAdDismissed()
{
    Player.Coins += CoinsEarned;
}
```

### Localization

Games on the Wortal need to support localization. <b>WortalSDK</b> offers functionality to check the player's
browser language to determine what language the game should be played in. This can be enabled or disabled in the
`WortalSettings`.

For all languages that will be supported in the game, you must add them to the `Supported Languages` config
in the `WortalSettings`. Here you can also change the default language that will be used if the player's
preferred language is not supported.

<b>WortalSDK</b> does not offer localization features itself, only checking the player's preferred language.

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
