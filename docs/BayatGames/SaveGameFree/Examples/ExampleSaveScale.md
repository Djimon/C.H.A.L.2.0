# BayatGames.SaveGameFree.Examples.ExampleSaveScale

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Scale/ExampleSaveScale.cs`._

# Purpose
- Manages saving and loading of scale data for a target Transform.
- Can automatically load data on start based on configuration.

# Public API
- Namespace: `BayatGames.SaveGameFree.Examples`
- Types
  - `public class ExampleSaveScale : MonoBehaviour`
    - Public fields/properties:
      - `public Transform target;` - The Transform whose scale is managed.
      - `public bool loadOnStart;` - Indicates if data should be loaded on start.
      - `public string identifier;` - The identifier for saving/loading scale data.
    - Public methods:
      - `public void Save();` - Saves the current scale of the target Transform.
      - `public void Load();` - Loads the saved scale and applies it to the target Transform.

# Key Behavior & Side Effects
- `Start()`: Loads scale data if `loadOnStart` is true.
- `Update()`: Adjusts the target's scale based on horizontal and vertical input.
- `OnApplicationQuit()`: Saves the current scale when the application quits.

# Constraints & Failure Modes
- The `Load()` method defaults to a scale of (1f, 1f, 1f) if no saved data is found.
- Assumes that the `target` Transform is assigned before use.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public ExampleSaveScale exampleSaveScale;

    void Start()
    {
        exampleSaveScale.Load(); // Load scale data on start
    }
}
```

# Unknowns
- None.
