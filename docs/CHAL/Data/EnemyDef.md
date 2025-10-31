# CHAL.Data.EnemyDef

_Automatically generated/updated from `Assets/src/Data/Defs/EnemyDef.cs`._

# Purpose
- Defines the `EnemyDef` class as a ScriptableObject for enemy definitions in the game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `EnemyDef` [extends ScriptableObject]
    - Public fields/properties:
      - `string enemyId` - Identifier for the enemy.
      - `string displayNameKey` - Localization key for the enemy's name.
      - `int baseHP` - Base health points of the enemy.
      - `int baseDamage` - Base damage dealt by the enemy.
      - `float moveSpeed` - Movement speed of the enemy.
      - `float sightRange` - Range within which the enemy can detect players.
      - `int lootValue` - Influence on the budget system for loot.
      - `int xpReward` - Experience points rewarded for defeating the enemy.
      - `EnemyRank BaseRank` - Rank of the enemy (e.g., Normal, Elite).
      - `List<string> baseTags` - Tags associated with the enemy.
      - `List<SkillData> baseAttacks` - List of base skills for the enemy.
      - `EnemyAIType aiType` - AI strategy type for the enemy.
      - `GameObject prefab` - Prefab model for spawning the enemy.
      - `Sprite icon` - UI icon for the enemy.

# Key Behavior & Side Effects
- The `EnemyDef` class is used to define various attributes and behaviors of enemies in the game, including stats, AI type, and visual representation.

# Constraints & Failure Modes
- No explicit guards or null/empty handling noted.
- Assumes valid data is provided for all fields.

# Example
```csharp
EnemyDef enemyDefinition = ScriptableObject.CreateInstance<EnemyDef>();
enemyDefinition.enemyId = "insect_worker";
enemyDefinition.baseHP = 10;
enemyDefinition.baseDamage = 2;
```

# Unknowns
- No information on the `SkillData` type or `EnemyRank` and `EnemyAIType` enumerations.

