# Assets/src/UI/RecipeListView.cs

_Automatically generated/updated from `Assets/src/UI/RecipeListView.cs`._

# Purpose
- Defines the `RecipeListView` class for displaying a list of recipes in a UI.

# Public API
- Namespace: `CHAL.UI`
- Types
  - public sealed class `RecipeListView` : `MonoBehaviour`
    - Public fields/properties:
      - `event Action<RecipeDef> OnSelect`: Event triggered when a recipe is selected.
      - `UIDocument doc`: Reference to the UI document.
    - Public methods:
      - `void SetData(IEnumerable<RecipeDef> recipes, IDictionary<RecipeDef, bool> craftableMap)`: Sets the data for the recipe display, organizing recipes into groups.

# Key Behavior & Side Effects
- `Awake()`: Initializes the `doc` and `_scroll` fields.
- `SetData()`: Clears the scroll view and populates it with grouped recipes. Each recipe can trigger the `OnSelect` event when clicked.

# Constraints & Failure Modes
- If `doc` is not assigned, it attempts to get the `UIDocument` component from the GameObject.
- If `recipes` is null, the method returns early without making changes.
- Handles null `RecipeDef` entries by skipping them.

# Example
```csharp
var recipeListView = new RecipeListView();
recipeListView.SetData(recipes, craftableMap);
```

# Unknowns
- None.

