# CHAL.Systems.Crafting.CraftingCatalog

_Automatically generated/updated from `Assets/src/Systems/Crafting/CraftingCatalog.cs`._

```text
Purpose
- Defines a Unity ScriptableObject asset to hold crafting recipes.
- Exposes a public list of RecipeDef named recipes to store recipe definitions.
- Enables Editor asset creation via CreateAssetMenu (CraftingCatalog) under Data/CraftingCatalog.

Public API
- Namespace: CHAL.Systems.Crafting
- Types
  - public class CraftingCatalog : ScriptableObject
    - public List<RecipeDef> recipes
      - Serialized field; holds recipe definitions (initialized to empty by default)

Notes: No methods are defined in this type.

Key Behavior & Side Effects
- Initialization: recipes is initialized to an empty List<RecipeDef> by default (new()).
- Editor integration: Creates an asset via Unity Editor menu Data/CraftingCatalog with default fileName CraftingCatalog.
- Serialization: The recipes list is serialized as part of the asset.

Constraints & Failure Modes
- recipes is non-null by default; runtime null handling is not defined here.
- No runtime logic or methods; behavior is limited to asset creation and serialization.
- RecipeDef type is not defined in this file; its structure and serialization are defined elsewhere.
- Loading/usage at runtime (e.g., how the catalog is accessed) is not specified.

Unknowns
- Definition and structure of RecipeDef.
- How CraftingCatalog is consumed at runtime (loading paths, references).
- Any editor tooling beyond CreateAssetMenu (custom inspectors, editors).

```
