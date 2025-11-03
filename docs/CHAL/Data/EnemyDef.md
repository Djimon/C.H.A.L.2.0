# CHAL.Data.EnemyDef

_Automatically generated/updated from `Assets/src/Data/Defs/EnemyDef.cs`._

Purpose
- Defines EnemyDef as a ScriptableObject data asset under CHAL.Data.
- Exposes public fields to configure identity, base stats, rewards, AI, and visuals for an enemy.
- Supports Unity Editor creation via CreateAssetMenu.

Public API
- Namespace/module: CHAL.Data
- Types
  - public class EnemyDef : ScriptableObject
    - Public fields
      - public string enemyId;               // e.g., "insect_worker"
      - public string displayNameKey;        // localization key for name
      - public int baseHP = 10;
      - public int baseDamage = 2;
      - public float moveSpeed = 2f;
      - public float sightRange = 10f;
      - public int lootValue = 1;            // influences budget system
      - public int xpReward = 1;
      - public EnemyRank BaseRank = EnemyRank.Normal;  // Rank = Spawn, Normal, Magic, Elite, Boss
      - public List<string> baseTags = new();             // e.g., "insectoid", "poison"
      - public List<SkillData> baseAttacks = new();       // base skills
      - public EnemyAIType aiType = EnemyAIType.AttackFirst;  // simple AI strategy
      - public GameObject prefab;            // model/prefab to spawn
      - public Sprite icon;                  // UI icon
    - No public methods defined

Key Behavior & Side Effects
- No methods or runtime behavior defined; this file is a data container.
- Asset creation via CreateAssetMenu is an Editor-time convenience; runtime behavior is driven by other systems that consume this data.

Constraints & Failure Modes
- Lists baseTags and baseAttacks are initialized to non-null empty lists by default.
- prefab and icon are not initialized by default; may be null unless set in the asset.
- No validation present; relies on consuming code to handle nulls or invalid values.
- Types referenced but not defined here (EnemyRank, SkillData, EnemyAIType) are assumed to exist elsewhere in the project.

Example
// Minimal runtime instantiation (not saved as asset)
using UnityEngine;
using CHAL.Data;

public class EnemyDefExampleUsage
{
    public void CreateExample()
    {
        EnemyDef def = ScriptableObject.CreateInstance<EnemyDef>();
        def.enemyId = "example_enemy";
        // other fields will retain defaults unless set here
        // e.g., def.baseHP = 15;
    }
}

Unknowns
- Definitions and semantics of EnemyRank, SkillData, EnemyAIType beyond their usage here.
- How this asset interacts with systems creating enemies, spawning logic, or reward calculations.
- Any serialization behavior specifics beyond the provided default values (e.g., persistence of initializers in Unity editor).

