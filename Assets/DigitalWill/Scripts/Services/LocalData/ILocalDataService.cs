namespace DigitalWill.Services
{
    /// <summary>
    /// Base interface for local data services.
    /// </summary>
    public interface ILocalDataService : IGameService
    {
        /// <summary>
        /// Player's current score.
        /// </summary>
        int CurrentScore { get; }
        /// <summary>
        /// Current level being played.
        /// </summary>
        int CurrentLevel { get; }
        /// <summary>
        /// Max number of levels in the game overall.
        /// </summary>
        int MaxLevels { get; }
        /// <summary>
        /// Current amount of currency the player possesses.
        /// </summary>
        int Currency { get; }

        /// <summary>
        /// Starts a new game for the current player.
        /// </summary>
        void NewGame();

        /// <summary>
        /// Saves the current player's game.
        /// </summary>
        /// <returns>True if game saved successfully, false if the save failed.</returns>
        bool SaveGame();

        /// <summary>
        /// Loads a saved game with the given ID.
        /// </summary>
        /// <param name="saveId">ID of the save game to load. This should be the player's ID.</param>
        /// <returns>True if game loaded successfully, false if the load failed.</returns>
        bool LoadGame(string saveId);

        /// <summary>
        /// Gets the player's score from the given level.
        /// </summary>
        /// <param name="level">Level to get the player's score from.</param>
        /// <returns>Score from the given level.</returns>
        int GetScoreFromLevel(int level);

        /// <summary>
        /// Saves the player's score on the current level.
        /// </summary>
        void SaveScoreForCurrentLevel();

        /// <summary>
        /// Sets the player's current score in the current level.
        /// </summary>
        void AddScore(int score);

        /// <summary>
        /// Clears the current score for the current level.
        /// </summary>
        void ClearCurrentScore();

        /// <summary>
        /// Clears all the player's saved scores.
        /// </summary>
        void ClearAllScores();

        /// <summary>
        /// Sets the current level that the player is on.
        /// </summary>
        void SetCurrentLevel(int level);

        /// <summary>
        /// Sets the maximum number of levels in the game.
        /// </summary>
        void SetMaxLevels(int levels);

        /// <summary>
        /// Adds the given amount of currency to the player's current stash.
        /// </summary>
        /// <param name="amount">Amount of currency to add.</param>
        void AddCurrency(int amount);

        /// <summary>
        /// Deducts the given amount of currency from the player's current stash.
        /// </summary>
        /// <param name="amount">Amount of currency to spend.</param>
        void SpendCurrency(int amount);

        /// <summary>
        /// Is the content with the given ID unlocked or not.
        /// </summary>
        /// <param name="id">ID of the content (item, level, etc).</param>
        /// <returns>Whether or not the content is unlocked.</returns>
        bool IsUnlocked(int id);

        /// <summary>
        /// Unlocks content with the given ID.
        /// </summary>
        /// <param name="id">ID of the content to unlock.</param>
        void Unlock(int id);
    }
}
