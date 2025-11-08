# CHAL.Systems.Unit.IUnitController

_Automatically generated/updated from `Assets/src/Systems/Unit/IUnitController.cs`._

# Purpose
- Defines the `IUnitController` interface for unit control in the system.

# Public API
- Namespace: `CHAL.Systems.Unit`
- Types
  - public interface `IUnitController`
    - Public methods
      - `EffectReceiver GetEffectReceiver();` - Retrieves the `EffectReceiver` associated with the unit.

# Key Behavior & Side Effects
- No explicit state changes or error handling defined in this interface.

# Constraints & Failure Modes
- No specific guards, null/empty handling, or threading/async notes evident.

# Example
```csharp
public class UnitController : IUnitController
{
    private EffectReceiver effectReceiver;

    public EffectReceiver GetEffectReceiver()
    {
        return effectReceiver;
    }
}
```

# Unknowns
- No unknowns present; all information is derived from the file.
