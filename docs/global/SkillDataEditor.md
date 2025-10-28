# global.SkillDataEditor

_Automatically generated/updated from `Assets/src/Editor/SkillDataEditor.cs`._

# Purpose
- Defines a custom editor for the `SkillData` class in Unity.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public class SkillDataEditor : Editor`
    - Public methods:
      - `public override void OnInspectorGUI()`
        - Renders the custom inspector GUI for `SkillData`.
      - `private AnimationType FilterAnimationType(AnimationType current, params AnimationType[] allowed)`
        - Ensures the current animation type is within the allowed set.

# Key Behavior & Side Effects
- Updates the serialized object before rendering the GUI.
- Modifies properties of the `SkillData` instance based on user input.
- Marks the `SkillData` instance as dirty if any GUI changes occur.

# Constraints & Failure Modes
- None explicitly mentioned in the file.

# Example
```csharp
// Example usage in Unity Editor
[CustomEditor(typeof(SkillData))]
public class SkillDataEditor : Editor
{
    // Custom editor implementation...
}
```

# Unknowns
- Specific details about the `SkillData` class and its properties are not provided in this file.
- The behavior of `OnCastImpactEffects` and `OnHitImpactEffects` properties is not defined here.

