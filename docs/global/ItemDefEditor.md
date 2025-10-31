# global.ItemDefEditor

_Automatically generated/updated from `Assets/src/Editor/ItemDefEdtitor.cs`._

# Purpose
- Defines a custom editor for the `ItemDef` class in Unity's Editor.

# Public API
- Namespace/module: None
- Types
  - public class ItemDefEditor : Editor
    - Public methods
      - `OnInspectorGUI()`: Overrides the default inspector GUI to display and edit properties of `ItemDef`.

# Key Behavior & Side Effects
- Displays fields for `ItemDef` properties in the inspector.
- Handles specific fields based on the `itemId` prefix (e.g., "remains:", "rune:", "part:", "module:").
- Marks the target as dirty if any GUI changes are made, triggering a save.

# Constraints & Failure Modes
- Assumes `remainData`, `runeData`, `partData`, and `moduleData` are not null when accessed.
- Uses `EditorGUILayout` for UI elements, which is specific to the Unity Editor.

# Example
```csharp
// Example usage in Unity Editor
// This class is automatically used when selecting an ItemDef asset in the inspector.
```

# Unknowns
- Specific details about the `ItemDef` class and its properties are not provided in this file.

