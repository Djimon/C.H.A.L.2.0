# BayatGames.SaveGameFree.Examples.ExampleSavePosition

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Position/ExampleSavePosition.cs`._

```text
1) Purpose
- Defines a serializable data container StorageSG with a DateTime field to demonstrate saving custom data.
- Defines ExampleSavePosition, a Unity MonoBehaviour that demonstrates saving/loading a Transform's position and a sample StorageSG instance using SaveGameFree.
- Demonstrates configuration of SaveGameFree (encode password, serializer) and simple in-scene position manipulation via input.

```

```text
2) Public API
- Namespace/module
  - BayatGames.SaveGameFree.Examples

- Types
  - public class StorageSG
    - public DateTime myDateTime
    - public StorageSG()  // constructor

  - public class ExampleSavePosition : MonoBehaviour
    - private string _encodePassword
    - public Transform target
    - public bool loadOnStart
    - public string identifier
    - Start()           // Unity lifecycle (not public)
    - Update()          // Unity lifecycle (not public)
    - OnApplicationQuit() // Unity lifecycle (not public)
    - public void Save()
    - public void Load()

```

```text
3) Key Behavior & Side Effects
- StorageSG data container
  - StorageSG.myDateTime is initialized to UTC now in the constructor.

- ExampleSavePosition (Unity MonoBehaviour)
  - Start (Unity lifecycle)
    - Sets _encodePassword to a fixed string.
    - SaveGame.EncodePassword = _encodePassword
    - SaveGame.Encode = true
    - SaveGame.Serializer = new SaveGameFree.Serializers.SaveGameBinarySerializer()
    - Creates a StorageSG instance (ssg) and saves it under the key "pizza2"
    - Loads a StorageSG from key "pizza2" into ssgLoaded
    - Logs ssgLoaded.myDateTime converted to local time
    - If loadOnStart is true, calls Load()

  - Update (Unity lifecycle)
    - Reads Input.GetAxis("Horizontal") and Input.GetAxis("Vertical")
    - Adjusts target.position accordingly (affects x and y components)

  - OnApplicationQuit (Unity lifecycle)
    - Calls Save() to persist current state

  - Save()
    - Saves target.position as Vector3Save under the path identifier
    - Uses SerializerDropdown.Singleton.ActiveSerializer for serialization

  - Load()
    - Loads a Vector3Save from the path identifier
    - Applies the loaded value to target.position
    - Uses Vector3.zero as default if no saved value is found
    - Uses SerializerDropdown.Singleton.ActiveSerializer for deserialization

```

```text
4) Constraints & Failure Modes
- Target assignment risk
  - Update, Save, and Load all access target.position; if target is null, NullReferenceException occurs.

- Dependency on in-scene references
  - Requires target to be assigned in the inspector or at runtime to function.

- Encoding and serialization
  - Encode/password handling is hard-coded; changes require code edit.
  - Uses SaveGameBinarySerializer by default in Start; changing serializer affects compatibility with saved data.

- Default/fallback behavior
  - Load() uses Vector3.zero as a default if no saved data exists under the given identifier.

- Threading and async
  - No explicit threading or async behavior; all operations are synchronous.

```

```text
5) Unknowns
- Details of Vector3Save type and how exactly it maps to Transform.position (implicit conversions or wrappers are not defined in this file).
- Behavior and guarantees of SaveGameFree.Encode/password handling beyond this file.
- Full semantics of SerializerDropdown.Singleton.ActiveSerializer beyond selecting a serializer at runtime.
- Any platform-specific file I/O implications or storage locations used by the underlying SaveGameFree implementation.

```
