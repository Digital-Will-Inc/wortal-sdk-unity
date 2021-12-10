using System;
using UnityEngine;

namespace DigitalWill.Core
{
    /// <summary>
    /// Logging class that catches and filters Soup-related log calls. Also handles saving logs to disk.
    /// </summary>
    public static class SoupLog
    {
        public static void Log(string message, LogType type)
        {
            SoupSettings.SoupLogLevel level = SoupComponent.SoupSettings.LogLevel;

            switch (level)
            {
                case SoupSettings.SoupLogLevel.Error:
                    switch (type)
                    {
                        case LogType.Log:
                            return;
                        case LogType.Warning:
                            return;
                        case LogType.Error:
                            Debug.LogError(message);
                            break;
                    }
                    break;

                case SoupSettings.SoupLogLevel.Warning:
                    switch (type)
                    {
                        case LogType.Log:
                            return;
                        case LogType.Warning:
                            Debug.LogWarning(message);
                            break;
                        case LogType.Error:
                            Debug.LogError(message);
                            break;
                    }
                    break;

                case SoupSettings.SoupLogLevel.Info:
                    switch (type)
                    {
                        case LogType.Log:
                            Debug.Log(message);
                            break;
                        case LogType.Warning:
                            Debug.LogWarning(message);
                            break;
                        case LogType.Error:
                            Debug.LogError(message);
                            break;
                    }
                    break;

                default:
                    throw new ArgumentException("SoupLog.Log was passed an invalid SoupLogLevel.");
            }
        }
    }
}
