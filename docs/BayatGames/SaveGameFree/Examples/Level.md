# BayatGames.SaveGameFree.Examples.Level

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Custom/ExampleSaveCustom.cs`._

1) Purpose
- Defines ExampleSaveCustom MonoBehaviour with serializable nested types Level and CustomData for demonstrating saving/loading custom data via SaveGameFree.
- Exposes public fields to configure data and UI, and public methods to set values, save, and load using the active serializer.

2) Public API
- Namespace/module
  - BayatGames.SaveGameFree.Examples

- Types
  - public struct Level
    - public bool unlocked
      - public field indicating level unlock state
    - public bool completed
      - public field indicating level completion state
    - public Level(bool unlocked, bool completed)
      - Constructor to initialize fields

  - public class CustomData
    - public int score
      - public field
    - public int highScore
      - public field
    - public List<Level> levels
      - public field
    - public CustomData()
      - Constructor initializing score/highScore and dummy levels list

  - public class ExampleSaveCustom : MonoBehaviour
    - public CustomData customData
      - public field
    - public bool loadOnStart
      - public field
    - public InputField scoreInputField
      - public field
    - public InputField highScoreInputField
      - public field
    - public string identifier
      - public field

    - public void SetScore(string score)
      - Signature: SetScore(string)
      - Behavior: parses score and assigns to customData.score

    - public void SetHighScore(string highScore)
      - Signature: SetHighScore(string)
      - Behavior: parses highScore and assigns to customData.highScore

    - public void Save()
      - Signature: Save()
      - Behavior: SaveGame.Save<CustomData>(identifier, customData, SerializerDropdown.Singleton.ActiveSerializer)

    - public void Load()
      - Signature: Load()
      - Behavior: loads into customData via SaveGame.Load<CustomData>(identifier, new CustomData(), SerializerDropdown.Singleton.ActiveSerializer)
      - Updates scoreInputField.text and highScoreInputField.text with loaded values

3) Key Behavior & Side Effects
- Start (implicit private): if loadOnStart is true, calls Load()
- SetScore(string): parses string to int and sets customData.score
- SetHighScore(string): parses string to int and sets customData.highScore
- Save(): persists customData using the active serializer
- Load(): retrieves CustomData, replacing customData; updates UI fields with loaded score/highScore

4) Constraints & Failure Modes
- No guards around parsing:
  - int.Parse(score) and int.Parse(highScore) may throw if inputs are not valid integers
- Potential NullReference risks:
  - scoreInputField or highScoreInputField may be null if not assigned in the inspector
  - customData may be null before Load/after failed Load
- External dependencies:
  - SaveGame.Save/Load and SerializerDropdown.Singleton.ActiveSerializer determine runtime behavior; behavior not defined in this file

5) Example
- Not provided in this file (no explicit usage example beyond the public API)

6) Unknowns
- Exact behavior/return types of SaveGame.Save and SaveGame.Load
- What SerializerDropdown.Singleton.ActiveSerializer returns/implements
- Threading, async, or error-handling semantics of SaveGameFree methods
- Persistence format and how Level/List<Level> is serialized/deserialized
- Lifecycle guarantees for Unity UI InputField references at runtime

