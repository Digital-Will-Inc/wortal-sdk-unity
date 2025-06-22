using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
        public class WebGLWortalAnalytics : IWortalAnalytics
        {
                public bool IsSupported => true; // Changed to true since we're implementing the functionality

                public void LogLevelStart(string level)
                {
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LogLevelStartJS(level);
#else
                        Debug.Log($"[Wortal] Mock Analytics.LogLevelStart({level})");
#endif
                }

                public void LogLevelEnd(string level, string score, bool wasCompleted)
                {
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LogLevelEndJS(level, score, wasCompleted);
#else
                        Debug.Log($"[Wortal] Mock Analytics.LogLevelEnd({level}, {score}, {wasCompleted})");
#endif
                }

                public void LogLevelUp(int level)
                {
                        // Convert int to string to match the original implementation
                        string levelString = level.ToString();
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LogLevelUpJS(levelString);
#else
                        Debug.Log($"[Wortal] Mock Analytics.LogLevelUp({levelString})");
#endif
                }

                public void LogScore(int score)
                {
                        // Convert int to string to match the original implementation
                        string scoreString = score.ToString();
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LogScoreJS(scoreString);
#else
                        Debug.Log($"[Wortal] Mock Analytics.LogScore({scoreString})");
#endif
                }

                public void LogTutorialStart()
                {
                        // Use a default tutorial name since the interface doesn't take parameters
                        LogTutorialStart("Tutorial");
                }

                public void LogTutorialStart(string tutorial)
                {
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LogTutorialStartJS(tutorial);
#else
                        Debug.Log($"[Wortal] Mock Analytics.LogTutorialStart({tutorial})");
#endif
                }

                public void LogTutorialEnd()
                {
                        // Use a default tutorial name and assume completion
                        LogTutorialEnd("Tutorial", true);
                }

                public void LogTutorialEnd(string tutorial, bool wasCompleted)
                {
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LogTutorialEndJS(tutorial, wasCompleted);
#else
                        Debug.Log($"[Wortal] Mock Analytics.LogTutorialEnd({tutorial}, {wasCompleted})");
#endif
                }

                public void LogGameChoice(string decision, string choice)
                {
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LogGameChoiceJS(decision, choice);
#else
                        Debug.Log($"[Wortal] Mock Analytics.LogGameChoice({decision}, {choice})");
#endif
                }

                public void LogSocialInvite(string placement)
                {
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LogSocialInviteJS(placement);
#else
                        Debug.Log($"[Wortal] Mock Analytics.LogSocialInvite({placement})");
#endif
                }

                public void LogSocialShare(string placement)
                {
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LogSocialShareJS(placement);
#else
                        Debug.Log($"[Wortal] Mock Analytics.LogSocialShare({placement})");
#endif
                }

                public void LogPurchase(string productID, string details)
                {
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LogPurchaseJS(productID, details);
#else
                        Debug.Log($"[Wortal] Mock Analytics.LogPurchase({productID} / {details})");
#endif
                }

                public void LogPurchaseSubscription(string productID, string details)
                {
#if UNITY_WEBGL && !UNITY_EDITOR
            PluginManager.LogPurchaseSubscriptionJS(productID, details);
#else
                        Debug.Log($"[Wortal] Mock Analytics.LogPurchaseSubscription({productID} / {details})");
#endif
                }

                public void LogCustomEvent(string eventName, Dictionary<string, object> parameters)
                {
                        // Since the original Wortal SDK doesn't have a custom event method,
                        // we can implement this as a game choice or just log it
                        string paramString = parameters != null ? string.Join(", ", parameters) : "null";
#if UNITY_WEBGL && !UNITY_EDITOR
            // mow temporary implement it as a custom JS function for this, or use LogGameChoice as a fallback
            PluginManager.LogGameChoiceJS("CustomEvent", $"{eventName}:{paramString}");
#else
                        Debug.Log($"[Wortal] Mock Analytics.LogCustomEvent({eventName}, {paramString})");
#endif
                }

        }
}
