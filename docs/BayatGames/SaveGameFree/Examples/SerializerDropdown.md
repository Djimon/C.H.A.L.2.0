# BayatGames.SaveGameFree.Examples.SerializerDropdown

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Shared/Scripts/SerializerDropdown.cs`._

# Purpose
- Defines a dropdown UI component for selecting different save game serializers.

# Public API
- Namespace: `BayatGames.SaveGameFree.Examples`
- Types
  - `public class SerializerDropdown : Dropdown`
    - Public fields/properties:
      - `public ISaveGameSerializer ActiveSerializer` - Gets the currently active serializer.
    - Public methods:
      - `protected override void Awake()` - Initializes the dropdown and singleton instance.
      - `protected virtual void OnValueChanged(int index)` - Updates the active serializer based on dropdown selection.
      - `protected virtual void OnApplicationQuit()` - Saves the selected serializer index when the application quits.

# Key Behavior & Side Effects
- Implements a singleton pattern for the `SerializerDropdown`.
- Initializes the dropdown options for XML, JSON, and Binary serializers.
- Listens for value changes to update the active serializer.
- Saves the selected serializer index on application quit.

# Constraints & Failure Modes
- Ensures only one instance of `SerializerDropdown` exists; destroys duplicates.
- Initializes `m_ActiveSerializer` to a default `SaveGameJsonSerializer` if not set.

# Example
```csharp
// Example usage in a Unity scene
void Start() {
    SerializerDropdown dropdown = FindObjectOfType<SerializerDropdown>();
    dropdown.value = 1; // Set to JSON serializer
}
```

# Unknowns
- None.

