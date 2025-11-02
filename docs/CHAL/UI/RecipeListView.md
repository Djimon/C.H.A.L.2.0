# CHAL.UI.RecipeListView

_Automatically generated/updated from `Assets/src/UI/RecipeListView.cs`._

1) Purpose
- Defines a Unity MonoBehaviour RecipeListView in the CHAL.UI namespace.
- Presents a grouped, scrollable list of RecipeDef entries with craftability status.
- Exposes OnSelect event to notify listeners when a recipe is chosen; provides SetData to populate UI.

2) Public API
- Namespace/module: CHAL.UI

- Types
  - public sealed class RecipeListView : MonoBehaviour
    - public event Action<RecipeDef> OnSelect
    - public void SetData(IEnumerable<RecipeDef> recipes, IDictionary<RecipeDef, bool> craftableMap)
      - Populates UI with recipes grouped by slotType.ToString() (or "Misc" for nulls); adds foldouts per group; creates rows with craftability status; wires row/button clicks to OnSelect.
    - private VisualElement MakeRow(RecipeDef r)
      - Builds a single row UI element containing a left-aligned button for the given recipe and wires selection; returns the row.

3) Key Behavior & Side Effects
- Awake
  - Resolves UIDocument: if doc is null, uses GetComponent<UIDocument>(); finds ScrollView named "list-scroll" from root VisualElement.
- SetData
  - Clears existing _scroll contents.
  - If _scroll is null or recipes is null, returns early.
  - Groups recipes by slotType.ToString() (or "Misc" for nulls); sorts groups by key.
  - For each group:
    - Creates a Foldout labeled with the group key; expanded by default; adds CSS class "group-foldout".
    - For each recipe in the group (skipping nulls):
      - Creates a row VisualElement with CSS class "recipe-row".
      - Determines craftable from craftableMap (null-safe).
      - Chooses display text: r.displayKey if present, otherwise r.name.
      - Appends status text: " (craftable)" or " (missing mats)".
      - Creates a Button with text "<baseText><status>"; wires button click to OnSelect(r).
      - Aligns button text to the left; registers row click callback to OnSelect(r); adds button to row; adds row to foldout.
- MakeRow
  - Builds a single row with a Button labeled by r.displayKey or r.name; wires OnSelect; returns the row.
  - Note: MakeRow is defined but not used by SetData in this file.

4) Constraints & Failure Modes
- Null handling
  - If doc is missing or _scroll cannot be found, SetData exits safely after initial guards.
  - craftableMap can be null; craftable defaults to false.
  - Individual recipes or their fields (displayKey, name) may be null; null recipes are skipped during rendering.
- Event invocation
  - OnSelect is invoked via both the row's ClickEvent callback and the Button's click handler; potential multiple triggers if both fire per interaction.
  - OnSelect invocation uses null-conditional to avoid exceptions if no subscribers.
- Threading
  - UI modifications occur on Unity main thread as part of standard MonoBehaviour lifecycle.

5) Example
```csharp
// Example usage: subscribe to selection and populate data
using System.Collections.Generic;
using UnityEngine;
using CHAL.UI;

public class RecipeListExample : MonoBehaviour
{
    [SerializeField] private RecipeListView listView;

    void Start()
    {
        if (listView != null)
        {
            listView.OnSelect += r => Debug.Log("Selected recipe: " + r?.name);
            var recipes = new List<RecipeDef>
            {
                // populate with actual RecipeDef instances
            };
            var craftMap = new Dictionary<RecipeDef, bool>();
            listView.SetData(recipes, craftMap);
        }
    }
}
```

6) Unknowns
- Definition details of RecipeDef (properties like name, displayKey, slotType) beyond usage in this file.
- Exact semantics of slotType values and how Foldout grouping visually renders in the target UI.
- Behavior and styling of UIElements (Foldout, ScrollView, VisualElement) beyond class names used here.
- Any external effects of OnSelect beyond this file (consumers’ side effects).
