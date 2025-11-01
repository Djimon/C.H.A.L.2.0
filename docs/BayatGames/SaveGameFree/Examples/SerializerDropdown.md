# BayatGames.SaveGameFree.Examples.SerializerDropdown

_Automatically generated/updated from `Assets/src/xTernal/SaveGameFree/Examples/Shared/Scripts/SerializerDropdown.cs`._

```text
1) Purpose
- Defines a Unity UI Dropdown (SerializerDropdown) to select the serialization format (XML, JSON, Binary) used by SaveGameFree.
- Implements a singleton pattern and exposes the instance via SerializerDropdown.Singleton.
- Maintains a static list of available serializers and exposes the currently active serializer (ActiveSerializer) with lazy default to JSON; persists the selected index to storage on quit.

2) Public API
- Namespace/module
  - BayatGames.SaveGameFree.Examples

- Type
  - public class SerializerDropdown : Dropdown
    - Public properties
      - public static SerializerDropdown Singleton { get; }
      - public ISaveGameSerializer ActiveSerializer { get; }
    - (No additional public methods are defined; Awake/OnValueChanged/OnApplicationQuit are protected or virtual.)

3) Key Behavior & Side Effects
- Singleton enforcement in Awake:
  - If an instance already exists, destroy the duplicate object; otherwise assign m_Singleton and continue.
- Initialization in Awake:
  - Call base.Awake().
  - Set options to three entries: "XML", "JSON", "Binary".
  - Register OnValueChanged to respond to value changes.
  - Load previously saved serializer index (int) from storage and assign it to the dropdown value.
- Active serializer management:
  - Public ActiveSerializer returns the current serializer; if null, defaults to a new SaveGameJsonSerializer.
  - OnValueChanged(int index): sets m_ActiveSerializer to the serializer at m_Serializers[index].
- Persistence on quit:
  - OnApplicationQuit: saves the current dropdown value index under the key "serializer" using SaveGame.Save with a JSON serializer instance.
- Serializers available (static, in-order):
  - XML, JSON, Binary (instances provided in m_Serializers).

4) Constraints & Failure Modes
- Bounds assumption:
  - OnValueChanged(index) directly indexes m_Serializers; no explicit bounds checking in this method.
- OnApplicationQuit reliance:
  - Saving serializer index occurs in OnApplicationQuit; Unity may not invoke this in all contexts (e.g., editor play vs. built app).
- Lazy initialization:
  - ActiveSerializer defaults to JSON if not previously set; changing the dropdown may be required to align with m_ActiveSerializer.
- Singleton lifecycle:
  - Destruction of duplicates depends on Awake; multiple scenes could instantiate the component if not managed properly.

5) Example
- Not applicable (no derivable minimal usage snippet from this file alone).

6) Unknowns
- Exact behavior and side effects of SaveGame.Load/Save calls beyond the signatures shown.
- Interactions with external code that might manipulate the dropdown value or serializer list at runtime.
- Whether additional serializers are added elsewhere or at runtime (only three are defined here).
```
