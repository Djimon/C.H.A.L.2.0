# Assets/src/xTernal/SaveGameFree/Editor/Tests/SaveGameTests.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines unit tests for the `SaveGame` functionality in the `BayatGames.SaveGameFree` namespace.

# Public API
- Namespace: `BayatGames.SaveGameFree.Tests`
- Types
  - `public class SaveGameTests`
    - Public methods:
      - `public void SaveTests()`
        - Tests saving functionality, including null/empty identifiers and basic save/load operations.
      - `public void LoadTests()`
        - Tests loading functionality, including null/empty identifiers and default value returns.
      - `public void ExistsTests()`
        - Tests existence checks for saved data, including null/empty identifiers.
      - `public void DeleteTests()`
        - Tests deletion of saved data, including null/empty identifiers.
      - `public void ClearTests()`
        - Tests clearing all saved data.

# Key Behavior & Side Effects
- Each test method verifies specific behaviors of the `SaveGame` API.
- Tests assert that exceptions are thrown for null or empty identifiers.
- Tests check the existence of saved data and validate the correctness of loaded values.
- The `SaveGame.Clear()` method is called at the end of each test to reset the state.

# Constraints & Failure Modes
- Tests expect `SaveGame` methods to handle null and empty string inputs by throwing exceptions.
- The tests assume that the `SaveGame` API is functioning correctly and that the state is reset after each test.

# Example
```csharp
[Test]
public void ExampleTest()
{
    SaveGame.Save<string>("example/save", "example");
    Assert.IsTrue(SaveGame.Exists("example/save"));
    Assert.AreEqual(SaveGame.Load<string>("example/save", "default"), "example");
    SaveGame.Clear();
}
```

# Unknowns
- The implementation details of the `SaveGame` class and its methods are not provided in this file.
```
