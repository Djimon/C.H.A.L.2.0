# CHAL Skill-Families & Execution Profiles

Dieses Dokument beschreibt die 13 Skill-Familien im CHAL-Skillsystem, jeweils mit:
- Konfigurationsachsen (Daten, nicht Code)
- Vorschlag für 3 Execution Profiles: **Melee**, **Ranged**, **Caster**

Annahme:
- **Melee-Profil** = Vanguard / Bruiser / Piercer
- **Ranged-Profil** = Ranger / Marksman / Saboteur
- **Caster-Profil** = Arcanist / Alchemist / Oracle / Hierophant (+ Summoner mit extra Minion-Tag)


---

## F1 – Spinning Assault
Namensideen: *Maelstrom-, Cyclone-, Gyro-, Vortex-Modul*

### Konfigurationsachsen (Daten)

- `base_radius` – Grundradius um den Caster.
- `can_move_while_channeling` – true/false.
- `move_speed_mod` – Bewegungs-Multiplikator während des Spins.
- `tick_rate` – Hits pro Sekunde.
- `damage_per_tick` – Basis-DMG pro Tick.
- `max_duration` – maximale Channel-Dauer.
- `startup_time` – Windup bis der Spin aktiv ist.
- `has_magnet_pull` – leichter Sog-Effekt ja/nein.
- `damage_falloff` – Schaden außen vs. innen (optional).

### Execution Profiles

| Parameter               | Melee-Profil (Vanguard/Bruiser/Piercer)                    | Ranged-Profil (Ranger/Marksman/Saboteur)                               | Caster-Profil (Arcanist/Alchemist/Oracle/Hierophant)                   |
|-------------------------|------------------------------------------------------------|-------------------------------------------------------------------------|-------------------------------------------------------------------------|
| `base_radius`           | mittel–groß (2.5–3.5)                                      | klein–mittel (2.0–2.5, eher um eine Turret/Zone)                       | mittel (2.5–3.0, magische Orbs um den Caster)                          |
| `can_move_while_channeling` | true                                                  | abhängig vom Skill: häufig **false** (stationäre Turret/Zone)          | true, aber `move_speed_mod` reduziert                                  |
| `move_speed_mod`        | 80–100 % (Vanguard langsamer, Bruiser normal, Piercer schneller) | 0–60 % (stationär oder langsamer Move während Spin-Zone)          | 60–80 % (Caster kann sich bewegen, aber spürbar langsamer)             |
| `tick_rate`             | 3–5 Hits/s                                                 | 2–3 Hits/s (Turret-/Trap-Feeling)                                      | 2–4 Hits/s (Orbs treffen in Intervallen)                               |
| `damage_per_tick`       | mittel–hoch, mit Defensive-Boni kombiniert                | mittel, dafür guter Clear-Radius / Utility                             | mittel, skaliert stärker mit Spellpower / Status-Effekten              |
| `max_duration`          | lang (4–6 s), limitiert durch Ressource                    | mittel (3–4 s)                                                         | mittel (3–4 s), ggf. kürzer bei starken Sekundäreffekten               |
| `has_magnet_pull`       | schwach–mittel (zieht Mobs leicht rein)                    | selten, meistens 0                                                     | optional schwach (Void-/Gravity-Theme)                                 |


---

## F2 – Impact Slam
Namensideen: *Seismic-, Graviton-, Rupture-, Impact-Module*

### Konfigurationsachsen

- `cast_time` – Windup vor dem Schlag.
- `impact_radius` – Radius der Primär-Explosion.
- `aftershock_radius` – Radius zusätzlicher Bodenwelle.
- `aftershock_delay` – Verzögerung bis Aftershock.
- `leap_or_dash_range` – Distanz, falls Bewegung enthalten ist.
- `damage_multiplier` – Basisschaden pro Impact.
- `knockback_force` – Rückstoßstärke.
- `can_chain_between_targets` – springt der Effekt über?

### Execution Profiles

| Parameter           | Melee-Profil                                             | Ranged-Profil                                              | Caster-Profil                                                    |
|---------------------|----------------------------------------------------------|------------------------------------------------------------|------------------------------------------------------------------|
| 'cast_range'        | 0-2 (self-weaponrange)                                   | 8-12 (mid-range)                                 | 10-14 (Ground-Target-distnace)  |
| `cast_time`         | kurz–mittel (0.3–0.6 s)                                  | mittel (0.4–0.7 s, Aim + Schuss)                          | mittel–lang (0.5–0.9 s, „Meteor“-Cast)                           |
| `impact_radius`     | groß (3–4)                                               | mittel (2–3)                                               | mittel–groß (3–3.5)                                              |
| `aftershock_radius` | optional, oft etwas kleiner als Impact                   | selten, eher 0–1                                           | häufig aktiv (Earthquake-/Comet-Aftershock)                      |
| `aftershock_delay`  | kurz (0.2–0.4 s)                                        | selten genutzt                                             | klar spürbar (0.4–0.7 s, telegraphed AoE-Zone)                   |
| `leap_or_dash_range`| 0–6 (Stampede/EQ-Leap möglich)                           | 0 (Schlag ist am Einschlagpunkt des Projektils)            | 0 (reiner Spell-Impact von oben / aus Distanz)                   |
| `damage_multiplier` | sehr hoch, ggf. Self-Punish (Boneshatter-Trauma)         | hoch, Single-Target fokussiert                             | hoch, oft mehr AoE-Anteil / Elementar-Skalierung                 |
| `knockback_force`   | mittel–hoch (Bruiser/Frontline-Werkzeug)                 | gering (max. kleiner stagger)                             | gering–mittel, eher CC/Slow als harter Knockback                |


---

## F3 – Dash Strike
Namensideen: *Blink-, Phase-, Vector-Module*

### Konfigurationsachsen

- `dash_range` – Distanz der Bewegung.
- `dash_duration` – wie schnell die Bewegung ausgeführt wird.
- `hit_shape` – Linie/Breite + End-Radius.
- `can_pass_through_enemies` – ja/nein.
- `end_aoe_radius` – AoE am Landepunkt.
- `max_charges` – Anzahl möglicher Charges.
- `cooldown` – CD pro Charge.

### Execution Profiles

| Parameter         | Melee-Profil                                                 | Ranged-Profil                                                  | Caster-Profil                                                |
|-------------------|--------------------------------------------------------------|----------------------------------------------------------------|--------------------------------------------------------------|
| `dash_range`      | mittel–lang (5–8)                                           | kurz–mittel (4–6, eher Reposition)                            | kurz (3–5, Teleport-/Blink-Range)                            |
| `dash_duration`   | sehr kurz (0.1–0.25 s, snappy)                              | kurz (0.15–0.3 s)                                             | kurz, aber mit Cast-Effekt (0.2–0.35 s)                      |
| `hit_shape`       | breite Linie + kleiner End-Radius                           | schmale Linie, optional Autoshot am Ende                      | kleiner Kreis am Start oder Ende (Arcane Nova / Blink-Nova)  |
| `can_pass_through_enemies` | meist true (Gap-closer)                           | teilweise true, aber eher Collision wichtig für Positioning   | true (Blink geht durch Ziele hindurch)                       |
| `end_aoe_radius`  | mittel (2–2.5)                                              | klein (1–1.5)                                                 | klein–mittel (1.5–2), dafür ggf. starker CC oder Debuff      |
| `max_charges`     | 2–3                                                          | 1–2                                                           | 1–2                                                          |
| `cooldown`        | kurz (3–6 s pro Charge)                                     | kurz–mittel (4–8 s)                                           | mittel (6–10 s, weil Mobility + Schaden + Utility kombiniert)|


---

## F4 – Nova Burst
Namensidee: *Pulse-, Wave-, Radiant-, Bloom-, Nova-Module*

### Konfigurationsachsen

- `nova_radius` – Ausdehnung der Explosion.
- `damage_falloff` – innen außen Unterschied.
- `cast_time` – Windup der Nova.
- `can_be_targeted_via_projectile` – ja/nein.
- `element_tag` – Fire, Cold, Lightning, Holy, etc.
- `cc_effect` – Knockback, Slow, Stun-Chance.
- `nova_pulse_count` – einmalig oder mehrfach pulsend.

### Execution Profiles

| Parameter         | Melee-Profil                                             | Ranged-Profil                                                | Caster-Profil                                                    |
|-------------------|----------------------------------------------------------|--------------------------------------------------------------|------------------------------------------------------------------|
| 'cast_range'        | 0-2 (self-weaponrange)                                   | 8-12 (mid-range)                                 | 10-14 (Ground-Target-distnace)  |
| `nova_radius`     | klein–mittel (2–2.5, du stehst mitten drin)             | klein (1.5–2, zentriert auf getroffenem Ziel)               | mittel–groß (2.5–3.5, um den Caster)                             |
| `damage_falloff`  | wenig bis kein Falloff (heftiger Melee-Focus)           | stärkerer Falloff (ST auf Primärziel, AoE für Adds)         | moderater Falloff (gleichmäßiger Spell-DMG im Bereich)          |
| `cast_time`       | kurz (0.2–0.4 s)                                        | sehr kurz (instant bei Treffer / on-hit ausgelöst)          | mittel (0.3–0.6 s)                                              |
| `can_be_targeted_via_projectile` | nein                                     | ja (Nova triggert, wenn Projectile trifft)                  | optional (Nova direkt um Caster ohne Projectile)               |
| `cc_effect`       | leichter Knockback, ggf. Stagger                        | kleiner Slow/Weaken auf getroffenen Feinden                 | starker Slow/Freeze/Chill, ggf. Stun-Chance                    |
| `nova_pulse_count`| 1 (Burst)                                               | 1 (On-Hit-Burst)                                             | 1–3 Pulse (z. B. „wachsende“ Nova)                             |


---

## F5 – Projectile Barrage
Namensideen: *Shard-, Lance-, Rail-Module*

### Konfigurationsachsen

- `projectile_count` – 1, 3, 7, …
- `spread_angle` – 0° (Single) bis 90°+ (Fächer).
- `can_pierce` – ja/nein.
- `chain_count` – Kettenanzahl.
- `explode_on_hit` – AoE beim Treffer.
- `range` – Flugdistanz.
- `projectile_speed` – Geschwindigkeit.
- `prioritize_primary_target` – Ja/Nein (ST vs. Clear).
- `damage_tags` – Physical, Lightning, Cold, Chaos, etc.

### Execution Profiles

| Parameter             | Melee-Profil (Cleave-Linie)                                  | Ranged-Profil (klassischer Schütze)                                   | Caster-Profil (magische Geschosse)                                       |
|-----------------------|--------------------------------------------------------------|-------------------------------------------------------------------------|----------------------------------------------------------------------------|
| `projectile_count`    | 3–5 „unsichtbare“ Hit-Segmente in einer Linie                | 1–7 Projektile je nach Skill (Marksman eher 1–3, Ranger 3–7)          | 1–3 magische Geschosse                                                     |
| `spread_angle`        | 45–90° (breiter Cleave nah vor dir)                          | 0–45° (Single bis leichte Streuung)                                    | 0–30° (Fokus auf kontrollierbare Spell-Shots)                             |
| `can_pierce`          | optional (Piercer stark, Vanguard/Bruiser eher selten)       | häufig, v. a. für Clear-Skills                                         | optional, dafür eher Chain/Explode auf Casterseite                        |
| `chain_count`         | 0–1                                                          | 0–2                                                                     | 0–3 (Arc/Lightning-Style)                                                 |
| `explode_on_hit`      | klein (Mini-AoE entlang der Cleave-Linie)                    | situativ (Explosive Arrow, Kinetic Blast)                              | häufiger, v. a. bei Element-Spells                                        |
| `range`               | sehr kurz (2–4, quasi Nahkampf-Reichweite)                   | mittel–lang (8–14)                                                     | mittel–lang (8–12)                                                         |
| `projectile_speed`    | sehr hoch / instant (Cleave simuliert Geschossbahnen)        | hoch (klassische Schüsse)                                              | mittel–hoch (Spells können sichtbar „fliegen“)                            |
| `prioritize_primary_target` | meist ja (hoher ST-Schaden vorne)                     | Split: Marksman ja, Ranger eher nein (Clear)                           | je nach Spell: ST-Nuke oder kleine, gleichmäßig verteilte Treffer         |


---

## F6 – Heavy Nuke
Namensidee: *Meteor-, Detoantion-, Warhead-, Burst-Module*

### Konfigurationsachsen

- `targeting_mode` – Self-Buff, Projectile, Ground-Target, Corpse-Target.
- `impact_radius` – AoE-Größe.
- `delay_time` – Flug-/Fallzeit.
- `damage_multiplier` – Basisschaden.
- `splash_falloff` – Randschaden.
- `self_cost` – HP-/Ressourcen-Kosten.
- `projectile_speed` – Geschwindigkeit.

### Execution Profiles

| Parameter          | Melee-Profil                                               | Ranged-Profil                                             | Caster-Profil                                                       |
|--------------------|------------------------------------------------------------|-----------------------------------------------------------|---------------------------------------------------------------------|
| 'cast_range'        | 0-2 (self-weaponrange)                                   | 8-12 (mid-range)                                 | 10-14 (Ground-Target-distnace)  |
| `targeting_mode`   | Self-Buff auf nächsten Schlag oder kurzer Wurf (2–4 Range) | Projectile auf anvisiertes Ziel                           | Ground-Target / AoE-Spell (z. B. „klick auf Boden“)                 |
| `impact_radius`    | mittel (2–3)                                              | klein–mittel (1.5–2.5)                                    | mittel–groß (3–4)                                                   |
| `delay_time`       | sehr kurz (0–0.2 s)                                       | kurz (Flugzeit 0.2–0.4 s)                                 | klar sichtbar (0.4–0.8 s, telegraphed Meteor/Comet)                 |
| `damage_multiplier`| sehr hoch, ggf. mit Self-Punish (Trauma/HP-Kosten)        | hoch, ST mit kleinem Splash                               | hoch–sehr hoch, oft „Glass Cannon“-Spell                           |
| `splash_falloff`   | wenig (nah am Schlag vollen Schaden)                      | moderat                                                   | klarer Falloff (Zentrum sehr stark, Ränder spürbar schwächer)      |
| `self_cost`        | optional hoch (z. B. HP-Opfer für starke Melee-Nuke)      | gering–keine                                              | mittel (Mana-/Ressourcen-Kosten hoch, HP-Kosten selten)            |
| `projectile_speed` | instant/niedrig relevant                                  | hoch                                                      | mittel (Nuke soll „fühlbar“ sein, nicht Hitscan)                    |


---

## F7 – Ground Hazard / DoT-Zone
Namensideen: *Contagion-, Zone-, Corrosion-, Hazard-, Grid-Module*

### Konfigurationsachsen

- `shape` – Circle, Line, Donut, Path.
- `radius_or_length` – Größe der Fläche.
- `duration` – Lebensdauer der Zone.
- `tick_rate` – Hits pro Sekunde.
- `damage_per_tick` – Basis-DPS.
- `extra_effects` – Slow, Chill, Poison, Vuln, etc.
- `placement_source` – unter Caster, Punktziel, Projectile-Treffer.
- `stacking_behavior` – können mehrere Zonen stacken?

### Execution Profiles

| Parameter           | Melee-Profil                                           | Ranged-Profil                                                 | Caster-Profil                                                          |
|---------------------|--------------------------------------------------------|----------------------------------------------------------------|------------------------------------------------------------------------|
| `shape`             | Kreis oder kurzer „Ring“ um den Caster                | Kreis am Einschlagspunkt eines Projektils                     | frei platzierbarer Kreis/Line/Donut                                   |
| `radius_or_length`  | klein–mittel (2–3)                                    | klein–mittel (2–3)                                            | mittel–groß (3–4)                                                     |
| `duration`          | kurz–mittel (2–4 s, aggressiver, hoher DPS)           | mittel (3–5 s)                                                | mittel–lang (4–8 s, mehr Kontrolle, weniger Raw-DPS)                 |
| `tick_rate`         | hoch (3–5/s)                                          | mittel (2–3/s)                                                | 2–4/s je nach Spell                                                   |
| `damage_per_tick`   | hoch, balanced durch eigene Exposition im Feld        | mittel, Clear-Fokus                                           | mittel–hoch, skaliert stark mit Spellpower/DoT-Boni                  |
| `extra_effects`     | Weak Slow/Armor-Shred/Weaken im Nahbereich            | Slow/Poison/Bleed-Vibes                                       | starke Status (Chill/Freeze/Poison/Vuln/Curse etc.)                  |
| `placement_source`  | entsteht unter/um Caster, oft an Melee-Hit gebunden   | entsteht am Einschlag des ersten/letzten Projektils          | entsteht am gewählten Zielpunkt oder unter Zielgegner                |
| `stacking_behavior` | begrenzt (1–2 Zonen)                                  | 2–3 Zonen möglich                                             | höheres Stack-Potential, aber mit Balance-Grenzen                    |


---

## F8 – Trap / Remote Trigger
Namensideen: *Mesh-, Ambush-, Trap, Trigger-, Snare-Module*

### Konfigurationsachsen

- `trap_count` – Anzahl gleichzeitig aktiver Fallen.
- `arming_time` – Zeit bis Falle scharf ist.
- `trigger_radius` – Auslöseradius.
- `trap_duration` – Lebensdauer der Falle.
- `damage_on_trigger` – Burst-Schaden.
- `remote_trigger_option` – darf Spieler manuell auslösen?
- `throw_range` – wie weit Fallen platziert werden können.

### Execution Profiles

| Parameter            | Melee-Profil                                            | Ranged-Profil                                              | Caster-Profil                                                    |
|----------------------|---------------------------------------------------------|------------------------------------------------------------|------------------------------------------------------------------|
| `trap_count`         | niedrig–mittel (2–4), du bist eh nah dran              | mittel–hoch (3–6)                                          | mittel (3–5)                                                     |
| `arming_time`        | sehr kurz (0–0.2 s)                                    | kurz (0.2–0.4 s)                                           | kurz–mittel (0.2–0.5 s)                                         |
| `trigger_radius`     | klein–mittel (1.5–2.5)                                 | mittel (2–3)                                               | mittel (2–3)                                                     |
| `trap_duration`      | kurz–mittel (4–8 s, eher offensiv)                     | mittel–lang (6–12 s, Gebietskontrolle)                    | mittel–lang (6–12 s)                                             |
| `damage_on_trigger`  | sehr hoch, ggf. kleiner Self-Risk, wenn zu nah         | hoch, AoE-Clear                                            | hoch, mit zusätzlichen Status-Effekten                          |
| `remote_trigger_option` | selten, hauptsächlich Auto-Trigger                  | gelegentlich (Sniper-Detos)                                | häufiger (klassische „Detonate Traps“-Caster)                   |
| `throw_range`        | kurz (2–4, droppt quasi vor deinen Füßen)              | mittel–lang (8–12)                                         | mittel (6–10)                                                    |


---

## F9 – Orbits & Orbs
Namensideen: *Orb-, Cluster-, Swarm-, Satellite-Module*

### Konfigurationsachsen

- `orb_count` – Anzahl aktiver Orbs.
- `orbit_radius` – Abstand zum Zentrum.
- `rotation_speed` – Umdrehungen pro Sekunde.
- `hit_mode` – Kontakt-Hit, Auto-Projectiles oder Puls-AoE.
- `duration` – orb-Lebensdauer.
- `detach_option` – Können Orbs „abgeschossen“ werden?
- `targeting_logic` – nächster Gegner, zufällig, in Linie etc.

### Execution Profiles

| Parameter        | Melee-Profil                                              | Ranged-Profil                                                | Caster-Profil                                                   |
|------------------|-----------------------------------------------------------|--------------------------------------------------------------|-----------------------------------------------------------------|
| `orb_count`      | 1–3 (dicke Orbs nah am Körper)                            | 1–2 pro Turret/Station (mehrere Turrets möglich)            | 3–5 kleinere Orbs                                               |
| `orbit_radius`   | klein–mittel (1.5–2.5, du stehst mit im Hitbereich)       | klein (1–2 um stationäres Objekt)                           | mittel (2–3 um Caster)                                         |
| `rotation_speed` | mittel–hoch (1–2 U/s)                                     | mittel (1 U/s)                                              | variabel (0.8–2 U/s, Spell-Fokus)                              |
| `hit_mode`       | Kontakt-Hits, ggf. leichte AoE-Pulse im Nahbereich       | Auto-Projektile in Zielrichtung / Random innerhalb Range    | Kontakt + Auto-Casts / Lightning-Style                         |
| `duration`       | mittel (4–6 s)                                           | lang (8–12 s)                                               | mittel–lang (6–10 s)                                           |
| `detach_option`  | optional (starker Burst, schickt alle Orbs nach vorne)    | selten (Turrets bleiben stationär)                          | häufig optional (Deto-Mechanik für Comet-/Volley-Spells)       |


---

## F10 – Minions & Swarms
Namensideen: *Spawn-, Seed-, Hive-, Puppet-, Swarm-Module*

### Konfigurationsachsen

- `minion_count` – Anzahl gleichzeitig beschworener Diener.
- `lifespan` – dauerhaft oder zeitlich begrenzt.
- `ai_behavior` – melee, ranged, suicide, orbit.
- `summon_style` – instant, channel, on-kill.
- `control_mode` – fire&forget, target-command, follow-only.
- `upkeep_cost` – Ressourcenkosten / Reservierung.

### Execution Profiles

| Parameter        | Melee-Profil                                         | Ranged-Profil                                           | Caster-/Summoner-Profil                                                   |
|------------------|------------------------------------------------------|---------------------------------------------------------|----------------------------------------------------------------------------|
| `minion_count`   | klein (1–4, eher „Begleiter“)                        | klein–mittel (1–3 Turrets/Constructs)                   | mittel–hoch (5–12 Skelett-/Geister-Schwärme)                               |
| `lifespan`       | lang / dauerhaft                                     | mittel–lang (Turrets halten lange)                      | unterschiedlich: dauerhaft (Skeletons) oder kurz (Raging Spirits/Swarm)   |
| `ai_behavior`    | melee/frontline                                      | ranged/turret                                           | gemischt, aber Fokus auf Spell-/DoT-/Debuff-Patterns                      |
| `summon_style`   | instant, selten Channel                              | instant oder in Salven                                  | instant, Channel oder On-Kill-Procs                                       |
| `control_mode`   | follow + auto-attack                                 | stationär + Auto-Fire                                   | fire&forget, z. T. Zielsteuerung (Fokus-Target, „angreifen dort“)         |
| `upkeep_cost`    | gering–mittel                                        | mittel                                                  | mittel–hoch (Summoner-Identität, viel Power für Ressourceneinsatz)       |


---

## F11 – Auren & Shouts
Namensideen: *Echo-, Emitter-, Induciton-, Aura-Module*

### Konfigurationsachsen

- `aura_radius` – Reichweite.
- `uptime_mode` – Toggle, Channel, zeitlich begrenzt.
- `buff_values_offense` – Schaden/Crit etc.
- `buff_values_defense` – Rüstung, Resistenzen, Block etc.
- `pulse_interval` – Tick-Abstand für Effekte.
- `resource_reservation` – Reservierter Ressourcenteil.
- `self_damage_per_tick` – für RF-artige Effekte.

### Execution Profiles

| Parameter              | Melee-Profil                                           | Ranged-Profil                                               | Caster-Profil                                                  |
|------------------------|--------------------------------------------------------|-------------------------------------------------------------|----------------------------------------------------------------|
| `aura_radius`          | klein–mittel (4–6), du stehst mitten im Team          | mittel (6–8, mehr „Team-Bubble“ für hintere Reihe)         | mittel–groß (6–10, v. a. für Support-Caster)                   |
| `uptime_mode`          | Shouts (kurz stark) + ggf. 1–2 permanente Auren       | meist Totems/Banner mit mittlerer Dauer                    | viele permanente Auren / Reservierungen                        |
| `buff_values_offense`  | hoch ST/off-Hand-Boni, Melee-DMG, Leech               | Projectile-DMG, Crit, Accuracy                             | Spell-DMG, DoT, Crit, Elementar / Chaos                        |
| `buff_values_defense`  | Rüstung, Life, Block, Fortify                         | kleinere Def-Buffs, eher Utility                           | Resistenz-/DR-Buffs, Support-/Heal-Over-Time                   |
| `pulse_interval`       | 0.5–1 s (Warcry/Pulse-Heals etc.)                     | 1–2 s                                                      | 1–2 s                                                          |
| `resource_reservation` | gering–mittel (Frontline braucht aktive Ressourcen)   | gering–mittel                                              | mittel–hoch (klassische „Aura-Reservation“-Mechanik)          |
| `self_damage_per_tick` | optional für RF-/Berserker-Auren                      | selten/nicht                                               | bei DoT-/RF-artigen Spells möglich, aber eher Nischenfall      |


---

## F12 – Curses & Detonations
Namensideen: *Injection-, Curse-, Hex-, Omen-Module*

### Konfigurationsachsen

- `curse_radius` – AoE-Größe beim Cast.
- `max_cursed_targets` – Limit pro Cast/Skill.
- `debuff_types` – -Res, +DamageTaken, Slow, Weaken, etc.
- `detonate_condition` – On-Kill, On-Recast, On-Secondary-Skill.
- `detonate_radius` – AoE beim Deto.
- `scaling_with_curse_stacks` – Skalierungsformel.
- `duration` – Laufzeit des Debuffs.

### Execution Profiles

| Parameter              | Melee-Profil                                             | Ranged-Profil                                               | Caster/Oracle-Profil                                       |
|------------------------|----------------------------------------------------------|-------------------------------------------------------------|------------------------------------------------------------|
| `curse_radius`         | klein–mittel (2–3, du „markierst“ im Nahkampf)          | klein (1.5–2, Sniper-Marks)                                | mittel–groß (3–4, Flächen-Curse)                           |
| `max_cursed_targets`   | gering–mittel (3–6)                                     | gering–mittel (3–6)                                        | mittel–hoch (6–12)                                         |
| `debuff_types`         | physischer Vulnerability, Armor-Shred, Bleed-Boost      | Projectile-DMG+/Crit-Marks, -Evasion                       | -Resistenzen, +DoT-Taken, Slow/Weaken                      |
| `detonate_condition`   | On-Kill (Corpse Explosion) oder „Heavy-Hit“-Finisher    | On-Recast / On-Hit mit bestimmtem Skill                    | On-Recast, On-Channel-Ende, spezifische Hexblast-Trigger   |
| `detonate_radius`      | mittel (2–3)                                            | klein–mittel (2–2.5)                                       | mittel–groß (3–4)                                          |
| `scaling_with_curse_stacks` | simpel (starker One-Shot, wenig Stack-Logik)      | moderat (Stacks wichtig, aber nicht extrem)                | stark stack-basiert (mehr Stacks → massiv stärkere Detos) |
| `duration`             | kurz–mittel (4–8 s)                                     | mittel (6–10 s)                                            | mittel–lang (8–14 s, für Setups)                          |


## F13 – Target Marks & Focus Locks  
Namensideen: *Mark-, Focus-, Lock-Module*

### Konfigurationsachsen

- `max_marked_targets` – Wie viele Ziele gleichzeitig markiert sein können.
- `mark_duration` – Dauer der Markierung.
- `apply_mode` – Wie die Markierung aufgebracht wird (On-Hit, On-Cast, Ground-Target, Blick).
- `consume_condition` – Wann die Markierung verbraucht wird (nächster Hit, Tod, Deto-Skill, Ally-Hit).
- `effects_on_attacker` – Buff-Effekte für Angreifer gegen markierte Ziele (z. B. +Crit, +Damage, On-Hit-Effekte).
- `effects_on_victim` – Debuffs auf dem markierten Ziel (z. B. -DamageDealt, +DamageTaken, -MoveSpeed).
- `mark_visibility` – Wie stark die Markierung visuell hervorgehoben wird (UI/FX-Intensität).
- `recast_behavior` – Verhalten bei erneutem Wirken (überschreiben, verschieben, refreshen).

### Execution Profiles

| Parameter             | Melee-Profil (Vanguard/Bruiser/Piercer)                            | Ranged-Profil (Ranger/Marksman/Saboteur)                                | Caster/Oracle-Profil (Arcanist/Alchemist/Oracle/Hierophant)                    |
|-----------------------|---------------------------------------------------------------------|---------------------------------------------------------------------------|--------------------------------------------------------------------------------|
| `max_marked_targets`  | 1–2 (Fokus auf einzelnes Priority-Target im Nahkampf)              | 1–3 (Boss + 1–2 wichtige Adds)                                           | 1–5 (Oracle/Support kann mehrere Ziele gleichzeitig markieren)                |
| `mark_duration`       | kurz–mittel (4–8 s, aggressiver, hit-naher Playstyle)             | mittel (6–10 s, gut für Pre-Mark vor Engage / Kiten)                     | mittel–lang (8–12 s, als Setup- und Teamwerkzeug)                             |
| `apply_mode`          | primär On-Hit im Nahkampf (z. B. Heavy-Angriff markiert Ziel)      | On-Hit per Projectile oder gezielter Mark-Skill (Single-Target-Aim)      | On-Cast mit freier Zielwahl oder kleiner AoE-Cone/Linie auf Distanz           |
| `consume_condition`   | `OnNextHitByCaster` (nächster Schlag verbraucht Mark für Big-Hit)  | `OnHitByCaster` oder `OnDeath` (Sniper-Focus, On-Kill-Chains)            | `OnDetonateSkill` oder `OnHitByAnyAlly` (Team-Synergien / Spell-Detos)        |
| `effects_on_attacker` | +CritChance/+CritMulti/+Damage vs. Marked, ggf. Life/Res on Hit    | +CritMulti, Penetration, Projectile-Flat-DMG vs. Marked                  | +ResourceGain, +DoTScaling, +StatusChance vs. Marked                          |
| `effects_on_victim`   | -DamageDealt, -Armor, -MoveSpeed, leichter Stagger vs. Marked      | +DamageTaken vom Mark-Anwender, -Evasion/-Dodge                          | -Resistenzen, +DoTTaken, -DamageDealt, evtl. -Cast-/AttackSpeed               |
| `recast_behavior`     | überschreibt alte Mark, Fokus bleibt meist auf einem Ziel          | Recast verschiebt Mark auf neues Ziel oder überschreibt die älteste Mark | kann Marks refreshen oder mehrere parallel halten (Support-/Control-Rolle)    |



...