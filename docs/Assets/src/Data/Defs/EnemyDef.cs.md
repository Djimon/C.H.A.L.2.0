# Assets/src/Data/Defs/EnemyDef.cs

_Automatic generated/updated._

```markdown
# Purpose
- Defines the `EnemyDef` class as a ScriptableObject for enemy definitions in the game.

# Public API
- Namespace: `CHAL.Data`
- Types
  - `public class EnemyDef : ScriptableObject`
    - Public fields/properties:
      - `public string enemyId` - Unique identifier for the enemy.
      - `public string displayNameKey` - Localization key for the enemy's name.
      - `public int baseHP` - Base health points of the enemy.
      - `public int baseDamage` - Base damage dealt by the enemy.
      - `public float moveSpeed` - Movement speed of the enemy.
      - `public float sightRange` - Range at which the enemy can detect players.
      - `public int lootValue` - Value of loot dropped by the enemy.
      - `public int xpReward` - Experience points awarded for defeating the enemy.
      - `public EnemyRank BaseRank` - Rank classification of the enemy.
      - `public List<string> baseTags` - Tags associated with the enemy.
      - `public List<SkillData> baseAttacks` - List of skills the enemy can use.
      - `public EnemyAIType aiType` - AI behavior type of the enemy.
      - `public GameObject prefab` - Prefab used for spawning the enemy.
      - `public Sprite icon` - Icon used in the UI for the enemy.

# Key Behavior & Side Effects
- The `EnemyDef` class serves as a data/config asset for defining enemy characteristics and behaviors.

# Constraints & Failure Modes
- None explicitly stated in the file.

# Example
```csharp
EnemyDef enemyDefinition = ScriptableObject.CreateInstance<EnemyDef>();
enemyDefinition.enemyId = "insect_worker";
enemyDefinition.baseHP = 10;
enemyDefinition.baseDamage = 2;
```

# Unknowns
- No information on the implementation of `SkillData` or `EnemyRank` and `EnemyAIType` types.
```
