# Change Log
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/)
and this project adheres to [Semantic Versioning](http://semver.org/).

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
