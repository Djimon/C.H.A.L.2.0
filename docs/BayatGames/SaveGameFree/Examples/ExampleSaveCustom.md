# BayatGames.SaveGameFree.Examples.ExampleSaveCustom

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Custom/ExampleSaveCustom.cs`._

1) Purpose
- Defines a Unity MonoBehaviour example (ExampleSaveCustom) that demonstrates saving and loading a custom data structure using SaveGameFree.
- Declares serializable nested types Level (struct) and CustomData (class) to model the saved data.
- Exposes public fields for inspector wiring (customData, loadOnStart, scoreInputField, highScoreInputField, identifier) and implements Start/SetScore/SetHighScore/Save/Load to manage persistence and UI.

2) Public API
- Namespace: BayatGames.SaveGameFree.Examples
- Types
  - public class ExampleSaveCustom : MonoBehaviour
    - Public fields
      - public CustomData customData
      - public bool loadOnStart
      - public UnityEngine.UI.InputField scoreInputField
      - public UnityEngine.UI.InputField highScoreInputField
      - public string identifier
    - Public methods
      - public void Start()
      - public void SetScore(string score)
      - public void SetHighScore(string highScore)
      - public void Save()
      - public void Load()
  - public struct Level
    - public Level(bool unlocked, bool completed)
      - (initializes level state)
  - public class CustomData
    - public int score
    - public int highScore
    - public List<Level> levels
    - public CustomData()
      - initializes default values and dummy levels

3) Key Behavior & Side Effects
- Start()
  - If loadOnStart is true, calls Load().
- SetScore(string score)
  - Parses string to int and assigns to customData.score.
- SetHighScore(string highScore)
  - Parses string to int and assigns to customData.highScore.
- Save()
  - Calls SaveGame.Save<CustomData>(identifier, customData, SerializerDropdown.Singleton.ActiveSerializer).
- Load()
  - Loads into customData via SaveGame.Load<CustomData>(identifier, new CustomData(), SerializerDropdown.Singleton.ActiveSerializer).
  - Updates scoreInputField.text with customData.score and highScoreInputField.text with customData.highScore.

4) Constraints & Failure Modes
- Null references
  - scoreInputField, highScoreInputField, identifier, or customData may be null if not wired/initialized; operations like setting text or accessing customData will throw.
- Parsing errors
  - SetScore/SetHighScore use int.Parse; throws on non-integer input.
- Assumptions
  - Uses SerializerDropdown.Singleton.ActiveSerializer; behavior depends on external configuration.
- Threading
  - All operations run on Unity main thread; no async behavior shown.

5) Example
- Not provided (not clearly derivable as a standalone minimal usage snippet from this file).

6) Unknowns
- Implementation details of SaveGame.Save/Load and SerializerDropdown behavior.
- Exact serialization semantics for nested types Level and CustomData beyond what's shown.
- Impact of missing or invalid saved data beyond default CustomData constructor.

