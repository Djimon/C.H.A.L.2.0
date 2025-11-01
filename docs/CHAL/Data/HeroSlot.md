# CHAL.Data.HeroSlot

_Automatically generated/updated from `Assets/src/Data/Enums/HeroEnums.cs`._

Purpose
- Defines two public enums within CHAL.Data: HeroSlot and HeroAttribs.
- Encapsulated in namespace CHAL.Data.

Public API
- Namespace/module: CHAL.Data
- Types
  - public enum HeroSlot
    - Head
    - Charm
    - Torso
    - RightHand
    - LeftHand
  - public enum HeroAttribs
    - STR
    - DEX
    - CON
    - INT
    - WIL

Key Behavior & Side Effects
- None. This file only defines enum types; no runtime behavior or side effects.

Example
```csharp
// Example usage
HeroSlot slot = HeroSlot.RightHand;
HeroAttribs attr = HeroAttribs.DEX;
```

Unknowns
- How these enums are serialized/deserialized in other systems (e.g., JSON, database) is not defined here.
- Underlying numeric values and any explicit underlying type are not specified in this file (default C# behavior applies).
- Usage context and any evolution of these enums outside this file are not defined.
