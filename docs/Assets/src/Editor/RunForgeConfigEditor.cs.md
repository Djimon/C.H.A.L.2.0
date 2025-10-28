# Assets/src/Editor/RunForgeConfigEditor.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a custom editor for the `RuneForgeConfig` class in Unity's Editor.
- Provides a user interface for managing entries and associated rune chances.

# Public API
- Namespace: None
- Types
  - `RuneForgeConfigEditor : Editor`
    - Public methods:
      - `OnInspectorGUI()`: Customizes the inspector GUI for `RuneForgeConfig`.

# Key Behavior & Side Effects
- Initializes `config.entries` if null.
- Displays warnings if no "remains" or "runes" are found in the `ItemRegistry`.
- Allows adding/removing `RuneChance` entries and modifying their properties.
- Marks the `RuneForgeConfig` as dirty if any changes are made.

# Constraints & Failure Modes
- Handles null entries for `runes` and `remain`.
- Synchronizes the `runeFoldouts` list with the number of `runes`.
- Uses `EditorGUILayout.Popup` for dropdowns, which may lead to index errors if not handled properly.

# Example
```csharp
// Example usage in Unity Editor
// This script is automatically used when selecting a RuneForgeConfig asset in the Inspector.
```

# Unknowns
- The structure and properties of `RuneForgeConfig`, `RuneForgeEntry`, and `RuneChance` are not defined in this file.
- The behavior of `ItemRegistry.Instance.GetAllItemsByType` is not detailed here.
```
