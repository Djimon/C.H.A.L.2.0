# Assets/src/Editor/SkillDataEditor.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines a custom editor for the `SkillData` class in Unity, allowing for enhanced editing of skill properties in the Inspector.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public class SkillDataEditor : Editor`
    - Public methods:
      - `public override void OnInspectorGUI()`
        - Renders the custom inspector GUI for `SkillData`, allowing editing of various skill properties.
      - `private AnimationType FilterAnimationType(AnimationType current, params AnimationType[] allowed)`
        - Ensures the current animation type is valid; returns the first allowed type if not.

# Key Behavior & Side Effects
- Updates the serialized object before rendering the GUI.
- Modifies properties of the `SkillData` instance based on user input in the Inspector.
- Marks the `SkillData` object as dirty if any changes are made, prompting Unity to save changes.

# Constraints & Failure Modes
- Assumes `SkillData` is properly set up with the expected fields.
- Handles null/empty values implicitly; no explicit guards are present.
- Performance implications are not evident from the code.

# Example
```csharp
// Example usage in Unity Editor
[CustomEditor(typeof(SkillData))]
public class SkillDataEditor : Editor { /* ... */ }
```

# Unknowns
- Specific details about the `SkillData` class and its properties are not provided in this file.
- The behavior of `OnCastImpactEffects` and `OnHitImpactEffects` properties is not defined here.
```
