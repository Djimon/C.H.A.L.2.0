# CHAL.Systems.Crafting.CraftingController

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingController.cs`._

# Purpose
- Manages crafting operations and interactions within the game.
- Handles inventory, recipes, and UI elements related to crafting.

# Public API
- Namespace: `CHAL.Systems.Crafting`
- Types
  - public class `CraftingController` : `MonoBehaviour`
    - Public fields/properties:
      - `CraftingCatalog catalog`: Reference to the crafting catalog.
      - `InventoryDomain inv`: Reference to the inventory system.
      - `ResearchUnlockRegistry unlocks`: Reference to the research unlock registry.
      - `RecipeListView listView`: UI component for displaying the list of recipes.
      - `RecipeDetailPanel detailPanel`: UI component for displaying recipe details.
    - Public methods:
      - `void OnEnable()`: Initializes UI wiring.
      - `void OnDisable()`: Cleans up UI wiring.
      - `void HandleSelectRecipe(RecipeDef recipe)`: Updates selected recipe and refreshes details.
      - `void HandleCraftClicked()`: Attempts to craft the selected recipe and updates UI.

# Key Behavior & Side Effects
- On enable, wires UI components for recipe selection and crafting actions.
- On disable, unwires UI components to prevent memory leaks.
- Initializes crafting UI after one frame, checking for necessary components and logging warnings if any are missing.
- Refreshes the recipe list and details based on the current inventory and unlocks.
- Handles crafting logic, including checking if the recipe can be crafted and updating the UI accordingly.

# Constraints & Failure Modes
- If `inv`, `catalog`, or `unlocks` are null during initialization, appropriate warnings are logged, and UI may not initialize.
- Crafting fails if the output inventory is unknown or if there are insufficient materials or currency.
- The system only refreshes the UI when relevant inventory slots change.

# Example
```csharp
CraftingController craftingController = new CraftingController();
craftingController.OnEnable();
```

# Unknowns
- The behavior of `GameManager.Instance` and its properties cannot be determined from this file.
- The structure and contents of `RecipeDef`, `CraftingCatalog`, and other referenced types are not defined in this file.

