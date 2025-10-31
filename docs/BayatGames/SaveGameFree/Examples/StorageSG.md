# BayatGames.SaveGameFree.Examples.StorageSG

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Position/ExampleSavePosition.cs`._

# Purpose
- Defines a Unity MonoBehaviour for saving and loading the position of a target Transform.

# Public API
- Namespace: BayatGames.SaveGameFree.Examples
- Types
  - public class StorageSG
    - Public fields/properties:
      - System.DateTime myDateTime: Stores the current UTC date and time.
  - public class ExampleSavePosition : MonoBehaviour
    - Public fields/properties:
      - Transform target: The target Transform whose position will be saved/loaded.
      - bool loadOnStart: Indicates whether to load the position on start.
      - string identifier: The identifier for the save file.
    - Public methods:
      - void Save(): Saves the current position of the target Transform.
      - void Load(): Loads the position of the target Transform.

# Key Behavior & Side Effects
- On Start:
  - Sets an encoding password and initializes the SaveGame system.
  - Saves a new StorageSG instance with the current date and time.
  - Loads the saved StorageSG instance and logs the date/time.
  - If loadOnStart is true, calls Load() to set the target's position.
- On Update:
  - Updates the target's position based on user input (Horizontal and Vertical axes).
- On Application Quit:
  - Calls Save() to save the target's position.

# Constraints & Failure Modes
- Assumes that the target Transform is assigned before use.
- Uses a fixed encoding password; security implications should be considered.
- The SaveGame system must be properly initialized for saving/loading to work.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public ExampleSavePosition exampleSavePosition;

    void Start()
    {
        exampleSavePosition.Save();
        exampleSavePosition.Load();
    }
}
```

# Unknowns
- The behavior of the SaveGame system and its serializers is not detailed in this file.
- The structure of Vector3Save is not defined in this file.

