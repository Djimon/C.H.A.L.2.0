# CHAL.Systems.Unit.IUnitController

_Automatically generated/updated from `Assets/src/Systems/Unit/IUnitController.cs`._

Purpose
- Defines the IUnitController interface in the CHAL.Systems.Unit namespace.
- Declares a contract to obtain an associated EffectReceiver from a unit.

Public API
- Namespace/module: CHAL.Systems.Unit
- Types
  - public interface IUnitController
    - Public methods
      - EffectReceiver GetEffectReceiver();

Key Behavior & Side Effects
- No behavior is defined in this file; it only declares a contract.
- Implementors decide how to supply or construct the associated EffectReceiver.

Constraints & Failure Modes
- None explicit in this file.
- No guidance on nullability, threading, or asynchronous behavior.

Example
```csharp
namespace Example
{
    public class MyUnitController : CHAL.Systems.Unit.IUnitController
    {
        public CHAL.Systems.Unit.EffectReceiver GetEffectReceiver()
        {
            // Implementation-specific
            throw new System.NotImplementedException();
        }
    }
}
```

Unknowns
- Definition and location of EffectReceiver are not provided here.
- Behavior, nullability, and lifecycle of the returned EffectReceiver are unspecified.
- Any dependencies or context required by implementors are not described.
