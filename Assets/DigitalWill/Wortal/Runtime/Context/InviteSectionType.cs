using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Represents the type of section to include. All section types may include both new and existing contexts and players.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum InviteSectionType
    {
        /// <summary>
        /// This contains group contexts, such as contexts from group threads.
        /// </summary>
        GROUPS,
        /// <summary>
        /// This contains individual users, such as friends or 1:1 threads.
        /// </summary>
        USERS,
    }
}
