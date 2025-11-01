# CHAL.UI.MainMenuUI

_Automatically generated/updated from `Assets/src/UI/MainMenuUI.cs`._

```text
1) Purpose
- Defines CHAL.UI.MainMenuUI as a MonoBehaviour to manage the main menu UI via UIDocument UIElements.
- Wires up button interactions for New, Continue, Options, and Exit, and controls the visibility/state of the continue flow.
- Holds a reference to a character-creation UI panel to show when starting a new game.

2) Public API
- Namespace/module: CHAL.UI

- Types
  - public class MainMenuUI : MonoBehaviour
    - Public fields
      - public GameObject characterCreationMenue
        - Reference to the character creation menu panel to be shown when starting a new game.
    - Private/internal fields (not part of public API)
      - private string _startSceneName
      - private VisualElement root
      - private Button btnNew
      - private Button btnContinue
      - private Button btnOptions
      - private Button btnExit
    - Public methods
      - (none)
    - Private methods
      - void Awake()
        - Cache root VisualElement from UIDocument.

3) Key Behavior & Side Effects
- Awake
  - root = GetComponent<UIDocument>().rootVisualElement
- Start
  - If GameManager.Instance?.Profile == null -> btnContinue.SetEnabled(false)
  - Else -> btnContinue.SetEnabled(true)
- OnEnable
  - btnNew = root.Q<Button>("NewGame"); btnNew.clicked += OnStartBtnClicked
  - btnContinue = root.Q<Button>("Continue"); btnContinue.clicked += OnContinueBtnClicked
  - btnOptions = root.Q<Button>("Options"); btnOptions.clicked += OnOptoinsBtnClicked
  - btnExit = root.Q<Button>("Exit"); btnExit.clicked += OnExitBtnClicked
- OnStartBtnClicked
  - characterCreationMenue.SetActive(true)
- OnContinueBtnClicked
  - DebugManager.Log("Continue game", DebugManager.EDebugLevel.Test, "UI")
  - GameManager.Instance.ContinueGame()
- OnExitBtnClicked
  - GameManager.Quit()
- OnOptoinsBtnClicked
  - DebugManager.Log("ToDo: optionen")

4) Constraints & Failure Modes
- UI element assumptions
  - Expects UIDocument with VisualElements named: "NewGame", "Continue", "Options", "Exit"
  - If any of the btn* lookups fail (null), null reference exceptions may occur when wiring or clicking.
- Character creation panel
  - characterCreationMenue must be non-null; otherwise OnStartBtnClicked will throw when calling SetActive(true).
- Game manager assumptions
  - GameManager.Instance may be null; OnContinueBtnClicked and OnExitBtnClicked assume a valid singleton.
  - GameManager.Instance.Profile existence is used to enable/disable Continue button.
- Serialized field
  - _startSceneName is declared but not used anywhere in this file (potential confusion).
- Side effects
  - Start toggles UI state; OnContinueBtnClicked triggers game flow; OnExitBtnClicked quits the app.
  
5) Example
- Not applicable/derivable from this file alone.

6) Unknowns
- Details of DebugManager.Log implementation and EDebugLevel enum values.
- Exact behavior of GameManager.ContinueGame() and GameManager.Quit() beyond their invocation here.
- What the character creation UI contains or how it interacts with this menu beyond being activated.
- Any additional UI elements or lifecycle expectations not visible in this file.

```
