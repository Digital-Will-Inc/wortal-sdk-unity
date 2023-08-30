# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

## [5.6.3] - 2023-08-30
### Fixed
- Core
  - IsInitialized calls a function when it should check a property

## [5.6.2] - 2023-08-29
### Fixed
- Instance initializes before SDK Core when loaded from cache

## [5.6.1] - 2023-08-29
### Fixed
- Core
  - Typo in instance.js that prevented initializeAsync auto call

## [5.6.0] - 2023-08-24
### Added
- Core
  - Tournament API
  - Manual initialization option
- Ads
  - IsAdBlocked API
- Session
  - GetDevice, GetOrientation, OnOrientationChange, SwitchGame APIs

### Fixed
- Core
  - Newtonsoft JSON dependency is now included
- Notifications
  - Template is now wrapped in compiler directives to prevent errors when Localization package is not present

### Changed
- Core
  - Upgraded to SDK Core v1.6.0
  - SDK is now distributed as a UPM package via git URL

## [5.5.2] - 2023-07-07
### Fixed
- Notifications
  - CancelAll uses wrong API call in js lib
  - Cancel and CancelAll don't return success status

## [5.5.1] - 2023-06-30
### Added
- Core
  - GetErrorCode for getting the parsed enum from a WortalError

### Fixed
- Core
  - Removed leftover debug logs

### Changed
- Core
  - Removed duplicate SDK script loading prevention in index.html as can cause issues on Facebook

## [5.5.0] - 2023-06-28
### Added
- Core
  - Notifications API
  - Haptic feedback
  - GetSupportedAPIs for checking which APIs are available on the current platform
- Analytics
  - New events: LogPurchase, LogPurchaseSubscription, LogSocialInvite, LogSocialShare
- Context
  - InviteAsync API
  - New payload types and templates

### Fixed
- Context
  - GetPlayers missing callback
- Leaderboard
  - GetEntryCount using wrong callback

### Changed
- Core
  - Upgraded to SDK Core v1.5.0
  - WortalErrorCodes are now an enum
  - Organized project structure via modules

## [5.4.0] - 2023-05-25
### Added
- Context Payload templates
- Linker to prevent code stripping of Wortal classes

### Changed
- Context types are now enums
- Error codes are now enums
- Wortal modules now constructed on Wortal creation instead of on Initialization

## [5.3.1] - 2023-04-26
### Fixed
- Installer uses new API that is only supported by Unity 2022.1+

## [5.3.0] - 2023-04-26
### Added
- Ads on Viber
- Group chat context for FB Instant Games
- Game ID parsing for FB Instant Games
- Converter util for FB leaderboards
- Ads API: noFill callback for ad calls
- Context APIs: getType, getPlayersAsync, shareLinkAsync, isSizeBetween
- Player API: flushDataAsync, getASIDAsync, getSignedASIDAsync, canSubscribeBotAsync, subscribeBotAsync
- Session API: getPlatform

### Changed
- Context and Player APIs now have optional null return values to match platform SDKs
- Improved docs
- Improved error handling

## [5.2.0] - 2023-02-21
### Added
- Support for FB Instant Games
- onPause callback

### Fixed
- Possible type mismatch for ad unit IDs

## [5.1.1] - 2022-12-27
### Added
- Mock calls for testing in editor
- Loading progress percentage in splash screen
- Loading progress text in splash screen

### Fixed
- Incorrect logging in installer checks for compression

## [5.1.0] - 2022-12-06
### Added
- Support for Game Distribution platform
- Support for compressed builds

### Fixed
- Typos in documentation

### Changed
- Upgraded to SDK Core v1.2.0

## [5.0.0] - 2022-11-28
### Breaking Change
- API access is now by module (Wortal.Ads, Wortal.Analytics)

### Added
- Context API
- In-App Purchase API
- Leaderboard API
- Player API
- Session API
- Example scene
- Examples in documentation

### Changed
- Extension now uses Wortal SDK Core
- Removed Wortal module from WASM build
- Removed Ad callback events, callbacks are now passed in directly
- WortalInstaller now warns for incorrect settings on load

## [4.2.1] - 2022-10-04
### Added
- Support for ad blockers

## [4.2.0] - 2022-09-29
### Added
- Analytics API

### Fixed
- No callbacks if Link AdUnitID missing
- Type being passed into showRewardedAd
- Possible type error when setting load progress bar

### Changed
- Moved logic and data to JS
- Consolidated JS scripts
- Renamed reward events for clarity
- Removed platform specific ad implementations
- Removed unused delegates and callbacks
- Removed Localization helpers
- Removed OpenLink API for security reasons
- Removed WortalSettings

## [4.1.0] - 2022-08-17
### Added
- Viber Play support
- Platform check on load
- Fetch Link Ad Unit IDs on load
- Support for Unity 2021.2+
- Runtime script loading for different platforms
- Brazilian Portuguese locale detection
- Option to disable language check

### Fixed
- Missing ad unit IDs in Link ad calls
- Typo in wortal-init-link

### Changed
- Consolidated pointers and delegates into globals instead of platform specific
- Refactored stylesheet for flexibility
- AdDone event is now AfterAd for consistency across the SDK
- AdTimedOut event has been removed
- Rewarded Ad events names shortened
- Removed several less common languages/locales

## [4.0.0] - 2022-07-07
### Added
- Rakuten Games Link support
- Assembly definition for Wortal
- Wortal WebGL template
- Wortal settings
- Improved localization support

### Changed
- All calls are now accessed via the Wortal class
- Ad calls are made via the Wortal SDK
- OpenLink is now internal only
- All JS scripts have been moved to external files for easier maintenance
- Installer now only sets project settings
- LanguageCode event and property have been changed to Language
- Updated README with up to date code examples

### Fixed
- Vertical scrollbar no longer shows
- Game no longer shows before preroll ad finishes
- Simultaneous AdBreakDone and NoShow callbacks no longer cause duplicate events

## [3.0.6] - 2022-04-14
### Fixed
- Missing OpenLink function

## [3.0.5] - 2022-04-14
### Fixed
- LanguageCode not parsed correctly

## [3.0.4] - 2022-04-14
### Fixed
- LanguageCodeSet event never fires

## [3.0.3] - 2022-04-13
### Fixed
- Wortal and AdSense not reachable

## [3.0.2] - 2022-04-13
### Added
- Assembly definition for Editor script

## [3.0.1] - 2022-04-13
### Fixed
- Missing WebGL template path

## [3.0.0] - 2022-04-13
### Added
- Installer that sets required project settings automatically

### Changed
- Plugin is now a UPM package
- Removed unnecessary features
- LanguageCode is now a string instead of an enum

## [2.0.0] - 2021-12-10

### Added

- Modified Soup2 package
- Integrated AFG plugin
- OpenLink function for opening links from in-game
- Custom editor window for settings
- Built-in Japanese font support
- Font Atlas Builder for populating dynamic TMP_FontAssets
- Loading progress bar for WebGL instance
- Preroll ads

### Changed

- Wortal class is replaced with SoupComponent
- Logging is handled by SoupLog
- Configuration is done in editor window instead of prefab inspector
- Updated style.css for canvas sizing on wortal

### Fixed

- Possible null reference if core prefab missing

## [1.1.0] - 2021-10-21

### Added

- AdSense wrapper class
- Error handling for AdSense
- Wortal prefab with auto-launcher

## [1.0.1] - 2021-10-14

### Added

- Editor support for testing
- IsLanguageCodeSet property
- Static access to event and properties

## [1.0.0] - 2021-10-11

Initial release.
