# Assets/src/xTernal/SaveGameFree/Examples/Save Position/ExampleSavePosition.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a Unity MonoBehaviour for saving and loading the position of a target Transform.
- Implements a simple serialization mechanism using the SaveGameFree library.

## Public API
- Namespace: `BayatGames.SaveGameFree.Examples`
- Types
  - `public class StorageSG`
    - Public fields/properties:
      - `System.DateTime myDateTime`: Stores the current UTC date and time.
  - `public class ExampleSavePosition : MonoBehaviour`
    - Public fields/properties:
      - `Transform target`: The Transform to save/load position.
      - `bool loadOnStart`: Indicates whether to load position on start.
      - `string identifier`: The identifier for saving/loading position data.
    - Public methods:
      - `void Save()`: Saves the target's position using the specified identifier.
      - `void Load()`: Loads the target's position from the specified identifier.

## Key Behavior & Side Effects
- `Start()`: Initializes encoding settings, saves a `StorageSG` instance, and optionally loads the target's position.
- `Update()`: Updates the target's position based on user input (Horizontal and Vertical axes).
- `OnApplicationQuit()`: Calls `Save()` to persist the target's position when the application quits.

## Constraints & Failure Modes
- The `target` Transform must be assigned; otherwise, position updates will fail.
- The `identifier` must be unique for each save to avoid overwriting data.
- Assumes `SerializerDropdown.Singleton.ActiveSerializer` is properly initialized.

## Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public ExampleSavePosition exampleSavePosition;

    void Start()
    {
        exampleSavePosition.Load(); // Load position at start
    }
}
```

## Unknowns
- The behavior of `SerializerDropdown.Singleton.ActiveSerializer` is not defined in this file.
- The implementation details of `SaveGame.Save<T>()` and `SaveGame.Load<T>()` are not provided.
```
