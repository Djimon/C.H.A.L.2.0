# CHAL.Core.InputManager

_Automatically generated/updated from `Assets/src/Core/InputManager.cs`._

```text
1) Purpose
- Defines InputManager as a MonoBehaviour that handles mouse-based interaction with clickable objects via raycasting from the main camera.
- Tracks hover state with lastHovered to trigger OnHoverEnter/OnHoverExit on ClickableObject instances.
- Responds to Escape by navigating to the main menu via GameManager.Instance.GoToMainMenu(), and logs interactions via DebugManager.

```

```text
2) Public API
- Namespace/module: CHAL.Core
- Types
  - public class InputManager : MonoBehaviour
    - Public fields/properties: none
    - Public methods: none
```

```text
3) Key Behavior & Side Effects
- Update()
  - Invokes HandleClickableObjects() each frame.
  - If Escape is pressed, calls GameManager.Instance.GoToMainMenu().
- HandleClickableObjects()
  - Casts a ray from the main camera through the current mouse position: Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit)
  - If the hit collider has tag "clickableObject", attempts to locate a ClickableObject via GetComponent<ClickableObject>() or GetComponentInChildren<ClickableObject>().
  - Hover flow:
    - If the currently identified clickable differs from lastHovered, calls OnHoverExit() on lastHovered (if not null).
    - If a new clickable is found, calls OnHoverEnter() on it and logs via DebugManager.
    - Updates lastHovered to the new clickable.
- Click handling
  - If a clickable is identified and the left mouse button is pressed (Input.GetMouseButtonDown(0)), calls clickable.OnClick() and logs via DebugManager.
```

```text
4) Constraints & Failure Modes
- Raycasting relies on a valid Camera.main; absence of a main camera can cause null reference or no hits.
- Only objects with tag "clickableObject" participate in the clickable lookup.
- If no ClickableObject component is found on the hit collider or its children, clickable remains null.
- All input handling runs on the main thread per Unity's Update cycle; no async handling here.
- null-safety: OnHoverExit is guarded by lastHovered != null; OnHoverEnter/OnClick only invoked when a valid clickable is present.
```

```text
5) Example
```csharp
// Minimal usage notes:
// - Attach InputManager to a GameObject in the scene.
// - Ensure interactive objects have the tag "clickableObject" and expose a ClickableObject component.
// - Ensure a main camera exists in the scene (Camera.main will be used for raycasts).
```

```text
6) Unknowns
- Details of ClickableObject: API of OnHoverEnter, OnHoverExit, OnClick, and any side effects.
- Behavior of GameManager.Instance.GoToMainMenu(): navigation specifics or scene transitions.
- Implementation details of DebugManager.Log and its log levels.
- Whether multiple cameras or layers affect raycast behavior beyond the default configuration.
- Any additional UI or game logic that may influence what is considered a “clickableObject” in practice.
```
