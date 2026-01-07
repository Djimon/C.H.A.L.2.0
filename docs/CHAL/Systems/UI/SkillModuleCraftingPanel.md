# Assets/src/UI/SkilLCraftingUIController.cs

_Automatically generated/updated from `Assets/src/UI/SkilLCraftingUIController.cs`._

# Purpose
- Defines the `SkillModuleCraftingPanel` class for managing the crafting UI of skill modules in the game.

# Public API
- Namespace: `CHAL.Systems.UI`
- Types
  - public class `SkillModuleCraftingPanel` [extends `IngameUI`]
    - Public fields/properties: None
    - Public methods:
      - `void Awake()`
      - `void BindUI()`
      - `void BuildModuleList()`
      - `List<ItemDef> GetAllSkillModuleItems()`
      - `void HookEvents()`
      - `void SelectModule(ItemDef moduleItem)`
      - `void BuildCoreDropdownForModule(ItemDef moduleItem)`
      - `List<ItemDef> GetAllCoreItemsForSkill(SkillModuleDef skillDef, out ItemDef defaultCoreItem)`
      - `ItemDef ResolveCoreFromDropdownValue(string value)`
      - `void RefreshPreview()`
      - `void RenderPreview(SkillModuleCraftPreview preview)`
      - `void OnCraftClicked()`

# Key Behavior & Side Effects
- Initializes UI components and binds them to the game data in `Awake()`.
- Builds a list of skill modules and populates the UI with available options.
- Handles user interactions through event callbacks for UI elements like sliders and buttons.
- Crafts skill modules and updates the UI based on crafting success or failure.

# Constraints & Failure Modes
- If the `root` or `GameManager` is null during initialization, the component is disabled.
- The crafting process checks for valid selections of modules and cores; failure messages are displayed if crafting cannot proceed.
- The tier slider is clamped between minimum and maximum values based on configuration.

# Example
```csharp
var craftingPanel = new SkillModuleCraftingPanel();
craftingPanel.Awake(); // Initializes the UI and binds data
```

# Unknowns
- The exact structure of `ItemDef`, `SkillModuleCraftPreview`, and other referenced types is not defined in this file.
- The behavior of `CraftingService` methods is not detailed in this file.

