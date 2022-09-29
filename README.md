# Wortal SDK

## Installation

- Install the <b>Wortal SDK</b> Unity package.
- On installation the plugin sets the necessary project settings. If changed, these can be set again via toolbar:
`DigitalWill/Wortal/Set Project Settings`.

## How to Use

### Ads

Ads are called via the `Wortal` class:
```c#
// Show an interstitial ad when the player advances to the next level.
Wortal.ShowInterstitialAd(Placement.Next, "Level5Finished");

// Show a rewarded ad when the player loses and may revive to continue.
Wortal.ShowRewardedAd("ReviveAndContinue");
```

<b>Wortal SDK</b> offers event callbacks to manage game flow during the ad cycle.

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
Wortal.RewardPlayer += OnRewardPlayer;
void OnRewardPlayer()
{
    Player.Coins += CoinsEarned * RewardMultiplier;
}

// The player dismissed the rewarded ad before finishing and should not be rewarded.
Wortal.RewardSkipped += OnRewardSkipped;
void OnRewardSkipped()
{
    Player.Coins += CoinsEarned;
}
```

### Analytics

The analytics API can track game events to help you understand better how players are interacting with your game.

```c#
// Logs the start of a level and begins ticking the level timer.
LogLevelStart(level);

// Logs the end of a level. If the level name is the same as the previous call to LogLevelStart() then the event
// will include the time taken to finish the level.
LogLevelEnd(level, score);

LogLevelUp(level);
LogScore(score);

// Log the player’s choice when offered different options.
// This can be useful for determining which characters are more popular, or paths are more commonly taken, etc.
// This can be a powerful tool for balancing the game and giving the players more of what they enjoy.
LogGameChoice(decision, choice);
```
