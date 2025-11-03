# CHAL.UI.InGameUI

_Automatically generated/updated from `Assets/src/UI/misc/InGameUI.cs`._

1) Purpose
- Defines an abstract MonoBehaviour base class for in-game UI panels that manage a root UIElements VisualElement.
- Caches the root VisualElement from a UIDocument and hides it on initialization.
- Provides a public API to show/hide the UI and to query visibility; exposes a feature identifier field for derived usage.

2) Public API
- Namespace/module
  - CHAL.UI
- Types
  - public abstract class IngameUI : MonoBehaviour
    - protected VisualElement root
      - Public field: the root VisualElement for this UI (cached from UIDocument)
    - public string requiredFeatureID = "none"
      - Public field: feature identifier placeholder for derived behavior
    - protected virtual void Awake()
      - Signature: protected virtual void Awake()
      - Side effects: root = GetComponent<UIDocument>().rootVisualElement; root.style.display = DisplayStyle.None;
    - public virtual void Show(bool show)
      - Signature: public virtual void Show(bool show)
      - Side effects: root.style.display = show ? DisplayStyle.Flex : DisplayStyle.None;
    - public bool IsVisible => root.style.display == DisplayStyle.Flex
      - Public property: indicates current visibility state based on display style

3) Key Behavior & Side Effects
- Awake
  - Retrieves rootVisualElement from the UIDocument component and stores it in root.
  - Sets initial display to None (hidden).
- Show(bool)
  - Toggles visibility by setting display to Flex when shown, or None when hidden.
- IsVisible
  - Reads root.style.display to report whether the UI is currently visible (Flex means visible).

4) Constraints & Failure Modes
- Potential null dereference
  - If UIDocument component is missing, GetComponent<UIDocument>() may return null; accessing rootVisualElement would throw.
  - If root remains null, Show and IsVisible will throw when accessing root.style.
- Inheritance
  - Class is abstract; intended to be subclassed for concrete in-game UI panels.
- requiredFeatureID
  - Public field with default "none"; usage is not defined in this file.

5) Example
- Not provided (no derivable concrete usage in this file).

6) Unknowns
- How derived classes use requiredFeatureID and how it affects behavior.
- How this base class integrates with other UI management systems in the project.
