# CHAL.UI.ClickableObject

_Automatically generated/updated from `Assets/src/UI/misc/ClickableObject.cs`._

```text
1) Purpose
- Defines a Unity MonoBehaviour ClickableObject that toggles a per-object shimmer shader on hover and shows a UI menu on click.
- Exposes a public field to assign a menu UI GameObject in the Inspector.
- Coordinates with global systems (GameManager, DebugManager) to gate UI visibility by feature unlocks.

2) Public API
- Namespace/module: CHAL.UI
- Types
  - public class ClickableObject : MonoBehaviour
- Public fields
  - public GameObject menuUI
    - UI menu to show when the object is clicked (assigned in Inspector)
- Public methods
  - public void OnHoverEnter()
  - public void OnHoverExit()
  - public void OnClick()
- Notes
  - No public properties beyond menuUI
  - SetShimmer is private (helper)

3) Key Behavior & Side Effects
- Awake()
  - rend = GetComponent<Renderer>()
  - mpb = new MaterialPropertyBlock()
  - If rend.sharedMaterial lacks property "_shimmerOn", logs a warning via DebugManager
  - Calls SetShimmer(false) to disable shimmer on start
- OnHoverEnter()
  - Calls SetShimmer(true) to enable shimmer
- OnHoverExit()
  - Calls SetShimmer(false) to disable shimmer
- OnClick()
  - If menuUI is non-null:
    - ui = menuUI.GetComponent<IngameUI>()
    - unlocked = true
    - If ui.requiredFeatureID != "none": unlocked = GameManager.Instance.ResearchUnlocks.IsUnlockedCraftingFeature(ui.requiredFeatureID)
    - If ui != null && unlocked: ui.Show(true)
    - Calls SetShimmer(false)
- SetShimmer(bool on)
  - Clears mpb
  - mpb.SetFloat("_shimmerOn", on ? 1f : 0f)
  - rend.SetPropertyBlock(mpb)

4) Constraints & Failure Modes
- Renderer/null risk
  - Awake assumes a Renderer component is present; missing renders can cause NullReferenceException.
- Shader property assumption
  - Warns if _shimmerOn is missing, but SetShimmer will set the property block regardless; may have no visual effect if the shader lacks the property.
- OnClick null-safety gap
  - ui is obtained via menuUI.GetComponent<IngameUI>(); if menuUI has no IngameUI, ui is null; the code accesses ui.requiredFeatureID before null-check, which can throw NullReferenceException.
- External dependencies
  - Depends on GameManager.Instance.ResearchUnlocks.IsUnlockedCraftingFeature
  - Depends on IngameUI.Show(bool) and ui.requiredFeatureID
- Performance
  - Uses MaterialPropertyBlock for per-renderer property changes (efficient).
- Threading
  - All interactions occur on Unity main thread (typical for Unity UI/renderer work).

5) Example
```csharp
// Typical usage (manual invocation; in practice, wired to input events)
var obj = GetComponent<ClickableObject>();
obj.OnHoverEnter();  // enable shimmer
obj.OnHoverExit();   // disable shimmer
obj.OnClick();       // attempt to show associated UI if available and unlocked
```

6) Unknowns
- Exact behavior/signature of IngameUI.Show(bool) and how the UI behaves once shown.
- Details of GameManager.Instance.ResearchUnlocks and IsUnlockedCraftingFeature implementation.
- Shader behavior beyond property name "_shimmerOn" (e.g., how it visually renders).
- Any additional side effects from external scripts or editor tooling not present in this file.
