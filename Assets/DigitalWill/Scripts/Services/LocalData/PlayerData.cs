using System.Collections.Generic;

namespace DigitalWill.Services
{
    /// <summary>
    /// Data about the player that can be saved and loaded. Common uses might be player level, in-game currency amount,
    /// game progress, etc.
    /// </summary>
    [System.Serializable]
    public class PlayerData
    {
        public string Id;
        public int Currency;
        public List<int> LevelsUnlocked;

        /// <summary>
        /// Constructs a PlayerData object with data about the player to be saved or loaded.
        /// </summary>
        /// <param name="playerId">ID of the player.</param>
        /// <param name="currency">Amount of currency the player has.</param>
        /// <param name="levelsUnlocked">Int array of Quiz IDs that the player has unlocked.</param>
        public PlayerData(string playerId, int currency, List<int> levelsUnlocked)
        {
            Id = playerId;
            Currency = currency;
            LevelsUnlocked = levelsUnlocked;
        }
    }
}
