# BayatGames.SaveGameFree.Examples.ExampleSaveRotation

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Rotation/ExampleSaveRotation.cs`._

# Purpose
- Defines a MonoBehaviour that saves and loads the rotation of a specified Transform.

# Public API
- Namespace: BayatGames.SaveGameFree.Examples
- Types
  - public class ExampleSaveRotation : MonoBehaviour
    - Public fields/properties:
      - Transform target: The Transform whose rotation will be saved/loaded.
      - bool loadOnStart: Indicates if the rotation should be loaded at the start.
      - string identifier: The file name used for saving/loading the rotation.
    - Public methods:
      - void Save(): Saves the current rotation of the target Transform.
      - void Load(): Loads the rotation for the target Transform.

# Key Behavior & Side Effects
- On Start, if `loadOnStart` is true, the `Load()` method is called to set the target's rotation.
- In Update, the target's rotation is modified based on horizontal input.
- On application quit, the `Save()` method is called to persist the target's rotation.

# Constraints & Failure Modes
- No explicit guards or null handling for `target`; assumes it is assigned.
- No threading or async behavior noted.
- Performance considerations are not explicitly mentioned.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public ExampleSaveRotation exampleSaveRotation;

    void Start()
    {
        exampleSaveRotation.Load(); // Load rotation if loadOnStart is true
    }
}
```

# Unknowns
- No information on the behavior of `SerializerDropdown.Singleton.ActiveSerializer`.
- No details on the implementation of `SaveGame.Save` and `SaveGame.Load`.

