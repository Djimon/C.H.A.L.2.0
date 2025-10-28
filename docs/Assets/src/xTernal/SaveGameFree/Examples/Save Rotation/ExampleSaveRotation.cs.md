# Assets/src/xTernal/SaveGameFree/Examples/Save Rotation/ExampleSaveRotation.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a MonoBehaviour that saves and loads the rotation of a specified Transform.

## Public API
- Namespace: `BayatGames.SaveGameFree.Examples`
- Types
  - `public class ExampleSaveRotation : MonoBehaviour`
    - Public fields/properties:
      - `public Transform target;` - The Transform to rotate.
      - `public bool loadOnStart;` - Indicates if rotation should be loaded on start.
      - `public string identifier;` - The identifier for the save file.
    - Public methods:
      - `public void Save();` - Saves the current rotation of the target Transform.
      - `public void Load();` - Loads the rotation for the target Transform.

## Key Behavior & Side Effects
- `Start()`: Loads the rotation if `loadOnStart` is true.
- `Update()`: Updates the target's rotation based on horizontal input.
- `OnApplicationQuit()`: Saves the current rotation when the application quits.

## Constraints & Failure Modes
- No explicit error handling for save/load operations.
- Assumes `target` is assigned; behavior is undefined if it is null.

## Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public ExampleSaveRotation exampleSaveRotation;

    void Start()
    {
        exampleSaveRotation.Load();
    }
}
```

## Unknowns
- No information on the behavior of `SaveGame.Save` and `SaveGame.Load` methods.
```
