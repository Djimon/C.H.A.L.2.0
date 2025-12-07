# C.H.A.L. Skill-System – Basis-Design  
_Families → Module → Archetype → Core → Orbits_

Ziel: Wenige saubere Datenschichten, klarer Resolve-Flow, viel Ausdruckskraft beim Skilldesign.

---

## 1. Schichten-Überblick

1. **Skill-Family (F1–F13) – Form & Standardverhalten**  
   - Spin, Slam, Dash, Nova, Projectile, Hazard, Minion, Aura, Curse etc.  
   - Enthält Default-Werte (Damage, Radius, Dauer, Cooldown, CastTime).  
   - Wird selten angefasst, dient als Template & Filter.

2. **Skill-Module (ModuleDef) – konkreter Skill**  
   - Eintrag wie `CausticField`, `RazorMaelstrom`, `SolarGuardian`.  
   - Verweist auf `skill_family`.  
   - Überschreibt nur Abweichungen (`base_override`).  
   - Definiert `mechanic_tags` und nutzt das `main_stat_scaling` der Family über `default_scale_axes`  
   - Hängt Gameplay-Effekte an: `on_cast`, `on_hit`, `on_end`.  
   - Referenziert ein optionales `vfx_profile_id` (nur Presentation).

3. **Archetype-Override – Hero-spezifische Ausprägung**  
   - Pro Archetyp optional.  
   - Skaliert Zahlen (Damage/Radius/Dauer/Cooldown).  
   - Kann Effekte add/remove (z.B. mehr Nova, weniger Buff).

4. **Core – DamageType & Element-Fantasie**  
   - Kinetic, Blazing, Glacial, Static, Venomous, Infernal, Radiant, …  
   - Legt DamageType & eventuelle Conversions/Element-Spezialitäten fest.

5. **Orbit-System – Build-Finetuning**  
   - Nodes auf dem Socket/Orbit-Baum.  
   - Drehen Achsen (Damage, Radius, Dauer, Projectiles, Nova-on-End etc.).  
   - Können ebenfalls Effekte hinzufügen.

6. **wietere Modifiers**
   - Gear, Passives, Buffs und ähnliche Systeme können Skills acuh nochmla beeinflussen
   - dies passiert meist über tag-spezifische modifier z.B.: "..erhöht Schaden für `AoE`-Skills"

---

## 2. Skill-Family – Template-Ebene

### 2.1 Zweck

- Einheitliche Startwerte pro “Form”.
- Orientierung beim Design: “Wie sieht ein typischer Slam/Hazard/Projectile aus?”
- Gemeinsame Stat-Scaling-Defaults (1-Haupt-Affinity: bestimmt später in welchen Sockel es gesetzt werden kann).

### 2.2 Beispiel: F7 – Ground Hazard

```jsonc
{
  "family_id": "F7_GroundHazard",
  "base": {
    "damage_per_tick": 10.0,
    "ticks_per_second": 0.5, 
    "duration": 4.0,
    "radius": 4.0,
    "cooldown": 6.0,
    "cast_time": 0.8
  },
  "main_stat_scaling": "INT",
  "default_scale_axes": ["damage_per_tick", "duration", "radius"],
  "tags": ["AoE", "Ground", "Hazard", "DoT"],

  "ai_role_default": "ZoneControl"
}
```

**Interpretation:**  
- Standard-Hazard: mittlere Zone, moderater DoT, ~4s Dauer, ~6s CD.  
- Damage Skaliert mit INT (kann nur in INT-Sockel gesetzt werden),
- skaliert primär über Damage/Radius/Dauer/Ticks.

---

## 3. Skill-Module – konkrete Skills

### 3.1 Struktur (abstrakt)

```jsonc
{
  "id": "CausticField",
  "skill_family": "F7_GroundHazard",

  "mechanic_tags": ["M08_GroundHazard", "M11_DebuffCurse"],

  "base_override": {
    // nur Felder eintragen, die vom Family-Default abweichen
    "damage_per_tick": 14.0,
    "duration": 5.0
  },

  "tags_add": ["Curse"],

  "on_cast_effects": [
    "ApplySelfBuff_LingeringToxins_Small"
  ],
  "on_hit_effects": [
    "ApplyPoisonDot_Low"
  ],
  "on_end_effects": [
    "NovaSmall_PoisonExplosion"
  ],

  "vfx_profile_id": "VFX_CausticField_Default"
}
```

### 3.2 Beispiel 1 – `CausticField` (Poison-Hazard)

- Family F7 liefert Basis.  
- Modul macht den Skill tödlicher & länger:

```jsonc
{
  "id": "CausticField",
  "skill_family": "F7_GroundHazard",
  "mechanic_tags": ["M08_GroundHazard", "M11_DebuffCurse"],

  "base_override": {
    "damage_per_tick": 14.0,  // statt 10
    "duration": 5.0           // statt 4
  },

  "on_cast_effects": [
    "ApplySelfBuff_LingeringToxins_Small"
  ],
  "on_hit_effects": [
    "ApplyPoisonDot_Low"
  ],
  "on_end_effects": [
    "NovaSmall_PoisonExplosion"
  ],

  "vfx_profile_id": "VFX_CausticField_Default"
}
```

### 3.3 Beispiel 2 – `RazorMaelstrom` (Spin-Melee, F1)

```jsonc
{
  "id": "RazorMaelstrom",
  "skill_family": "F1_SpinningAssault",
  "mechanic_tags": ["M12_SpinNova", "M01_DirectHit"],

  "base_override": {
    "damage_per_tick": 8.0,
    "radius": 3.0,
    "duration": 3.5,
    "cooldown": 6.0
  },

  "tags_add": ["Hit"],

  "on_cast_effects": [
    "ApplyBuff_MoveSpeed_Small"
  ],
  "on_hit_effects": [
    "DealHitDamage_MeleeSpin"
  ],
  "on_end_effects": [],

  "vfx_profile_id": "VFX_RazorMaelstrom_Default"
}
```

---

## 4. EffectDef – zentrale Effekte

### 4.1 Struktur

```jsonc
{
  "id": "EffectId",
  "effect_type": "AddDot | DamageHit | ApplyBuff | ApplyDebuff | SpawnHazard | SpawnProjectile | SpawnMinion | Nova | Teleport | DashMove",
  "params": { /* effect-spezifische Parameter */ },
  "mechanic_tags": ["Mxx_..."]
}
```

### 4.2 Beispiele

**DoT-Effekt**

```jsonc
{
  "id": "ApplyPoisonDot_Low",
  "effect_type": "AddDot",
  "params": {
    "dot_damage_per_second": 20,
    "duration": 4.0,
    "damage_type": "Poison",
    "max_stacks": 5
  },
  "mechanic_tags": ["M08_GroundHazard", "M11_DebuffCurse"]
}
```

**Buff-Effekt**

```jsonc
{
  "id": "ApplyBuff_MoveSpeed_Small",
  "effect_type": "ApplyBuff",
  "params": {
    "target": "Self",
    "duration": 3.0,
    "modifiers": [
      { "stat": "MoveSpeed", "type": "AddPercent", "value": 0.20 }
    ]
  },
  "mechanic_tags": []
}
```

**Nova-Effekt**

```jsonc
{
  "id": "NovaSmall_PoisonExplosion",
  "effect_type": "Nova",
  "params": {
    "radius": 3.0,
    "base_damage": 40,
    "damage_type": "Poison"
  },
  "mechanic_tags": ["M12_SpinNova"]
}
```

---

## 5. Archetype-Overrides – Module pro Heldetyp

### 5.1 Struktur

```jsonc
{
  "module_id": "CausticField",
  "archetype_id": "PLAGUE_MAGE",

  "multipliers": {
    "damage_per_tick_mult": 1.2,
    "radius_mult": 1.2,
    "duration_mult": 1.3,
    "cooldown_mult": 1.1
  },

  "tags_add": ["Plague"],

  "effects_add": {
    "on_cast_effects": [],
    "on_hit_effects": [],
    "on_end_effects": ["NovaSmall_PoisonExplosion"]
  },
  "effects_remove": {
    "on_cast_effects": [],
    "on_hit_effects": [],
    "on_end_effects": []
  }
}
```

### 5.2 Beispiel – BRUISER vs. PLAGUE_MAGE für `CausticField`

**BRUISER**

```jsonc
{
  "module_id": "CausticField",
  "archetype_id": "BRUISER",
  "multipliers": {
    "damage_per_tick_mult": 1.0,
    "radius_mult": 0.85,
    "duration_mult": 0.8,
    "cooldown_mult": 0.9
  },

  "tags_add": [],

  "effects_add": {
    "on_cast_effects": ["ApplyBuff_ArmorWhileInHazard"],
    "on_hit_effects": [],
    "on_end_effects": []
  },
  "effects_remove": {
    "on_cast_effects": [],
    "on_hit_effects": [],
    "on_end_effects": []
  }
}
```

**PLAGUE_MAGE**

```jsonc
{
  "module_id": "CausticField",
  "archetype_id": "PLAGUE_MAGE",
  "multipliers": {
    "damage_per_tick_mult": 1.2,
    "radius_mult": 1.2,
    "duration_mult": 1.3,
    "cooldown_mult": 1.1
  },

"tags_add": ["Plague"],

  "effects_add": {
    "on_cast_effects": [],
    "on_hit_effects": [],
    "on_end_effects": ["NovaSmall_PoisonExplosion"]
  },
  "effects_remove": {
    "on_cast_effects": [],
    "on_hit_effects": [],
    "on_end_effects": []
  }
}
```

---

## 6. Core – DamageType & Conversion

### 6.1 Struktur

```jsonc
{
  "id": "BlazingCore",
  "damage_type": "Fire",
  "conversions": [
    { "from": "Base", "to": "Fire", "value": 1.0 }
  ],
  "tags_add": ["Fire"],
  "extra_effects": [
    "IncreaseIgniteChance_Small"
  ]
}
```

Weitere Beispiele:

```jsonc
{
  "id": "KineticCore",
  "damage_type": "Physical",
  "conversions": [],
  "extra_effects": []
}
```

```jsonc
{
  "id": "VenomousCore",
  "damage_type": "Poison",
  "conversions": [
    { "from": "Base", "to": "Poison", "value": 1.0 }
  ],
  "tags_add": ["Poison"],
  "extra_effects": [
    "IncreaseDotDuration_Small"
  ]
}
```

---

## 7. Orbit-System – Nodes als Mod-Sets

### 7.1 Struktur

```jsonc
{
  "id": "Orbit_Hazard_Damage",
  "modifiers": [
    { "axis": "damage_per_tick", "type": "Mult", "value": 1.20 }
  ],
  "effects_add": {
    "on_cast_effects": [],
    "on_hit_effects": [],
    "on_end_effects": []
  },
  "effects_remove": {
    "on_cast_effects": [],
    "on_hit_effects": [],
    "on_end_effects": []
  }
}
```

### 7.2 Beispiele

**Mehr Radius, weniger Damage**

```jsonc
{
  "id": "Orbit_Hazard_BigZone",
  "modifiers": [
    { "axis": "radius", "type": "Mult", "value": 1.25 },
    { "axis": "damage_per_tick", "type": "Mult", "value": 0.90 }
  ],
  "effects_add": {},
  "effects_remove": {}
}
```

**Explosive End-Hazard**

```jsonc
{
  "id": "Orbit_Hazard_ExplosiveEnd",
  "modifiers": [],
  "effects_add": {
    "on_end_effects": ["NovaSmall_PoisonExplosion"]
  },
  "effects_remove": {}
}
```

---

## 8. Resolve-Pipeline (High-Level)

### 8.1 Reihenfolge

1. **Family + Module** → Basiswerte + Mechanik + Effekte  
2. **Archetype-Override** → Multipliers + Effekt-Add/Remove  
3. **Hero-Stats & Gear** → generische Mod-Pipeline  
4. **Orbit-Nodes** → Feintuning & zusätzliche Effekte  
5. **Core** → DamageType + Conversions + element-spezifische Extras  

### 8.2 Pseudocode

```csharp
ResolvedSkill ResolveSkill(Hero hero, SkillModuleDef module, CoreDef core)
{
    var family = GetSkillFamilyTemplate(module.skill_family);

    // 1) Family + Module
    var baseStats = MergeFamilyAndModule(family, module);

    // 2) Archetype
    var archeOverride = GetArchetypeOverride(hero.Archetype, module.Id);
    if (archeOverride != null)
        baseStats = ApplyArchetypeMultipliers(baseStats, archeOverride.Multipliers);

    // 3) Stats & Gear
    baseStats = ApplyStatAndGearMods(hero, baseStats);

    // 4) Orbit
    var orbitMods = hero.GetOrbitModsFor(module.Id);
    baseStats = ApplyOrbitModifiers(baseStats, orbitMods);

    // 5) Core
    baseStats.DamageType = core.DamageType;
    baseStats = ApplyCoreConversionsAndExtras(baseStats, core);

    // Effekte einsammeln
    var effects = CollectEffects(module, archeOverride, orbitMods);

    return new ResolvedSkill(baseStats, effects);
}
```

---

## 9. End-to-End-Beispiel – `CausticField` (PLAGUE_MAGE + VenomousCore)

1. **Family F7**: dmg/tick=10, dur=4, rad=4, cd=6  
2. **Module `CausticField`**: dmg/tick=14, dur=5  
3. **Archetype `PLAGUE_MAGE`**:  
   - damage_mult=1.2 → ~16.8/tick  
   - radius_mult=1.2 → ~4.8  
   - duration_mult=1.3 → ~6.5  
   - cooldown_mult=1.1 → ~6.6  
4. **Stats/Gear**: INT/WIL erhöhen Damage & Dauer weiter.  
5. **Orbit**:  
   - `Orbit_Hazard_BigZone` → noch mehr Radius, etwas weniger Damage  
   - `Orbit_Hazard_ExplosiveEnd` → fügt OnEnd-Nova hinzu  
6. **Core `VenomousCore`**: DamageType=Poison, DoT-Duration +10 %

**Resultat-Fantasie:**  
- Große, lange, giftige Zone mit Explosion am Ende.  
- Genau das, was man von einem Plague-Mage-Build erwartet.
