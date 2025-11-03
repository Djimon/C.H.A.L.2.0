# BayatGames.SaveGameFree.Examples.StorageSG

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Position/ExampleSavePosition.cs`._

1) Purpose
- Demonstrates saving and loading a position using the SaveGameFree library.

2) Public API
- Namespace: BayatGames.SaveGameFree.Examples
- Types
  - public class StorageSG
    - Public fields/properties:
      - System.DateTime myDateTime: Stores the current UTC date and time.
    - Public methods:
      - StorageSG(): Constructor that initializes myDateTime to the current UTC time.
  
  - public class ExampleSavePosition : MonoBehaviour
    - Public fields/properties:
      - Transform target: The target object whose position will be saved and loaded.
      - bool loadOnStart: Indicates whether to load the position on start.
      - string identifier: The identifier for saving/loading the position.
    - Public methods:
      - void Save(): Saves the current position of the target.
      - void Load(): Loads the saved position into the target.

3) Key Behavior & Side Effects
- On Start, initializes encoding settings and saves the current date and time.
- Updates the target's position based on user input every frame.
- Saves the target's position when the application quits.

4) Constraints & Failure Modes
- The target must be assigned for position saving/loading to work.
- If no saved position exists, Load() defaults to Vector3.zero.

5) Example
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

6) Unknowns
- None.

