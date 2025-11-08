# How-To: Waves & Maps (ScriptableObjects)

**Purpose**  
Define encounter structure (waves) and the maps that host them, with clear constraints and spawn backloading.

---

## 1) Assets & Locations
- **Map:** `CHAL.Data.MapDef` — `Create > Data > Map Definition` (e.g., `/Assets/Data/Maps/`)
- **Wave:** `CHAL.Data.WaveDef` — `Create > Data > Wave Definition` (e.g., `/Assets/Data/Waves/`)

---

## 2) WaveDef (Fields)
| Field | Type | Required | Notes |
|---|---|---:|---|
| `spawnCount` | `int` | – | Count of rank `Spawn` enemies. |
| `normalCount` | `int` | – | Count of rank `Normal`. |
| `magicCount` | `int` | – | Count of rank `Magic`. |
| `eliteCount` | `int` | – | Count of rank `Elite`. |
| `bossCount` | `int` | – | Count of rank `Boss`. |
| `championCount` | `int` | – | Count of rank `Champion`. |
| `maxTagsPerEnemy` | `int` | – | Constraint for tagging system. |
| `maxElites` | `int` | – | Upper bound. |
| `maxBosses` | `int` | – | Upper bound. |
| `maxChampions` | `int` | – | Upper bound. |
| `backload` | `BackloadProfile` | – | Spawn delays per rank (0…5). |

`BackloadProfile` exposes `GetSpawnDelayAlpha(EnemyRank)` and per-rank delay alphas:  
`alphaSpawnDelay`, `alphaNormalDelay`, `alphaMagicDelay`, `alphaEliteDelay`, `alphaBossDelay`, `alphaChampionDelay`.  fileciteturn1file8

**Runtime note:** `ToComposition(int baseLevel, MapDifficulty difficulty)` creates a `WaveComposition` shell to be populated by the `WaveManager`.  fileciteturn1file8

**EnemyRank (project enum)**: `Spawn, Normal, Magic, Elite, Boss, Champion` (per project spec).

---

## 3) MapDef (Fields)
| Field | Type | Required | Notes |
|---|---|---:|---|
| `mapId` | `int` | ✅ | Internal numeric ID (comment example suggests slugs like `desert_01`, but field is `int`). |
| `displayNameKey` | `string` | – | Localization key (e.g., `MAP_DESERT`). |
| `previewImage` | `Sprite` | – | Used by MapSelection UI. |
| `mapPrefab` | `GameObject` | – | Scene content spawned by MapManager. |
| `baseLevel` | `int` | – | Starting enemy level. |
| `maxWaves` | `int` | – | Number of waves in this map. |
| `difficulty` | `MapDifficulty` | – | Base difficulty for tuning. |
| `heroSlots` | `int` | – | Team size. |
| `allowedEnemies` | `List<EnemyDef>` | – | Pool of allowed enemies. |
| `allowedModifiers` | `List<string>` | – | Additional gameplay modifiers. |
| `waveDefs` | `List<WaveDef>` | – | Template waves (concrete for now). |
| `subWaveCount` | `int` | – | Subwaves per wave. |
| `interSubWaveDelay` | `float` | – | Time between subwaves (s). |
| `maxConCurrentEnemies` | `int` | – | Concurrency cap. |

> Note: The code comment for `mapId` hints at a textual ID like `"desert_01"`, but the field type is `int`. Ensure the numeric `mapId` is unique and consider adding a separate string slug if needed.  fileciteturn1file9

---

## 4) Minimal Examples

**WaveDef**
```yaml
normalCount: 12
magicCount: 2
eliteCount: 1
bossCount: 0
championCount: 0
maxTagsPerEnemy: 2
maxElites: 2
maxBosses: 1
maxChampions: 0
backload:
  alphaSpawnDelay: 0
  alphaNormalDelay: 0
  alphaMagicDelay: 0
  alphaEliteDelay: 1.5
  alphaBossDelay: 2
  alphaChampionDelay: 5
```

**MapDef**
```yaml
mapId: 1001
displayNameKey: "MAP_DESERT"
baseLevel: 1
maxWaves: 5
difficulty: Normal
heroSlots: 1
allowedEnemies: [ref:enemy_scorpion, ref:enemy_voidling]
allowedModifiers: ["heat_haze"]
waveDefs: [ref:wave_desert_01, ref:wave_desert_02]
subWaveCount: 5
interSubWaveDelay: 10.0
maxConCurrentEnemies: 25
```

---

## 5) Validation Checklist
- [ ] `mapId` unique (numeric) and referenced where needed.
- [ ] `waveDefs` exist; counts align with `maxWaves` or manager logic.
- [ ] Backload/profile values sensible; no negative caps.
- [ ] Concurrency limit realistic for performance targets.

---

## 6) Reward Guard (Runtime Rule)
- Reward screen must **only** open if **`NoPendingLoot == true`**. Make sure spawn pacing and subwave delays can’t strand loot on the ground at wave end.

---

## 7) Related
- `WaveDef.cs`, `MapDef.cs` (sources).  fileciteturn1file8turn1file9
