# Assets/src/UI/CheatMenuController.cs

_Automatically generated/updated from `Assets/src/UI/CheatMenuController.cs`._

# Purpose
- Defines the `CheatMenuController` class for managing a cheat menu in the game UI.

# Public API
- Namespace: `CHAL.UI`
- Types
  - `public sealed class CheatMenuController : IngameUI`
    - Public fields/properties:
      - `UIDocument document`: The UI document for the cheat menu.
      - `string partsPrefix`: Prefix for parts items.
      - `string remainsPrefix`: Prefix for remains items.
      - `string gearPrefix`: Prefix for gear items.
      - `string modulesPrefix`: Prefix for module items.
    - Public methods:
      - `void OnEnable()`: Initializes the cheat menu UI.
      - `void ClearInventoryAndCleanupGearInstances(PlayerInventoryType t)`: Clears inventory and cleans up gear instances.
      - `void FillDropdownsFromRegistry()`: Fills dropdowns with items from the item registry.
      - `bool TryAddToInventoryDomain(string itemId, int count, string contextLabel)`: Attempts to add an item to the inventory.
      - `void AddGearRolled(string itemId, string tierStr)`: Adds a rolled gear item to the inventory.
      - `void AddGearCustom(string itemId, string tierStr)`: Adds a custom gear item to the inventory.
      - `void ResetAllHeroesProgress()`: Resets all heroes' progress to level 1.
      - `void UnlockAllResearch()`: Unlocks all research nodes.

# Key Behavior & Side Effects
- On enabling, initializes UI components and binds button actions.
- Buttons trigger inventory modifications, such as adding items or resetting hero progress.
- Dropdowns are populated from the item registry and can be refreshed based on gear type selection.

# Constraints & Failure Modes
- If `UIDocument` is missing, logs an error and does not proceed with UI initialization.
- Inventory operations check for the existence of `GameManager` and its components, logging errors if they are missing.
- Limits on the number of implicits and affixes that can be added to gear are enforced.

# Example
```csharp
var cheatMenuController = new CheatMenuController();
cheatMenuController.OnEnable();
```

# Unknowns
- The exact structure and properties of `ImplicitDef`, `AffixDef`, and other referenced types are not defined in this file.
- The behavior of `GameManager`, `ItemRegistry`, and other external systems is assumed based on their usage.

