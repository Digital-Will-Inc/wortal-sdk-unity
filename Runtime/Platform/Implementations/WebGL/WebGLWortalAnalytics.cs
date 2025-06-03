using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
    public class WebGLWortalAnalytics : IWortalAnalytics
    {
        public bool IsSupported => false;

        public void LogLevelStart(string level)
        {
            Debug.Log($"[WebGL Platform] IWortalAnalytics.LogLevelStart({level}) called - Not implemented");
            // Could still log to Unity Analytics or other services
        }

        public void LogLevelEnd(string level, string score, bool wasCompleted)
        {
            Debug.Log($"[WebGL Platform] IWortalAnalytics.LogLevelEnd({level}, {score}, {wasCompleted}) called - Not implemented");
        }

        public void LogLevelUp(int level)
        {
            Debug.Log($"[WebGL Platform] IWortalAnalytics.LogLevelUp({level}) called - Not implemented");
        }

        public void LogScore(int score)
        {
            Debug.Log($"[WebGL Platform] IWortalAnalytics.LogScore({score}) called - Not implemented");
        }

        public void LogTutorialStart()
        {
            Debug.Log("[WebGL Platform] IWortalAnalytics.LogTutorialStart() called - Not implemented");
        }

        public void LogTutorialEnd()
        {
            Debug.Log("[WebGL Platform] IWortalAnalytics.LogTutorialEnd() called - Not implemented");
        }

        public void LogGameChoice(string decision, string choice)
        {
            Debug.Log($"[WebGL Platform] IWortalAnalytics.LogGameChoice({decision}, {choice}) called - Not implemented");
        }

        public void LogSocialInvite(string placement)
        {
            Debug.Log($"[WebGL Platform] IWortalAnalytics.LogSocialInvite({placement}) called - Not implemented");
        }

        public void LogSocialShare(string placement)
        {
            Debug.Log($"[WebGL Platform] IWortalAnalytics.LogSocialShare({placement}) called - Not implemented");
        }

        public void LogPurchase(string productID, string details)
        {
            Debug.Log($"[WebGL Platform] IWortalAnalytics.LogPurchase({productID}, {details}) called - Not implemented");
        }

        public void LogPurchaseSubscription(string productID, string details)
        {
            Debug.Log($"[WebGL Platform] IWortalAnalytics.LogPurchaseSubscription({productID}, {details}) called - Not implemented");
        }

        public void LogCustomEvent(string eventName, Dictionary<string, object> parameters)
        {
            Debug.Log($"[WebGL Platform] IWortalAnalytics.LogCustomEvent({eventName}, {parameters?.Count} params) called - Not implemented");
        }
    }
}