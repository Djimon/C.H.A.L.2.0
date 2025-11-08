# BayatGames.SaveGameFree.Examples.ExampleSavePosition

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Position/ExampleSavePosition.cs`._

# Purpose
- Manages saving and loading game positions.

# Public API
- Namespace: BayatGames.SaveGameFree.Examples
- Types
  - public class StorageSG
    - Public fields/properties:
      - System.DateTime myDateTime: Stores the current UTC date and time.
    - Public methods:
      - StorageSG(): Constructor that initializes myDateTime to the current UTC date and time.
  - public class ExampleSavePosition : MonoBehaviour
    - Public fields/properties:
      - Transform target: The target object whose position will be saved and loaded.
      - bool loadOnStart: Indicates whether to load the saved position on start.
      - string identifier: The identifier for the saved position file.
    - Public methods:
      - void Save(): Saves the current position of the target.
      - void Load(): Loads the saved position and updates the target's position.

# Key Behavior & Side Effects
- On Start, initializes encoding settings and saves the current date and time.
- Updates the target's position based on user input in the Update method.
- Saves the target's position when the application quits.

# Constraints & Failure Modes
- The Save and Load methods rely on the SaveGame system and its serializers.
- If the saved data does not exist, Load will return the default Vector3.zero.

# Example
```csharp
void Start()
{
    ExampleSavePosition example = new ExampleSavePosition();
    example.Load();
}
```

# Unknowns
- None.

