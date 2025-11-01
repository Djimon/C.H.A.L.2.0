# CHAL.UI.CharacterCreationUI

_Automatically generated/updated from `Assets/src/UI/CharacterCreationUI.cs`._

1) Purpose
- Defines a Unity MonoBehaviour that handles the character creation UI.
- Reads UI elements from a UIDocument root and initializes a PlayerProfile.
- Triggers start-of-game flow or hides the UI when Back is pressed.

2) Public API
- Namespace/module: CHAL.UI
- Types
  - public class CharacterCreationUI : MonoBehaviour
    - Public fields/properties: none
    - Public methods: none

3) Key Behavior & Side Effects
- Awake
  - Finds the UIDocument root VisualElement and initializes colors[0] to a specific blue shade (Color(50/255f, 50/255f, 180/255f)).
- OnEnable
  - Queries UI elements by name:
    - Start button: "StartGame" -> btnNewGame
    - Back button: "Back" -> btnBack
    - Name input: "InputName" -> name_input
  - Subscribes to button click events:
    - btnNewGame.clicked += OnNewGameBtnClicked
    - btnBack.clicked += OnBackBtnClicked
- OnNewGameBtnClicked
  - Creates a new PlayerProfile
  - Calls profile.InitializePlayer(name_input.text, colors)
  - Calls GameManager.Instance.StartNewGame(profile)
- OnBackBtnClicked
  - Deactivates this GameObject (hides the UI)
- Side effects/notes
  - OnEnable subscribes to button events each time the component is enabled; there is no corresponding OnDisable to unsubscribe, which can lead to multiple subscriptions if the UI is toggled on/off repeatedly.
  - Relies on UI elements existing in the UIDocument with exact names StartGame, Back, and InputName; otherwise null references may occur.

4) Constraints & Failure Modes
- Nullability guards are not present:
  - If the UIDocument or root VisualElement is missing, Awake will fail.
  - If StartGame/Back buttons or InputName TextField are not found, btnNewGame/btnBack may be null, causing NullReferenceException when subscribing to clicked.
  - name_input may be null; OnNewGameBtnClicked uses name_input.text.
- No OnDisable handling; potential multiple event subscriptions if the UI is enabled multiple times.
- _startSceneName is serialized but unused within this file.

5) Example
- Not derivable from this file (no public API surface beyond the class). No code example included.

6) Unknowns
- Behavior of PlayerProfile.InitializePlayer (validation rules, defaults).
- Semantics and side effects of GameManager.Instance.StartNewGame(profile).
- Exact UI structure and how colors are used elsewhere.
- Any external side effects from disabling/enabling this UI beyond the subscription behavior described.
