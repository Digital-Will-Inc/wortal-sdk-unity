namespace DigitalWill.Services
{
    /// <summary>
    /// Data wrapper used for saving and loading the player and game data. Converts data into JSON.
    /// </summary>
    /// <remarks>Can serialize Lists of custom data types into JSON arrays.</remarks>
    [System.Serializable]
    public class SaveData
    {
        public PlayerData PlayerData;
        public GameData GameData;

        /// <summary>
        /// Constructs a SaveData wrapper.
        /// </summary>
        /// <param name="playerData">PlayerData to be saved.</param>
        /// <param name="gameData">GameData to be saved.</param>
        public SaveData(PlayerData playerData, GameData gameData)
        {
            PlayerData = playerData;
            GameData = gameData;
        }
    }
}
