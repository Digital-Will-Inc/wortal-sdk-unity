# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

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
