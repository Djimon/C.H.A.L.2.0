using CHAL.Systems;
using CHAL.Systems.Enemy;
using CHAL.Systems.Hero;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Unit
{

    /// <summary>
    /// Scene-scoped Locator für aktive Units.
    /// Hänge diese Komponente an dasselbe GameObject wie den MapManager.
    /// Controller rufen Register/Unregister in OnEnable/OnDisable.
    /// </summary>
    public sealed class UnitLocator : MonoBehaviour
    {
        // Scene-scoped Instance (kein DontDestroyOnLoad).
        public static UnitLocator Instance { get; private set; }

        // Intern HashSet für O(1) Remove, nach außen nur lesend.
        private readonly HashSet<HeroController> _heroes = new();
        private readonly HashSet<EnemyController> _enemies = new();

        public IReadOnlyCollection<HeroController> ActiveHeroes => _heroes;
        public IReadOnlyCollection<EnemyController> ActiveEnemies => _enemies;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                DebugManager.Error("[UnitLocator] Mehrere Instanzen in der Scene gefunden – bitte nur eine!");
            }
            Instance = this;
            DebugManager.Log("[UnitLocator] Ready (scene-scoped).", DebugManager.EDebugLevel.Dev, "Combat");
        }

        private void OnDestroy()
        {
            // Safety: Bei Scene-Unload sauber aufräumen
            if (Instance == this) Instance = null;
            _heroes.Clear();
            _enemies.Clear();
        }

        // ---------- Registrierung (Pooling-freundlich) ----------

        public void Register(HeroController hero)
        {
            if (hero == null) return;
            _heroes.Add(hero);
            DebugManager.Log($"[UnitLocator] +Hero {hero.name} (now {_heroes.Count})", DebugManager.EDebugLevel.Debug, "Combat");
        }

        public void Unregister(HeroController hero)
        {
            if (hero == null) return;
            _heroes.Remove(hero);
            DebugManager.Log($"[UnitLocator] -Hero {hero?.name} (now {_heroes.Count})", DebugManager.EDebugLevel.Debug, "Combat");
        }

        public void Register(EnemyController enemy)
        {
            if (enemy == null) return;
            _enemies.Add(enemy);
            DebugManager.Log($"[UnitLocator] +Enemy {enemy.name} (now {_enemies.Count})", DebugManager.EDebugLevel.Debug, "Combat");
        }

        public void Unregister(EnemyController enemy)
        {
            if (enemy == null) return;
            _enemies.Remove(enemy);
            DebugManager.Log($"[UnitLocator] -Enemy {enemy?.name} (now {_enemies.Count})", DebugManager.EDebugLevel.Debug, "Combat");
        }

        // ---------- Queries ----------

        /// <summary>
        /// Liefert das nächste gegnerische Ziel innerhalb von sightRange (Tie-Breaker: first found).
        /// Gibt das Transform des Gegners zurück oder null, wenn keins gefunden.
        /// </summary>
        public Transform GetNearestEnemy(Vector3 origin, UnitTeam myTeam, float sightRange)
        {
            float best = float.MaxValue;
            Transform bestTr = null;

            if (myTeam == UnitTeam.Player)
            {
                CleanupDead(_enemies);
                foreach (var e in _enemies)
                {
                    if (!IsValid(e)) continue;
                    float d = (e.transform.position - origin).sqrMagnitude;
                    if (d > sightRange * sightRange) continue;
                    if (d < best) { best = d; bestTr = e.transform; }
                }
            }
            else // AI sucht Helden
            {
                CleanupDead(_heroes);
                foreach (var h in _heroes)
                {
                    if (!IsValid(h)) continue;
                    float d = (h.transform.position - origin).sqrMagnitude;
                    if (d > sightRange * sightRange) continue;
                    if (d < best) { best = d; bestTr = h.transform; }
                }
            }

            return bestTr;
        }

        /// <summary>
        /// Liefert das gegnerische Ziel mit der höchsten aktuellen HP innerhalb von sightRange (Tie-Breaker: first found).
        /// Gibt das Transform des Gegners zurück oder null, wenn keins gefunden.
        /// </summary>
        public Transform GetHighestHPEnemy(Vector3 origin, UnitTeam myTeam, float sightRange)
        {
            float bestHP = float.MinValue;
            Transform bestTr = null;

            if (myTeam == UnitTeam.Player)
            {
                CleanupDead(_enemies);
                foreach (var e in _enemies)
                {
                    if (!IsValid(e)) continue;
                    // Reichweite prüfen
                    float d2 = (e.transform.position - origin).sqrMagnitude;
                    if (d2 > sightRange * sightRange) continue;

                    // HP lesen
                    var hp = e.EnemyInstance != null ? e.EnemyInstance.CurrentHP : 0f;
                    if (hp > bestHP) { bestHP = hp; bestTr = e.transform; }
                }
            }
            else // AI sucht Helden
            {
                CleanupDead(_heroes);
                foreach (var h in _heroes)
                {
                    if (!IsValid(h)) continue;
                    float d2 = (h.transform.position - origin).sqrMagnitude;
                    if (d2 > sightRange * sightRange) continue;

                    // HP über EffectReceiver des Helden (EnemyController nutzt diese Methode bereits)
                    var rec = h.GetEffectReceiver(); // in eurem Code verwendet, daher hier konsistent
                    float hp = rec != null ? rec.CurrentHP : 0f;
                    if (hp > bestHP) { bestHP = hp; bestTr = h.transform; }
                }
            }

            return bestTr;
        }

        // ---------- Helpers ----------

        private static bool IsValid(HeroController h)
            => h != null && h.IsAlive; // HeroController.IsAlive nutzt HeroInstance.CurrentHP > 0. :contentReference[oaicite:1]{index=1}

        private static bool IsValid(EnemyController e)
            => e != null && e.IsAlive; // EnemyController.IsAlive nutzt EnemyInstance.CurrentHP > 0. :contentReference[oaicite:2]{index=2}

        // Entfernt null/zerstörte Einträge (billig, verringert NRE-Risiko)
        private static void CleanupDead(HashSet<HeroController> set)
        {
            set.RemoveWhere(x => x == null || !x.IsAlive);
        }

        private static void CleanupDead(HashSet<EnemyController> set)
        {
            set.RemoveWhere(x => x == null || !x.IsAlive);
        }
    }
}