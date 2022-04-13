# Wortal Plugin

Unity plugin for WebGL games deployed to the Wortal.

### Install and Setup

- Add the [DW UPM registry](https://github.com/Digital-Will-Inc/dw-upm-server) to your package manifest.
- Install the Wortal Plugin package.

### AdSense

`AdSense.cs` implements the ad calls and callbacks for the `AFG Plugin for Unity`. You should code your callback
logic in this class and make any calls for showing ads through this class also.

You should subscribe to the `AdTimeout` event wherever you are calling for ads. This will trigger if we have
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

Games on the Wortal need to be localized. The Wortal plugin detects the user's preferred browser language and
sets the property `LanguageCode` with the 2-letter ISO code for the language detected. You can use this to
determine what language to localize the game into.
