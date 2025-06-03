using System;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class iOSWortalAds : IWortalAds
    {
        public bool IsSupported => false;

        public void ShowInterstitial(Placement placement, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalAds.ShowInterstitial({placement}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "ShowInterstitial implementation"
            });
        }

        public void ShowRewarded(Placement placement, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalAds.ShowRewarded({placement}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "ShowRewarded implementation"
            });
        }

        public void ShowBanner(bool shouldShow, BannerPosition position, Action onSuccess, Action<WortalError> onError)
        {
            Debug.Log($"[iOS Platform] IWortalAds.ShowBanner({shouldShow}, {position}) called - Not implemented");
            onError?.Invoke(new WortalError
            {
                Code = "NOT_IMPLEMENTED",
                Message = "Not implemented on iOS platform",
                Context = "ShowBanner implementation"
            });
        }

        public bool IsAdBlocked()
        {
            Debug.Log("[iOS Platform] IWortalAds.IsAdBlocked() called - Not implemented");
            return false;
        }

        public bool IsEnabled()
        {
            Debug.Log("[iOS Platform] IWortalAds.IsEnabled() called - Not implemented");
            return false;
        }
    }
}