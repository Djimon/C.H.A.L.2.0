# Assets/src/Systems/Crafting/CraftingController.cs

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingController.cs`._

# Purpose
- Manages crafting operations and interactions within the game.
- Handles inventory, recipes, and UI elements related to crafting.

# Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - public class `CraftingController` [extends `MonoBehaviour`]
    - Public fields/properties:
      - `CraftingCatalog catalog`: Reference to the crafting catalog.
      - `InventoryDomain inv`: Reference to the inventory system.
      - `ResearchUnlockRegistry unlocks`: Reference to the research unlock registry.
      - `RecipeListView listView`: UI component for displaying the list of recipes.
      - `RecipeDetailPanel detailPanel`: UI component for displaying details of a selected recipe.
    - Public methods:
      - `void OnEnable()`: Initializes UI wiring.
      - `void OnDisable()`: Cleans up UI wiring.
      - `void HandleSelectRecipe(RecipeDef recipe)`: Selects a recipe and refreshes the detail panel.
      - `void HandleCraftClicked()`: Attempts to craft the selected recipe and updates the UI.
      - `void HandleSlotChanged(string instanceId, int slotIndex, ItemStack? newStack)`: Refreshes the preview if relevant inventory slots change.

# Key Behavior & Side Effects
- On enabling, the UI is wired to handle recipe selection and crafting actions.
- On disabling, the UI wiring is cleaned up.
- Recipes are rebuilt and displayed based on the current inventory and unlocks.
- Crafting attempts are logged, and UI feedback is provided based on success or failure.

# Constraints & Failure Modes
- If the inventory or wallet is null, the crafting UI will not initialize.
- If the catalog is null or empty, no recipes will be displayed.
- Crafting can fail due to insufficient materials, currency, or full output inventory, with appropriate messages shown in the UI.

# Example
```csharp
CraftingController craftingController = new CraftingController();
craftingController.OnEnable(); // Initializes the crafting UI
```

# Unknowns
- The behavior of `GameManager.Instance` and its properties cannot be determined from this file.
- The structure and contents of `RecipeDef`, `CraftingCatalog`, `InventoryDomain`, and other referenced types are not defined in this file.

