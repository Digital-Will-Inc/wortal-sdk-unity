# Wortal Plugin

Unity plugin for WebGL games deployed to the Wortal. `Wortal prefab` is automatically instantiated on load and handles
most of the plugin functionality.

### AdSense

`AdSense.cs` implements the ad calls and callbacks for the `AFG Plugin for Unity`. You should code your callback
logic in this class and make any calls for showing ads through this class also.

You should subscribe to the `Wortal.AdTimeout` event wherever you are calling for ads. This will trigger if we have
received an error status from AdSense and cannot show ads. 

This guards against an infinite wait where the logic
in the `AdSense.cs` callbacks never gets reached because the browser or a plugin has blocked AdSense from sending ads.

```c#
Wortal.AdTimeout += OnAdTimeout;

private void OnAdTimeout()
{
    // Ad failed to load so we should skip ahead.
    LoadNextLevel();
}
```

### Localization

Gets the user's preferred browser language and parses it to a `LanguageCode` enum of the 2-letter ISO code to be used with localization services.
