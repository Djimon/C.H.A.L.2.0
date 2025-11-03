# CHAL.UI.CharacterCreationUI

_Automatically generated/updated from `Assets/src/UI/CharacterCreationUI.cs`._

Purpose
- Defines a Unity MonoBehaviour CHAL.UI.CharacterCreationUI that manages the character creation UI.
- Awake initializes the root VisualElement from UIDocument and sets the starting color in colors[0].
- OnEnable binds UI elements (StartGame, Back, InputName) and subscribes to button click handlers to create a profile and start a new game or hide the UI.

Public API
- Namespace/module: CHAL.UI
- Types
  - public class CharacterCreationUI : MonoBehaviour
    - Public fields/properties: 
      - _startSceneName: serialized string for the starting scene name (unused).
    - Public methods: none

Key Behavior & Side Effects
- Unity lifecycle
  - Awake: obtains root VisualElement via GetComponent<UIDocument>().rootVisualElement; initializes colors[0] to a specific color (50/255f, 50/255f, 180/255f).
  - OnEnable: queries UI elements and wires event handlers
    - btnNewGame = root.Q<Button>("StartGame"); btnNewGame.clicked += OnNewGameBtnClicked;
    - btnBack = root.Q<Button>("Back"); btnBack.clicked += OnBackBtnClicked;
    - name_input = root.Q<TextField>("InputName");
- OnNewGameBtnClicked
  - Creates a new PlayerProfile
  - Calls profile.InitializePlayer(name_input.text, colors)
  - Calls GameManager.Instance.StartNewGame(profile)
- OnBackBtnClicked
  - Deactivates the GameObject (gameObject.SetActive(false))

Constraints & Failure Modes
- Potential null references if UI elements are missing or misnamed (btnNewGame, btnBack, name_input); no null checks are present before subscribing or querying.
- Subscribing to btnNewGame.clicked in OnEnable without corresponding unsubscribe can accumulate handlers if the object is enabled/disabled repeatedly.
- _startSceneName is serialized but unused in this file; potential leftover data.
- colors is length-1; only colors[0] is used; no handling for changes beyond initialization.
- Behavior of PlayerProfile, InitializePlayer, and GameManager.Instance.StartNewGame is not defined here; relies on external implementations.

Unknowns
- Exact implementations and side effects of PlayerProfile.InitializePlayer and GameManager.Instance.StartNewGame.
- Structure and contents of the UIDocument and the UIElements (IDs: StartGame, Back, InputName).
- Any additional behavior triggered by StartNewGame beyond starting a new game (e.g., scene transitions, loading indicators).
- What Replace or reset happens if the UI is reopened after being closed (state retention).

Unity lifecycle (summary)
- Awake: initialize root VisualElement and color data.
- OnEnable: bind UI elements and event handlers.

