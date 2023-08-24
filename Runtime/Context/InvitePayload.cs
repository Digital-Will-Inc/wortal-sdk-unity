using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Payload for Context.InviteAsync. Defines the content to be sent in the invite.
    /// </summary>
    [Serializable]
    public struct InvitePayload
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
        /// An optional title to display at the top of the invite dialog instead of the generic title.
        /// This param is not sent as part of the message, but only displays in the dialog header.
        /// The title can be either a string or an object with the default text as the value of 'default' and another object
        /// mapping locale keys to translations as the value of 'localizations'.
        ///
        /// PLATFORM NOTE: Facebook only.
        /// </summary>
        [JsonProperty("dialogTitle", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public LocalizableContent DialogTitle;
        /// <summary>
        /// Object passed to any session launched from this context message.
        /// Its size must be less than or equal to 1000 chars when stringified.
        /// It can be accessed from <code>Wortal.Context.GetEntryPointData()</code>.
        /// </summary>
        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Dictionary<string, object> Data;
        /// <summary>
        /// An array of filters to be applied to the friend list. Only the first filter is currently used.
        /// </summary>
        [JsonProperty("filters", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InviteFilter[] Filters;
        /// <summary>
        /// The set of sections to be included in the dialog. Each section can be assigned a maximum number of results to be
        /// returned (up to a maximum of 10). If no max is included, a default max will be applied. Sections will be included
        /// in the order they are listed in the array. The last section will include a larger maximum number of results, and
        /// if a maxResults is provided, it will be ignored. If this array is left empty, default sections will be used.
        ///
        /// PLATFORM NOTE: Facebook only.
        /// </summary>
        [JsonProperty("sections", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        public InviteSection[] Sections;
    }
}
