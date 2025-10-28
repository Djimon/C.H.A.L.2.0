# Assets/src/xTernal/SaveGameFree/Examples/Save Scale/ExampleSaveScale.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines an example MonoBehaviour for saving and loading the scale of a target Transform.

# Public API
- Namespace: `BayatGames.SaveGameFree.Examples`
- Types
  - `public class ExampleSaveScale : MonoBehaviour`
    - Public fields/properties:
      - `public Transform target;` - The Transform whose scale will be modified.
      - `public bool loadOnStart;` - Indicates whether to load scale on start.
      - `public string identifier;` - The identifier for saving/loading scale data.
    - Public methods:
      - `public void Save();` - Saves the current scale of the target Transform.
      - `public void Load();` - Loads the scale for the target Transform.

# Key Behavior & Side Effects
- `Start()`: Loads the scale on start if `loadOnStart` is true.
- `Update()`: Modifies the target's scale based on horizontal and vertical input.
- `OnApplicationQuit()`: Saves the current scale when the application quits.

# Constraints & Failure Modes
- No explicit null or empty handling for `target`.
- Assumes `target` is assigned before `Load()` or `Save()` is called.
- Uses `Input.GetAxis` for scale modification, which may not be responsive if input is not set up correctly.

# Example
```csharp
void Start()
{
    ExampleSaveScale example = new ExampleSaveScale();
    example.target = someTransform; // Assign a Transform
    example.loadOnStart = true;
    example.identifier = "exampleSaveScale.dat";
    example.Start(); // Loads scale if loadOnStart is true
}
```

# Unknowns
- The behavior of `SerializerDropdown.Singleton.ActiveSerializer` is not defined in this file.
```
