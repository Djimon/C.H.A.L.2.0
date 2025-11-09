# Assets/src/Systems/_test/DebugCraftingRunner.cs

_Automatically generated/updated from `Assets/src/Systems/_test/DebugCraftingRunner.cs`._

# Purpose
- Defines a debug runner for crafting operations in a Unity game.

# Public API
- Namespace: None
- Types
  - public sealed class CraftingDebugRunner : MonoBehaviour
    - Public fields/properties:
      - public CraftingCatalog catalog; // Catalog of crafting recipes.
      - public int recipeIndex; // Index of the selected recipe.
      - public string materialsInventoryId; // Identifier for the materials inventory.
      - public string outputInventoryId; // Identifier for the output inventory.
      - public bool runOnStart; // Flag to run crafting on start.
      - public bool simulateCurrencyMissing; // Flag to simulate missing currency.
      - public int grantCrafts; // Number of crafts to grant.
    - Public methods:
      - void Awake(); // Initializes inventory and wallet on awake.
      - void Start(); // Ensures inventory instances and runs crafting if flagged.
      - [ContextMenu("RunOnce")] void RunOnce(); // Executes a crafting operation once.
      - [ContextMenu("GrantRequirements")] void GrantRequirements(); // Grants required materials for crafting.
  
  - private sealed class WalletProxyMissing : IWallet
    - Public methods:
      - int GetCurrency(string id); // Retrieves the current amount of currency for the specified identifier.
      - bool CanSpend(string id, int amt); // Checks if a specified amount of currency can be spent.
      - bool SpendCurrency(string id, int amt); // Deducts a specified amount of currency.
      - void Refund(string id, int amt); // Processes a refund for a specified amount.

# Key Behavior & Side Effects
- `Awake`: Initializes the inventory and wallet.
- `Start`: Ensures the existence of specified inventory instances and optionally runs crafting.
- `RunOnce`: Attempts to craft an item using the selected recipe and logs success or failure.
- `GrantRequirements`: Prepares the inventory and wallet for crafting by simulating the addition of required materials and currency.

# Constraints & Failure Modes
- `RunOnce` and `GrantRequirements` depend on valid recipe indices and inventory states.
- `WalletProxyMissing` simulates currency absence, causing crafting to fail if invoked.
- The crafting operation may fail due to insufficient materials or currency, which is logged.

# Example
```csharp
CraftingDebugRunner debugRunner = new CraftingDebugRunner();
debugRunner.recipeIndex = 0; // Set the desired recipe index
debugRunner.RunOnce(); // Execute crafting operation
```

# Unknowns
- The specific implementation details of `CraftingCatalog`, `CraftingService`, and `InventoryDomain` are not provided in this file.

