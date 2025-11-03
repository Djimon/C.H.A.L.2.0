# BayatGames.SaveGameFree.SaveGameAuto

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/SaveGameAuto.cs`._

Section 1) Purpose
- Defines a Unity MonoBehaviour (SaveGameAuto) that automatically saves and loads a GameObject's position, rotation, and/or scale.
- Provides a SaveFormat enum to select the serialization format (XML, JSON, Binary).
- Exposes configurable identifiers, encoding, serializer/encoder, path, and which transforms to save/load.

```

```csharp
// (No code here; this section documents the public surface only.)
```

Section 2) Public API
- Namespace/module
  - BayatGames.SaveGameFree

- Types
  - public class SaveGameAuto : MonoBehaviour
    - public enum SaveFormat
      - XML
      - JSON
      - Binary
    - Public fields
      - string positionIdentifier
      - string rotationIdentifier
      - string scaleIdentifier
      - bool encode
      - string encodePassword
      - SaveFormat format
      - ISaveGameSerializer serializer
      - ISaveGameEncoder encoder
      - Encoding encoding
      - SaveGamePath savePath
      - bool resetBlanks
      - bool savePosition
      - bool saveRotation
      - bool saveScale
      - Vector3 defaultPosition
      - Vector3 defaultRotation
      - Vector3 defaultScale
      - bool saveOnAwake
      - bool saveOnStart
      - bool saveOnEnable
      - bool saveOnDisable
      - bool saveOnApplicationQuit
      - bool saveOnApplicationPause
      - bool loadOnAwake
      - bool loadOnStart
      - bool loadOnEnable
    - Public methods
      - public virtual void Save ()
      - public virtual void Load ()

Section 3) Key Behavior & Side Effects
- Awake
  - If resetBlanks is true:
    - Fills encodePassword with SaveGame.EncodePassword if empty
    - Fills serializer/encoder/encoding with SaveGame defaults if null
  - Sets serializer based on format:
    - Binary -> new SaveGameBinarySerializer()
    - JSON -> new SaveGameJsonSerializer()
    - XML -> new SaveGameXmlSerializer()
  - If loadOnAwake is true -> Load()
  - If saveOnAwake is true -> Save()

- Start
  - If loadOnStart is true -> Load()
  - If saveOnStart is true -> Save()

- OnEnable
  - If loadOnEnable is true -> Load()
  - If saveOnEnable is true -> Save()

- OnDisable
  - If saveOnDisable is true -> Save()

- OnApplicationQuit
  - If saveOnApplicationQuit is true -> Save()

- OnApplicationPause
  - If saveOnApplicationPause is true -> Save()

- Save()
  - If savePosition -> SaveGame.Save<Vector3Save>(positionIdentifier, transform.position, ...)
  - If saveRotation -> SaveGame.Save<QuaternionSave>(rotationIdentifier, transform.rotation, ...)
  - If saveScale -> SaveGame.Save<Vector3Save>(scaleIdentifier, transform.localScale, ...)

- Load()
  - If savePosition -> transform.position = SaveGame.Load<Vector3Save>(positionIdentifier, defaultPosition, ...)
  - If saveRotation -> transform.rotation = SaveGame.Load<QuaternionSave>(rotationIdentifier, Quaternion.Euler(defaultRotation), ...)
  - If saveScale -> transform.localScale = SaveGame.Load<Vector3Save>(scaleIdentifier, defaultScale, ...)

Section 4) Constraints & Failure Modes
- resetBlanks only affects defaults when true; otherwise serializer/encoder/encoding may remain null.
- Null serializer/encoder/encoding are passed through to SaveGame.Save/Load; behavior depends on underlying SaveGame API (not shown here).
- Identity strings (positionIdentifier, rotationIdentifier, scaleIdentifier) are user-supplied; empty/invalid values rely on external SaveGame behavior.
- DefaultRotation is converted via Quaternion.Euler before use.
- Save/Load flows are driven by Unity lifecycle events and the boolean flags (e.g., saveOnAwake, loadOnEnable); no explicit error handling shown in this file.
- No threading/async logic is present here; all operations occur synchronously on the calling thread.

Section 5) Example
- Minimal usage (attach and configure in code):
```csharp
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    void Awake()
    {
        var sga = gameObject.AddComponent<BayatGames.SaveGameFree.SaveGameAuto>();
        sga.positionIdentifier = "player_pos";
        sga.rotationIdentifier = "player_rot";
        sga.scaleIdentifier = "player_scale";
        sga.saveOnAwake = true;
        sga.loadOnStart = true;
        sga.savePosition = true;
        sga.saveRotation = true;
        sga.saveScale = true;
    }
}
```

Section 6) Unknowns
- Details of SaveGame.SaveGameFree classes (ISaveGameSerializer, ISaveGameEncoder, Encoding, SaveGamePath) are not defined in this file.
- Exact behavior when identifiers are blank or null depends on the underlying SaveGame API.
- Error handling, exceptional cases, and performance characteristics are not specified in this file.
- Interaction with other SaveGameFree features or conflicting saves across multiple components is not described here.

