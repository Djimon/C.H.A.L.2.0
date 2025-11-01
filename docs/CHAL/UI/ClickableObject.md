# CHAL.UI.ClickableObject

_Automatically generated/updated from `Assets/src/UI/misc/ClickableObject.cs`._

1) Purpose
- Unity component that toggles a shader "shimmer" effect on hover and click.
- Uses MaterialPropertyBlock to set the "_shimmerOn" property on the attached Renderer.
- Optionally shows a menu UI by activating a referenced GameObject with an IngameUI component on click.

2) Public API
- Namespace/module: CHAL.UI
- Types
  - public class ClickableObject : MonoBehaviour
    - Public fields
      - GameObject menuUI
        - Inspector: assign the menu GameObject to show on click
    - Public methods
      - void OnHoverEnter()
      - void OnHoverExit()
      - void OnClick()
    - Private helpers (not public API)
      - void SetShimmer(bool on)

3) Key Behavior & Side Effects
- Awake
  - Retrieves Renderer via GetComponent<Renderer>() and initializes a new MaterialPropertyBlock.
  - Checks if the attached material has property "_shimmerOn" and logs a warning if missing.
  - Calls SetShimmer(false) to ensure shimmer is off at start.
- OnHoverEnter
  - Calls SetShimmer(true) to enable shimmer.
- OnHoverExit
  - Calls SetShimmer(false) to disable shimmer.
- OnClick
  - If menuUI is assigned, gets IngameUI from that GameObject and calls Show(true) if found.
  - Regardless, calls SetShimmer(false) after attempting to show the UI.
- SetShimmer(bool on)
  - Clears the MaterialPropertyBlock, sets "_shimmerOn" to 1f or 0f, and applies the block to the Renderer.

4) Constraints & Failure Modes
- Requires a Renderer component on the same GameObject; no null checks for rend, so missing Renderer can cause null reference exceptions when shimmering.
- Uses sharedMaterial for the initial HasProperty check; behavior with multiple materials may be ambiguous.
- Sets "_shimmerOn" via MaterialPropertyBlock without guarding for property existence; if the shader lacks "_shimmerOn", the call may have no effect.
- menuUI is optional; OnClick safely succeeds even if menuUI is null or if IngameUI is not present (no crash, just no UI shown).

5) Example
- Not provided (no derivable minimal code snippet from the file itself beyond usage notes).

6) Unknowns
- Exact behavior of DebugManager.Warning: message text and logging behavior are not defined here.
- What IngameUI.Show(bool) does internally beyond being invoked with true.
- Shader-side expectations for "_shimmerOn" and how it interacts with other material properties.
- Behavior when there are multiple materials on the Renderer (only sharedMaterial is checked; per-material behavior is not defined).
