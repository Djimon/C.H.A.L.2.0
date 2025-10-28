# Assets/src/xTernal/SaveGameFree/Examples/Save Custom/ExampleSaveCustom.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a Unity MonoBehaviour for saving and loading custom game data.
- Provides a structure for level data and a class for custom game data.

# Public API
- Namespace: `BayatGames.SaveGameFree.Examples`
- Types
  - `public class ExampleSaveCustom : MonoBehaviour`
    - Public fields/properties:
      - `CustomData customData`: Holds the custom game data.
      - `bool loadOnStart`: Determines if data should be loaded on start.
      - `InputField scoreInputField`: UI input field for score.
      - `InputField highScoreInputField`: UI input field for high score.
      - `string identifier`: Identifier for saving/loading data.
    - Public methods:
      - `void Start()`: Loads data if `loadOnStart` is true.
      - `void SetScore(string score)`: Sets the score from input.
      - `void SetHighScore(string highScore)`: Sets the high score from input.
      - `void Save()`: Saves the custom data using the SaveGame system.
      - `void Load()`: Loads the custom data and updates UI fields.

# Key Behavior & Side Effects
- On `Start()`, if `loadOnStart` is true, it calls `Load()`.
- `Load()` updates the `scoreInputField` and `highScoreInputField` with loaded data.

# Constraints & Failure Modes
- Assumes valid integer input for `SetScore` and `SetHighScore`; no error handling for parsing.
- Uses `SaveGame.Save` and `SaveGame.Load` methods which may have their own constraints.

# Example
```csharp
ExampleSaveCustom example = new ExampleSaveCustom();
example.SetScore("100");
example.SetHighScore("200");
example.Save();
example.Load();
```

# Unknowns
- Behavior of `SaveGame.Save` and `SaveGame.Load` methods is not defined in this file.
- Details of `SerializerDropdown.Singleton.ActiveSerializer` are not provided.
```
