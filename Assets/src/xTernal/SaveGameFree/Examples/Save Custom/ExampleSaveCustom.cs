using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BayatGames.SaveGameFree.Examples
{

/// <summary>
/// Manages the saving and loading of game data.
/// </summary>
    public class ExampleSaveCustom : MonoBehaviour
    {

        [System.Serializable]
        public struct Level
        {
            public bool unlocked;
            public bool completed;

            public Level(bool unlocked, bool completed)
            {
                this.unlocked = unlocked;
                this.completed = completed;
            }
        }

        [System.Serializable]
/// <summary>
/// Represents custom data for a game, including scores and levels.
/// </summary>
        public class CustomData
        {

            public int score;
            public int highScore;
            public List<Level> levels;

            public CustomData()
            {
                score = 0;
                highScore = 0;

                // Dummy data
                levels = new List<Level>() {
                    new Level ( true, false ),
                    new Level ( false, false ),
                    new Level ( false, true ),
                    new Level ( true, false )
                };
            }

        }

        public CustomData customData;
        public bool loadOnStart = true;
        public InputField scoreInputField;
        public InputField highScoreInputField;
        public string identifier = "exampleSaveCustom";

        void Start()
        {
            if (loadOnStart)
            {
                Load();
            }
        }

/// <summary>
/// Sets the score for the game.
/// </summary>
/// <param name="score">The score to set as a string.</param>
        public void SetScore(string score)
        {
            customData.score = int.Parse(score);
        }

/// <summary>
/// Sets the high score for the game.
/// </summary>
/// <param name="highScore">The high score to set as a string.</param>
        public void SetHighScore(string highScore)
        {
            customData.highScore = int.Parse(highScore);
        }

/// <summary>
/// Saves the current game data.
/// </summary>
        public void Save()
        {
            SaveGame.Save<CustomData>(identifier, customData, SerializerDropdown.Singleton.ActiveSerializer);
        }

/// <summary>
/// Loads the game data and updates the score fields.
/// </summary>
        public void Load()
        {
            customData = SaveGame.Load<CustomData>(
                identifier,
                new CustomData(),
                SerializerDropdown.Singleton.ActiveSerializer);
            scoreInputField.text = customData.score.ToString();
            highScoreInputField.text = customData.highScore.ToString();
        }

    }

}
