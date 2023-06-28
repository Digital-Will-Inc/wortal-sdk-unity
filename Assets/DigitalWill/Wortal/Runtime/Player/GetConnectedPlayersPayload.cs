using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Payload for the GetConnectedPlayers API.
    /// </summary>
    [Serializable]
    public class GetConnectedPlayersPayload
    {
        /// <summary>
        /// Specify where to start fetch the friend list.
        /// This parameter only applies when NEW_INVITATIONS_ONLY filter is used.
        /// When not specified with NEW_INVITATIONS_ONLY filter, default cursor is 0.
        /// </summary>
        [JsonProperty("cursor", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Cursor;
        /// <summary>
        /// Filter to be applied to the friend list.
        /// </summary>
        [JsonProperty("filter", NullValueHandling = NullValueHandling.Ignore)]
        public ConnectedPlayerFilter Filter;
        /// <summary>
        /// Specify how long a friend should be filtered out after the current player sends them a message.
        /// This parameter only applies when NEW_INVITATIONS_ONLY filter is used.
        /// When not specified, it will filter out any friend who has been sent a message.
        /// </summary>
        [JsonProperty("hoursSinceInvitation", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int HoursSinceInvitation;
        /// <summary>
        /// Specify how many friends to be returned in the friend list.
        /// This parameter only applies when NEW_INVITATIONS_ONLY filter is used.
        /// When not specified with NEW_INVITATIONS_ONLY filter, default cursor is 25.
        /// </summary>
        [JsonProperty("size", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Size;
    }
}
