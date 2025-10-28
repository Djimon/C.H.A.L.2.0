# Assets/src/Systems/Research/UI/ResearchNodeWidget.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines the `ResearchNodeWidget` class for displaying and interacting with research nodes in the UI.

## Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - `public sealed class ResearchNodeWidget : MonoBehaviour, IPointerClickHandler`
    - Public fields/properties:
      - `public Image background;` - Background image of the node.
      - `public Image icon;` - Icon image of the node.
      - `public TMP_Text title;` - Title text of the node.
      - `public Button button;` - Button for interacting with the node.
      - `public string nodeId;` - Unique identifier for the node.
    - Public methods:
      - `public void Init(ResearchMapView map, string id, string titleText, Sprite iconSprite);` - Initializes the widget with the provided parameters.
      - `public void ApplyState(bool isSelected = false);` - Updates the visual state of the widget based on the node's status.
      - `public void OnPointerClick(PointerEventData eventData);` - Handles pointer click events to trigger node selection.

## Key Behavior & Side Effects
- `Init` method sets up the widget's visuals and state based on the provided parameters.
- `ApplyState` method updates the widget's appearance based on the node's completion and availability status, and sets button interactivity.
- `OnPointerClick` method triggers the node click action when the widget is clicked.

## Constraints & Failure Modes
- If `_map` is `null`, methods that depend on it will not execute their logic.
- The `icon` and `title` fields will not update if their corresponding UI elements are not assigned.
- The button's interactivity is determined by the node's availability and completion status.

## Example
```csharp
ResearchNodeWidget nodeWidget = new ResearchNodeWidget();
nodeWidget.Init(researchMapView, "node1", "Research Node 1", iconSprite);
```

## Unknowns
- The implementation details of `ResearchMapView` and its methods (`IsCompleted`, `IsNodeAvailable`, `GetActiveNodeId`, `OnNodeClicked`) are not provided in this file.
```
