# BayatGames.SaveGameFree.Examples.ExampleSaveRotation

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Rotation/ExampleSaveRotation.cs`._

1) Purpose
- Defines a Unity MonoBehaviour example that saves/loads a Transform's rotation using BayatGames.SaveGameFree.
- Exposes public fields to configure: target (Transform), loadOnStart (bool), identifier (string).
- On Start, optionally loads saved rotation into the target.
- On each Update, rotates the target around the Z axis based on the Horizontal input.
- On application quit, saves the current target rotation.
- Save/Load methods wrap SaveGame.Save<QuaternionSave> and SaveGame.Load<QuaternionSave> using the configured identifier and active serializer.

2) Public API
- Namespace/module
  - BayatGames.SaveGameFree.Examples
- Types
  - public class ExampleSaveRotation : MonoBehaviour
    - Public fields
      - public Transform target
        - Rotation target to save/load (modified in Update)
      - public bool loadOnStart
        - If true, loads saved rotation in Start()
      - public string identifier
        - Save file/name used by Save/Load
    - Public methods
      - public void Save()
        - Saves target.rotation via SaveGame.Save<QuaternionSave>(identifier, target.rotation, SerializerDropdown.Singleton.ActiveSerializer)
      - public void Load()
        - Loads rotation via SaveGame.Load<QuaternionSave>(identifier, Quaternion.identity, SerializerDropdown.Singleton.ActiveSerializer)
        - Applies loaded value to target.rotation

3) Key Behavior & Side Effects
- Start
  - If loadOnStart is true, calls Load()
- Update
  - Reads horizontal input: rotation.z += Input.GetAxis("Horizontal")
  - Applies rotation: target.rotation = Quaternion.Euler(rotation)
- OnApplicationQuit
  - Calls Save()
- Save
  - Persists target.rotation to storage using identifier and the active serializer
- Load
  - Retrieves saved Quaternion (defaulting to Quaternion.identity) and assigns to target.rotation

4) Constraints & Failure Modes
- No null checks for target before using target.rotation; potential NullReferenceException if target is not assigned.
- Identifier, target, and serializer rely on external configuration (Inspector or code); null serializer could cause errors.
- Uses Unity's "Horizontal" input axis; behavior depends on input setup.
- No explicit error handling or asynchronous behavior in Save/Load wrappers.

5) Example
- Skipped: trivial/auto-generated.

6) Unknowns
- Exact behavior/format of QuaternionSave and how it maps to Quaternion in saved data.
- Details of SerializerDropdown.Singleton.ActiveSerializer type and behavior when null.
- Storage location, file format, and error handling when a save/load fails (beyond the default value in Load).
- Thread-safety and any cross-scene persistence implications.

