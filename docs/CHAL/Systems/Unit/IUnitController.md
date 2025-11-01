# CHAL.Systems.Unit.IUnitController

_Automatically generated/updated from `Assets/src/Systems/Unit/IUnitController.cs`._

1) Purpose
- Defines the IUnitController interface in the CHAL.Systems.Unit namespace.
- Declares a contract to expose an EffectReceiver via GetEffectReceiver.

2) Public API
- Namespace/module: CHAL.Systems.Unit
- Types
  - public interface IUnitController
    - Public methods:
      - EffectReceiver GetEffectReceiver()

3) Key Behavior & Side Effects
- No implementations or runtime behavior in this file; it only declares a contract.
- GetEffectReceiver has no documented side effects here; implementations determine how the EffectReceiver is provided.

4) Constraints & Failure Modes
- No nullability, threading, or performance guarantees specified.
- Return value semantics (e.g., nullability) are implementation-defined.

5) Example
```csharp
namespace CHAL.Systems.Unit
{
    public class SimpleUnitController : IUnitController
    {
        private readonly EffectReceiver _receiver;

        public SimpleUnitController(EffectReceiver receiver)
        {
            _receiver = receiver;
        }

        public EffectReceiver GetEffectReceiver()
        {
            return _receiver;
        }
    }
}
```

6) Unknowns
- What EffectReceiver represents exactly.
- Whether GetEffectReceiver may return null and under what conditions.
- How the EffectReceiver is created, stored, and shared across instances.
- Thread-safety and lifecycle guarantees for implementations.
