# CHAL.UI.InGameUI

_Automatically generated/updated from `Assets/src/UI/misc/InGameUI.cs`._

```text
Purpose
- Defines an abstract base class IngameUI for UIElements-based in-game UI in CHAL.UI.
- On Awake, initializes root from the UIDocument and hides the UI (start hidden).
- Provides a public surface to show/hide and query visibility.

Public API
- Namespace/module: CHAL.UI
- Type
  - public abstract class IngameUI : MonoBehaviour
    - Public methods
      - public virtual void Show(bool show)
        - Sets root.style.display to DisplayStyle.Flex when true, or DisplayStyle.None when false.
    - Public properties
      - public bool IsVisible
        - get: returns true if root.style.display == DisplayStyle.Flex
    - Inherits from: MonoBehaviour

Key Behavior & Side Effects
- Awake (Lifecycle)
  - root = GetComponent<UIDocument>().rootVisualElement;
  - root.style.display = DisplayStyle.None;
  - Effect: UI starts hidden.
- Show(bool show)
  - Mutates the root VisualElement display style to show or hide.
  - Effect: toggles visibility in the UI hierarchy.
- IsVisible
  - Reads root.style.display to determine current visibility state.
  - Effect: non-mutating query of visibility.

Constraints & Failure Modes
- Assumes a UIDocument component is present on the same GameObject; no null checks are performed.
  - If UIDocument is missing, Awake will throw when accessing GetComponent<UIDocument>().rootVisualElement.
- root is assigned in Awake; calling Show or IsVisible before Awake could lead to NullReference.
- Abstract class; intended to be subclassed to implement concrete UI behavior.

Example
```csharp
using CHAL.UI;
using UnityEngine;

public class PauseMenuUI : IngameUI
{
    void Start()
    {
        // Ensure it's hidden initially (base class already hides on Awake)
        Show(false);
    }
    
    // Example usage could toggle with a key, etc.
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Show(!IsVisible);
        }
    }
}
```

Unknowns
- Specific UI content/layout for rootVisualElement is not defined here.
- How this base class interacts with other UI systems or game state beyond Show/IsVisible is not specified.
- Behavior if multiple UIDocuments or dynamic rootVisualElement changes are introduced is not covered.
