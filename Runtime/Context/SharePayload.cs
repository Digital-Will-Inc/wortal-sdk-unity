using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Payload for Context.ShareAsync. Defines the message to be sent to the context.
    /// </summary>
    [Serializable]
    public struct SharePayload
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
        /// Message format to be used. There's no visible difference among the available options.
        /// </summary>
        [JsonProperty("intent", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IntentType Intent;
        /// <summary>
        /// Defines the minimum number of players to be selected to start sharing.
        /// </summary>
        [JsonProperty("minShare", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int MinShare;
        /// <summary>
        /// Optional property to directly send share messages to multiple players with a confirmation prompt.
        /// Selection UI will be skipped if this property is set.
        ///
        /// PLATFORM NOTE: Viber only.
        /// </summary>
        [JsonProperty("playerIds", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string[] PlayerIDs;
        /// <summary>
        /// Optional property to switch share UI mode.
        /// </summary>
        [JsonProperty("ui", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public UIType UI;
        /// <summary>
        /// An optional array to set sharing destinations in the share dialog.
        /// If not specified all available sharing destinations will be displayed.
        ///
        /// PLATFORM NOTE: Facebook only.
        /// </summary>
        [JsonProperty("shareDestination", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ShareDestination[] ShareDestination;
        /// <summary>
        /// A flag indicating whether to switch the user into the new context created on sharing.
        ///
        /// PLATFORM NOTE: Facebook only.
        /// </summary>
        [JsonProperty("switchContext", NullValueHandling = NullValueHandling.Ignore)]
        public bool SwitchContext;
    }
}
