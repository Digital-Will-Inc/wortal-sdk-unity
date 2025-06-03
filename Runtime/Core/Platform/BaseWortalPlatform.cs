using System;
using UnityEngine;

namespace DigitalWill.WortalSDK.Core
{
    /// <summary>
    /// Base implementation for all platform implementations
    /// Provides common functionality and default implementations
    /// </summary>
    public abstract class BaseWortalPlatform : IWortalPlatform
    {
        protected bool _isInitialized = false;
        protected WortalSettings _settings;

        public abstract WortalPlatformType PlatformType { get; }
        public virtual bool IsInitialized => _isInitialized;

        // API implementations - will be overridden by platform-specific classes
        public abstract IWortalAuthentication Authentication { get; }
        public abstract IWortalAchievements Achievements { get; }
        public abstract IWortalAds Ads { get; }
        public abstract IWortalAnalytics Analytics { get; }
        public abstract IWortalContext Context { get; }
        public abstract IWortalIAP IAP { get; }
        public abstract IWortalLeaderboard Leaderboard { get; }
        public abstract IWortalPlayer Player { get; }
        public abstract IWortalSession Session { get; }

        protected BaseWortalPlatform()
        {
            _settings = WortalSettings.Instance;
        }

        public virtual void Initialize(Action onSuccess, Action<WortalError> onError)
        {
            if (_isInitialized)
            {
                onSuccess?.Invoke();
                return;
            }

            try
            {
                InitializePlatformSpecific(
                    () =>
                    {
                        _isInitialized = true;
                        Debug.Log($"[Wortal] {PlatformType} platform initialized successfully");
                        onSuccess?.Invoke();
                    },
                    onError
                );
            }
            catch (Exception e)
            {
                var error = new WortalError
                {
                    Code = WortalErrorCodes.INITIALIZATION_ERROR,
                    Message = $"Failed to initialize {PlatformType} platform: {e.Message}"
                };
                onError?.Invoke(error);
            }
        }

        /// <summary>
        /// Platform-specific initialization logic
        /// </summary>
        protected abstract void InitializePlatformSpecific(Action onSuccess, Action<WortalError> onError);

        public virtual string[] GetSupportedAPIs()
        {
            // Default implementation - platforms should override this
            return new string[0];
        }

        /// <summary>
        /// Helper method to check if a feature is supported
        /// </summary>
        protected bool IsAPISupported(string apiName)
        {
            var supportedAPIs = GetSupportedAPIs();
            return Array.IndexOf(supportedAPIs, apiName) > -1;
        }

        /// <summary>
        /// Helper method to create unsupported feature error
        /// </summary>
        protected WortalError CreateUnsupportedError(string feature)
        {
            return new WortalError
            {
                Code = WortalErrorCodes.NOT_SUPPORTED,
                Message = $"{feature} is not supported on {PlatformType} platform"
            };
        }
    }
}
