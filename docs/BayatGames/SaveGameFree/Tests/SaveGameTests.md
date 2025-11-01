# BayatGames.SaveGameFree.Tests.SaveGameTests

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Editor/Tests/SaveGameTests.cs`._

```csharp
1) Purpose
- Defines NUnit-based unit tests for SaveGame functionality (Save, Load, Exists, Delete, Clear).
- Exercises edge cases for identifiers and normal save/load workflows.
- Scoped to BayatGames.SaveGameFree.Tests namespace.

2) Public API
- Namespace/module
  - BayatGames.SaveGameFree.Tests

- Types
  - public class SaveGameTests
    - public void SaveTests()
    - public void LoadTests()
    - public void ExistsTests()
    - public void DeleteTests()
    - public void ClearTests()

3) Key Behavior & Side Effects
- SaveTests
  - Saving with a null identifier triggers an exception (Assert.Catch).
  - Saving with an empty identifier triggers an exception (Assert.Catch).
  - Simple save/load sequence:
    - Save<string>("test/save", "saved")
    - Exists("test/save") -> true
    - Load<string>("test/save", "not saved") -> "saved"
  - Cleanup: SaveGame.Clear()

- LoadTests
  - Loading with a null identifier triggers an exception (Assert.Catch).
  - Loading with an empty identifier triggers an exception (Assert.Catch).
  - Simple save/load sequence:
    - Save<string>("test/load", "saved")
    - Exists("test/load") -> true
    - Load<string>("test/load", "not saved") -> "saved"
  - Reset/default behavior for non-existent key:
    - Exists("test/load2") -> false
    - Load<string>("test/load2", "not saved") -> "not saved"
  - Cleanup: SaveGame.Clear()

- ExistsTests
  - Checking with null identifier triggers an exception (Assert.Catch).
  - Checking with empty identifier triggers an exception (Assert.Catch).
  - Existence flow:
    - Exists("test/exists") -> false
    - Save<string>("test/exists", "saved")
    - Exists("test/exists") -> true
  - Cleanup: SaveGame.Clear()

- DeleteTests
  - Deleting with null identifier triggers an exception (Assert.Catch).
  - Deleting with empty identifier triggers an exception (Assert.Catch).
  - Simple delete flow:
    - Save<string>("test/delete", "saved")
    - Exists("test/delete") -> true
    - Delete("test/delete")
    - Exists("test/delete") -> false
    - Load<string>("test/delete", "not saved") -> "not saved"
  - Cleanup: SaveGame.Clear()

- ClearTests
  - Save<string>("test/clear", "saved")
  - SaveGame.Clear()
  - Exists("test/clear") -> false
  - Load<string>("test/clear", "not saved") -> "not saved"

4) Constraints & Failure Modes
- Null/empty identifiers cause exceptions for Exists, Load, Save, Delete (as tested via Assert.Catch).
- No explicit tests for null values in Save payload; behavior outside tests is not defined here.
- No threading/async behavior shown; all calls are synchronous in tests.
- No explicit exception types asserted; tests only verify that an exception is thrown.

5) Example
```csharp
// Example usage (inferred from tests)
SaveGame.Save<string>("example/key", "value");
string v = SaveGame.Load<string>("example/key", "default"); // -> "value"
bool exists = SaveGame.Exists("example/key"); // -> true
SaveGame.Delete("example/key");
SaveGame.Clear();
```

6) Unknowns
- Exact exception types thrown by null/empty identifier usage (only that an exception is caught).
- Internal implementation details of SaveGame (storage backend, serialization, cross-platform behavior).
- Whether SaveGame.Save accepts null payloads for generic types.
- Thread-safety and potential race conditions.
- Any Unity-specific editor/runtime nuances not evident from this test file.
