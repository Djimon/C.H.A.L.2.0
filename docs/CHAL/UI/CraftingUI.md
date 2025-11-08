# CHAL.UI.CraftingUI

_Automatically generated/updated from `Assets/src/UI/CraftingUI.cs`._

# Purpose
- Manages the crafting user interface in the game.
- Inherits from `IngameUI` to provide additional functionality.

# Public API
- Namespace: `CHAL.UI`
- Types
  - public class `CraftingUI` [extends `IngameUI`]
    - Private fields:
      - `Button _btnExit`: Button for exiting the crafting UI.
    - Public methods:
      - `void Awake()`: Initializes the UI and sets up the exit button.

# Key Behavior & Side Effects
- Calls `base.Awake()` to initialize the base class.
- Retrieves the root visual element from the `UIDocument`.
- Sets up a click event on the exit button to hide the UI when clicked.

# Constraints & Failure Modes
- The exit button is optional; if not present, no action is taken.

