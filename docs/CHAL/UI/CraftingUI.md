# CHAL.UI.CraftingUI

_Automatically generated/updated from `Assets/src/UI/CraftingUI.cs`._

# Purpose
- Defines the `CraftingUI` class, which manages the crafting user interface in the game.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public class CraftingUI : IngameUI`
    - Public fields/properties: None
    - Public methods: None

# Key Behavior & Side Effects
- Inherits from `IngameUI` and overrides the `Awake` method to initialize the UI.
- Retrieves the root visual element from a `UIDocument` and assigns it to `root`.
- Sets up a button click event for the exit button, which hides the UI when clicked.

# Constraints & Failure Modes
- The exit button is optional; if it is not found, no action is taken.
- Assumes the presence of a `UIDocument` component for UI initialization.

# Example
```csharp
// Example usage in a Unity scene
CraftingUI craftingUI = gameObject.AddComponent<CraftingUI>();
```

# Unknowns
- None
