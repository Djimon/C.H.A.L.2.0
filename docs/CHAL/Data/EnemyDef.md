# CHAL.Data.EnemyDef

_Automatically generated/updated from `Assets/src/Data/Defs/EnemyDef.cs`._

# Purpose
- Defines the `EnemyDef` class, representing enemy definitions including identity, stats, and rewards.

# Public API
- Namespace: `CHAL.Data`
- Types
  - public class `EnemyDef` [extends `ScriptableObject`]
    - Public fields/properties:
      - `string enemyId` - Identifier for the enemy.
      - `string displayNameKey` - Localization key for the enemy's name.
      - `int baseHP` - Base health points of the enemy.
      - `int baseDamage` - Base damage dealt by the enemy.
      - `float moveSpeed` - Movement speed of the enemy.
      - `float sightRange` - Range within which the enemy can detect players.
      - `int lootValue` - Influence on the budget system for loot.
      - `int xpReward` - Experience points awarded for defeating the enemy.
      - `EnemyRank BaseRank` - Rank of the enemy (e.g., Spawn, Normal, Magic, Elite, Boss).
      - `List<string> baseTags` - Tags associated with the enemy (e.g., "insectoid", "poison").
      - `List<SkillData> baseAttacks` - List of basic skills for the enemy.
      - `EnemyAIType aiType` - AI strategy type for the enemy.
      - `GameObject prefab` - Model/prefab used for spawning the enemy.
      - `Sprite icon` - UI icon representing the enemy.

# Key Behavior & Side Effects
- The `EnemyDef` class is used to define enemy characteristics and behaviors in the game.

# Constraints & Failure Modes
- None explicitly mentioned in the code.

# Example
```csharp
EnemyDef enemyDefinition = ScriptableObject.CreateInstance<EnemyDef>();
enemyDefinition.enemyId = "insect_worker";
enemyDefinition.baseHP = 20;
enemyDefinition.baseDamage = 5;
```

# Unknowns
- None.

