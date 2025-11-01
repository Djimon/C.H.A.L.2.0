# CHAL.UI.RecipeListView

_Automatically generated/updated from `Assets/src/UI/RecipeListView.cs`._

```text
1) Purpose
- Defines a sealed Unity MonoBehaviour RecipeListView in namespace CHAL.UI.
- Exposes public event OnSelect to notify when a RecipeDef is selected.
- Provides SetData to render a grouped, scrollable list of recipes using UIElements; applies craftable/missing styling based on a provided map.
```

```text
2) Public API
- Namespace/module
  - CHAL.UI

- Types
  - public sealed class RecipeListView : MonoBehaviour
    - Public fields/properties
      - public event Action<RecipeDef> OnSelect
    - Public methods
      - public void SetData(IEnumerable<RecipeDef> recipes, IDictionary<RecipeDef, bool> craftableMap)
        - Builds UI: groups by recipe.slotType (string key) or "Misc" for nulls; sorts groups by key; creates foldouts and recipe rows.
```

```text
3) Key Behavior & Side Effects
- Awake behavior
  - If doc is null, assigns doc = GetComponent<UIDocument>().
  - Finds the ScrollView named "list-scroll" from doc.rootVisualElement and stores in _scroll.
- SetData behavior
  - Clears existing contents of _scroll via _scroll?.Clear().
  - If _scroll is null or recipes is null, exits early.
  - Groups recipes (ignoring nulls) by (r.slotType.ToString() if r != null, else "Misc"), then orders groups by key.
  - For each group:
    - Creates a Foldout with text = group key, value = true; adds class "group-foldout"; adds to _scroll.
    - For each recipe r in the group (skips nulls):
      - Creates a row VisualElement with class "recipe-row".
      - Creates a Button with:
        - Click handler invoking OnSelect?.Invoke(r)
        - Text set to r.displayKey if non-empty, else r.name
        - Class "recipe-btn"; text-aligned left.
      - Determines craftable via craftableMap (if not null, uses TryGetValue(r, out craftable); default false).
      - Applies classes: "is-craftable" if craftable; "is-missing" if not craftable.
      - Registers a ClickEvent on the row to invoke OnSelect?.Invoke(r) (in addition to the Button handler).
      - Adds the Button to the row, then the row to the foldout.
- MakeRow behavior
  - private VisualElement MakeRow(RecipeDef r) creates a row with a single Button for r; returns the row (not used by SetData).
```

```text
4) Constraints & Failure Modes
- Null handling
  - SetData gracefully handles null _scroll or null recipes by early return after clearing when possible.
  - craftableMap being null results in all recipes treated as not craftable.
- Awake risk
  - If doc remains null after Awake (no UIDocument attached), _scroll assignment will access null and throw.
- Duplicate invocation risk
  - Each recipe row wires:
    - Button(click) -> OnSelect?.Invoke(r)
    - Row ClickEvent -> OnSelect?.Invoke(r)
    This can cause OnSelect to be invoked twice per user action.
- Performance
  - SetData rebuilds the entire list; no virtualization or incremental updates.
```

```text
5) Example
```csharp
// Example usage
var view = FindObjectOfType<CHAL.UI.RecipeListView>();
view.OnSelect += recipe => Debug.Log("Selected: " + recipe?.name);
view.SetData(allRecipes, craftableMap);
```
```

```text
6) Unknowns
- Exact structure of RecipeDef beyond what is used here (slotType, displayKey, name) is not defined in this file.
- The concrete type/contents of r.slotType (likely an enum) and how it maps to UI text are not specified beyond ToString().
- The intended CSS/class responsibilities (e.g., styling for "is-craftable" vs "is-missing") are not defined here.
- Whether OnSelect may be invoked twice per click due to both Button and row ClickEvent handlers is not resolved from this file alone.
- MakeRow is defined but unused by SetData; its intended future use is not specified.
```
