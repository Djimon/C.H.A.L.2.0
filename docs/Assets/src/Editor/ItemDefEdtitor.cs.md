# Assets/src/Editor/ItemDefEdtitor.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a custom editor for the `ItemDef` class in Unity's Editor.
- Provides a user interface for editing various properties of `ItemDef` instances.

# Public API
- Namespace/module: `CHAL.Data`
- Types
  - `public class ItemDefEditor : Editor`
    - Public methods:
      - `public override void OnInspectorGUI()`
        - Displays and allows editing of `ItemDef` properties in the inspector.
        - Calls `EditorUtility.SetDirty(target)` if any GUI changes are made.

# Key Behavior & Side Effects
- Displays fields for `itemId`, `icon`, `rarity`, and `lootValue` in the inspector.
- Shows additional fields based on the prefix of `itemId`:
  - For `remains:`: Displays `remainType`.
  - For `rune:`: Displays `effectType` and `runeColortType`.
  - For `part:`: Displays `dnaType`.
  - For `module:`: Displays `modulePower` and `effect`.
- Marks the target as dirty if any changes are made in the inspector.

# Constraints & Failure Modes
- Assumes `remainData`, `runeData`, `partData`, and `moduleData` are not null when accessed.
- Uses `EditorGUILayout` for UI elements, which may not handle null values gracefully.

# Example
```csharp
// Example usage in Unity Editor
// This editor will be used automatically when selecting an ItemDef asset in the inspector.
```

# Unknowns
- Specific implementation details of the `ItemDef` class and its data structures (e.g., `remainData`, `runeData`, etc.) cannot be determined from this file.
```
