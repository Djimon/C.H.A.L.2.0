# Assets/src/Data/Defs/ImplicitGearTypeConfig.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `ImplicitGearTypeConfig` as a ScriptableObject for managing implicit gear types and their weights.

## Public API
- Namespace: `CHAL.Data`
- Types
  - `public class ImplicitGearTypeConfig : ScriptableObject`
    - Public fields/properties:
      - `public List<GearTypePool> Pools`: List of gear type pools with implicit weights.
    - Public methods:
      - `private void OnValidate()`: Validates gear type pools, ensuring no duplicates and clamping weights.
      - `private static bool IsValidId(string id)`: Checks if the given ID is valid based on specific character rules.

  - `public struct GearTypePool`
    - Public fields/properties:
      - `public GearType GearType`: The type of gear.
      - `public List<ImplicitWeight> Entries`: List of implicit weights associated with the gear type.

  - `public struct ImplicitWeight`
    - Public fields/properties:
      - `public string ImplicitId`: Identifier for the implicit weight.
      - `public int Weight`: Weight associated with the implicit ID.

## Key Behavior & Side Effects
- `OnValidate` method:
  - Prevents duplicate implicit IDs within the same gear type.
  - Clamps negative weights to zero.
  - Trims implicit IDs and logs warnings for unusual formats.
  - Adds default implicit IDs with a weight of zero if they are missing.

## Constraints & Failure Modes
- Handles null checks for `Pools` and `Entries`.
- Uses a dictionary to track seen IDs per gear type to avoid duplicates.
- Logs warnings for invalid IDs and duplicates, modifying weights accordingly.

## Example
```csharp
var config = ScriptableObject.CreateInstance<ImplicitGearTypeConfig>();
config.Pools.Add(new GearTypePool { GearType = GearType.Head, Entries = new List<ImplicitWeight>() });
```

## Unknowns
- The definition of `GearType` is not provided in this file.
```
