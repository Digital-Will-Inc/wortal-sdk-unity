using System;
using Newtonsoft.Json;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Represents the configurations used in creating a tournament.
    /// </summary>
    [Serializable]
    public struct CreateTournamentConfig
    {
        /// <summary>
        /// Optional title of the tournament.
        /// </summary>
        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
        public string Title;
        /// <summary>
        /// Optional base64 encoded image that will be associated with the tournament and included in posts sharing the tournament.
        /// </summary>
        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public string Image;
        /// <summary>
        /// Optional input for the ordering of which score is best in the tournament. The options are 'HIGHER_IS_BETTER'
        /// or 'LOWER_IS_BETTER'. If not specified, the default is 'HIGHER_IS_BETTER'.
        /// </summary>
        [JsonProperty("sortOrder", NullValueHandling = NullValueHandling.Ignore)]
        public SortOrder SortOrder;
        /// <summary>
        /// Optional input for the formatting of the scores in the tournament leaderboard. The options are 'NUMERIC'
        /// or 'TIME'. If not specified, the default is 'NUMERIC'.
        /// </summary>
        [JsonProperty("scoreFormat", NullValueHandling = NullValueHandling.Ignore)]
        public ScoreFormat ScoreFormat;
        /// <summary>
        /// Optional input for setting a custom end time for the tournament. The number passed in represents a
        /// unix timestamp. If not specified, the tournament will end one week after creation.
        /// </summary>
        [JsonProperty("endTime", NullValueHandling = NullValueHandling.Ignore)]
        public int EndTime;
    }
}
