# global.DebugCraftingRunner

_Automatically generated/updated from `Assets/src/Systems/_test/DebugCraftingRunner.cs`._

# CraftingDebugRunner.cs Documentation

## Purpose
- Provides a debug interface for testing crafting operations in the game.
- Manages crafting inventory and currency simulation.

## Public API
- Namespace: None

- Types
  - public sealed class CraftingDebugRunner : MonoBehaviour
    - Public fields/properties:
      - public CraftingCatalog catalog: Catalog of available crafting recipes.
      - public int recipeIndex: Index of the selected recipe in the catalog.
      - public string materialsInventoryId: Identifier for the materials inventory.
      - public string outputInventoryId: Identifier for the output inventory.
      - public bool runOnStart: Flag to run crafting on start.
      - public bool simulateCurrencyMissing: Flag to simulate missing currency.
      - public int grantCrafts: Number of crafts to grant materials for.
    - Public methods:
      - void Awake(): Initializes inventory and wallet on object creation.
      - void Start(): Ensures inventory instances and optionally runs crafting.
      - [ContextMenu("RunOnce")] void RunOnce(): Executes a crafting operation once.
      - [ContextMenu("GrantRequirements")] void GrantRequirements(): Grants required materials for crafting.
  
  - private sealed class WalletProxyMissing : IWallet
    - Public methods:
      - int GetCurrency(string id): Retrieves the current amount of currency (always returns 0).
      - bool CanSpend(string id, int amt): Checks if a specified amount of currency can be spent (always returns false).
      - bool SpendCurrency(string id, int amt): Deducts a specified amount of currency (always fails).
      - void Refund(string id, int amt): Processes a refund for a specified amount.

## Key Behavior & Side Effects
- `Awake`: Initializes the inventory and wallet from the game manager.
- `Start`: Ensures the existence of specified inventory instances and runs crafting if `runOnStart` is true.
- `RunOnce`: Attempts to craft an item using the selected recipe and logs success or failure.
- `GrantRequirements`: Prepares the inventory by granting required materials and currency for the selected recipe (currently commented out).
- `PrintPreview`: Displays a preview of the crafting operation, including material and currency requirements.

## Constraints & Failure Modes
- The crafting operation may fail due to insufficient materials or currency, which is logged.
- The `WalletProxyMissing` class simulates a scenario where currency is always insufficient, forcing failures in spending currency.

## Example
```csharp
CraftingDebugRunner debugRunner = new CraftingDebugRunner();
debugRunner.catalog = someCraftingCatalog; // Assign a valid CraftingCatalog
debugRunner.recipeIndex = 0; // Select the first recipe
debugRunner.RunOnce(); // Execute crafting operation
```

## Unknowns
- The exact structure and contents of `CraftingCatalog`, `RecipeDef`, and other referenced types are not defined in this file.
