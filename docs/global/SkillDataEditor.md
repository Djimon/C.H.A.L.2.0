# Assets/src/Editor/SkillDataEditor.cs

_Automatically generated/updated from `Assets/src/Editor/SkillDataEditor.cs`._

# Purpose
- Custom editor for `SkillData` objects in the Unity Inspector.

# Public API
- Namespace/module: `CHAL.Data`
- Types
  - `public class SkillDataEditor : Editor`
    - Public methods
      - `public override void OnInspectorGUI()`
        - Draws the custom inspector GUI for the `SkillData` object.

# Key Behavior & Side Effects
- Updates the serialized object before drawing the GUI.
- Modifies properties of the `SkillData` object based on user input in the inspector.
- Marks the `SkillData` object as dirty if any changes are made.

# Constraints & Failure Modes
- None explicitly noted in the code.

