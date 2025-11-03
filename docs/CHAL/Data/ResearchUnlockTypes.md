# CHAL.Data.ResearchUnlockTypes

_Automatically generated/updated from `Assets/src/Data/Enums/ResearchUnlockTypes.cs`._

```plaintext
Purpose
- Defines ResearchUnlockTypes enum under CHAL.Data namespace.
- Represents unlock type categories for research (WorldTier, CraftingFeature, Recipe, SkillBranch, Hero).
- Includes in-code hints via inline comments (e.g., WorldTier: Maps, difficulties; SkillBranch: Orbit-System und Corwn-sockets).

Public API
- Namespace/Module: CHAL.Data
- Types
  - public enum ResearchUnlockTypes
    - WorldTier = 0 //Maps, difficulties
    - CraftingFeature = 1
    - Recipe = 2
    - SkillBranch = 3 //Orbit-System und Corwn-sockets
    - Hero = 4

Key Behavior & Side Effects
- No runtime behavior defined in this file.
- This is a plain data type; no methods or state changes.

Constraints & Failure Modes
- No explicit guards or validation.
- Values are explicit constants; use as standard enum surface.
- No threading/async considerations present in this file.

Example
```csharp
using CHAL.Data;

public class Example
{
    public void HandleUnlock(ResearchUnlockTypes type)
    {
        switch (type)
        {
            case ResearchUnlockTypes.WorldTier:
                // handle maps/difficulties
                break;
            case ResearchUnlockTypes.CraftingFeature:
                break;
            case ResearchUnlockTypes.Recipe:
                break;
            case ResearchUnlockTypes.SkillBranch:
                break;
            case ResearchUnlockTypes.Hero:
                break;
        }
    }
}
```

Unknowns
- How this enum is used across the rest of the project is not shown here.
- No information on serialization behavior or editor tooling beyond the code.
- Possible additional enum members or underlying type implications beyond what is explicit in this file.
```
