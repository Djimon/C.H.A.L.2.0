# global.DebugCraftingRunner

_Automatically generated/updated from `Assets/src/Systems/_test/DebugCraftingRunner.cs`._

# Purpose
- Defines a `CraftingDebugRunner` MonoBehaviour for testing crafting functionality in a Unity game.

# Public API
- Namespace: None
- Types
  - public sealed class CraftingDebugRunner : MonoBehaviour
    - Public fields/properties:
      - public CraftingCatalog catalog
      - public int recipeIndex
      - public string materialsInventoryId
      - public string outputInventoryId
      - public bool runOnStart
      - public bool simulateCurrencyMissing
      - public int grantCrafts
    - Public methods:
      - void Awake()
      - void Start()
      - [ContextMenu("RunOnce")] public void RunOnce()
      - [ContextMenu("GrantRequirements")] public void GrantRequirements()

# Key Behavior & Side Effects
- `Awake`: Initializes inventory and wallet from `GameManager`.
- `Start`: Ensures inventory instances exist and optionally runs crafting on start.
- `RunOnce`: Attempts to craft an item and logs success or failure.
- `GrantRequirements`: Fulfills material and currency requirements for crafting and logs the results.
- `PrintPreview`: Logs the crafting preview details.

# Constraints & Failure Modes
- `RunOnce` and `GrantRequirements` depend on valid `recipeIndex` and `catalog`.
- Handles missing materials and currency gracefully, logging warnings when requirements are not met.
- `WalletProxyMissing` simulates a scenario where currency is unavailable, affecting crafting attempts.

# Example
```csharp
public class ExampleUsage : MonoBehaviour
{
    public CraftingDebugRunner debugRunner;

    void Start()
    {
        if (debugRunner.runOnStart)
        {
            debugRunner.RunOnce();
        }
    }
}
```

# Unknowns
- The structure and contents of `CraftingCatalog`, `RecipeDef`, `IInventoryDomain`, `IWallet`, and `ItemStack` cannot be determined from this file.
- The behavior of `CraftingService` methods is not defined in this file.

