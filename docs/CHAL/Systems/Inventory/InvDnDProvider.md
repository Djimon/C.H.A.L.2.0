# CHAL.Systems.Inventory.InvDnDProvider

_Automatically generated/updated from `Assets/src/Systems/Inventory/InvDnDProvider.cs`._

# Purpose
- Provides functionality for inventory drag and drop operations.
- Integrates with Unity's game object lifecycle through MonoBehaviour.

# Public API
- Namespace: `CHAL.Systems.Inventory`
- Types
  - `public class InvDnDProvider : MonoBehaviour`
    - Public fields/properties:
      - `IInventoryDomain domain`: Set per Inspector/Bootstrap.
      - `DragDropService Service`: Lazy-initialized service for drag and drop operations.
    - Public methods:
      - `private void OnValidate()`: Rebuilds the service in the editor if the domain is set and the service is null.

# Key Behavior & Side Effects
- The `Service` property initializes a new `DragDropService` instance if it is null and the `domain` is not null.
- The `OnValidate` method ensures that the service is rebuilt in the editor when the domain is assigned.

# Constraints & Failure Modes
- The `Service` property will return null if `domain` is null.
- The `OnValidate` method is only called in the Unity editor, not at runtime.
