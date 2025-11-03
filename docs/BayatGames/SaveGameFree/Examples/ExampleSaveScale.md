# BayatGames.SaveGameFree.Examples.ExampleSaveScale

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Scale/ExampleSaveScale.cs`._

Purpose
- Defines a Unity MonoBehaviour that saves and loads a Transform’s localScale using BayatGames.SaveGameFree.
- Supports optional automatic load on Start via loadOnStart.
- Saves on application quit using the configured identifier and active serializer.

Public API
- Namespace/Module: BayatGames.SaveGameFree.Examples
- Type: class ExampleSaveScale : MonoBehaviour
- Public fields
  - Transform target
    - The transform whose localScale is saved/loaded
  - bool loadOnStart
    - If true, Load() is called from Start()
  - string identifier
    - Save file key/name used by SaveGame
- Public methods
  - void Save()
    - Saves target.localScale via SaveGame.Save<Vector3Save> (identifier, target.localScale, SerializerDropdown.Singleton.ActiveSerializer)
  - void Load()
    - Loads a Vector3Save from identifier; on missing data, uses new Vector3Save(1f, 1f, 1f); assigns to target.localScale
- Note: Unity lifecycle methods (private) exist: Start, Update, OnApplicationQuit

Key Behavior & Side Effects
- Start()
  - If loadOnStart is true, calls Load()
- Update()
  - Reads Input.GetAxis("Horizontal") and Input.GetAxis("Vertical")
  - Increments target.localScale.x and .y by axis values
  - Writes back to target.localScale
- OnApplicationQuit()
  - Calls Save()
- Save()
  - Persists target.localScale as Vector3Save using identifier and the active serializer
- Load()
  - Retrieves Vector3Save from identifier; uses default Vector3Save(1,1,1) if none; assigns to target.localScale

Constraints & Failure Modes
- No null checks for target; if target is null, operations will throw NullReferenceException
- Depends on SerializerDropdown.Singleton.ActiveSerializer; if null, Save/Load may fail
- Load uses a default Vector3Save(1f, 1f, 1f) if nothing has been saved yet
- Update relies on Unity input axes named "Horizontal" and "Vertical"
- Behavior assumes the SaveGameFree library is present and correctly configured

Example
- Minimal usage example (attach to a GameObject and configure target):
```csharp
// Minimal usage: attach to a GameObject and assign target
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    public Transform objectToScale;

    void Start()
    {
        var ess = gameObject.AddComponent<BayatGames.SaveGameFree.Examples.ExampleSaveScale>();
        ess.target = objectToScale;
        ess.identifier = "exampleScale.dat";
        ess.loadOnStart = true;
    }
}
```

Unknowns
- Behavior when target is not assigned (null) is not defined beyond potential runtime error.
- External library specifics (e.g., exact behavior of active serializer) are not defined in this file.
- Any platform-specific serialization nuances or error handling within SaveGameFree are not described here.

