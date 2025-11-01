# CHAL.Data.HeroAttribs

_Automatically generated/updated from `Assets/src/Data/Enums/HeroEnums.cs`._

1) Purpose
- Defines data enums used for hero categorization and attributes.
- Provides two public enums under the CHAL.Data namespace.

2) Public API
- Namespace/module
  - CHAL.Data
- Types
  - public enum HeroSlot
    - Members: Head, Charm, Torso, RightHand, LeftHand
  - public enum HeroAttribs
    - Members: STR, DEX, CON, INT, WIL

3) Key Behavior & Side Effects
- None. This file contains only type definitions (enums) with no runtime behavior.

4) Constraints & Failure Modes
- None evident. No guards, async, or threading concerns.

5) Example
```csharp
// Example usage
var slot = CHAL.Data.HeroSlot.Head;
var attrib = CHAL.Data.HeroAttribs.STR;
```

6) Unknowns
- None identifiable beyond the provided definitions.
