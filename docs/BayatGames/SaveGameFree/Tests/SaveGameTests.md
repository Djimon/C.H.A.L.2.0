# BayatGames.SaveGameFree.Tests.SaveGameTests

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Editor/Tests/SaveGameTests.cs`._

# Purpose
- Contains tests for saving game data.

# Public API
- Namespace: `BayatGames.SaveGameFree.Tests`
- Types
  - `public class SaveGameTests`
    - Public methods:
      - `public void SaveTests()`
      - `public void LoadTests()`
      - `public void ExistsTests()`
      - `public void DeleteTests()`
      - `public void ClearTests()`

# Key Behavior & Side Effects
- Each test method validates the behavior of the `SaveGame` class with various input identifiers (null, empty, valid).
- Tests include saving, loading, checking existence, deleting, and clearing saved data.
- Each test method ensures that the state is reset by calling `SaveGame.Clear()` at the end.

# Constraints & Failure Modes
- Methods throw exceptions when provided with null or empty identifiers.
- The tests assume that the `SaveGame` class handles saving/loading data correctly.

# Example
```csharp
// Example of a test case for saving data
[Test]
public void SaveTests() {
    SaveGame.Save<string>("test/save", "saved");
    Assert.IsTrue(SaveGame.Exists("test/save"));
    Assert.AreEqual(SaveGame.Load<string>("test/save", "not saved"), "saved");
}
```

# Unknowns
- The implementation details of the `SaveGame` class are not provided in this file.

