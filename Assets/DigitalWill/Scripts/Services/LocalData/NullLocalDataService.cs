namespace DigitalWill.Services
{
    /// <summary>
    /// Empty service for stub testing or disabling the service.
    /// </summary>
    public class NullLocalDataService : ILocalDataService
    {
        public int CurrentScore => -1;
        public int CurrentLevel => -1;
        public int MaxLevels => -1;
        public int Currency => -1;
        public void NewGame() { }
        public bool SaveGame() { return false; }
        public bool LoadGame(string saveId) { return false; }
        public int GetScoreFromLevel(int level) { return -1; }
        public void SaveScoreForCurrentLevel() { }
        public void AddScore(int score) { }
        public void ClearCurrentScore() { }
        public void ClearAllScores() { }
        public void SetCurrentLevel(int level) { }
        public void SetMaxLevels(int levels) { }
        public void AddCurrency(int amount) { }
        public void SpendCurrency(int amount) { }
        public bool IsUnlocked(int id) { return false; }
        public void Unlock(int id) { }
    }
}
