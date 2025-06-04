using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Payload for Context.SwitchAsync. Defines the content to be sent in the invite.
    /// </summary>
    [Serializable]
    public struct SwitchPayload
    {
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
        /// If switching into a solo context, set this to true to switch silently, with no confirmation dialog or toast.
        /// This only has an effect when switching into a solo context. Defaults to false.
        ///
        /// PLATFORM NOTE: Facebook only.
        /// </summary>
        [JsonProperty("switchSilentlyIfSolo")]
        public bool SwitchSilentlyIfSolo;
    }
}
