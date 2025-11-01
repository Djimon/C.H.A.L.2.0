# CHAL.UI.WaveRewardUI

_Automatically generated/updated from `Assets/src/UI/WaveRewardUI.cs`._

1) Purpose
- UI component for wave reward screen using Unity UIElements (UIDocument root).
- Exposes three buttons: Retry, Next, Hideout, and a WaveStatus text element to display result.
- Locates MapManager in the scene and wires button callbacks to cause wave control actions.

2) Public API
- Namespace/module: CHAL.UI
- Types
  - public class WaveRewardUI : IngameUI
    - Private fields
      - Button btnRetry; // Retry button
      - Button btnNext; // Next button
      - Button btnHideout; // Hideout button
      - TextElement detailsText; // Wave status UI text (WaveStatus)
      - MapManager mapManager; // Map manager reference
    - Protected/_public methods
      - protected override void Awake()
        - Calls base.Awake()
        - root = GetComponent<UIDocument>().rootVisualElement
        - btnRetry = root.Q<Button>("Retry"); btnRetry.clicked += OnRetryBtnClicked
        - btnNext = root.Q<Button>("Next"); btnNext.clicked += OnNexBtnClicked
        - btnHideout = root.Q<Button>("Hideout"); btnHideout.clicked += OnHideoutBtnClicked
        - detailsText = root.Q<Label>("WaveStatus")
        - mapManager = FindFirstObjectByType<MapManager>()
      - public void populateText(bool succeded)
        - detailsText.text = succeded ? "Successful!" : "Failed!"
        - detailsText.style.color = succeded ? new Color(1f, 211f/255, 28f/255) : new Color(160f/255, 0f, 0f)
        - DebugManager.Log("Text updated")
    - Private methods
      - private void OnHideoutBtnClicked()
        - GameManager.Instance.ExitToHideout()
      - private void OnNexBtnClicked()
        - mapManager.NextWave()
      - private void OnRetryBtnClicked()
        - mapManager.StartWave()

3) Key Behavior & Side Effects
- Awake wires UI and event handlers, and resolves MapManager from the scene.
- populateText updates the WaveStatus text and color, and logs a message.
- OnHideoutBtnClicked triggers a transition to hideout via GameManager.
- OnNexBtnClicked advances to the next wave via MapManager.NextWave().
- OnRetryBtnClicked restarts the current wave via MapManager.StartWave().

4) Constraints & Failure Modes
- Potential null references:
  - mapManager may be null if FindFirstObjectByType<MapManager>() fails.
  - detailsText may be null if WaveStatus element is missing.
  - Any of the UI elements named "Retry", "Next", "Hideout" must exist in the UIDocument.
- No null guards for mapManager/detailsText before invoking methods on them.
- All UI interactions assume Unity main thread context (typical for UIActions).
- Color calculations rely on 0–1 color components (divisions by 255 are explicit).

5) Example
- Not applicable (no derivable standalone example from this file).

6) Unknowns
- Behavior/implementation of IngameUI, DebugManager, GameManager, MapManager beyond used methods.
- Exact scene setup, and availability of the UIDocument and named elements at Awake time.
- Any additional side effects of mapManager.StartWave/NextWave beyond what is shown.
