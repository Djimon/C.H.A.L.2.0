# Assets/src/UI/CraftingUI.cs

_Automatically generated/updated from `Assets/src/UI/CraftingUI.cs`._

# Purpose
- Manages the crafting user interface in the game.
- Inherits from `IngameUI` to provide additional functionality.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public class CraftingUI : IngameUI`
    - Public fields/properties: None
    - Public methods: None

# Key Behavior & Side Effects
- Overrides `Awake()` to initialize the UI and set up the exit button.
- If the exit button is present, it registers a click event to hide the UI.

# Constraints & Failure Modes
- The exit button is optional; if not present, no action is taken.
- Assumes the presence of a `UIDocument` component for UI functionality.

# Example
```csharp
// Example usage in a Unity scene
CraftingUI craftingUI = gameObject.AddComponent<CraftingUI>();
```

# Unknowns
- No unknowns identified from the provided code.
