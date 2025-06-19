using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalAds : IWortalAds
    {
        private static Action _beforeAdCallback;
        private static Action _afterAdCallback;
        private static Action _adDismissedCallback;
        private static Action _adViewedCallback;
        private static Action _noFillCallback;
        private static Action _successCallback;
        private static Action<WortalError> _errorCallback;

        public bool IsSupported => true;

        public void ShowInterstitial(Placement placement, Action onSuccess, Action<WortalError> onError)
        {
            _successCallback = onSuccess;
            _errorCallback = onError;

            // Set up default callbacks for the ad flow
            _beforeAdCallback = () => { }; // Can be extended if needed
            _afterAdCallback = () => _successCallback?.Invoke();
            _noFillCallback = () => _successCallback?.Invoke(); // Treat no fill as success

#if UNITY_WEBGL && !UNITY_EDITOR
            ShowInterstitialJS(
                placement.ToString().ToLower(),
                placement.ToString(), // Use placement as description
                BeforeAdCallback,
                AfterAdCallback,
                NoFillCallback);
#else
            Debug.Log($"[WebGL Platform] Mock Ads.ShowInterstitial({placement})");
            BeforeAdCallback();
            AfterAdCallback();
#endif
        }

        public void ShowRewarded(Placement placement, Action onSuccess, Action<WortalError> onError)
        {
            _successCallback = onSuccess;
            _errorCallback = onError;

            // Set up callbacks for rewarded ad flow
            _beforeAdCallback = () => { }; // Can be extended if needed
            _afterAdCallback = () => { }; // Don't call success here, wait for viewed/dismissed
            _adDismissedCallback = () =>
            {
                // Reward dismissed - this could be treated as an error or success depending on your needs
                _errorCallback?.Invoke(new WortalError
                {
                    Code = "AD_DISMISSED",
                    Message = "Rewarded ad was dismissed by user",
                    Context = "ShowRewarded operation"
                });
            };
            _adViewedCallback = () => _successCallback?.Invoke(); // Reward granted
            _noFillCallback = () =>
            {
                _errorCallback?.Invoke(new WortalError
                {
                    Code = "NO_FILL",
                    Message = "No rewarded ad available",
                    Context = "ShowRewarded operation"
                });
            };

#if UNITY_WEBGL && !UNITY_EDITOR
            ShowRewardedJS(
                placement.ToString(), // Use placement as description
                BeforeAdCallback,
                AfterAdCallback,
                AdDismissedCallback,
                AdViewedCallback,
                NoFillCallback);
#else
            Debug.Log($"[WebGL Platform] Mock Ads.ShowRewarded({placement})");
            BeforeAdCallback();
            AfterAdCallback();
            // For testing: simulate viewed callback
            AdViewedCallback();
#endif
        }

        public void ShowBanner(bool shouldShow, BannerPosition position, Action onSuccess, Action<WortalError> onError)
        {
            _successCallback = onSuccess;
            _errorCallback = onError;

#if UNITY_WEBGL && !UNITY_EDITOR
            ShowBannerJS(shouldShow, position.ToString().ToLower());
            // Banner operations are typically fire-and-forget, so we call success immediately
            _successCallback?.Invoke();
#else
            Debug.Log($"[WebGL Platform] Mock Ads.ShowBanner({shouldShow}, {position})");
            _successCallback?.Invoke();
#endif
        }

        public bool IsAdBlocked()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return IsAdBlockedJS();
#else
            Debug.Log("[WebGL Platform] Mock Ads.IsAdBlocked()");
            return false; // Return false for mock to allow testing
#endif
        }

        public bool IsEnabled()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return IsEnabledJS();
#else
            Debug.Log("[WebGL Platform] Mock Ads.IsEnabled()");
            return true; // Return true for mock to allow testing
#endif
        }

        #region JSlib Interface

        [DllImport("__Internal")]
        private static extern bool IsAdBlockedJS();

        [DllImport("__Internal")]
        private static extern bool IsEnabledJS();

        [DllImport("__Internal")]
        private static extern void ShowInterstitialJS(string type,
                                                      string description,
                                                      Action beforeAdCallback,
                                                      Action afterAdCallback,
                                                      Action noFillCallback);

        [DllImport("__Internal")]
        private static extern void ShowRewardedJS(string description,
                                                  Action beforeAdCallback,
                                                  Action afterAdCallback,
                                                  Action adDismissedCallback,
                                                  Action adViewedCallback,
                                                  Action noFillCallback);

        [DllImport("__Internal")]
        private static extern void ShowBannerJS(bool shouldShow, string position);

        [MonoPInvokeCallback(typeof(Action))]
        private static void BeforeAdCallback()
        {
            _beforeAdCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void AfterAdCallback()
        {
            _afterAdCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void AdDismissedCallback()
        {
            _adDismissedCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void AdViewedCallback()
        {
            _adViewedCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(Action))]
        private static void NoFillCallback()
        {
            _noFillCallback?.Invoke();
        }

        #endregion JSlib Interface
    }
}