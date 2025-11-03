# CHAL.Systems.Crafting.CraftingCatalog

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingCatalog.cs`._

1) Purpose
- Defines CraftingCatalog as a ScriptableObject in CHAL.Systems.Crafting.
- Exposes a public List<RecipeDef> named recipes to hold crafting recipe definitions.
- Enables Unity Editor asset creation via CreateAssetMenu (CraftingCatalog).

2) Public API
- Namespace/module
  - CHAL.Systems.Crafting
- Types
  - public class CraftingCatalog : ScriptableObject
    - Attributes
      - [CreateAssetMenu(fileName = "CraftingCatalog", menuName = "Data/CraftingCatalog")]
    - Public fields/properties
      - public List<RecipeDef> recipes = new();
        - Role: stores the crafting recipes; serialized by Unity
    - Public methods
      - None

3) Key Behavior & Side Effects
- Editor behavior
  - CreateAssetMenu enables creating a CraftingCatalog asset from the Unity editor (Assets > Create > Data > CraftingCatalog).
- Data behavior
  - recipes is initialized to an empty list by default.
  - The asset acts as a data container for RecipeDef items; no runtime methods defined in this file.

4) Constraints & Failure Modes
- Serialization
  - public List<RecipeDef> recipes is serialized by Unity; no explicit null guards in this file.
- Validation
  - No in-file validation or guards for nulls, duplicates, or constraints on the list.
- Concurrency/async
  - Not applicable within this file.

5) Example
- Unity editor usage:
  - Create a CraftingCatalog asset via Assets > Create > Data > CraftingCatalog.
  - In the inspector, populate the recipes list with RecipeDef entries as needed.

6) Unknowns
- Definition and structure of RecipeDef (not present in this file).
- How CraftingCatalog is consumed by systems at runtime.
- Any additional fields or methods that may be added in other parts of the project.

