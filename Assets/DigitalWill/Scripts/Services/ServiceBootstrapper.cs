using System;
using System.Collections;
using DigitalWill.Core;
using DigitalWill.Util;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DigitalWill.Services
{
    /// <summary>
    /// Bootstrapper that handles loading and initializing the different game services.
    /// </summary>
    /// <seealso cref="GameServices"/>
    public static class ServiceBootstrapper
    {
        /// <summary>
        /// The bootstrapper has finished loading and initializing all the required services.
        /// </summary>
        public static Action LoadingDone;

        /// <summary>
        /// Have the services finished loading or not.
        /// </summary>
        public static bool IsLoadingDone { get; private set; }

        private static MonoBehaviour _mb;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            // Shouldn't be possible, but stranger things have happened. We guard against it anyways.
            if (IsLoadingDone)
            {
                Soup.LogError("ServiceBootstrapper: Services already loaded.");
                return;
            }

            // Find our surrogate MonoBehaviour.
            _mb = Object.FindObjectOfType<MonoBehaviour>();

            // This should be a rare edge case because we should be able to pick up the Bootstrapper, but just in case.
            if (_mb == null)
            {
                throw new SystemException("No surrogate MonoBehaviour was found in the scene. Ensure there is at least one MonoBehaviour in the scene.");
            }

            // Initialize default service locator.
            GameServices.Init();
            Soup.Log("ServiceBootstrapper: Initializing GameServices.");

            // Register all our services here.
            try
            {
                switch (SoupComponent.SoupSettings.Audio)
                {
                    case ServiceType.AudioType.None:
                        GameServices.I.Register<IAudioService>(new NullAudioService());
                        break;
                    case ServiceType.AudioType.SoupAudio:
                        GameServices.I.Register<IAudioService>(new SoupAudioService());
                        break;
                    default:
                        throw new ArgumentException("Invalid AudioType passed to ServiceBootstrapper.");
                }

                switch (SoupComponent.SoupSettings.Localization)
                {
                    case ServiceType.LocalizationType.None:
                        GameServices.I.Register<ILocalizationService>(new NullLocalizationService());
                        break;
                    case ServiceType.LocalizationType.SoupLocalization:
                        GameServices.I.Register<ILocalizationService>(new SoupLocalizationService());
                        break;
                    default:
                        throw new ArgumentException("Invalid LocalizationType passed to ServiceBootstrapper.");
                }

                switch (SoupComponent.SoupSettings.LocalData)
                {
                    case ServiceType.LocalDataType.None:
                        GameServices.I.Register<ILocalDataService>(new NullLocalDataService());
                        break;
                    case ServiceType.LocalDataType.SoupLocalData:
                        GameServices.I.Register<ILocalDataService>(new SoupLocalDataService());
                        break;
                    default:
                        throw new ArgumentException("Invalid LocalDataType passed to ServiceBootstrapper.");
                }
            }
            catch (Exception e)
            {
                Soup.LogError($"ServiceBootstrapper: Failed to load one or more services. \n{e}");
            }

            // Use our surrogate to run our loading sequence.
            _mb.StartCoroutine(CheckForLoadingDone());
        }

        private static IEnumerator CheckForLoadingDone()
        {
            while (!SoupComponent.I.IsLanguageCodeSet)
            {
                yield return null;
            }

            GameServices.I.Get<ILocalizationService>().Init(SoupComponent.I.LanguageCode);
            yield return YieldHelper.TENTH_SECOND;

            // Now we init Soup to hook up the services for easy access.
            Soup.Init();
            yield return YieldHelper.HALF_SECOND;

            // Subscribe to this event in the Bootstrapper and set the initial scene to load when this is triggered.
            LoadingDone?.Invoke();
            IsLoadingDone = true;
        }
    }
}
