using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Payload for Context.UpdateAsync. Defines the message to be sent to the context.
    /// </summary>
    [Serializable]
    public struct UpdatePayload
    {
        /// <summary>
        /// Data URL of base64 encoded image to be displayed. This is required for the payload to be sent.
        /// </summary>
        [JsonProperty("image")]
        public string Image;
        /// <summary>
        /// Message body. This is required for the payload to be sent.
        /// </summary>
        [JsonProperty("text")]
        public LocalizableContent Text;
        /// <summary>
        /// Text of the call-to-action button.
        /// </summary>
        [JsonProperty("cta", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LocalizableContent CTA;
        /// <summary>
        /// Object passed to any session launched from this context message.
        /// Its size must be less than or equal to 1000 chars when stringified.
        /// It can be accessed from <code>Wortal.Context.GetEntryPointData()</code>.
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<string, object> Data;
        /// <summary>
        /// Optional content for a gif or video. At least one image or media should be provided in order to render the update.
        ///
        /// PLATFORM NOTE: Facebook only.
        /// </summary>
        [JsonProperty("media", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MediaParams Media;
        /// <summary>
        /// Specifies notification setting for the custom update. This can be 'NO_PUSH' or 'PUSH', and defaults to 'NO_PUSH'.
        /// Use push notification only for updates that are high-signal and immediately actionable for the recipients.
        /// Also note that push notification is not always guaranteed, depending on user setting and platform policies.
        /// </summary>
        [JsonProperty("notifications", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public NotificationsType Notifications;
        /// <summary>
        /// Specifies how the update should be delivered. If no strategy is specified, we default to 'IMMEDIATE'.
        /// </summary>
        [JsonProperty("strategy", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public StrategyType Strategy;
        /// <summary>
        /// Message format to be used. Not currently used.
        /// </summary>
        [JsonProperty("action", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Action;
        /// <summary>
        /// ID of the template this custom update is using. Templates should be predefined in fbapp-config.json.
        /// See the [Bundle Config documentation](https://developers.facebook.com/docs/games/instant-games/bundle-config)
        /// for documentation about fbapp-config.json.
        ///
        /// PLATFORM NOTE: Facebook only.
        /// </summary>
        [JsonProperty("template", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Template;
    }
}
