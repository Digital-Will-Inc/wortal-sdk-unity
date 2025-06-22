using System.Runtime.InteropServices;
using UnityEngine;

namespace DigitalWill.WortalSDK
{
        public static class PluginManager
        {
                // Only declare DllImport for WebGL platform
#if UNITY_WEBGL && !UNITY_EDITOR
        // Achievements
        [DllImport("__Internal")]
        private static extern void AchievementsGetAchievementsJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void AchievementsUnlockAchievementJS(string achievementID, Action<bool> callback, Action<string> errorCallback);

        // Ads
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

        // Analytics
        [DllImport("__Internal")]
        private static extern void LogLevelStartJS(string level);

        [DllImport("__Internal")]
        private static extern void LogLevelEndJS(string level, string score, bool wasCompleted);

        [DllImport("__Internal")]
        private static extern void LogLevelUpJS(string level);

        [DllImport("__Internal")]
        private static extern void LogScoreJS(string score);

        [DllImport("__Internal")]
        private static extern void LogTutorialStartJS(string tutorial);

        [DllImport("__Internal")]
        private static extern void LogTutorialEndJS(string tutorial, bool wasCompleted);

        [DllImport("__Internal")]
        private static extern void LogGameChoiceJS(string decision, string choice);

        [DllImport("__Internal")]
        private static extern void LogPurchaseJS(string productID, string data);

        [DllImport("__Internal")]
        private static extern void LogPurchaseSubscriptionJS(string productID, string data);

        [DllImport("__Internal")]
        private static extern void LogSocialInviteJS(string placement);

        [DllImport("__Internal")]
        private static extern void LogSocialShareJS(string placement);

        // Context
        [DllImport("__Internal")]
        private static extern string ContextGetIdJS();

        [DllImport("__Internal")]
        private static extern string ContextGetTypeJS();

        [DllImport("__Internal")]
        private static extern void ContextGetPlayersJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextChooseJS(string payload, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextInviteJS(string payload, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextShareJS(string payload, Action<int> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextShareLinkJS(string payload, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextUpdateJS(string payload, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextCreateJS(string playerId, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void ContextSwitchJS(string contextId, Action callback, Action<string> errorCallback);

        // IAP
        [DllImport("__Internal")]
        private static extern bool IAPIsEnabledJS();

        [DllImport("__Internal")]
        private static extern void IAPGetCatalogJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void IAPGetPurchasesJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void IAPMakePurchaseJS(string purchase, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void IAPConsumePurchaseJS(string token, Action callback, Action<string> errorCallback);

        // Leaderboards
        [DllImport("__Internal")]
        private static extern void LeaderboardGetJS(string name, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LeaderboardSendEntryJS(string name, int score, string details, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LeaderboardGetEntriesJS(string name, int count, int offset, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LeaderboardGetPlayerEntryJS(string name, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LeaderboardGetEntryCountJS(string name, Action<int> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LeaderboardGetConnectedPlayersEntriesJS(string name, int count, int offset, Action<string> callback, Action<string> errorCallback);

        // Notifications
        [DllImport("__Internal")]
        private static extern void NotificationsScheduleJS(string payload, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void NotificationsGetHistoryJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void NotificationsCancelJS(string id, Action<bool> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void NotificationsCancelAllJS(Action<bool> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void NotificationsCancelAllLabelJS(string label, Action<bool> callback, Action<string> errorCallback);

        // Player
        [DllImport("__Internal")]
        private static extern string PlayerGetIDJS();

        [DllImport("__Internal")]
        private static extern string PlayerGetNameJS();

        [DllImport("__Internal")]
        private static extern string PlayerGetPhotoJS();

        [DllImport("__Internal")]
        private static extern bool PlayerIsFirstPlayJS();

        [DllImport("__Internal")]
        private static extern void PlayerGetDataJS(string keys, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerSetDataJS(string data, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerFlushDataJS(Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerGetConnectedPlayersJS(string payload, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerGetSignedPlayerInfoJS(Action<string, string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerGetASIDJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerGetSignedASIDJS(Action<string, string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerCanSubscribeBotJS(Action<bool> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void PlayerSubscribeBotJS(Action callback, Action<string> errorCallback);

        // Session
        [DllImport("__Internal")]
        private static extern string SessionGetEntryPointDataJS();

        [DllImport("__Internal")]
        private static extern void SessionGetEntryPointJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void SessionSetSessionDataJS(string data);

        [DllImport("__Internal")]
        private static extern string SessionGetLocaleJS();

        [DllImport("__Internal")]
        private static extern string SessionGetTrafficSourceJS();

        [DllImport("__Internal")]
        private static extern string SessionGetPlatformJS();

        [DllImport("__Internal")]
        private static extern string SessionGetDeviceJS();

        [DllImport("__Internal")]
        private static extern string SessionGetOrientationJS();

        [DllImport("__Internal")]
        private static extern void SessionOnOrientationChangeJS(Action<string> callback);

        [DllImport("__Internal")]
        private static extern void SessionSwitchGameJS(Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void SessionHappyTimeJS();

        [DllImport("__Internal")]
        private static extern void SessionGameplayStartJS();

        [DllImport("__Internal")]
        private static extern void SessionGameplayStopJS();

        [DllImport("__Internal")]
        private static extern bool SessionIsAudioEnabledJS();

        [DllImport("__Internal")]
        private static extern void SessionOnAudioStatusChangeJS(Action<int> callback);

        // Stats
        [DllImport("__Internal")]
        private static extern void StatsGetStatsJS(string level, string payload, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void StatsPostStatsJS(string level, int value, string payload, Action callback, Action<string> errorCallback);

        // Tournaments
        [DllImport("__Internal")]
        private static extern void TournamentGetCurrentJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void TournamentGetAllJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void TournamentPostScoreJS(int score, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void TournamentCreateJS(string payload, Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void TournamentShareJS(string payload, Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void TournamentJoinJS(string tournamentID, Action callback, Action<string> errorCallback);

        // Generic
        [DllImport("__Internal")]
        private static extern bool IsInitializedJS();

        [DllImport("__Internal")]
        private static extern void InitializeJS(Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void StartGameJS(Action callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void AuthenticateJS(Action<string> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void LinkAccountJS(Action<bool> callback, Action<string> errorCallback);

        [DllImport("__Internal")]
        private static extern void SetLoadingProgressJS(int value);

        [DllImport("__Internal")]
        private static extern void OnPauseJS(Action callback);

        [DllImport("__Internal")]
        private static extern void OnResumeJS(Action callback);

        [DllImport("__Internal")]
        private static extern string GetSupportedAPIsJS();

        [DllImport("__Internal")]
        private static extern void PerformHapticFeedbackJS(Action callback, Action<string> errorCallback);

        // Public wrapper methods for Generic
        public static bool IsInitialized()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return IsInitializedJS();
#else
            Debug.LogWarning("[PluginManager] IsInitialized not available on this platform");
            return false;
#endif
        }

        public static void Initialize(Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            InitializeJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] Initialize not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void StartGame(Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            StartGameJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] StartGame not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void Authenticate(Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            AuthenticateJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] Authenticate not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void LinkAccount(Action<bool> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LinkAccountJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] LinkAccount not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void SetLoadingProgress(int value)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SetLoadingProgressJS(value);
#else
            Debug.LogWarning("[PluginManager] SetLoadingProgress not available on this platform");
#endif
        }

        public static void OnPause(Action callback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            OnPauseJS(callback);
#else
            Debug.LogWarning("[PluginManager] OnPause not available on this platform");
#endif
        }

        public static void OnResume(Action callback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            OnResumeJS(callback);
#else
            Debug.LogWarning("[PluginManager] OnResume not available on this platform");
#endif
        }

        public static string GetSupportedAPIs()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return GetSupportedAPIsJS();
#else
            Debug.LogWarning("[PluginManager] GetSupportedAPIs not available on this platform");
            return null;
#endif
        }

        public static void PerformHapticFeedback(Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            PerformHapticFeedbackJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] PerformHapticFeedback not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        // Public wrapper methods for Achievements
        public static void AchievementsGetAchievements(Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            AchievementsGetAchievementsJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] AchievementsGetAchievements not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void AchievementsUnlockAchievement(string achievementID, Action<bool> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            AchievementsUnlockAchievementJS(achievementID, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] AchievementsUnlockAchievement not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        // Public wrapper methods for Ads
        public static bool IsAdBlocked()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return IsAdBlockedJS();
#else
            Debug.LogWarning("[PluginManager] IsAdBlocked not available on this platform");
            return false;
#endif
        }

        public static bool IsEnabled()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return IsEnabledJS();
#else
            Debug.LogWarning("[PluginManager] IsEnabled not available on this platform");
            return false;
#endif
        }

        public static void ShowInterstitial(string type, string description, Action beforeAdCallback, Action afterAdCallback, Action noFillCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ShowInterstitialJS(type, description, beforeAdCallback, afterAdCallback, noFillCallback);
#else
            Debug.LogWarning("[PluginManager] ShowInterstitial not available on this platform");
            noFillCallback?.Invoke();
#endif
        }

        public static void ShowRewarded(string description, Action beforeAdCallback, Action afterAdCallback, Action adDismissedCallback, Action adViewedCallback, Action noFillCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ShowRewardedJS(description, beforeAdCallback, afterAdCallback, adDismissedCallback, adViewedCallback, noFillCallback);
#else
            Debug.LogWarning("[PluginManager] ShowRewarded not available on this platform");
            noFillCallback?.Invoke();
#endif
        }

        public static void ShowBanner(bool shouldShow, string position)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ShowBannerJS(shouldShow, position);
#else
            Debug.LogWarning("[PluginManager] ShowBanner not available on this platform");
#endif
        }

        // Public wrapper methods for Analytics
        public static void LogLevelStart(string level)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogLevelStartJS(level);
#else
            Debug.LogWarning("[PluginManager] LogLevelStart not available on this platform");
#endif
        }

        public static void LogLevelEnd(string level, string score, bool wasCompleted)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogLevelEndJS(level, score, wasCompleted);
#else
            Debug.LogWarning("[PluginManager] LogLevelEnd not available on this platform");
#endif
        }

        public static void LogLevelUp(string level)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogLevelUpJS(level);
#else
            Debug.LogWarning("[PluginManager] LogLevelUp not available on this platform");
#endif
        }

        public static void LogScore(string score)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogScoreJS(score);
#else
            Debug.LogWarning("[PluginManager] LogScore not available on this platform");
#endif
        }

        public static void LogTutorialStart(string tutorial)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogTutorialStartJS(tutorial);
#else
            Debug.LogWarning("[PluginManager] LogTutorialStart not available on this platform");
#endif
        }

        public static void LogTutorialEnd(string tutorial, bool wasCompleted)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogTutorialEndJS(tutorial, wasCompleted);
#else
            Debug.LogWarning("[PluginManager] LogTutorialEnd not available on this platform");
#endif
        }

        public static void LogGameChoice(string decision, string choice)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogGameChoiceJS(decision, choice);
#else
            Debug.LogWarning("[PluginManager] LogGameChoice not available on this platform");
#endif
        }

        public static void LogPurchase(string productID, string data)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogPurchaseJS(productID, data);
#else
            Debug.LogWarning("[PluginManager] LogPurchase not available on this platform");
#endif
        }

        public static void LogPurchaseSubscription(string productID, string data)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogPurchaseSubscriptionJS(productID, data);
#else
            Debug.LogWarning("[PluginManager] LogPurchaseSubscription not available on this platform");
#endif
        }

        public static void LogSocialInvite(string placement)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogSocialInviteJS(placement);
#else
            Debug.LogWarning("[PluginManager] LogSocialInvite not available on this platform");
#endif
        }

        public static void LogSocialShare(string placement)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LogSocialShareJS(placement);
#else
            Debug.LogWarning("[PluginManager] LogSocialShare not available on this platform");
#endif
        }

        // Public wrapper methods for Context
        public static string ContextGetId()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return ContextGetIdJS();
#else
            Debug.LogWarning("[PluginManager] ContextGetId not available on this platform");
            return null;
#endif
        }

        public static string ContextGetType()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return ContextGetTypeJS();
#else
            Debug.LogWarning("[PluginManager] ContextGetType not available on this platform");
            return null;
#endif
        }

        public static void ContextGetPlayers(Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextGetPlayersJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] ContextGetPlayers not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void ContextChoose(string payload, Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextChooseJS(payload, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] ContextChoose not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void ContextInvite(string payload, Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextInviteJS(payload, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] ContextInvite not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void ContextShare(string payload, Action<int> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextShareJS(payload, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] ContextShare not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void ContextShareLink(string payload, Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextShareLinkJS(payload, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] ContextShareLink not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void ContextUpdate(string payload, Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextUpdateJS(payload, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] ContextUpdate not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void ContextCreate(string playerId, Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextCreateJS(playerId, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] ContextCreate not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void ContextSwitch(string contextId, Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            ContextSwitchJS(contextId, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] ContextSwitch not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        // Public wrapper methods for IAP
        public static bool IAPIsEnabled()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return IAPIsEnabledJS();
#else
            Debug.LogWarning("[PluginManager] IAPIsEnabled not available on this platform");
            return false;
#endif
        }

        public static void IAPGetCatalog(Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            IAPGetCatalogJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] IAPGetCatalog not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void IAPGetPurchases(Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            IAPGetPurchasesJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] IAPGetPurchases not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void IAPMakePurchase(string purchase, Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            IAPMakePurchaseJS(purchase, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] IAPMakePurchase not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void IAPConsumePurchase(string token, Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            IAPConsumePurchaseJS(token, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] IAPConsumePurchase not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        // Public wrapper methods for Leaderboards
        public static void LeaderboardGet(string name, Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LeaderboardGetJS(name, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] LeaderboardGet not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void LeaderboardSendEntry(string name, int score, string details, Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LeaderboardSendEntryJS(name, score, details, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] LeaderboardSendEntry not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void LeaderboardGetEntries(string name, int count, int offset, Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LeaderboardGetEntriesJS(name, count, offset, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] LeaderboardGetEntries not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void LeaderboardGetPlayerEntry(string name, Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LeaderboardGetPlayerEntryJS(name, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] LeaderboardGetPlayerEntry not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void LeaderboardGetEntryCount(string name, Action<int> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LeaderboardGetEntryCountJS(name, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] LeaderboardGetEntryCount not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void LeaderboardGetConnectedPlayersEntries(string name, int count, int offset, Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            LeaderboardGetConnectedPlayersEntriesJS(name, count, offset, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] LeaderboardGetConnectedPlayersEntries not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        // Public wrapper methods for Notifications
        public static void NotificationsSchedule(string payload, Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            NotificationsScheduleJS(payload, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] NotificationsSchedule not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void NotificationsGetHistory(Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            NotificationsGetHistoryJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] NotificationsGetHistory not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void NotificationsCancel(string id, Action<bool> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            NotificationsCancelJS(id, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] NotificationsCancel not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void NotificationsCancelAll(Action<bool> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            NotificationsCancelAllJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] NotificationsCancelAll not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void NotificationsCancelAllLabel(string label, Action<bool> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            NotificationsCancelAllLabelJS(label, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] NotificationsCancelAllLabel not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        // Public wrapper methods for Player
        public static string PlayerGetID()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return PlayerGetIDJS();
#else
            Debug.LogWarning("[PluginManager] PlayerGetID not available on this platform");
            return null;
#endif
        }

        public static string PlayerGetName()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return PlayerGetNameJS();
#else
            Debug.LogWarning("[PluginManager] PlayerGetName not available on this platform");
            return null;
#endif
        }

        public static string PlayerGetPhoto()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return PlayerGetPhotoJS();
#else
            Debug.LogWarning("[PluginManager] PlayerGetPhoto not available on this platform");
            return null;
#endif
        }

        public static bool PlayerIsFirstPlay()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return PlayerIsFirstPlayJS();
#else
            Debug.LogWarning("[PluginManager] PlayerIsFirstPlay not available on this platform");
            return false;
#endif
        }

        public static void PlayerGetData(string keys, Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerGetDataJS(keys, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] PlayerGetData not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void PlayerSetData(string data, Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerSetDataJS(data, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] PlayerSetData not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void PlayerFlushData(Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerFlushDataJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] PlayerFlushData not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void PlayerGetConnectedPlayers(string payload, Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerGetConnectedPlayersJS(payload, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] PlayerGetConnectedPlayers not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void PlayerGetSignedPlayerInfo(Action<string, string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerGetSignedPlayerInfoJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] PlayerGetSignedPlayerInfo not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void PlayerGetASID(Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerGetASIDJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] PlayerGetASID not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void PlayerGetSignedASID(Action<string, string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerGetSignedASIDJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] PlayerGetSignedASID not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void PlayerCanSubscribeBot(Action<bool> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerCanSubscribeBotJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] PlayerCanSubscribeBot not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void PlayerSubscribeBot(Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            PlayerSubscribeBotJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] PlayerSubscribeBot not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        // Public wrapper methods for Session
        public static string SessionGetEntryPointData()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return SessionGetEntryPointDataJS();
#else
            Debug.LogWarning("[PluginManager] SessionGetEntryPointData not available on this platform");
            return null;
#endif
        }

        public static void SessionGetEntryPoint(Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionGetEntryPointJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] SessionGetEntryPoint not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void SessionSetSessionData(string data)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionSetSessionDataJS(data);
#else
            Debug.LogWarning("[PluginManager] SessionSetSessionData not available on this platform");
#endif
        }

        public static string SessionGetLocale()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return SessionGetLocaleJS();
#else
            Debug.LogWarning("[PluginManager] SessionGetLocale not available on this platform");
            return null;
#endif
        }

        public static string SessionGetTrafficSource()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return SessionGetTrafficSourceJS();
#else
            Debug.LogWarning("[PluginManager] SessionGetTrafficSource not available on this platform");
            return null;
#endif
        }

        public static string SessionGetPlatform()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return SessionGetPlatformJS();
#else
            Debug.LogWarning("[PluginManager] SessionGetPlatform not available on this platform");
            return null;
#endif
        }

        public static string SessionGetDevice()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return SessionGetDeviceJS();
#else
            Debug.LogWarning("[PluginManager] SessionGetDevice not available on this platform");
            return null;
#endif
        }

        public static string SessionGetOrientation()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return SessionGetOrientationJS();
#else
            Debug.LogWarning("[PluginManager] SessionGetOrientation not available on this platform");
            return null;
#endif
        }

        public static void SessionOnOrientationChange(Action<string> callback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionOnOrientationChangeJS(callback);
#else
            Debug.LogWarning("[PluginManager] SessionOnOrientationChange not available on this platform");
#endif
        }

        public static void SessionSwitchGame(Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionSwitchGameJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] SessionSwitchGame not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void SessionHappyTime()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionHappyTimeJS();
#else
            Debug.LogWarning("[PluginManager] SessionHappyTime not available on this platform");
#endif
        }

        public static void SessionGameplayStart()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionGameplayStartJS();
#else
            Debug.LogWarning("[PluginManager] SessionGameplayStart not available on this platform");
#endif
        }

        public static void SessionGameplayStop()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionGameplayStopJS();
#else
            Debug.LogWarning("[PluginManager] SessionGameplayStop not available on this platform");
#endif
        }

        public static bool SessionIsAudioEnabled()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return SessionIsAudioEnabledJS();
#else
            Debug.LogWarning("[PluginManager] SessionIsAudioEnabled not available on this platform");
            return false;
#endif
        }

        public static void SessionOnAudioStatusChange(Action<int> callback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            SessionOnAudioStatusChangeJS(callback);
#else
            Debug.LogWarning("[PluginManager] SessionOnAudioStatusChange not available on this platform");
#endif
        }

        // Public wrapper methods for Stats
        public static void StatsGetStats(string level, string payload, Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            StatsGetStatsJS(level, payload, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] StatsGetStats not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void StatsPostStats(string level, int value, string payload, Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            StatsPostStatsJS(level, value, payload, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] StatsPostStats not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        // Public wrapper methods for Tournaments
        public static void TournamentGetCurrent(Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            TournamentGetCurrentJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] TournamentGetCurrent not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void TournamentGetAll(Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            TournamentGetAllJS(callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] TournamentGetAll not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void TournamentPostScore(int score, Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            TournamentPostScoreJS(score, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] TournamentPostScore not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void TournamentCreate(string payload, Action<string> callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            TournamentCreateJS(payload, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] TournamentCreate not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void TournamentShare(string payload, Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            TournamentShareJS(payload, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] TournamentShare not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

        public static void TournamentJoin(string tournamentID, Action callback, Action<string> errorCallback)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            TournamentJoinJS(tournamentID, callback, errorCallback);
#else
            Debug.LogWarning("[PluginManager] TournamentJoin not available on this platform");
            errorCallback?.Invoke("NOT_SUPPORTED");
#endif
        }

#endif

        }
}
