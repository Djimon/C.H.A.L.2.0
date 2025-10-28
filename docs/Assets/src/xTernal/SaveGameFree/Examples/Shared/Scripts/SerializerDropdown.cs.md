# Assets/src/xTernal/SaveGameFree/Examples/Shared/Scripts/SerializerDropdown.cs

_Automatic generated/updated._

```markdown
## Purpose
- Defines a dropdown UI component for selecting different save game serializers.

## Public API
- Namespace: `BayatGames.SaveGameFree.Examples`
- Types
  - `public class SerializerDropdown : Dropdown`
    - Public fields/properties:
      - `public ISaveGameSerializer ActiveSerializer` - Gets the currently active serializer.
    - Public methods:
      - `protected override void Awake()` - Initializes the dropdown and singleton instance.
      - `protected virtual void OnValueChanged(int index)` - Updates the active serializer based on dropdown selection.
      - `protected virtual void OnApplicationQuit()` - Saves the selected serializer index on application quit.

## Key Behavior & Side Effects
- Implements a singleton pattern for the `SerializerDropdown`.
- Initializes dropdown options for XML, JSON, and Binary serializers.
- Listens for value changes to update the active serializer.
- Saves the selected serializer index when the application quits.

## Constraints & Failure Modes
- Ensures only one instance of `SerializerDropdown` exists; destroys duplicates.
- Defaults to `SaveGameJsonSerializer` if no active serializer is set.
- Uses `SaveGame.Load` and `SaveGame.Save` for persistent storage of the selected serializer index.

## Example
```csharp
// Example usage in a Unity scene
void Start() {
    var dropdown = SerializerDropdown.Singleton;
    dropdown.value = 1; // Set to JSON serializer
}
```

## Unknowns
- None.
```
