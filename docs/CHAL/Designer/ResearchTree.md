# How-To: Research (Tree & Nodes)

**Purpose**  
Set up research nodes, their unlocks, and the tree layout (lanes/stages). Use deterministic positions and explicit parent links.

---

## 1) Assets & Types
- **Tree:** `CHAL.Data.ResearchTreeDef` (Create > Research > Tree)  
- **Node:** `CHAL.Data.ResearchNodeDef` (Create > Research > Node)

Typical folders:  
- `/Assets/Data/Research/Tree/` (one tree SO)  
- `/Assets/Data/Research/Nodes/` (many node SOs)

---

## 2) ResearchNodeDef (Fields)
| Field | Type | Required | Notes |
|---|---|---:|---|
| `id` | `string` | ✅ | Stable ID. |
| `title` | `string` | ✅ | Defaults to asset name if empty. |
| `unlocks` | `List<ResearchUnlock>` | – | Each unlock = `{ unlockType, targetId }`. |
| `requirements` | `ResearchRequirement` | – | AND-logic of gates/flags/tags (runtime type). |
| *(internal)* `desc` | `string` | – | Internal text. |

`ResearchUnlock` = `{ unlockType: ResearchUnlockTypes, targetId: string }`.  
(`ResearchUnlockTypes` & `ResearchRequirement` are defined in runtime code; see your data model.)  fileciteturn1file6

Validation hints: ensure every `targetId` exists (recipe/map/feature), and requirements are satisfiable.

---

## 3) ResearchTreeDef (Layout & View)
| Field | Type | Notes |
|---|---|---|
| `researchLanes` | `List<ResearchLane>` | Lane labels & colors used by UI. |
| `nodeWidth`/`nodeHeight` | `int` | UI layout constants. |
| `stageStepY` | `int` | Vertical distance between stages. |
| `laneBaseX` | `List<int>` | X positions per lane (default `[300,700,1100,1500]`). |
| `topMarginY` | `int` | UI top margin. |
| `defaultGateGlyph` | `Sprite` | Icon for gates/chips. |
| `alwaysUnlockedIds` | `List<string>` | Node IDs always available from start. |
| `researchTreeLanes` | `List<ResearchTreeLane>` | The actual structured tree content. |

**Structured content**  
- `ResearchTreeLane { laneName, laneColor, stages[] }`  
- `ResearchTreeStage { nodes[] }` → **same Y (stage)**  
- `ResearchTreeNodeRef { node: ResearchNodeDef, parentRefs: List<ResearchNodeDef> }` → **parents inside the SO; compiled to IDs**

Helpers:  
- `GetLaneName(int lane)` / `GetLaneColor(int lane)` resolve labels & colors by index.  fileciteturn1file7

---

## 4) Deterministic Placement Rules
- 4 lanes recommended; vertical stage progression (10-step raster typical).  
- **No overlaps** on the same stage within a lane.  
- Parents must be reachable; avoid cycles.

---

## 5) Minimal Examples (Node & Tree Snippets)

**Node**
```yaml
id: "research_boots_t1"
title: "Boots Crafting I"
unlocks:
  - { unlockType: CraftRecipe, targetId: "craft_boots_iron_01" }
requirements:
  # Example: gate/tag-based requirement (see runtime ResearchRequirement)
```

**Tree (excerpt)**
```yaml
researchLanes:
  - { laneName: "Forge", laneColor: "#9E6AFF" }
  - { laneName: "Hunt", laneColor: "#4AC3A1" }
alwaysUnlockedIds: ["research_intro"]
researchTreeLanes:
  - laneName: "Forge"
    stages:
      - nodes:
          - { node: ref:research_intro, parentRefs: [] }
      - nodes:
          - { node: ref:research_boots_t1, parentRefs: [ref:research_intro] }
```

---

## 6) Validation Checklist
- [ ] Each `node.id` is unique and referenced correctly in the tree.
- [ ] `alwaysUnlockedIds` exist as nodes.
- [ ] No cyclic parent references.
- [ ] Unlocks target existing IDs (recipes/maps/features).

---

## 7) Related
- `ResearchNodeDef.cs`, `ResearchTreeDef.cs` (sources).  fileciteturn1file6turn1file7
- Event → Progress mapping: see project’s runtime (`ResearchEventBridge`, `ResearchService`). (Use your `KillTagCount`, `StageProgress`, etc.)
