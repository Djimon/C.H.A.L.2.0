# BayatGames.SaveGameFree.Examples.CustomData

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
      - `InputField scoreInputField`: UI element for score input.
      - `InputField highScoreInputField`: UI element for high score input.
      - `string identifier`: Identifier for saving/loading data.
    - Public methods:
      - `void Start()`: Loads data if `loadOnStart` is true.
      - `void SetScore(string score)`: Sets the score from input.
      - `void SetHighScore(string highScore)`: Sets the high score from input.
      - `void Save()`: Saves `customData` using the specified identifier.
      - `void Load()`: Loads `customData` and updates input fields.

# Key Behavior & Side Effects
- On `Start()`, if `loadOnStart` is true, it calls `Load()`.
- `Load()` updates the `scoreInputField` and `highScoreInputField` with loaded values.

# Constraints & Failure Modes
- `SetScore` and `SetHighScore` assume valid integer input; no error handling for parsing.
- `Load()` initializes `customData` with a new instance if no saved data exists.

# Example
```csharp
ExampleSaveCustom example = new ExampleSaveCustom();
example.SetScore("100");
example.SetHighScore("200");
example.Save();
example.Load();
```

# Unknowns
- No information on the implementation of `SaveGame.Save` and `SaveGame.Load`.
- No details on the `SerializerDropdown.Singleton.ActiveSerializer`.

