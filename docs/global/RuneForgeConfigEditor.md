# global.RuneForgeConfigEditor

_Automatically generated/updated from `Assets/src/Editor/RunForgeConfigEditor.cs`._

# Purpose
- Defines a custom editor for the `RuneForgeConfig` class in Unity's Editor.

# Public API
- Namespace: None
- Types
  - public class `RuneForgeConfigEditor` [extends `Editor`]
    - Public methods:
      - `OnInspectorGUI()`: Overrides the default inspector GUI to customize the display and editing of `RuneForgeConfig`.

# Key Behavior & Side Effects
- Initializes `config.entries` if null.
- Displays warnings if no "remains" or "runes" are found in the `ItemRegistry`.
- Allows adding/removing `RuneChance` entries and `RuneForgeEntry` entries through the inspector.
- Marks the `RuneForgeConfig` as dirty if any changes are made.

# Constraints & Failure Modes
- Handles null checks for `config.entries` and `entry.runes`.
- Uses `EditorGUILayout.Popup` for dropdowns, which may return -1 if no selection is made.
- List synchronization for `runeFoldouts` is maintained based on the number of `runes`.

# Example
```csharp
// Example usage in Unity Editor
// This class is used to create a custom inspector for RuneForgeConfig
```

# Unknowns
- Specific details about the `RuneForgeConfig`, `RuneForgeEntry`, and `RuneChance` classes cannot be determined from this file.
- The behavior of `ItemRegistry.Instance.GetAllItemsByType` is not defined in this file.

