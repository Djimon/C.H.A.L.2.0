# Assets/src/xTernal/SaveGameFree/Scripts/SaveGameAuto.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a `SaveGameAuto` MonoBehaviour for automatically saving and loading game object transformations (position, rotation, scale).

## Public API
- Namespace: `BayatGames.SaveGameFree`
- Types
  - `public class SaveGameAuto : MonoBehaviour`
    - Public fields/properties:
      - `public string positionIdentifier` - Identifier for saving position.
      - `public string rotationIdentifier` - Identifier for saving rotation.
      - `public string scaleIdentifier` - Identifier for saving scale.
      - `public bool encode` - Flag to determine if data should be encoded.
      - `public string encodePassword` - Password for encoding.
      - `public SaveFormat format` - Format for saving data (XML, JSON, Binary).
      - `public ISaveGameSerializer serializer` - Custom serializer for saving data.
      - `public ISaveGameEncoder encoder` - Custom encoder for saving data.
      - `public Encoding encoding` - Encoding type for saving data.
      - `public SaveGamePath savePath` - Path where data will be saved.
      - `public bool resetBlanks` - Flag to reset empty fields to defaults.
      - `public bool savePosition` - Flag to save position.
      - `public bool saveRotation` - Flag to save rotation.
      - `public bool saveScale` - Flag to save scale.
      - `public Vector3 defaultPosition` - Default position value.
      - `public Vector3 defaultRotation` - Default rotation value.
      - `public Vector3 defaultScale` - Default scale value.
      - `public bool saveOnAwake` - Flag to save on Awake.
      - `public bool saveOnStart` - Flag to save on Start.
      - `public bool saveOnEnable` - Flag to save on OnEnable.
      - `public bool saveOnDisable` - Flag to save on OnDisable.
      - `public bool saveOnApplicationQuit` - Flag to save on application quit.
      - `public bool saveOnApplicationPause` - Flag to save on application pause.
      - `public bool loadOnAwake` - Flag to load on Awake.
      - `public bool loadOnStart` - Flag to load on Start.
      - `public bool loadOnEnable` - Flag to load on OnEnable.
    - Public methods:
      - `public virtual void Save()` - Saves position, rotation, and scale based on flags.
      - `public virtual void Load()` - Loads position, rotation, and scale based on flags.

## Key Behavior & Side Effects
- On `Awake()`, initializes serializer based on the selected format and optionally loads or saves data.
- On `Start()`, optionally loads or saves data.
- On `OnEnable()`, optionally loads or saves data.
- On `OnDisable()`, saves data if the corresponding flag is set.
- On `OnApplicationQuit()`, saves data if the corresponding flag is set.
- On `OnApplicationPause()`, saves data if the corresponding flag is set.

## Constraints & Failure Modes
- Requires valid identifiers for saving position, rotation, and scale.
- If `resetBlanks` is true, defaults are applied for empty fields.
- The `Save()` and `Load()` methods depend on the specified flags for saving/loading each transformation component.

## Example
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
        autoSaver.loadOnStart = true;
    }
}
```

## Unknowns
- Specific implementation details of `ISaveGameSerializer` and `ISaveGameEncoder`.
- Behavior of `SaveGame.Save<T>()` and `SaveGame.Load<T>()` methods.
```
