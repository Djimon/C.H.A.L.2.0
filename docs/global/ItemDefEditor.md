# Assets/src/Editor/ItemDefEdtitor.cs

_Automatically generated/updated from `Assets/src/Editor/ItemDefEdtitor.cs`._

# Purpose
- Provides a custom editor for the `ItemDef` object in the Unity Inspector, allowing users to edit item properties visually.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public class ItemDefEditor : Editor`
    - Public methods
      - `public override void OnInspectorGUI()`
        - Draws the custom inspector GUI for the `ItemDef` object, allowing editing of item properties in the Unity Inspector.

# Key Behavior & Side Effects
- Displays fields for `itemId`, `icon`, `rarity`, and `lootValue`.
- Conditionally displays additional fields based on the prefix of `itemId` (e.g., "remains:", "rune:", "part:", "module:", "gear:").
- Calls `EditorUtility.SetDirty(target)` if any GUI changes are made, marking the target as dirty to ensure changes are saved.

# Constraints & Failure Modes
- Uses `Ensure<T>(ref T field)` to initialize data fields if they are null.
- Handles null or empty arrays in `DrawStringArray(ref string[] arr, string label)` by resizing the array based on user input.

# Example
```csharp
// Example usage in Unity Editor
// This class is used automatically when selecting an ItemDef object in the Inspector.
```

# Unknowns
- None.

