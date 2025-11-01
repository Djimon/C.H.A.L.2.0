# CHAL.UI.MapRewardUI

_Automatically generated/updated from `Assets/src/UI/MapRewardUI.cs`._

```text
1) Purpose
- Defines a UI component MapRewardUI (CHAL.UI) that manages map-reward display and interactions.
- Wires up Retry and Hideout buttons to their handlers and references a status label.
- Interfaces with MapManager and GameManager to drive map flow and game exits.
```

```text
2) Public API
- Namespace/module
  - CHAL.UI

- Types
  - public class MapRewardUI : IngameUI
    - Public fields/properties: none
    - Public methods:
      - public void populateText(bool succeded)
        - Updates the status label text to "Successful!" or "Failed!"
        - Sets label color: success -> color near yellow; fail -> red
```

```text
3) Key Behavior & Side Effects
- Awake (protected override)
  - Calls base.Awake()
  - Finds and assigns btnRetry (Button) via root.Q<Button>("Retry")
  - Subscribes btnRetry.clicked to OnRetryBtnClicked
  - Finds and assigns btnHideout (Button) via root.Q<Button>("Hideout")
  - Subscribes btnHideout.clicked to OnHideoutBtnClicked
  - Finds and assigns detailsText (TextElement) via root.Q<Label>("MapStatus")
  - Retrieves mapManager via FindFirstObjectByType<MapManager>()
- populateText(bool succeded)
  - Sets detailsText.text to "Successful!" if succeded is true, else "Failed!"
  - Updates detailsText.style.color:
    - succeded: new Color(1f, 211f/255, 28f/255)
    - failed: new Color(160f/255, 0f, 0f)
- OnHideoutBtnClicked()
  - Invokes GameManager.Instance.ExitToHideout()
- OnRetryBtnClicked()
  - Invokes mapManager.ResetWave()
```

```text
4) Constraints & Failure Modes
- No null checks on UI elements or references:
  - btnRetry, btnHideout, detailsText, and mapManager may be null if UI nodes or objects are missing.
  - OnRetryBtnClicked() directly uses mapManager without null-checks; potential NullReferenceException if not found.
- mapManager is retrieved via FindFirstObjectByType<MapManager>() and may return null.
- UI element queries rely on exact names/types: "Retry", "Hideout", "MapStatus".
- Method name parameter is spelled succeded (potential for confusion or mismatch with callers).
```

```text
5) Example
- Not applicable (no external example derivable from this file).
```

```text
6) Unknowns
- Details of IngameUI (root, lifecycle, and base behavior) are not shown.
- Behavior when UI elements are missing or not yet instantiated is not defined here.
- The broader lifecycle and storage of MapManager and GameManager instances are not defined in this file.
- Any additional side effects from ResetWave or ExitToHideout beyond their method names are not specified.
```
