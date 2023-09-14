using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Payload for Context.Choose. Defines the filters and search parameters to apply to the friend list.
    /// </summary>
    [Serializable]
    public struct ChoosePayload
    {
        /// <summary>
        /// Optional customizable text field in the share UI.
        /// This can be used to describe the incentive a user can get from sharing.
        /// </summary>
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LocalizableContent Description;
        /// <summary>
        /// An array of filters to be applied to the friend list. Only the first filter is currently used.
        /// </summary>
        [JsonProperty("filters", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ContextFilter[] Filters;
        /// <summary>
        /// Specify how long a friend should be filtered out after the current player sends them a message.
        /// This parameter only applies when `NEW_INVITATIONS_ONLY` filter is used.
        /// When not specified, it will filter out any friend who has been sent a message.
        ///
        /// PLATFORM NOTE: Viber only.
        /// </summary>
        [JsonProperty("hoursSinceInvitation", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int HoursSinceInvitation;
        /// <summary>
        /// Context maximum size.
        /// </summary>
        [JsonProperty("maxSize", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MaxSize;
        /// <summary>
        /// Context minimum size.
        /// </summary>
        [JsonProperty("minSize", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MinSize;
        /// <summary>
        /// Image which will be displayed to contact.
        /// A string containing data URL of a base64 encoded image.
        /// If not specified, game's icon image will be used by default.
        ///
        /// PLATFORM NOTE: Link only.
        /// </summary>
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Image;
        /// <summary>
        /// Message which will be displayed to contact.
        /// If not specified, "SENDER_NAMEと一緒に「GAME_NAME」をプレイしよう！" will be used by default.
        ///
        /// PLATFORM NOTE: Link only.
        /// </summary>
        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LocalizableContent Text;
        /// <summary>
        /// Text of the call-to-action button.
        /// If not specified, "今すぐプレイ" will be used by default.
        ///
        /// PLATFORM NOTE: Link only.
        /// </summary>
        [JsonProperty("caption", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LocalizableContent Caption;
        /// <summary>
        /// Object passed to any session launched from this update message.
        /// It can be accessed from `Wortal.session.getEntryPointData()`.
        /// Its size must be less than or equal to 1000 chars when stringified.
        ///
        /// PLATFORM NOTE: Link only.
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<string, object> Data;
    }
}
