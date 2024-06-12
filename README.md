# Wortal SDK

## Installation

1. Install the <b>Wortal SDK</b> Unity package via git URL:

`https://github.com/Digital-Will-Inc/wortal-sdk-unity.git`

2. Install the Wortal WebGL template via toolbar: `Wortal/Install WebGL Template`

3. Set the necessary project settings via toolbar: `Wortal/Set Project Settings`

## Updating

To update the Wortal SDK, simply open the Package Manager, select the Wortal package, and click the `Update` button.

The WebGL build may change between versions so it is recommended to re-install the WebGL template after updating the package. Delete the `Assets/WebGLTemplates/Wortal` folder and re-install the template via the toolbar `Wortal/Install WebGL Template`.

## How to Use

### Ads

[API Reference](https://docs.games-api.ai/api/ads/)

Interstitial ads can be shown at various points in the game such as a level end, restart or a timed
interval in games with longer levels.

```csharp
// Player reached the next level.
Wortal.Ads.ShowInterstitial(Placement.Next, "NextLevel",
    () => PauseGame(),
    () => ResumeGame());

// Player paused the game.
Wortal.Ads.ShowInterstitial(Placement.Pause, "PausedGame",
    () => PauseGame(),
    () => ResumeGame());

// Player opened the IAP shop.
Wortal.Ads.ShowInterstitial(Placement.Browse, "BrowseShop",
    () => PauseGame(),
    () => ResumeGame());
```

Rewarded ads can be shown too. These are longer, optional ads that the player can earn a reward for watching. The player
must be notified of the ad and give permission to show before it can be shown.

```csharp
// This example shows the game flow independent of the outcome of the ad.
// Ex: Player gets bonus coins for watching the ad, but the game continues regardless of the outcome.
Wortal.Ads.ShowRewarded("BonusCoins",
    () => PauseGame(),
    () => ResumeGame(),
    () => DontReward(),
    () => RewardPlayer());

// This example shows the game flow depending on the outcome of the ad.
// Ex: Player dies and can revive by watching an ad, but if they skip the ad they lose the level.
Wortal.Ads.ShowRewarded("ReviveAndContinue",
    () => PauseGame(),
    () => ResumeAudio(),
    () => EndGame(),
    () => RevivePlayerAndContinue());
```

**NOTE**: Players should only be rewarded in the `AdViewed` callback.

### Analytics

[API Reference](https://docs.games-api.ai/api/analytics/)

The Analytics API can be used to track game events that can help better understand how players are interacting with
the game. This data will be available for viewing in the Wortal dashboard.

```csharp
// Logs the start of the level.
Wortal.Analytics.LogLevelStart("Level 3");

// Logs the end of the level. Will track the time spent playing the level if the name matches
// the name of the last logLevelStart() call.
Wortal.Analytics.LogLevelEnd("Level 3", "100", true);

// Logs a choice the player made in the game. This can be useful for balancing the game
// and seeing what content your players interact with the most.
Wortal.Analytics.LogGameChoice("Character", "Blue");
```

### Achievements

[API Reference](https://docs.games-api.ai/api/achievements/)

The Achievements API is used to track player progress in the game. This can be useful for rewarding players
for completing certain tasks or for tracking player progress.

```csharp
// Unlock an achievement.
Wortal.Achievements.UnlockAchievement("achievementID",
        success => Debug.Log("Achievement Unlocked: " + success),
        error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));

// Get the player's progress on an achievement.
Wortal.Achievements.GetAchievements(achievements =>
    {
        foreach (Achievement achievement in achievements)
        {
            Debug.Log(achievement.Name);
        }
    },
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
```

### Context

[API Reference](https://docs.games-api.ai/api/context/)

The Context API is used to connect players and allow them to interact in the game session, share their content
and send messages to each other.

```csharp
// Invite a friend to play the game. Does not switch the player's current context.
Wortal.Context.Invite(invitePayload,
    () => Debug.Log("Invite sent"),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));

// Share your game activity with friends.
Wortal.Context.Share(sharePayload,
    shareResult => Debug.Log("Number of shares: " + shareResult),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
```

#### Context Payload Templates

To make it easier to create and manage payloads for use with the Context API, you can create templates
from the `Wortal` menu. These templates are ScriptableObjects that can be easily designed and
modified in the Unity Editor without writing any code. See [this discussion](https://github.com/Digital-Will-Inc/wortal-sdk-unity/discussions/137)
for more details and limitations of the templates.

### In-App Purchases

[API Reference](https://docs.games-api.ai/api/iap/)

The In-App Purchases (IAP) API is used to provide an interface for in-game transactions on the platforms.
This process will differ based on what platform the game is being played on, but the API remains the same.

```csharp
// Get the catalog of products the player can purchase.
Wortal.IAP.GetCatalog(
    catalog => Debug.Log(catalog[0].Title),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));

// Purchase a product.
Wortal.IAP.MakePurchase(new WortalIAP.PurchaseConfig
    {
        ProductID = "id.code.for.product",
    },
    purchase => Debug.Log(purchase.PurchaseToken), // Use this token to consume purchase
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
```

### Leaderboards

[API Reference](https://docs.games-api.ai/api/leaderboard/)

The Leaderboard API gives the game access to the platform's leaderboard functionality. This is where
you can track player's scores and compare them to other players.

```csharp
// Get the top 10 entries on the global leaderboard.
Wortal.Leaderboard.GetEntries("global", 10,
    entries => Debug.Log("Score: " + entries[0].Score),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));

// Add the player's score to the leaderboard.
Wortal.Leaderboard.SendEntry("global", 100,
    entry => Debug.Log("Score: " + entry.Score),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
```

### Notifications

[API Reference](https://docs.games-api.ai/api/notifications/)

The notifications API is used to send notifications to the player. This can be useful for letting the player
know when they have received a gift or when they haven't played in a while.

```csharp
// Send a notification to the player.
Wortal.Notifications.Schedule(payload,
    result => Debug.Log("Notification sent: " + result.ID),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));

// Cancel a notification.
Wortal.Notifications.Cancel(notificationID,
    () => Debug.Log("Notification cancelled"),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
```

#### Notification Payload Templates

To make it easier to create and manage payloads for use with the Notifications API, you can create templates
from the `Wortal` menu. These templates are ScriptableObjects that can be easily designed and
modified in the Unity Editor without writing any code. See [this discussion](https://github.com/Digital-Will-Inc/wortal-sdk-unity/discussions/137)
for more details and limitations of the templates.

### Player

[API Reference](https://docs.games-api.ai/api/player/)

You can find details about the current player via the Player API.

```csharp
// Get a list of the player's friends who have also played this game.
Wortal.Player.GetConnectedPlayers(payload,
    players => Debug.Log(players[0].GetName()),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));

// Save the player's data to the platform
Dictionary<string, object> data = new()
{
    { "items", new Dictionary<string, int>
        {
            { "coins", 100 },
            { "boosters", 2 },
        }
    },
    { "lives", 3 },
};
Wortal.Player.SetData(data,
    () => Debug.Log("Data set"),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));

// Get player's save data from the platform
Wortal.Player.GetData(new[] { "items", "lives" },
    data =>
    {
        // Check the return values types before casting or operating on them.
        foreach (KeyValuePair<string, object> kvp in data)
        {
            Debug.Log("Key name: " + kvp.Key);
            Debug.Log("Value type: " + kvp.Value.GetType());
        }

        // Nested objects should de-serialize as IDictionary<string, object>
        var items = (Dictionary<string, object>)data["items"];
        Debug.Log("Coins: " + items["coins"]);
    },
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
```

### Session

[API Reference](https://docs.games-api.ai/api/session/)

Details about the current session can be accessed in the Session API.

```csharp
// Get the entry point of where the game started from.
Wortal.Session.GetEntryPoint(
    entryPoint => Debug.Log(entryPoint),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));

// Get the entry point data from a social invite or share.
// This is useful for tracking where players are coming from or
// providing a reward for players who were invited to play.
var entryPointData = Wortal.Session.GetEntryPointData();
foreach (KeyValuePair<string, object> kvp in data)
{
    // Do stuff with the data.
}
```

### Stats

[API Reference](https://docs.games-api.ai/api/stats/)

The Stats API is used to track player statistics in the game. This can be useful for tracking player progress
and balancing the game.

```csharp
// Get a player's stats.
Wortal.Stats.GetStats(
    "Level 1",
    new GetStatsPayload { Period = StatPeriod.ALLTIME },
    stats =>
    {
        foreach (Stats stat in stats)
        {
            Debug.Log(stat.Name);
        }
    },
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));

// Update a player's stats.
Wortal.Stats.PostStats(
    "Level 1",
    100,
    new PostStatsPayload { Period = StatPeriod.DAILY },
    allback => Debug.Log("Stats posted!"),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
};
```

### Tournament

[API Reference](https://docs.games-api.ai/api/tournament/)

The Tournament API is used to create and manage tournaments for your game.

```csharp
// Create a tournament.
Wortal.Tournament.Create(createPayload,
    tournament => Debug.Log("Tournament ID: " + tournament.ID),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));

// Post a score to a tournament.
Wortal.Tournament.PostScore(200,
    () => Debug.Log("Score posted!"),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
```
