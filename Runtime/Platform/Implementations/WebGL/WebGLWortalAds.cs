using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalAds : IWortalAds
    {
        public bool IsSupported => false;

        public void ShowInterstitial(Placement placement, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalAds.ShowInterstitial({placement}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "ShowInterstitial implementation"
            });
        }

        public void ShowRewarded(Placement placement, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalAds.ShowRewarded({placement}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "ShowRewarded implementation"
            });
        }

        public void ShowBanner(bool shouldShow, BannerPosition position, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[WebGL Platform] IWortalAds.ShowBanner({shouldShow}, {position}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on WebGL platform",
                Context = "ShowBanner implementation"
            });
        }

        public bool IsAdBlocked()
        {
            Debug.Log("[WebGL Platform] IWortalAds.IsAdBlocked() called - Not implemented");
            return false;
        }

        public bool IsEnabled()
        {
            Debug.Log("[WebGL Platform] IWortalAds.IsEnabled() called - Not implemented");
            return false;
        }
    }
}