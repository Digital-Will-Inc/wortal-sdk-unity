using System;
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
    }
}
