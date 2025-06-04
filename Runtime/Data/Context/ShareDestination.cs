using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// A parameter that may be applied to a shareAsync operation. This set up sharing destination in the share dialog.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShareDestination
    {
        /// <summary>
        /// Ignores this parameter and does not send to the SDK.
        /// </summary>
        NONE,
        /// <summary>
        /// Enable share to newsfeed option.
        /// </summary>
        NEWSFEED,
        /// <summary>
        /// Enable share to official game group option. This is only available for games with official game group.
        /// To set up official game group, add a page in the game app setting in https://www.developers.facebook.com,
        /// and then create a group for the page in https://facebook.com.
        /// </summary>
        GROUP,
        /// <summary>
        /// Enable copy the game link in clipboard.
        /// </summary>
        COPY_LINK,
        /// <summary>
        /// Enable share game to messenger option.
        /// </summary>
        MESSENGER,
    }
}
