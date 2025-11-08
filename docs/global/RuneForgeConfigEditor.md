# global.RuneForgeConfigEditor

_Automatically generated/updated from `Assets/src/Editor/RunForgeConfigEditor.cs`._

# Purpose
- Provides a custom editor for configuring RuneForge settings.

# Public API
- Namespace: None
- Types
  - public class RuneForgeConfigEditor : Editor
    - Public methods
      - public override void OnInspectorGUI()
        - Draws the custom inspector GUI for the RuneForge configuration and manages the entries and rune chances.

# Key Behavior & Side Effects
- Initializes `config.entries` if null.
- Displays warnings if no "remains" or "runes" are found in the ItemRegistry.
- Allows adding/removing of rune chances and remains through the GUI.
- Marks the `config` as dirty if any changes are made.

# Constraints & Failure Modes
- Handles null entries for `config.entries` and `entry.runes`.
- Ensures `runeFoldouts` list is synchronized with the number of rune chances.
- Uses `EditorGUILayout.Popup` for dropdowns, which may result in no selection if the index is out of range.

# Example
```csharp
// Example usage in Unity Editor
// This script is used to create a custom editor for RuneForgeConfig assets.
```

# Unknowns
- None.

