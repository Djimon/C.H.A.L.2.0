# BayatGames.SaveGameFree.Examples.ExampleSavePosition

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Position/ExampleSavePosition.cs`._

# Purpose
- Defines a Unity MonoBehaviour for saving and loading the position of a target Transform.
- Implements a simple serialization mechanism using the SaveGameFree library.

# Public API
- Namespace: `BayatGames.SaveGameFree.Examples`
- Types
  - **public class ExampleSavePosition : MonoBehaviour**
    - Public fields/properties:
      - `Transform target`: The target Transform whose position will be saved/loaded.
      - `bool loadOnStart`: Indicates whether to load the position on start.
      - `string identifier`: The identifier for the saved position file.
    - Public methods:
      - `void Save()`: Saves the current position of the target Transform.
      - `void Load()`: Loads the position of the target Transform from saved data.

# Key Behavior & Side Effects
- On `Start()`, initializes encoding settings and saves the current date/time.
- On `Update()`, updates the target's position based on user input.
- On application quit, triggers the `Save()` method to persist the target's position.
- If `loadOnStart` is true, loads the target's position at the start.

# Constraints & Failure Modes
- Assumes `target` is assigned; no null checks are present.
- Uses a fixed encoding password for serialization.
- Relies on the `SaveGameFree` library for serialization and deserialization.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public ExampleSavePosition exampleSavePosition;

    void Start()
    {
        exampleSavePosition.Load(); // Load position at the start
    }
}
```

# Unknowns
- The behavior of `SerializerDropdown.Singleton.ActiveSerializer` is not defined in this file.
- The structure of `Vector3Save` is not provided in this file.

