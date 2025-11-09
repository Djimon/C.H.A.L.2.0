# Assets/src/UI/RecipeDetailPanelView.cs

_Automatically generated/updated from `Assets/src/UI/RecipeDetailPanelView.cs`._

# Purpose
- Defines the `RecipeDetailPanel` class for displaying and managing recipe details in the UI.

# Public API
- Namespace: `CHAL.UI`
- Types
  - public sealed class `RecipeDetailPanel` : `MonoBehaviour`
    - Public fields/properties:
      - `event Action OnCraftClicked`: Invoked when the craft button is clicked.
      - `UIDocument doc`: Reference to the UI document.
    - Public methods:
      - `void Clear()`: Resets UI elements to their default state.
      - `void ShowRecipeDetails(RecipeDef r, CraftingService.RecipePreview preview, int needGold, int haveGold, Dictionary<string, int> haveByItemId)`: Displays the details of a recipe.
      - `void ShowFail(string message)`: Displays a failure message to the user.
      - `void ShowSuccess()`: Displays a success message to the user.

# Key Behavior & Side Effects
- `Awake()`: Initializes UI elements and sets up event listeners for button clicks and slider value changes.
- `Clear()`: Resets all displayed fields and UI elements.
- `ShowRecipeDetails(...)`: Updates the UI with recipe details, checks crafting conditions, and enables/disables the craft button based on available ingredients and gold.
- `ShowFail(...)`: Updates the failure label with a provided message.
- `ShowSuccess()`: Clears the failure label for a success indication.
- `ToggleTooltip()`: Toggles the visibility of the tooltip and populates it with placeholder data if empty.

# Constraints & Failure Modes
- `doc` must be assigned either in the inspector or via `GetComponent<UIDocument>()` in `Awake()`.
- Ingredients and gold availability are checked to enable or disable the craft button.
- Tooltip content is initially empty and populated only when toggled for the first time.

# Example
```csharp
var recipePanel = gameObject.AddComponent<RecipeDetailPanel>();
recipePanel.ShowRecipeDetails(recipeDef, recipePreview, requiredGold, availableGold, availableItems);
```

# Unknowns
- The implementation details of `RecipeDef` and `CraftingService.RecipePreview` are not provided in this file.
- The exact behavior of the crafting process and how it interacts with the UI is not defined here.

