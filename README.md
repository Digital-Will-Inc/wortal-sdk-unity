# Wortal SDK

## Installation

- Install the <b>Wortal SDK</b> Unity package.
- Set the necessary project settings via toolbar: `DigitalWill/Wortal/Set Project Settings`

## How to Use

### Ads

[API Reference](https://sdk.html5gameportal.com/api/ads/)

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
Wortal.Ads.ShowRewarded("BonusCoins",
    () => PauseGame(),
    () => ResumeGame(),
    () => DontReward(),
    () => RewardPlayer());

// This example shows the game flow depending on the outcome of the ad.
Wortal.Ads.ShowRewarded("ReviveAndContinue",
    () => PauseGame(),
    () => ResumeAudio(),
    () => EndGame(),
    () => RevivePlayerAndContinue());
```

**NOTE**: Players should only be rewarded in the `AdViewed` callback.

### Analytics

[API Reference](https://sdk.html5gameportal.com/api/analytics/)

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

### Context

[API Reference](https://sdk.html5gameportal.com/api/context/)

The Context API is used to connect players and allow them to interact in the game session, share their content
and send messages to each other.

```csharp
// Invite a friend to play the game.
Wortal.Context.ChooseAsync(payload,
    () => Debug.Log("New context: " + Wortal.Context.GetID()),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));

// Share your game activity with friends.
Wortal.Context.ShareAsync(payload,
    shareResult => Debug.Log("Number of shares: " + shareResult),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));

// Example ContextPayload
var payload = new ContextPayload
{
    Image = "dataURLToBase64Image",
    Text = new LocalizableContent
    {
        Default = "Play",
        Localizations = new Dictionary<string, string>
        {
            {"en_US", "Play"},
            {"ja_JP", "プレイ"},
        },
    },
    Data = new Dictionary<string, object>
    {
        {"current_level", 1},
    },
};
```

#### Context Payload Templates

To make it easier to create and manage payloads for use with the Context API, you can create `ContextPayloadTemplates`
from the `Wortal/Context Payload Template` menu. These templates are ScriptableObjects that can be easily designed and
modified in the Unity Editor without writing any code. Some things to consider when using the template:

- The templates utilize Unity's Localization system for passing `LocalizableContent`, simply selecting a key in the
template will automatically populate the payload with all available locales. If your project does not
include the Localization package, the template script will not be compiled and will not show up in your project.
- Images selected in the template will be converted to a base64 encoded string and added to the payload, this requires
the image to be uncompressed and have `Read/Write` enabled.
- Values left default or `NONE` will be ignored and not passed into the payload object.
- You can convert the template to a `ContextPayload` object to use with the Context API via `ContextPayloadTemplate.GetPayload()`.

![payload-template.jpg](Docs/Images/payload-template.jpg)

### In-App Purchases

[API Reference](https://sdk.html5gameportal.com/api/iap/)

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

[API Reference](https://sdk.html5gameportal.com/api/leaderboard/)

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

### Player

[API Reference](https://sdk.html5gameportal.com/api/player/)

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

[API Reference](https://sdk.html5gameportal.com/api/session/)

Details about the current session can be accessed in the Session API.

```csharp
// Get the entry point of where the game started from.
Wortal.Session.GetEntryPoint(
    entryPoint => Debug.Log(entryPoint),
    error => Debug.Log("Error Code: " + error.Code + "\nError: " + error.Message));
```
