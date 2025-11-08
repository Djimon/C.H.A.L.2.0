# global.ItemDefEditor

_Automatically generated/updated from `Assets/src/Editor/ItemDefEdtitor.cs`._

# Purpose
- Provides a custom editor for the `ItemDef` object in the Unity Inspector, allowing users to edit item properties visually.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public class ItemDefEditor : Editor`
    - Public methods
      - `public override void OnInspectorGUI()`
        - Draws the custom inspector GUI for the `ItemDef` object, allowing editing of item properties.

# Key Behavior & Side Effects
- Displays fields for `itemId`, `icon`, `rarity`, and `lootValue`.
- Conditionally displays additional fields based on the prefix of `itemId` (e.g., `remains:`, `rune:`, `part:`, `module:`, `gear:`).
- Calls `EditorUtility.SetDirty(target)` if any GUI changes occur, marking the target as dirty for saving.

# Constraints & Failure Modes
- Uses a helper method `Ensure<T>(ref T field)` to initialize data fields if they are null.
- Handles dynamic array resizing for string arrays with `DrawStringArray(ref string[] arr, string label)`.

# Example
```csharp
// Example usage in Unity Editor
[CustomEditor(typeof(ItemDef))]
public class ItemDefEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Custom GUI logic for ItemDef
    }
}
```

# Unknowns
- None.

