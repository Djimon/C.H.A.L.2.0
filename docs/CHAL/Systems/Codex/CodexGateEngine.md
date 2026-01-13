# Assets/src/Systems/Research/CodexGateEngine.cs

_Automatically generated/updated from `Assets/src/Systems/Research/CodexGateEngine.cs`._

# Purpose
- Defines the `CodexGateEngine` class for managing visibility and availability of groups and deeds in a deterministic gate system.

# Public API
- Namespace: `CHAL.Systems.Codex`
- Types
  - `public sealed class CodexGateEngine`
    - **Constructor**
      - `CodexGateEngine(CodexDef def, CodexState state, Config cfg = default)`
    - **Public Methods**
      - `public GroupGateState ComputeGroupGate(string groupId)`: Computes visibility and completion state of a group.
      - `public DeedGateState ComputeDeedGate(string deedId)`: Computes visibility and availability state of a deed.

  - `public readonly struct Config`
    - Public fields:
      - `public readonly bool chainVisibilityClampEnabled`: Enables visibility clamping for future deeds.
      - `public readonly int maxFutureDeedsVisible`: Maximum number of future deeds visible when clamping is enabled.

# Key Behavior & Side Effects
- `ComputeGroupGate` and `ComputeDeedGate` methods return states indicating visibility and availability based on internal logic and configurations.
- Visibility of deeds is influenced by their associated groups and the configuration settings.

# Constraints & Failure Modes
- Throws `ArgumentNullException` if `def` or `state` is null during construction.
- Returns default states for groups and deeds if IDs are invalid or not found.
- Handles visibility clamping based on configuration settings.

# Example
```csharp
var engine = new CodexGateEngine(def, state, new CodexGateEngine.Config(true, 3));
var groupState = engine.ComputeGroupGate("groupId");
var deedState = engine.ComputeDeedGate("deedId");
```

# Unknowns
- The exact structure and properties of `CodexDef`, `CodexState`, `CodexChapter`, `CodexChapterGroup`, and `DeedSlot` are not defined in this file.

