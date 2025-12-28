# Assets/src/Systems/Items/Gear/GearInstance.cs

_Automatically generated/updated from `Assets/src/Systems/Items/Gear/GearInstance.cs`._

# Purpose
- Defines a `GearInstance` representing a persisted gear item with rolled implicits and affixes.
- Provides structures for `ImplicitRoll` and `AffixRoll` to manage rolled values and their metadata.
- Includes an enumeration `GearBaseTier` to categorize gear items by tier.

# Public API
- Namespace: `CHAL.Data`
- Types
  - **public sealed class** `GearInstance`
    - Public fields/properties:
      - `string instanceId`: Unique identifier for the gear instance (GUID).
      - `string gearItemId`: Reference to the static gear definition (ScriptableObject).
      - `GearBaseTier baseTier`: Base tier of the gear item.
      - `List<ImplicitRoll> implicits`: List of rolled implicits.
      - `List<AffixRoll> affixes`: List of rolled affixes.
    - Public methods:
      - `static GearInstance CreateNew(string gearDefId, GearBaseTier baseTier)`: Creates a new `GearInstance` with a unique ID and specified base tier.
      - `override string ToString()`: Returns a string representation of the `GearInstance`.
      - `bool TryAddImplicit(ImplicitRoll roll, int maxAllowed)`: Attempts to add an implicit roll if within the allowed limit.
      - `bool TryAddAffix(AffixRoll roll, int maxAllowed)`: Attempts to add an affix roll if within the allowed limit.
  
  - **public struct** `ImplicitRoll`
    - Public fields/properties:
      - `string implicitId`: Reference to the implicit definition ID.
      - `float value`: Rolled value.
      - `int slotIndex`: Index for debugging/UI.
      - `GearBaseTier rolledFromTier`: Base tier used for the roll.
    - Public methods:
      - `ImplicitRoll(string implicitId, float value, int slotIndex, GearBaseTier rolledFromTier)`: Constructor to initialize an `ImplicitRoll`.

  - **public struct** `AffixRoll`
    - Public fields/properties:
      - `string affixId`: Reference to the affix definition ID.
      - `float value`: Rolled value.
      - `int slotIndex`: Index for debugging/UI.
      - `GearBaseTier rolledFromTier`: Base tier used for the roll.
    - Public methods:
      - `AffixRoll(string affixId, float value, int slotIndex = 0, GearBaseTier rolledFromTier = GearBaseTier.T1)`: Constructor to initialize an `AffixRoll`.

  - **public enum** `GearBaseTier`
    - Values:
      - `T1 = 1`
      - `T2 = 2`
      - `T3 = 3`

# Key Behavior & Side Effects
- `CreateNew` generates a new `GearInstance` with a unique `instanceId` and initializes lists for implicits and affixes.
- `TryAddImplicit` and `TryAddAffix` methods manage the addition of rolls, ensuring they do not exceed the specified maximum allowed.

# Constraints & Failure Modes
- `TryAddImplicit` and `TryAddAffix` methods check for null lists and maximum allowed counts before adding rolls.
- If `maxAllowed` is less than or equal to zero, or if the current count of rolls meets or exceeds `maxAllowed`, the addition fails.

# Example
```csharp
var gearInstance = GearInstance.CreateNew("gearDefId123", GearBaseTier.T2);
gearInstance.TryAddImplicit(new ImplicitRoll("implicitId456", 10.0f, 0, GearBaseTier.T2), 3);
gearInstance.TryAddAffix(new AffixRoll("affixId789", 5.0f), 3);
```

# Unknowns
- None.
