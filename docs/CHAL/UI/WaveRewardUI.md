# CHAL.UI.WaveRewardUI

_Automatically generated/updated from `Assets/src/UI/WaveRewardUI.cs`._

1) Purpose
- Defines a UI controller WaveRewardUI for the in-game wave-reward screen.
- Wires up Retry, Next, and Hideout buttons to their handlers; updates the WaveStatus text.
- Delegates actions to MapManager (NextWave/StartWave) and GameManager (ExitToHideout); logs text updates.
- Manages an auto-start feature for the next wave with a countdown.

2) Public API
- Namespace/module
  - CHAL.UI

- Types
  - public class WaveRewardUI : IngameUI
    - Public methods
      - public void populateText(bool succeded)
        - Updates detailsText to show "Successful!" or "Failed!"
        - Sets detailsText color based on success state
        - Logs "Text updated" via DebugManager
      - public override void Show(bool visible)
        - Shows or hides the UI; manages auto-start toggle and countdown.

3) Key Behavior & Side Effects
- Awake (protected override)
  - Calls base.Awake()
  - Retrieves root VisualElement from UIDocument
  - Queries and binds:
    - Button btnRetry = root.Q<Button>("Retry"); subscribes btnRetry.clicked to OnRetryBtnClicked
    - Button btnNext = root.Q<Button>("Next"); subscribes btnNext.clicked to OnNexBtnClicked
    - Button btnHideout = root.Q<Button>("Hideout"); subscribes btnHideout.clicked to OnHideoutBtnClicked
    - TextElement detailsText = root.Q<Label>("WaveStatus")
    - Toggle _autoStartToggle = root.Q<Toggle>("AutoStartToggle")
    - Label _autoStartCountdown = root.Q<Label>("AutoStartCountdown")
  - Logs warnings if _autoStartToggle or _autoStartCountdown are not found.
  - MapManager instance:
    - mapManager = FindFirstObjectByType<MapManager>()
    - WaveManager instance:
    - waveManager = FindFirstObjectByType<WaveManager>()
  - Registers a value change callback for _autoStartToggle to manage auto-start behavior.

- Show(bool visible)
  - Manages the visibility of the UI and the auto-start countdown based on the current state of the map.
  - Starts the countdown if conditions are met when shown; stops it when hidden.

- StartAutoStartCountdown()
  - Initiates a countdown for auto-starting the next wave.

- AutoStartCountdownRoutine(int seconds)
  - Counts down from the specified seconds, updating the UI and checking for cancellation conditions.

- populateText(bool succeded)
  - detailsText.text = succeded ? "Successful!" : "Failed!"
  - detailsText.style.color = succeded ? new Color(1f, 211f/255, 28f/255) : new Color(160f/255, 0f, 0f)
  - DebugManager.Log("Text updated")

- OnHideoutBtnClicked()
  - Stops any running countdown and calls GameManager.Instance.ExitToHideout().

- OnNexBtnClicked()
  - Stops any running countdown and calls mapManager.NextWave().

- OnRetryBtnClicked()
  - Stops any running countdown and calls mapManager.StartWave().

4) Constraints & Failure Modes
- Potential null references
  - mapManager may be null if no MapManager exists in the scene; OnNexBtnClicked/OnRetryBtnClicked will throw if invoked.
  - detailsText may be null if UI element "WaveStatus" is missing.
  - root may be null if UIDocument is not present or misnamed; subsequent Q queries would fail.
  - GameManager.Instance may be null; OnHideoutBtnClicked would throw.
  - _autoStartToggle and _autoStartCountdown may be null if their respective UI elements are missing.
- UI wiring assumptions
  - Buttons named exactly "Retry", "Next", and "Hideout" must exist in the UIDocument root.
  - WaveStatus label must exist as a Label element named "WaveStatus".
  - AutoStartToggle and AutoStartCountdown must exist as Toggle and Label elements respectively.
- succeded parameter in populateText
  - Note the parameter name is succeded (typo in code); behavior depends on its boolean value.

5) Example
- Minimal usage (example usage in scene script)
```csharp
// Somewhere after WaveRewardUI is initialized
var ui = FindObjectOfType<CHAL.UI.WaveRewardUI>();
ui?.populateText(true); // show successful state
```

6) Unknowns
- Behavior of IngameUI base class beyond Awake call.
- Exact runtime guarantees of MapManager and GameManager singletons (presence, initialization timing).
- UI layout specifics beyond element names used here.
- Any additional side effects from clicking buttons beyond NextWave/StartWave/ExitToHideout.
