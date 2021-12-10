using System;
using System.Collections.Generic;
using DigitalWill.Core;

namespace DigitalWill.Services
{
    /// <summary>
    /// Service locator for <see cref="IGameService"/> instances. Only one type of each service can be registered at a
    /// time. This should be initialized in the ServiceBootstrapper.
    /// </summary>
    public class GameServices
    {
        private static bool _isInit;

        private GameServices() {}

        private readonly Dictionary<string, IGameService> _services = new Dictionary<string, IGameService>();

        /// <summary>
        /// Gets the currently active service locator instance.
        /// </summary>
        public static GameServices I { get; private set; }

        /// <summary>
        /// Initializes the service locator with a new instance.
        /// </summary>
        /// <remarks>Should only be called from <see cref="ServiceBootstrapper"/> during game load.</remarks>
        public static void Init()
        {
            if (_isInit)
            {
                Soup.LogError("GameServices.Init should only be called once.");
                return;
            }

            I = new GameServices();
            _isInit = true;
        }

        /// <summary>
        /// Gets the service instance of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the service to lookup.</typeparam>
        /// <returns>The service instance.</returns>
        public T Get<T>() where T : IGameService
        {
            string key = typeof(T).Name;

            if (_services.ContainsKey(key))
            {
                return (T)_services[key];
            }

            Soup.LogError($"{key} not registered with {GetType().Name}.");
            throw new InvalidOperationException();
        }

        /// <summary>
        /// Registers the service with the current service locator.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        /// <param name="service">Service instance.</param>
        public void Register<T>(T service) where T : IGameService
        {
            string key = typeof(T).Name;

            if (_services.ContainsKey(key))
            {
                Soup.LogError($"Attempted to register service of type {key} which is already registered with the {GetType().Name}.");
                return;
            }

            _services.Add(key, service);
            Soup.Log($"Registered {key}.");
        }

        /// <summary>
        /// Unregisters the service from the current service locator.
        /// </summary>
        /// <typeparam name="T">Service type.</typeparam>
        public void Unregister<T>() where T : IGameService
        {
            string key = typeof(T).Name;

            if (!_services.ContainsKey(key))
            {
                Soup.LogError($"Attempted to unregister service of type {key} which is not registered with the {GetType().Name}.");
                return;
            }

            _services.Remove(key);
            Soup.Log($"Unregistered {key}.");
        }
    }
}
