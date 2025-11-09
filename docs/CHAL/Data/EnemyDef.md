# Assets/src/Data/Defs/EnemyDef.cs

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
      - `int lootValue` - Influence on the budget system.
      - `int xpReward` - Experience points rewarded upon defeat.
      - `EnemyRank BaseRank` - Rank of the enemy (e.g., Spawn, Normal, Magic, Elite, Boss).
      - `List<string> baseTags` - Tags associated with the enemy (e.g., "insectoid", "poison").
      - `List<SkillData> baseAttacks` - List of basic skills the enemy can use.
      - `EnemyAIType aiType` - Type of AI strategy employed by the enemy.
      - `GameObject prefab` - Model/prefab used for spawning the enemy.
      - `Sprite icon` - UI icon representing the enemy.

# Key Behavior & Side Effects
- The `EnemyDef` class serves as a data container for enemy attributes, which can be used to instantiate enemies in the game.

# Constraints & Failure Modes
- None explicitly defined in the code.

# Example
```csharp
EnemyDef enemyDefinition = ScriptableObject.CreateInstance<EnemyDef>();
enemyDefinition.enemyId = "insect_worker";
enemyDefinition.baseHP = 20;
```

# Unknowns
- None.

