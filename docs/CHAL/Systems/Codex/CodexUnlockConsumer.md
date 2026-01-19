# Assets/src/Systems/Research/CodexUnlockConsumer.cs

_Automatically generated/updated from `Assets/src/Systems/Research/CodexUnlockConsumer.cs`._

# Purpose
- Translates unlocks into real gameplay effects.

# Public API
- Namespace: `CHAL.Systems.Codex`
- Types
  - `public sealed class CodexUnlockConsumer`
    - **Public fields/properties**: None
    - **Public methods**:
      - `public CodexUnlockConsumer(CodexService codexService)` - Constructor that initializes the consumer with a CodexService instance.
      - `public void Apply(string deedId, IReadOnlyList<CodexUnlock> unlocks)` - Applies the unlocks associated with a deedId; logs the number of features unlocked.

# Key Behavior & Side Effects
- The `Apply` method processes a list of `CodexUnlock` objects and applies their effects.
- If the `unlocks` list is null or empty, no action is taken.
- The method logs the number of features unlocked using `DebugManager.DevLog`.
- The `ApplyOne` method handles different types of unlocks, currently only `CodexSlots`.

# Constraints & Failure Modes
- Throws `ArgumentNullException` if `codexService` is null during construction.
- The `Apply` method does not execute if `unlocks` is null or empty.
- The `ApplyCodexSlots` method attempts to unlock slots and logs a message if the unlock is blocked.

# Example
```csharp
var codexService = new CodexService();
var unlockConsumer = new CodexUnlockConsumer(codexService);
var unlocks = new List<CodexUnlock> { new CodexUnlock { unlockType = CodexUnlockTypes.CodexSlots, targetId = "2" } };
unlockConsumer.Apply("deed1", unlocks);
```

# Unknowns
- The implementation details of `CodexService` and `CodexUnlock` are not provided in this file.
- The possible unlock types beyond `CodexSlots` are not defined.

