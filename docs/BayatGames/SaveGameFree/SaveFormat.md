# BayatGames.SaveGameFree.SaveFormat

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Scripts/SaveGameAuto.cs`._

```text
1) Purpose
- Defines SaveGameAuto MonoBehaviour to automatically save/load a GameObject's position, rotation, and scale using SaveGameFree.
- Configurable save format (XML/JSON/Binary), serializer/encoder/encoding, path, and per-field identifiers.
- Hooks into Unity lifecycle and save/load events (Awake, Start, Enable, Disable, Quit, Pause) to perform Save/Load based on settings.

```

```text
2) Public API
- Namespace/module
  - BayatGames.SaveGameFree

- Types
  - public enum SaveFormat
    - XML
    - JSON
    - Binary

- public class SaveGameAuto : MonoBehaviour
  - public string positionIdentifier
    - The position data key used for saving/loading.
  - public string rotationIdentifier
    - The rotation data key used for saving/loading.
  - public string scaleIdentifier
    - The scale data key used for saving/loading.
  - public bool encode
    - Whether to encode the data.
  - public string encodePassword
    - Password used for encoding; empty string resets to default if resetBlanks is true.
  - public SaveFormat format
    - Serialization format (XML/JSON/Binary); defaults to JSON.
  - public ISaveGameSerializer serializer
    - Serializer implementation; used for save/load.
  - public ISaveGameEncoder encoder
    - Encoder implementation; used for save/load.
  - public Encoding encoding
    - Text encoding to use for serialization.
  - public SaveGamePath savePath
    - Destination path for saved data; defaults to PersistentDataPath.
  - public bool resetBlanks
    - If true, reset certain fields to defaults on Awake.

- public bool savePosition
  - Save position each time Save() is called.

- public bool saveRotation
  - Save rotation each time Save() is called.

- public bool saveScale
  - Save scale each time Save() is called.

- public Vector3 defaultPosition
  - Default value used when loading if no saved data exists.

- public Vector3 defaultRotation
  - Default rotation (Euler angles) used when loading if no saved data exists.

- public Vector3 defaultScale
  - Default scale used when loading if no saved data exists.

- public bool saveOnAwake
  - Save on Awake() if true.

- public bool saveOnStart
  - Save on Start() if true.

- public bool saveOnEnable
  - Save on OnEnable() if true.

- public bool saveOnDisable
  - Save on OnDisable() if true.

- public bool saveOnApplicationQuit
  - Save on OnApplicationQuit() if true.

- public bool saveOnApplicationPause
  - Save on OnApplicationPause() if true.

- public bool loadOnAwake
  - Load on Awake() if true.

- public bool loadOnStart
  - Load on Start() if true (default true).

- public bool loadOnEnable
  - Load on OnEnable() if true.

- protected virtual void Awake()
  - Initialization; if resetBlanks, fill defaults (encodePassword, serializer, encoder, encoding) from SaveGame defaults; instantiate serializer based on format; optionally Load/Save on Awake.

- protected virtual void Start()
  - Optionally Load/Save on Start based on flags.

- protected virtual void OnEnable()
  - Optionally Load/Save on Enable based on flags.

- protected virtual void OnDisable()
  - Optionally Save on Disable if flag set.

- protected virtual void OnApplicationQuit()
  - Optionally Save on Quit if flag set.

- protected virtual void OnApplicationPause()
  - Optionally Save on Pause if flag set.

- public virtual void Save()
  - Saves enabled fields:
    - Vector3: positionIdentifier, transform.position
    - Quaternion: rotationIdentifier, transform.rotation
    - Vector3: scaleIdentifier, transform.localScale

- public virtual void Load()
  - Loads into:
    - transform.position from positionIdentifier or defaultPosition
    - transform.rotation from rotationIdentifier or Quaternion.Euler(defaultRotation)
    - transform.localScale from scaleIdentifier or defaultScale

```

```text
3) Key Behavior & Side Effects
- Awake initialization
  - If resetBlanks is true:
    - encodePassword = SaveGame.EncodePassword if encodePassword is empty.
    - serializer = SaveGame.Serializer if null.
    - encoder = SaveGame.Encoder if null.
    - encoding = SaveGame.DefaultEncoding if null.
  - Sets serializer based on format:
    - Binary -> new SaveGameBinarySerializer()
    - JSON -> new SaveGameJsonSerializer()
    - XML  -> new SaveGameXmlSerializer()
- Lifecycle-driven save/load
  - On Awake/Start/Enable/Disable/Quit/Pause, saves/loads occur based on corresponding flags.
  - Save() writes current transform.position, transform.rotation, and transform.localScale (per enabled flags) using SaveGame.Save with Vector3Save/QuaternionSave types.
  - Load() reads data and applies to the transform (or uses defaults if no data).
- Data identifiers and path
  - positionIdentifier, rotationIdentifier, scaleIdentifier specify keys for storage.
  - savePath selects where the data is stored.
- Defaults usage
  - Default position/rotation/scale are applied when loading if no saved data exists or identifier absent.

```

```text
4) Constraints & Failure Modes
- resetBlanks behavior
  - When true, certain fields are reset to defaults during Awake if they are null/empty.
- Format/serializer coupling
  - The serializer is set up according to the selected SaveFormat unless overridden earlier; custom serializer/encoder/encoding can be provided but may be overridden if resetBlanks runs.
- Identifiers
  - No explicit guards against empty identifiers are shown; behavior depends on underlying SaveGame methods when given empty keys.
- Data integrity
  - Load uses provided defaults (defaultPosition, Quaternion.Euler(defaultRotation), defaultScale) if data is unavailable.
- Threading/async
  - All operations are synchronous and tied to Unity lifecycle/events; no explicit async handling.
- Performance
  - Each Save/Load call touches transform data; multiple booleans allow selective saves to minimize work.

```

```text
5) Example
// Minimal runtime usage: attach and enable saving of position/rotation/scale on Awake
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    void Awake()
    {
        var auto = gameObject.AddComponent<BayatGames.SaveGameFree.SaveGameAuto>();
        auto.positionIdentifier = "pos";
        auto.rotationIdentifier = "rot";
        auto.scaleIdentifier = "scl";
        auto.saveOnAwake = true;
        auto.loadOnStart = true;
        auto.savePosition = true;
        auto.saveRotation = true;
        auto.saveScale = true;
        auto.defaultPosition = Vector3.zero;
        auto.defaultRotation = Vector3.zero;
        auto.defaultScale = Vector3.one;
    }
}

```

```text
6) Unknowns
- Exact behavior of SaveGame.Save/Load for various edge cases (e.g., missing identifiers, I/O errors) is not defined in this file.
- Interaction details with external SaveGameFree components (e.g., default implementations for serializer/encoder/encoding) are not shown here.
- Any side effects beyond transform updates (e.g., triggering events, callbacks) are not documented in this file.
