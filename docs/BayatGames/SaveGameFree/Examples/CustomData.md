# BayatGames.SaveGameFree.Examples.CustomData

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Custom/ExampleSaveCustom.cs`._

# Purpose
- Manages the saving and loading of game data.

# Public API
- Namespace: `BayatGames.SaveGameFree.Examples`
- Types
  - `public class ExampleSaveCustom : MonoBehaviour`
    - Public fields/properties:
      - `CustomData customData`: Holds the custom game data.
      - `bool loadOnStart`: Indicates whether to load data on start.
      - `InputField scoreInputField`: Input field for the score.
      - `InputField highScoreInputField`: Input field for the high score.
      - `string identifier`: Identifier for saving/loading data.
    - Public methods:
      - `void Start()`: Loads game data if `loadOnStart` is true.
      - `void SetScore(string score)`: Sets the score from a string input.
      - `void SetHighScore(string highScore)`: Sets the high score from a string input.
      - `void Save()`: Saves the current game data.
      - `void Load()`: Loads the game data and updates the score fields.

  - `public struct Level`
    - Public fields/properties:
      - `bool unlocked`: Indicates if the level is unlocked.
      - `bool completed`: Indicates if the level is completed.
    - Constructor:
      - `Level(bool unlocked, bool completed)`: Initializes level status.

  - `public class CustomData`
    - Public fields/properties:
      - `int score`: Current score.
      - `int highScore`: Highest score achieved.
      - `List<Level> levels`: List of levels with their statuses.
    - Constructor:
      - `CustomData()`: Initializes score, high score, and dummy level data.

# Key Behavior & Side Effects
- On `Start()`, if `loadOnStart` is true, the game data is loaded.
- `SetScore` and `SetHighScore` parse string inputs to integers and update the respective scores.
- `Save` method saves the `customData` using the specified identifier and active serializer.
- `Load` method retrieves saved data, updates `customData`, and reflects the score in the input fields.

# Constraints & Failure Modes
- `SetScore` and `SetHighScore` assume valid integer strings; invalid inputs may cause exceptions.
- The `Load` method initializes `customData` with a new instance if no data is found.

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
