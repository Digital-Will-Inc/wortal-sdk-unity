# Wortal SDK Native Platform Integration Guide

This Wortal SDK package has been structured to compile for multiple platforms (WebGL, Android, iOS, Standalone). However, please note the following:

## Current Functionality

*   **WebGL:** Full functionality is provided for the WebGL platform, utilizing the JavaScript libraries (`.jslib` files) included in this directory.
*   **Other Platforms (Android, iOS, Standalone, etc.):** The C# API methods for these platforms are currently **placeholders**. They will compile without error, but they do not implement actual native SDK functionality. Calling them will typically result in a `Debug.LogWarning` indicating that the feature is not yet implemented for that platform.

## Adding Native Functionality

To enable Wortal SDK features on platforms other than WebGL, you will need to:

1.  **Obtain Native Libraries:** Acquire the platform-specific Wortal SDK libraries (e.g., `.aar` for Android, `.framework` for iOS, `.dll`/`.so`/`.dylib` for Standalone) from Wortal. These are not included in this package.
2.  **Place Native Libraries:**
    *   Android: `Plugins/Android/`
    *   iOS: `Plugins/iOS/`
    *   Windows: `Plugins/Standalone/Windows/`
    *   macOS: `Plugins/Standalone/macOS/`
    *   Linux: `Plugins/Standalone/Linux/`
    *   Nintendo Switch: `Plugins/Switch/` (or as per Nintendo's guidelines)
3.  **Implement C# Bridge Code:** You will need to write or integrate C# wrapper code that calls the functions within these native libraries. This typically involves using:
    *   `UnityEngine.AndroidJavaObject` / `UnityEngine.AndroidJavaClass` for Android (JNI).
    *   `[DllImport("__Internal")]` for iOS (P/Invoke).
    *   `[DllImport("NativeLibName")]` for Standalone platforms (P/Invoke).
    The existing C# API methods in the `DigitalWill.WortalSDK` namespace have been structured with conditional compilation blocks (`#if UNITY_ANDROID`, `#if UNITY_IOS`, etc.) where this bridge code should be implemented.

This setup allows you to maintain a single codebase while progressively adding native support as required.
