# CHAL.Systems.IUnitController

_Automatically generated/updated from `Assets/src/Systems/Unit/IUnitController.cs`._

# Purpose
- Defines the `IUnitController` interface for unit control in the system.

# Public API
- Namespace: `CHAL.Systems`
- Types
  - `public interface IUnitController`
    - Public methods:
      - `EffectReceiver GetEffectReceiver();` (returns an `EffectReceiver` instance)

# Key Behavior & Side Effects
- No explicit state changes or error handling defined in this file.

# Constraints & Failure Modes
- No guards, null/empty handling, or threading/async notes evident in this file.

# Example
```csharp
public class UnitController : IUnitController
{
    public EffectReceiver GetEffectReceiver()
    {
        // Implementation here
    }
}
```

# Unknowns
- No information on the `EffectReceiver` type or its behavior.
