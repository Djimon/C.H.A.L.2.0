# BayatGames.SaveGameFree.Examples.SerializerDropdown

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Shared/Scripts/SerializerDropdown.cs`._

# Purpose
- Defines a dropdown UI component for selecting different serializers (XML, JSON, Binary).

# Public API
- Namespace: `BayatGames.SaveGameFree.Examples`
- Types
  - `public class SerializerDropdown : Dropdown`
    - Public fields/properties:
      - `ISaveGameSerializer ActiveSerializer`: Gets the currently active serializer, defaulting to JSON if not set.
    - Public methods:
      - `protected override void Awake()`: Initializes the dropdown and singleton instance.
      - `protected virtual void OnValueChanged(int index)`: Updates the active serializer based on the selected dropdown index.
      - `protected virtual void OnApplicationQuit()`: Saves the selected serializer index when the application quits.

# Key Behavior & Side Effects
- Implements a singleton pattern to ensure only one instance of `SerializerDropdown` exists.
- Initializes dropdown options for serializer selection and sets up a listener for value changes.
- Saves the selected serializer index to persistent storage on application quit.

# Constraints & Failure Modes
- If an instance of `SerializerDropdown` already exists, the new instance is destroyed.
- The `ActiveSerializer` defaults to a new `SaveGameJsonSerializer` if not explicitly set.

# Example
```csharp
// Example usage in a Unity scene
void Start()
{
    SerializerDropdown dropdown = FindObjectOfType<SerializerDropdown>();
    dropdown.onValueChanged.AddListener((index) => {
        Debug.Log("Selected Serializer: " + dropdown.ActiveSerializer.GetType().Name);
    });
}
```

# Unknowns
- None.

