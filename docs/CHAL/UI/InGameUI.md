# CHAL.UI.InGameUI

_Automatically generated/updated from `Assets/src/UI/misc/InGameUI.cs`._

```text
1) Purpose
- Defines an abstract MonoBehaviour IngameUI that manages a root VisualElement tied to a UIDocument on the same GameObject.
- Hides the root VisualElement on Awake by default.
- Provides Show(bool) to toggle visibility and IsVisible to query current visibility.
```

```text
2) Public API
- Namespace: CHAL.UI

- Type: Public abstract class IngameUI : MonoBehaviour
  - Protected VisualElement root
  - Protected virtual void Awake()
    - root = GetComponent<UIDocument>().rootVisualElement
    - root.style.display = DisplayStyle.None
  - Public virtual void Show(bool show)
    - root.style.display = show ? DisplayStyle.Flex : DisplayStyle.None
  - Public bool IsVisible
    - get: root.style.display == DisplayStyle.Flex
```

```text
3) Key Behavior & Side Effects
- Awake
  - Retrieves rootVisualElement from the UIDocument component on the same GameObject and stores it in root.
  - Sets root.style.display to DisplayStyle.None (start hidden).
- Show(bool)
  - Sets root.style.display to DisplayStyle.Flex when true, otherwise DisplayStyle.None.
- IsVisible
  - Returns true if root.style.display equals DisplayStyle.Flex; false otherwise.
- Design note
  - Requires a UIDocument component on the same GameObject; no null checks are performed, so missing UIDocument may throw a NullReferenceException during Awake.
```

```text
4) Constraints & Failure Modes
- Guarding: No guards for null UIDocument; potential crash if UIDocument is absent.
- Threading: All UI updates occur on Unity's main thread (implicit).
- Performance: Minimal, direct style toggling; no batching or async behavior.
```

```text
5) Example
```csharp
using CHAL.UI;

public class HUDUI : IngameUI { }

// Usage (attach to a GameObject with a UIDocument):
// HUDUI hud = GetComponent<HUDUI>();
// hud.Show(true);  // show
// bool visible = hud.IsVisible;
```
```

```text
6) Unknowns
- Specific UI structure under rootVisualElement (child elements, styling, event handling) is not defined here.
- Behavior when multiple IngameUI-derived components share or compete for the same root is not specified.
```
