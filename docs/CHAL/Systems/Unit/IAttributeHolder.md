# Assets/src/Systems/Unit/IAttributeHolder.cs

_Automatically generated/updated from `Assets/src/Systems/Unit/IAttributeHolder.cs`._

# Purpose
- Defines the `IAttributeHolder` interface for retrieving attribute values.

# Public API
- Namespace: `CHAL.Systems.Unit`
- Types
  - public interface `IAttributeHolder`
    - Public methods
      - `float GetAttributeValue(HeroAttribs attribute);` - Retrieves the value of the specified attribute.

# Key Behavior & Side Effects
- No explicit state changes or error handling defined in this interface.

# Constraints & Failure Modes
- No specific guards or threading considerations are evident.

# Example
```csharp
public class ExampleAttributeHolder : IAttributeHolder
{
    public float GetAttributeValue(HeroAttribs attribute)
    {
        // Implementation here
    }
}
```

# Unknowns
- No unknowns can be determined from this file.
