# CHAL.Core.InputManager

_Automatically generated/updated from `Assets/src/Core/InputManager.cs`._

```text
1) Purpose
- Defines a MonoBehaviour InputManager in CHAL.Core that handles mouse interactions with clickable objects.
- Performs per-frame raycasts from the mouse position to detect a ClickableObject (tag "clickableObject" on collider; component on collider or in children).
- Manages hover state (OnHoverEnter/OnHoverExit) and click actions (OnClick); Escape navigates to the main menu via GameManager.

2) Public API
- Namespace/module: CHAL.Core
- Types
  - public class InputManager : MonoBehaviour
    - Public fields/properties: none
    - Public methods: none

3) Key Behavior & Side Effects
- Update()
  - Calls HandleClickableObjects()
  - If Escape is pressed, calls GameManager.Instance.GoToMainMenu()
- HandleClickableObjects()
  - Raycasts from Camera.main through Input.mousePosition; on hit, if collider has tag "clickableObject", obtains a ClickableObject via GetComponent<ClickableObject>() or GetComponentInChildren<ClickableObject>()
  - Hover logic:
    - If current clickable differs from lastHovered:
      - If lastHovered != null, calls lastHovered.OnHoverExit()
      - If clickable != null, calls clickable.OnHoverEnter() and logs a hover event
      - Sets lastHovered = clickable
  - Click logic:
    - If clickable != null and left mouse button pressed, calls clickable.OnClick() and logs a click event
- Logging
  - Uses DebugManager.Log for hover and click events with Debug level

4) Constraints & Failure Modes
- Potential null references not guarded:
  - Camera.main may be null (no guard before ScreenPointToRay)
  - GameManager.Instance and DebugManager.Log assumptions may be null if not initialized
- Requires a collider with tag "clickableObject" to be hit; otherwise no clickable is detected
- clickable may be null after raycast (no ClickableObject on hit) and hover/click logic gracefully handles null
- click handling relies on GetMouseButtonDown(0) per frame; rapid clicking between frames may miss if not held

5) Example
- (omitted: no clearly derivable, self-contained example in this file)

6) Unknowns
- Behavior and implementation of ClickableObject (OnHoverEnter/OnHoverExit/OnClick) are defined elsewhere
- Details of GameManager.Instance.GoToMainMenu() and DebugManager.Log(...) semantics
- Exact project setup requirements: existence of the "clickableObject" tag, and presence of a ClickableObject component on the collider or its children
- Any broader UI flow triggered by main menu navigation beyond this file
```
