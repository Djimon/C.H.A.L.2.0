# BayatGames.SaveGameFree.Examples.ExampleSaveCustom

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Custom/ExampleSaveCustom.cs`._

# Purpose
- Manages the saving and loading of game data.

# Public API
- Namespace: BayatGames.SaveGameFree.Examples
- Types
  - public struct Level
    - Public fields:
      - bool unlocked: Indicates if the level is unlocked.
      - bool completed: Indicates if the level is completed.
  - public class CustomData
    - Public fields:
      - int score: Current score of the game.
      - int highScore: Highest score achieved.
      - List<Level> levels: List of levels with their unlock and completion status.
    - Public methods:
      - CustomData(): Initializes score, highScore, and dummy level data.
  - public class ExampleSaveCustom : MonoBehaviour
    - Public fields:
      - CustomData customData: Instance of custom game data.
      - bool loadOnStart: Determines if data should be loaded on start.
      - InputField scoreInputField: Input field for score.
      - InputField highScoreInputField: Input field for high score.
      - string identifier: Identifier for saving/loading data.
    - Public methods:
      - void SetScore(string score): Sets the score from a string input.
      - void SetHighScore(string highScore): Sets the high score from a string input.
      - void Save(): Saves the current game data.
      - void Load(): Loads game data and updates score fields.

# Key Behavior & Side Effects
- On Start, if `loadOnStart` is true, the game data is loaded.
- The `Load` method updates the `scoreInputField` and `highScoreInputField` with the loaded data.

# Constraints & Failure Modes
- The `SetScore` and `SetHighScore` methods assume valid integer strings are provided; otherwise, they may throw exceptions.
- The `Load` method initializes `customData` with a new instance of `CustomData` if no saved data is found.

# Example
```csharp
ExampleSaveCustom example = new ExampleSaveCustom();
example.SetScore("100");
example.SetHighScore("200");
example.Save();
example.Load();
```

# Unknowns
- None.

