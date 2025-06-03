using System;

namespace DigitalWill.WortalSDK
{
    /// <summary>
    /// Interface for player functionality across platforms
    /// </summary>
    public interface IWortalPlayer
    {
        /// <summary>
        /// Gets the player's ID
        /// </summary>
        /// <returns>Player ID</returns>
        string GetID();

        /// <summary>
        /// Gets the player's name
        /// </summary>
        /// <returns>Player name</returns>
        string GetName();

        /// <summary>
        /// Gets the player's photo URL
        /// </summary>
        /// <returns>Photo URL</returns>
        string GetPhoto();

        /// <summary>
        /// Checks if the player is a first-time player
        /// </summary>
        /// <returns>True if first-time player</returns>
        bool IsFirstPlay();

        /// <summary>
        /// Gets connected players (friends who also play this game)
        /// </summary>
        /// <param name="payload">Connected players request payload</param>
        /// <param name="onSuccess">Callback with connected players</param>
        /// <param name="onError">Callback for retrieval errors</param>
        void GetConnectedPlayersAsync(GetConnectedPlayersPayload payload, Action<IWortalPlayer[]> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Gets the player's signed info
        /// </summary>
        /// <param name="onSuccess">Callback with signed player info</param>
        /// <param name="onError">Callback for retrieval errors</param>
        void GetSignedPlayerInfoAsync(Action<string, string> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Checks if the player can subscribe to the bot
        /// </summary>
        /// <param name="onSuccess">Callback with subscription capability</param>
        /// <param name="onError">Callback for check errors</param>
        void CanSubscribeBotAsync(Action<bool> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Subscribes the player to the bot
        /// </summary>
        /// <param name="onSuccess">Callback for successful subscription</param>
        /// <param name="onError">Callback for subscription errors</param>
        void SubscribeBotAsync(Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Gets the player's data from the platform
        /// </summary>
        /// <param name="keys">Keys of data to retrieve</param>
        /// <param name="onSuccess">Callback with player data</param>
        /// <param name="onError">Callback for retrieval errors</param>
        void GetDataAsync(string[] keys, Action<PlayerData> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Sets the player's data on the platform
        /// </summary>
        /// <param name="data">Data to set</param>
        /// <param name="onSuccess">Callback for successful data setting</param>
        /// <param name="onError">Callback for data setting errors</param>
        void SetDataAsync(PlayerData data, Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Flushes pending data operations
        /// </summary>
        /// <param name="onSuccess">Callback for successful flush</param>
        /// <param name="onError">Callback for flush errors</param>
        void FlushDataAsync(Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Gets player stats
        /// </summary>
        /// <param name="keys">Keys of stats to retrieve</param>
        /// <param name="onSuccess">Callback with player stats</param>
        /// <param name="onError">Callback for retrieval errors</param>
        void GetStatsAsync(string[] keys, Action<PlayerStats> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Sets player stats
        /// </summary>
        /// <param name="stats">Stats to set</param>
        /// <param name="onSuccess">Callback for successful stats setting</param>
        /// <param name="onError">Callback for stats setting errors</param>
        void SetStatsAsync(PlayerStats stats, Action onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Increments player stats
        /// </summary>
        /// <param name="increments">Stats increments</param>
        /// <param name="onSuccess">Callback with updated stats</param>
        /// <param name="onError">Callback for increment errors</param>
        void IncrementStatsAsync(PlayerStats increments, Action<PlayerStats> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Gets the player's ASID (App-Specific ID)
        /// </summary>
        /// <param name="onSuccess">Callback with ASID</param>
        /// <param name="onError">Callback for retrieval errors</param>
        void GetASIDAsync(Action<string> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Gets the player's signed ASID (App-Specific ID)
        /// </summary>
        /// <param name="onSuccess">Callback with signed ASID</param>
        /// <param name="onError">Callback for retrieval errors</param>
        void GetSignedASIDAsync(Action<string, string> onSuccess, Action<WortalError> onError);

        /// <summary>
        /// Checks if player features are supported on this platform
        /// </summary>
        bool IsSupported { get; }
    }
}
