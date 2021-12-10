using System;
using System.Collections.Generic;
using System.IO;
using DigitalWill.Core;
using UnityEngine;

namespace DigitalWill.Services
{
    /// <summary>
    /// Local game data service that handles data about the game during gameplay.
    /// </summary>
    public class SoupLocalDataService : ILocalDataService
    {
        private const string PLAYER_ID_VAR = "playerID";
        private const string SAVE_PATH = "/save/";

        private PlayerData _playerData;
        private GameData _gameData;
        private int[] _scores;

        public int CurrentScore { get; private set; }
        public int CurrentLevel { get; private set; }
        public int MaxLevels { get; private set; }
        public int Currency { get; private set; }

        public void NewGame()
        {
            _playerData = new PlayerData("player1", 0, new List<int> { 2, 4, 7, 8 });
            _gameData = new GameData(LanguageCode.EN, true, 1f, 1f, 1f);
            Soup.Log("LocalDataService.NewGame");

            // We call this here to make sure the new game is saved to disk. This way if the game crashes or something,
            // the next time the player launches they will have a game to load corresponding to the PlayerPrefs key.
            SaveGame();
        }

        public bool SaveGame()
        {
            // Skip this because we're not going to be saving anything in WebGL builds.
            return true;
        }

        public bool LoadGame(string saveId)
        {
            // Skip this because we're not going to be saving anything in WebGL builds.
            _playerData = new PlayerData("player1", 0, new List<int> { 1, 2 });
            _gameData = new GameData(LanguageCode.EN, true, 1f, 1f, 1f);
            return true;
        }

        public int GetScoreFromLevel(int level)
        {
            return _scores[level];
        }

        public void SaveScoreForCurrentLevel()
        {
            _scores[CurrentLevel] = CurrentScore;
        }

        public void AddScore(int score)
        {
            CurrentScore += score;
        }

        public void ClearCurrentScore()
        {
            CurrentScore = 0;
        }

        public void ClearAllScores()
        {
            CurrentLevel = 0;

            for (int i = 0; i < _scores.Length; i++)
            {
                _scores[i] = 0;
            }
        }

        public void SetCurrentLevel(int level)
        {
            CurrentLevel = level;
        }

        public void SetMaxLevels(int levels)
        {
            MaxLevels = levels;

            // We add 1 because we don't have a level 0, we will start using the values from index 1.
            _scores = new int[levels + 1];
        }

        public void AddCurrency(int amount)
        {
            Currency += amount;
        }

        public void SpendCurrency(int amount)
        {
            Currency -= amount;
        }

        public bool IsUnlocked(int id)
        {
            // All quizzes are unlocked by default in WebGL build.
            return true;
        }

        public void Unlock(int id)
        {
            _playerData.LevelsUnlocked.Add(id);
            SaveGame();
        }
    }
}
