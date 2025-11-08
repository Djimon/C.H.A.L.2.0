# BayatGames.SaveGameFree.Examples.ExampleSaveWeb

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Save Web/ExampleSaveWeb.cs`._

# Purpose
- Manages saving and loading game data over the web.

# Public API
- Namespace: BayatGames.SaveGameFree.Examples
- Types
  - public class ExampleSaveWeb : MonoBehaviour
    - Public fields/properties:
      - Transform target
      - bool loadOnStart
      - string identifier
      - string username
      - string password
      - string url
      - bool encode
      - string encodePassword
    - Public methods:
      - void Load()
      - void Save()

# Key Behavior & Side Effects
- On `Start()`, calls `Load()` to load game data.
- `Load()` initiates an asynchronous operation to download game data.
- `Save()` initiates an asynchronous operation to upload game data.
- `LoadEnumerator()` and `SaveEnumerator()` handle the actual download and upload processes, respectively.

# Constraints & Failure Modes
- Uses `IEnumerator` for asynchronous operations.
- Assumes valid URL and credentials for web operations.
- Handles position updates based on user input in `Update()`.

# Example
```csharp
public class GameController : ExampleSaveWeb
{
    void Start()
    {
        if (loadOnStart)
        {
            Load();
        }
    }
}
```

# Unknowns
- None.

