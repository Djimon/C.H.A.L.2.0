# CHAL.Systems.Research.ResearchNodeWidget

_Automatically generated/updated from `Assets/src/Systems/Research/UI/ResearchNodeWidget.cs`._

# Purpose
- Defines the `ResearchNodeWidget` class for displaying and interacting with research nodes in a UI.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class ResearchNodeWidget : MonoBehaviour, IPointerClickHandler`
    - Public fields/properties:
      - `public Image background;`
      - `public Image icon;`
      - `public TMP_Text title;`
      - `public Button button;`
      - `public string nodeId;`
    - Public methods:
      - `public void Init(ResearchMapView map, string id, string titleText, Sprite iconSprite);`
        - Initializes the widget with the provided parameters.
      - `public void ApplyState(bool isSelected = false);`
        - Updates the visual state of the widget based on the research node's status.
      - `public void OnPointerClick(PointerEventData eventData);`
        - Handles pointer click events to trigger node selection.

# Key Behavior & Side Effects
- `Init` method sets up the widget with a research map, node ID, title, and icon.
- `ApplyState` method updates colors and button interactivity based on the node's completion and availability status.
- `OnPointerClick` triggers the node click event on the associated research map.

# Constraints & Failure Modes
- `ApplyState` does not execute if `_map` is null.
- Button interactivity is determined by the node's availability and completion status.
- Null checks for `background`, `icon`, and `title` are performed before accessing their properties.

# Example
```csharp
ResearchNodeWidget widget = new ResearchNodeWidget();
widget.Init(mapInstance, "node1", "Research Node 1", iconSprite);
widget.ApplyState();
```

# Unknowns
- The implementation details of `ResearchMapView` and `ResearchUIThemeDef` are not provided.
- The behavior of `_map.service` methods is not defined in this file.

