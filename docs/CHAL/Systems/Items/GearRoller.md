# Assets/src/Systems/Items/Gear/GearRoller.cs

_Automatically generated/updated from `Assets/src/Systems/Items/Gear/GearRoller.cs`._

# Purpose
- Defines the `GearRoller` class for rolling gear attributes such as implicits and affixes based on game balance configurations.

# Public API
- Namespace: `CHAL.Systems.Items`
- Types
  - `public sealed class GearRoller`
    - **Public fields/properties**: 
      - None
    - **Public methods**:
      - `public List<ImplicitRoll> RollImplicits(GearType gearType, ArmorClass armorClass, GearBaseTier baseTier, System.Random rng, List<ImplicitRoll> outRolls = null) : List<ImplicitRoll>`
      - `public List<AffixRoll> RollAffixes(GearType gearType, GearBaseTier baseTier, System.Random rng, AffixFamily? chosenFamily = null, List<AffixRoll> outRolls = null) : List<AffixRoll>`

# Key Behavior & Side Effects
- Rolls implicits and affixes based on specified gear type, armor class, and base tier.
- Utilizes random number generation for selection and rolling values.
- Logs an error if the balance configuration or mod registry is missing.

# Constraints & Failure Modes
- Returns an empty list if no implicits or affixes can be rolled due to configuration limits or missing data.
- Handles null or empty lists for output rolls by initializing them as needed.
- Avoids duplicates in rolled implicits and affixes based on existing rolls.

# Example
```csharp
var gearRoller = new GearRoller(balanceConfig, gearModRegistry);
var implicits = gearRoller.RollImplicits(GearType.Weapon, ArmorClass.None, GearBaseTier.T1, new Random());
var affixes = gearRoller.RollAffixes(GearType.Armor, GearBaseTier.T2, new Random());
```

# Unknowns
- The structure and contents of `GameBalanceConfig`, `GearModRegistry`, `ImplicitRoll`, `AffixRoll`, `ImplicitDef`, `AffixDef`, and related types are not defined in this file.

