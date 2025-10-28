# BayatGames.SaveGameFree.Examples.ExampleSaveScale

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Scale/ExampleSaveScale.cs`._

# Purpose
- Defines an example MonoBehaviour for saving and loading the scale of a target Transform.

# Public API
- Namespace: BayatGames.SaveGameFree.Examples
- Types
  - public class ExampleSaveScale : MonoBehaviour
    - Public fields/properties:
      - Transform target: The target Transform whose scale will be modified.
      - bool loadOnStart: Indicates whether to load scale on start.
      - string identifier: The identifier for saving/loading scale data.
    - Public methods:
      - void Save(): Saves the current scale of the target Transform.
      - void Load(): Loads the scale for the target Transform.

# Key Behavior & Side Effects
- On Start, if `loadOnStart` is true, the `Load()` method is called to set the target's scale.
- In Update, the target's scale is modified based on horizontal and vertical input.
- On application quit, the `Save()` method is called to save the target's scale.

# Constraints & Failure Modes
- The `Load()` method provides a default scale of (1f, 1f, 1f) if no saved data is found.
- Assumes that the `target` Transform is assigned; no null checks are present.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public ExampleSaveScale exampleSaveScale;

    void Start()
    {
        exampleSaveScale.Load(); // Load scale on start
    }
}
```

# Unknowns
- No information on the behavior of `SerializerDropdown.Singleton.ActiveSerializer`.

