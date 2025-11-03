# CHAL.Systems.Research.ResearchNodeWidget

_Automatically generated/updated from `Assets/src/Systems/Research/UI/ResearchNodeWidget.cs`._

1) Purpose
- Define a UI widget (ResearchNodeWidget) for representing a research node within the Research map UI.
- Manage visuals (background, icon, title) and interaction state (button enabled/disabled) based on map/service state.
- Forward node click events to the associated ResearchMapView.

2) Public API
- Namespace/module
  - CHAL.Systems.Research

- Types
  - Public sealed class ResearchNodeWidget : MonoBehaviour, IPointerClickHandler
    - Public fields
      - Image background
        - Bindable background image for the node
      - Image icon
        - Bindable icon image for the node
      - TMP_Text title
        - Bindable title text for the node
      - Button button
        - Bindable button for node interaction
      - string nodeId
        - Identifier of the node
    - Public methods
      - void Init(ResearchMapView map, string id, string titleText, Sprite iconSprite)
        - Initialize widget with map, node id, title text, and icon sprite
      - void ApplyState(bool isSelected = false)
        - Update visuals and interactivity based on node state; optionally apply selection highlight
      - void OnPointerClick(PointerEventData eventData)
        - Notify map of node click when pointer is released over this widget

3) Key Behavior & Side Effects
- Init(map, id, titleText, iconSprite)
  - Stores map reference and nodeId
  - Reads theme from map.theme
  - Caches colors: _normalColor, _disabledColor, _completedColor, _highlightColor, _highlightIntensity
  - Applies background sprite from theme and icon (iconSprite or theme default)
  - Sets title text
  - Calls ApplyState()
- ApplyState(isSelected = false)
  - If _map is null, returns early
  - Queries map.service for:
    - completed = IsCompleted(nodeId)
    - available = IsNodeAvailable(nodeId)
    - isActive = GetActiveNodeId() == nodeId
  - Determines foreground color (fg):
    - default _normalColor
    - if not available and not completed -> _disabledColor
    - if completed -> _completedColor
  - Applies fg to icon and title
  - Visual highlight:
    - If background exists and (isActive or isSelected): lerp toward _highlightColor by _highlightIntensity, then set alpha to 1
    - Else: background color set to white
  - Button state:
    - button.interactable = available && !completed && !isActive
    - Clears all listeners, then adds listener to map.OnNodeClicked(nodeId)
- OnPointerClick(eventData)
  - If _map != null, calls _map.OnNodeClicked(nodeId)

4) Constraints & Failure Modes
- Guard: ApplyState returns immediately if _map is null
- Listeners: Existing button listeners are cleared before adding a new one to avoid duplicates
- Null-safety: Accesses to background/icon/title/button/nodes are guarded with null checks in Init/ApplyState; _theme and colors are derived from map.theme during Init
- Dependencies: Behavior relies on ResearchMapView and its service for state (IsCompleted, IsNodeAvailable, GetActiveNodeId) and OnNodeClicked; not defined in this file

5) Example
- Not derivable from this file alone (no usage example or instantiation pattern provided)

6) Unknowns
- Details of ResearchMapView, its service interface, and the semantics of IsCompleted/IsNodeAvailable/GetActiveNodeId
- Definition and structure of CHAL.Data.ResearchUIThemeDef and its color fields
- Exact lifecycle/ownership: when Init is called relative to Unity lifecycle
- Any other UI interactions or external side effects not visible in this file

