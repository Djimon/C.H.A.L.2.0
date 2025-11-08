# BayatGames.SaveGameFree.SaveGameAuto

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/SaveGameAuto.cs`._

# Purpose
- Manages automatic saving of game data, including position, rotation, and scale of game objects.

# Public API
- Namespace: `BayatGames.SaveGameFree`
- Types
  - public class `SaveGameAuto` [extends `MonoBehaviour`]
    - Public fields/properties:
      - `string positionIdentifier`: Identifier for saving position.
      - `string rotationIdentifier`: Identifier for saving rotation.
      - `string scaleIdentifier`: Identifier for saving scale.
      - `bool encode`: Indicates if data should be encoded.
      - `string encodePassword`: Password for encoding.
      - `SaveFormat format`: Format for saving data (XML, JSON, Binary).
      - `ISaveGameSerializer serializer`: Serializer for saving data.
      - `ISaveGameEncoder encoder`: Encoder for saving data.
      - `Encoding encoding`: Encoding type for saving data.
      - `SaveGamePath savePath`: Path where data will be saved.
      - `bool resetBlanks`: Resets empty fields to default values.
      - `bool savePosition`: Indicates if position should be saved.
      - `bool saveRotation`: Indicates if rotation should be saved.
      - `bool saveScale`: Indicates if scale should be saved.
      - `Vector3 defaultPosition`: Default position value.
      - `Vector3 defaultRotation`: Default rotation value.
      - `Vector3 defaultScale`: Default scale value.
      - `bool saveOnAwake`: Save on Awake lifecycle event.
      - `bool saveOnStart`: Save on Start lifecycle event.
      - `bool saveOnEnable`: Save on OnEnable lifecycle event.
      - `bool saveOnDisable`: Save on OnDisable lifecycle event.
      - `bool saveOnApplicationQuit`: Save on application quit.
      - `bool saveOnApplicationPause`: Save on application pause.
      - `bool loadOnAwake`: Load on Awake lifecycle event.
      - `bool loadOnStart`: Load on Start lifecycle event.
      - `bool loadOnEnable`: Load on OnEnable lifecycle event.
    - Public methods:
      - `virtual void Save()`: Saves position, rotation, and scale based on specified identifiers.
      - `virtual void Load()`: Loads position, rotation, and scale based on specified identifiers.

# Key Behavior & Side Effects
- On `Awake()`, initializes serializer based on the selected format and optionally loads or saves data.
- On `Start()`, optionally loads or saves data.
- On `OnEnable()`, optionally loads or saves data.
- On `OnDisable()`, optionally saves data.
- On `OnApplicationQuit()`, optionally saves data.
- On `OnApplicationPause()`, optionally saves data.

# Constraints & Failure Modes
- Requires valid identifiers for saving position, rotation, and scale.
- If `resetBlanks` is true, defaults are applied for empty fields.
- The save path should be valid; `PersistentDataPath` is recommended.

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

