# Assets/src/Systems/_test/DebugCraftingRunner.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a `CraftingDebugRunner` MonoBehaviour for testing crafting functionality in a game.
- Provides methods to run crafting simulations and manage inventory and currency.

## Public API
- Namespace: None
- Types
  - `public sealed class CraftingDebugRunner : MonoBehaviour`
    - Public fields/properties:
      - `public CraftingCatalog catalog;`
      - `public int recipeIndex;`
      - `public string materialsInventoryId;`
      - `public string outputInventoryId;`
      - `public bool runOnStart;`
      - `public bool simulateCurrencyMissing;`
      - `public int grantCrafts;`
    - Public methods:
      - `void Awake()`
      - `void Start()`
      - `[ContextMenu("RunOnce")] public void RunOnce()`
      - `[ContextMenu("GrantRequirements")] public void GrantRequirements()`

## Key Behavior & Side Effects
- `Awake`: Initializes inventory and wallet from `GameManager`.
- `Start`: Ensures inventory instances exist and optionally runs crafting on start.
- `RunOnce`: Attempts to craft an item and logs success or failure.
- `GrantRequirements`: Fulfills material and currency requirements for crafting and logs the results.
- `PrintPreview`: Displays crafting requirements and current inventory status.

## Constraints & Failure Modes
- `RunOnce` and `GrantRequirements` depend on valid `recipeIndex` and `catalog`.
- `GrantRequirements` handles missing materials and currency by logging warnings.
- `WalletProxyMissing` simulates a scenario where currency is unavailable, forcing crafting failures.

## Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public CraftingDebugRunner craftingDebugRunner;

    void Start()
    {
        craftingDebugRunner.RunOnce();
        craftingDebugRunner.GrantRequirements();
    }
}
```

## Unknowns
- The structure and contents of `CraftingCatalog`, `RecipeDef`, `IInventoryDomain`, and `IWallet` cannot be determined from this file.
```
