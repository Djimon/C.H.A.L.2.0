# Assets/src/Systems/Research/UI/ResearchNodeWidget.cs

_Automatically generated/updated from `Assets/src/Systems/Research/UI/ResearchNodeWidget.cs`._

# Purpose
- Defines the `ResearchNodeWidget` class for displaying and interacting with research nodes in the UI.

# Public API
- Namespace: `CHAL.Systems.Research`
- Types
  - public sealed class `ResearchNodeWidget` : `MonoBehaviour`, `IPointerClickHandler`
    - Public fields/properties:
      - `Image background`: Background image of the node.
      - `Image icon`: Icon image of the node.
      - `TMP_Text title`: Title text of the node.
      - `Button button`: Button for interacting with the node.
      - `string nodeId`: Identifier for the node.
    - Public methods:
      - `void Init(ResearchMapView map, string id, string titleText, Sprite iconSprite)`: Initializes the widget with the research map view and node details.
      - `void ApplyState(bool isSelected = false)`: Updates the visual representation based on the node's state.
      - `void OnPointerClick(PointerEventData eventData)`: Handles pointer click events on the node.

# Key Behavior & Side Effects
- `Init`: Sets up the widget with the provided parameters and applies the initial state.
- `ApplyState`: Updates the visual appearance based on whether the node is completed, available, or active. It also configures the button's interactability and click listener.
- `OnPointerClick`: Triggers the node click event on the associated research map.

# Constraints & Failure Modes
- `Init`: If `iconSprite` is null, defaults to the theme's default icon.
- `ApplyState`: Does nothing if `_map` is null. Handles null checks for `background`, `icon`, `title`, and `button`.

# Example
```csharp
ResearchNodeWidget nodeWidget = new ResearchNodeWidget();
nodeWidget.Init(researchMapView, "node1", "Research Node 1", iconSprite);
nodeWidget.ApplyState();
```

# Unknowns
- None.

