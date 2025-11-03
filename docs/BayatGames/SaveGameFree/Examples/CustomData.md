# BayatGames.SaveGameFree.Examples.CustomData

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Custom/ExampleSaveCustom.cs`._

```csharp
1) Purpose
- Defines a Unity MonoBehaviour example (ExampleSaveCustom) that demonstrates saving/loading a custom data model with SaveGameFree.
- Defines serializable data shapes:
  - Level (struct) with unlocked/completed flags and a constructor.
  - CustomData (class) with score, highScore, and a list of Level, plus a constructor that initializes default/dummy data.
- Exposes public fields to wire up UI and save settings:
  - customData, loadOnStart, scoreInputField, highScoreInputField, identifier.

```

```csharp
2) Public API
- Namespace/module: BayatGames.SaveGameFree.Examples
- Types
  - public struct Level
    - Fields
      - public bool unlocked  // whether the level is unlocked
      - public bool completed // whether the level is completed
    - Constructors
      - public Level(bool unlocked, bool completed)

  - public class CustomData
    - Fields
      - public int score           // current score
      - public int highScore       // highest score
      - public List<Level> levels  // per-level status data
    - Constructors
      - public CustomData()
        - Initializes score and highScore to 0 and creates dummy levels.

  - public class ExampleSaveCustom : MonoBehaviour
    - Public fields
      - public CustomData customData
      - public bool loadOnStart
      - public InputField scoreInputField
      - public InputField highScoreInputField
      - public string identifier
    - Public methods
      - void Start()
        - If loadOnStart is true, calls Load()
      - void SetScore(string score)
        - Parses string to int and assigns to customData.score
      - void SetHighScore(string highScore)
        - Parses string to int and assigns to customData.highScore
      - void Save()
        - SaveGame.Save<CustomData>(identifier, customData, SerializerDropdown.Singleton.ActiveSerializer)
      - void Load()
        - customData = SaveGame.Load<CustomData>(identifier, new CustomData(), SerializerDropdown.Singleton.ActiveSerializer)
        - scoreInputField.text = customData.score.ToString()
        - highScoreInputField.text = customData.highScore.ToString()

```

```csharp
3) Key Behavior & Side Effects
- Start flow
  - On Start, if loadOnStart is true, Load() is invoked.
- Data editing
  - SetScore and SetHighScore mutate customData.Score/HighScore from string inputs (string -> int).
- Persistence
  - Save() writes the current customData to storage using the selected serializer and the identifier key.
  - Load() reads stored CustomData (or uses a new CustomData() as default if not found) and updates the UI fields (scoreInputField and highScoreInputField) to reflect loaded values.
- UI side effects
  - After Load(), scoreInputField.text and highScoreInputField.text are updated to reflect loaded scores.
- Assumptions/requirements
  - scoreInputField and highScoreInputField must be assigned (non-null) for Load() to update UI fields.
  - customData must be non-null when calling Save(); otherwise a null reference may occur.

```

```csharp
4) Constraints & Failure Modes
- Input parsing
  - SetScore(string) and SetHighScore(string) use int.Parse; non-numeric input may throw FormatException.
- Nullability risks
  - scoreInputField and highScoreInputField must be assigned; otherwise Load() will throw when accessing .text.
  - customData must be non-null when calling Save(); otherwise Save() may throw.
- External dependencies
  - SaveGame.Load/Save and SerializerDropdown.Singleton.ActiveSerializer determine persistence behavior; not defined here.
- Silent failures
  - No error handling is present for Save/Load operations; exceptions would propagate.

```

```csharp
5) Unknowns
- Implementation details of SaveGame.Save and SaveGame.Load (persistence location, format, threading).
- Behavior of SerializerDropdown.Singleton.ActiveSerializer (supported serializers, compatibility).
- Exact runtime behavior if Save/Load fails (exceptions, fallbacks).
- How List<Level> serialization is handled by the active serializer.
```
