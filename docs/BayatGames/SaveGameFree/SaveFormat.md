# BayatGames.SaveGameFree.SaveFormat

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/SaveGameAuto.cs`._

# Purpose
- Manages automatic saving of game data, including position, rotation, and scale of game objects.

# Public API
- Namespace: BayatGames.SaveGameFree
- Types
  - public class SaveGameAuto : MonoBehaviour
    - Public fields/properties:
      - string positionIdentifier: Identifier for saving position.
      - string rotationIdentifier: Identifier for saving rotation.
      - string scaleIdentifier: Identifier for saving scale.
      - bool encode: Indicates if data should be encoded.
      - string encodePassword: Password for encoding.
      - SaveFormat format: Format for saving data (XML, JSON, Binary).
      - ISaveGameSerializer serializer: Serializer for saving data.
      - ISaveGameEncoder encoder: Encoder for saving data.
      - Encoding encoding: Encoding type for saving data.
      - SaveGamePath savePath: Path where data will be saved.
      - bool resetBlanks: Resets empty fields to default values.
      - bool savePosition: Indicates if position should be saved.
      - bool saveRotation: Indicates if rotation should be saved.
      - bool saveScale: Indicates if scale should be saved.
      - Vector3 defaultPosition: Default position value.
      - Vector3 defaultRotation: Default rotation value.
      - Vector3 defaultScale: Default scale value.
      - bool saveOnAwake: Save on Awake() lifecycle method.
      - bool saveOnStart: Save on Start() lifecycle method.
      - bool saveOnEnable: Save on OnEnable() lifecycle method.
      - bool saveOnDisable: Indicates if save should occur on OnDisable().
      - bool saveOnApplicationQuit: Indicates if save should occur on application quit.
      - bool saveOnApplicationPause: Indicates if save should occur on application pause.
      - bool loadOnAwake: Load on Awake() lifecycle method.
      - bool loadOnStart: Load on Start() lifecycle method.
      - bool loadOnEnable: Load on OnEnable() lifecycle method.
    - Public methods:
      - virtual void Save(): Saves position, rotation, and scale based on specified identifiers.
      - virtual void Load(): Loads position, rotation, and scale based on specified identifiers.

# Key Behavior & Side Effects
- On Awake(), if `loadOnAwake` is true, it loads the saved data; if `saveOnAwake` is true, it saves the current data.
- On Start(), if `loadOnStart` is true, it loads the saved data; if `saveOnStart` is true, it saves the current data.
- On OnEnable(), if `loadOnEnable` is true, it loads the saved data; if `saveOnEnable` is true, it saves the current data.
- On OnDisable(), if `saveOnDisable` is true, it saves the current data.
- On OnApplicationQuit(), if `saveOnApplicationQuit` is true, it saves the current data.
- On OnApplicationPause(), if `saveOnApplicationPause` is true, it saves the current data.

# Constraints & Failure Modes
- Requires valid identifiers for position, rotation, and scale to save data.
- If `resetBlanks` is true, it resets empty fields to default values on Awake().
- The serializer and encoder can be set to null, which will default to the global settings.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    void Start()
    {
        SaveGameAuto autoSaver = gameObject.AddComponent<SaveGameAuto>();
        autoSaver.positionIdentifier = "playerPosition";
        autoSaver.rotationIdentifier = "playerRotation";
        autoSaver.scaleIdentifier = "playerScale";
        autoSaver.saveOnAwake = true;
    }
}
```

# Unknowns
- None.

