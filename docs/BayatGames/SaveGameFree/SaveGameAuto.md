# BayatGames.SaveGameFree.SaveGameAuto

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/SaveGameAuto.cs`._

# Purpose
- Defines the `SaveGameAuto` class for automatically saving and loading game object transformations (position, rotation, scale).

# Public API
- Namespace: `BayatGames.SaveGameFree`
- Types
  - **public class** `SaveGameAuto` [extends `MonoBehaviour`]
    - **public string** `positionIdentifier` - Identifier for saving position.
    - **public string** `rotationIdentifier` - Identifier for saving rotation.
    - **public string** `scaleIdentifier` - Identifier for saving scale.
    - **public bool** `encode` - Indicates if data should be encoded.
    - **public string** `encodePassword` - Password for encoding.
    - **public SaveFormat** `format` - Serialization format (XML, JSON, Binary).
    - **public ISaveGameSerializer** `serializer` - Custom serializer.
    - **public ISaveGameEncoder** `encoder` - Custom encoder.
    - **public Encoding** `encoding` - Custom encoding.
    - **public SaveGamePath** `savePath` - Path for saving data.
    - **public bool** `resetBlanks` - Resets empty fields to default values.
    - **public bool** `savePosition` - Flag to save position.
    - **public bool** `saveRotation` - Flag to save rotation.
    - **public bool** `saveScale` - Flag to save scale.
    - **public Vector3** `defaultPosition` - Default position value.
    - **public Vector3** `defaultRotation` - Default rotation value.
    - **public Vector3** `defaultScale` - Default scale value.
    - **public bool** `saveOnAwake` - Flag to save on Awake.
    - **public bool** `saveOnStart` - Flag to save on Start.
    - **public bool** `saveOnEnable` - Flag to save on OnEnable.
    - **public bool** `saveOnDisable` - Flag to save on OnDisable.
    - **public bool** `saveOnApplicationQuit` - Flag to save on application quit.
    - **public bool** `saveOnApplicationPause` - Flag to save on application pause.
    - **public bool** `loadOnAwake` - Flag to load on Awake.
    - **public bool** `loadOnStart` - Flag to load on Start.
    - **public bool** `loadOnEnable` - Flag to load on OnEnable.
    - **public virtual void** `Save()` - Saves the object's position, rotation, and scale.
    - **public virtual void** `Load()` - Loads the object's position, rotation, and scale.

# Key Behavior & Side Effects
- On `Awake()`, initializes serializer based on the selected format and optionally loads or saves data.
- On `Start()`, optionally loads or saves data.
- On `OnEnable()`, optionally loads or saves data.
- On `OnDisable()`, saves data if the flag is set.
- On `OnApplicationQuit()`, saves data if the flag is set.
- On `OnApplicationPause()`, saves data if the flag is set.
- The `Save()` method saves the object's position, rotation, and scale based on the specified identifiers.
- The `Load()` method loads the object's position, rotation, and scale, applying defaults if no saved data exists.

# Constraints & Failure Modes
- Requires valid identifiers for saving position, rotation, and scale.
- If `resetBlanks` is true, empty fields are reset to default values on `Awake()`.
- The `Load()` method uses default values if no saved data is found.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    void Start()
    {
        SaveGameAuto saveGameAuto = gameObject.AddComponent<SaveGameAuto>();
        saveGameAuto.positionIdentifier = "playerPosition";
        saveGameAuto.rotationIdentifier = "playerRotation";
        saveGameAuto.scaleIdentifier = "playerScale";
        saveGameAuto.Save();
    }
}
```

# Unknowns
- Specific implementations of `ISaveGameSerializer` and `ISaveGameEncoder` are not defined in this file.
- The behavior of `SaveGame.Save<T>()` and `SaveGame.Load<T>()` methods is not detailed in this file.

