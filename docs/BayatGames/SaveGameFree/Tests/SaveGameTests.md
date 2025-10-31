# BayatGames.SaveGameFree.Tests.SaveGameTests

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Editor/Tests/SaveGameTests.cs`._

# Purpose
- Defines unit tests for the SaveGame functionality in the SaveGameFree library.

# Public API
- Namespace: `BayatGames.SaveGameFree.Tests`
- Types
  - `public class SaveGameTests`
    - Public methods:
      - `void SaveTests()`
      - `void LoadTests()`
      - `void ExistsTests()`
      - `void DeleteTests()`
      - `void ClearTests()`

# Key Behavior & Side Effects
- Tests for saving, loading, checking existence, deleting, and clearing saved data.
- Each test checks for null and empty identifiers, asserting that exceptions are thrown.
- Validates that saved data can be loaded correctly and that it can be deleted.

# Constraints & Failure Modes
- Tests expect `SaveGame` methods to handle null and empty string identifiers by throwing exceptions.
- Assumes `SaveGame.Clear()` resets the state for subsequent tests.

# Example
```csharp
[Test]
public void ExampleTest()
{
    SaveGame.Save<string>("example/save", "data");
    Assert.IsTrue(SaveGame.Exists("example/save"));
    Assert.AreEqual(SaveGame.Load<string>("example/save", "default"), "data");
    SaveGame.Clear();
}
```

# Unknowns
- The implementation details of the `SaveGame` class and its methods are not provided in this file.

