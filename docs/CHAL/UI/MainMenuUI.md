# CHAL.UI.MainMenuUI

_Automatically generated/updated from `Assets/src/UI/MainMenuUI.cs`._

1) Purpose
- Defines a Unity MonoBehaviour (MainMenuUI) that wires up and handles the main menu UI actions.
- Manages a public character creation menu reference and four menu buttons (New Game, Continue, Options, Exit).
- Interfaces with GameManager and DebugManager to perform actions (start, continue, quit, and debug logging).

2) Public API
- Namespace/module
  - CHAL.UI

- Types
  - public class MainMenuUI : MonoBehaviour
    - Public fields
      - public GameObject characterCreationMenue;  // Menu shown when starting a new game
    - Public methods
      - (none)

3) Key Behavior & Side Effects
- Awake
  - root = GetComponent<UIDocument>().rootVisualElement
- Start
  - If GameManager.Instance?.Profile is null, disable the Continue button; otherwise enable it
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
- _startSceneName is defined but never used
- Internals rely on Unity UI Toolkit (UIDocument) being present; rootVisualElement must exist
- OnEnable assigns btnNew/btnContinue/btnOptions/btnExit via root.Q<Button>(...), but there are no null checks; if any query fails, btnX will be null and subsequent usage (btnX.clicked) will throw
- Start assumes btnContinue has been assigned by OnEnable; if not found, a NRE could occur
- Public surface is minimal; actual start behavior depends on GameManager and DebugManager implementations
- OnOptoinsBtnClicked is a placeholder (no real options handling)

5) Example
- Not applicable (no deriveable usage example beyond described wiring and calls)

6) Unknowns
- Exact implementations of CHAL.Core, CHAL.Data, GameManager, DebugManager
- Whether UIDocument, VisualElement queries, and button names ("NewGame", "Continue", "Options", "Exit") exist at runtime
- Behavior of characterCreationMenue outside this file
- Threading, async behavior, and scene management details beyond provided code

