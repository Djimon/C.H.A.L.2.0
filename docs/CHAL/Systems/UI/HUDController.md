# Assets/src/UI/HUDController.cs

_Automatically generated/updated from `Assets/src/UI/HUDController.cs`._

# Purpose
- Defines the `HudCodexController` class, which manages the HUD button for toggling the Codex UI.

# Public API
- Namespace: `CHAL.Systems.UI`
- Types
  - `public sealed class HudCodexController : IngameUI`
    - Public fields/properties:
      - `UIDocument codexDocument` (optional): Assigns the Codex UI document or auto-finds it.
    - Public methods:
      - `protected override void Awake()`: Initializes the controller, binds UI elements, and sets up event hooks.
      - `private void OnDestroy()`: Unsubscribes from events when destroyed.
      - `private void ToggleOpen()`: Toggles the visibility of the Codex UI.
      - `private void SetOpen(bool open)`: Sets the Codex UI open state.
      - `private void OnCodexChanged()`: Updates the Codex badge when the Codex changes.
      - `private void UpdateCodexBadge()`: Updates the badge text based on the active deed progress.

# Key Behavior & Side Effects
- On `Awake`, initializes the GameManager and CodexService, binds UI elements, resolves the Codex document, and hooks events.
- Toggles the Codex UI visibility when the button is clicked.
- Updates the badge text to reflect the progress of the active deed.

# Constraints & Failure Modes
- If `GameManager.Instance` or `GameManager.codexService` is null, the controller disables itself.
- If the button or badge is not found, errors are logged.
- If no `codexDocument` is assigned or found, an error is logged.

# Example
```csharp
// Example usage in a Unity scene
var hudCodexController = new HudCodexController();
hudCodexController.Awake(); // Initializes the controller
```

# Unknowns
- None.

