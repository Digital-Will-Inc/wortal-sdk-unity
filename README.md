# Wortal Plugin

Unity plugin for WebGL games deployed to the Wortal.

### Install and Setup

- Import the `WortalPlugin` package.
- Go to `DigitalWill/Soup Settings` in the Unity toolbar and configure what you need.
- Go to `Player Settings` and change the following:
    1. Resolution -> `Run In Background`
    2. WebGL Template -> `AdSense`
    3. Publishing Settings -> `Compression: Disabled`
- Use the `Preload` scene provided, or add `Bootstrapper` prefab to your empty scene at `buildIndex 0`.
    
### AdSense

`AdSense.cs` implements the ad calls and callbacks for the `AFG Plugin for Unity`. You should code your callback
logic in this class and make any calls for showing ads through this class also.

You should subscribe to the `AdTimeout` event wherever you are calling for ads. This will trigger if we have
received an error status from AdSense and cannot show ads.

This guards against an infinite wait where the logic
in the `AdSense.cs` callbacks never gets reached because the browser or a plugin has blocked AdSense from sending ads.

```c#
SoupComponent.I.AdTimeout += OnAdTimeout;

private void OnAdTimeout()
{
    // Ad failed to load so we should skip ahead.
    LoadNextLevel();
}
```

### Localization

Gets the user's preferred browser language and parses it to a `LanguageCode` enum of the 2-letter ISO code to be used with localization services.

There is a Japanese font and accompanying dynamic font asset with it. This asset needs to be populated with your localized text to ensure
all the characters are present in the atlas.

The easiest way to do this is to add the `FontAtlasBuilder` prefab to a scene after your localization service has been 
initialized. The `FontAtlasBuilder` will loop through the designated language's dictionary and add all characters required
to the font atlas. <ins>**Make sure to remove the `FontAtlasBuilder` prefab from the scene after using it.**</ins>
