# CHAL.UI.RecipeDetailPanelView

_Automatically generated/updated from `Assets/src/UI/RecipeDetailPanelView.cs`._

# Purpose
- Defines the `RecipeDetailPanel` class for displaying and managing recipe details in the UI.

# Public API
- Namespace: `CHAL.UI`
- Types
  - public sealed class `RecipeDetailPanel` : `MonoBehaviour`
    - Public fields/properties:
      - `event Action OnCraftClicked`: Triggered when the craft button is clicked.
      - `UIDocument doc`: Reference to the UI document.
    - Public methods:
      - `void Clear()`: Resets UI elements to their default state.
      - `void ShowRecipeDetails(RecipeDef r, CraftingService.RecipePreview preview, int needGold, int haveGold, Dictionary<string, int> haveByItemId)`: Displays the details of a recipe.
      - `void ShowFail(string message)`: Displays a failure message to the user.
      - `void ShowSuccess()`: Displays a success message to the user.

# Key Behavior & Side Effects
- `Awake()`: Initializes UI elements and sets up event callbacks for the craft button and refine slider.
- `Clear()`: Resets all displayed fields and UI elements.
- `ShowRecipeDetails()`: Updates the UI with recipe details, checks crafting conditions, and enables/disables the craft button based on available ingredients and gold.
- `ShowFail()`: Updates the failure label with a specified message.
- `ShowSuccess()`: Clears the failure label for a success indication.
- `ToggleTooltip()`: Toggles the visibility of the tooltip and populates it with placeholder data if empty.

# Constraints & Failure Modes
- `doc` must be assigned a valid `UIDocument` either in the inspector or found at runtime.
- The craft button is disabled if the required ingredients or gold are insufficient.
- The tooltip is populated with placeholder data only if it is empty when toggled.

# Example
```csharp
var recipeDetailPanel = gameObject.AddComponent<RecipeDetailPanel>();
recipeDetailPanel.ShowRecipeDetails(recipeDef, recipePreview, requiredGold, availableGold, availableItems);
```

# Unknowns
- The implementation details of `RecipeDef` and `CraftingService.RecipePreview` are not provided in this file.
- The behavior of the `ToggleTooltip()` method regarding dynamic content population is not fully defined.

