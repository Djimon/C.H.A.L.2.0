# BayatGames.SaveGameFree.Examples.ExampleSaveCustom

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Custom/ExampleSaveCustom.cs`._

# Purpose
- Defines a Unity MonoBehaviour for saving and loading custom game data.

# Public API
- Namespace: `BayatGames.SaveGameFree.Examples`
- Types
  - `public class ExampleSaveCustom : MonoBehaviour`
    - Public fields/properties:
      - `CustomData customData`: Holds the custom game data.
      - `bool loadOnStart`: Indicates if data should be loaded on start.
      - `InputField scoreInputField`: Input field for score.
      - `InputField highScoreInputField`: Input field for high score.
      - `string identifier`: Identifier for saving/loading data.
    - Public methods:
      - `void SetScore(string score)`: Sets the score from input.
      - `void SetHighScore(string highScore)`: Sets the high score from input.
      - `void Save()`: Saves the custom data using the specified identifier.
      - `void Load()`: Loads the custom data and updates input fields.

# Key Behavior & Side Effects
- On `Start()`, if `loadOnStart` is true, it calls `Load()` to initialize data.
- `Load()` updates the `scoreInputField` and `highScoreInputField` with loaded values.

# Constraints & Failure Modes
- Assumes valid integer input for `SetScore` and `SetHighScore`; no error handling for parsing.
- Uses a default `CustomData` instance when loading if no saved data exists.

# Example
```csharp
ExampleSaveCustom example = new ExampleSaveCustom();
example.SetScore("100");
example.SetHighScore("200");
example.Save();
example.Load();
```

# Unknowns
- No information on the implementation details of `SaveGame.Save` and `SaveGame.Load`.
- No details on the `SerializerDropdown.Singleton.ActiveSerializer`.

