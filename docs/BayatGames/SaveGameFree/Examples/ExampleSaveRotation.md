# BayatGames.SaveGameFree.Examples.ExampleSaveRotation

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Rotation/ExampleSaveRotation.cs`._

# Purpose
- Manages saving and loading of rotation data in a Unity game.

# Public API
- Namespace: `BayatGames.SaveGameFree.Examples`
- Types
  - `public class ExampleSaveRotation : MonoBehaviour`
    - Public fields/properties:
      - `public Transform target;` - The target object whose rotation is managed.
      - `public bool loadOnStart;` - Indicates whether to load rotation on start.
      - `public string identifier;` - The identifier for saving/loading rotation data.
    - Public methods:
      - `public void Save();` - Saves the current rotation of the target object.
      - `public void Load();` - Loads the saved rotation for the target object.

# Key Behavior & Side Effects
- `Start()`: Loads the rotation if `loadOnStart` is true.
- `Update()`: Updates the target's rotation based on horizontal input.
- `OnApplicationQuit()`: Saves the current rotation when the application quits.

# Constraints & Failure Modes
- No explicit guards or null handling present.
- Assumes `target` is assigned before use; otherwise, it may lead to null reference exceptions.

# Example
```csharp
void Start()
{
    ExampleSaveRotation example = new ExampleSaveRotation();
    example.target = someTransform; // Assign a Transform
    example.Save(); // Save the rotation
}
```

# Unknowns
- None.
