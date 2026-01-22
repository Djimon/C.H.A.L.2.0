using CHAL.Core;
using CHAL.Systems.Hero;
using CHAL.Systems.Inventory;
using CHAL.Systems.Items;
using CHAL.Systems.Codex;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CHAL.Systems.Codex.CodexSnapshot;

namespace CHAL.Data
{
    [Serializable]
/// <summary>
/// Represents a player's profile in the game, including customization and progress data.
/// Implements the IWallet interface for managing in-game currency.
/// </summary>
    public class PlayerProfile : IWallet
    {
        public string profileId;
        // --- Meta ---
        public DateTime LastSaveTime;            // Für Autosave / Debug

        // --- Charakter Customization ---
        public string playerName;
        public Color[] playerColors;

        // --- Charakter-Fortschritt ---
        public int XP;                           // Gesamt-XP
        public int Level;                        // optional: aus XP berechnet
        public int XPInCurrentLevel;    // XP innerhalb des aktuellen Levels
        public int XPToNextLevel;       // Gesamtmenge, die für das nächste Level nötig ist
        public int missingXP;           // Noch fehlende XP bis LevelUp
        public float levelProgress;     // 0..1 für UI-Balken

        //  --- Heros/ Roster ---
        public List<string> UnlockedHeroes = new();
        public List<HeroProgressData> HeroesData = new();

        // --- Currencies ---
        public Dictionary<string, int> Currencies = new();
        // Beispiel: { "gold" -> 1234, "dna" -> 50 

        // Map Progress
        //first int is MapNo, 
        //second int is highest difficulty succeded
        public Dictionary<int, int> MapProgress = new();
        // Setzen: SetMapProgress(1,MapDifficulty.easy,9)
        // Abfragen:  GetMapProgress(1, MapDifficulty.medium)

        // --- Items ---
        // Persistenter Snapshot aller Inventare (nur für Save/Load)
        public List<InventorySnapshot> InventorySave = new();

        // --- Research ---
        [NonSerialized] public CodexState CodexRuntimeState;

/// <summary>
/// Initializes the player with a name and a set of colors.
/// </summary>
/// <param name="name">The name of the player.</param>
/// <param name="colors">An array of colors associated with the player.</param>
        public void InitializePlayer(string name, Color[] colors)
        {
            playerName = name;
            playerColors = colors;

            //TODO: make name filepath save
            profileId = "p_" + name;

            var starterId = GameManager.Instance.starterHero != null ? GameManager.Instance.starterHero.HeroId : "TestHero";
            EnsureStarterHeroUnlocked(starterId);


            for (int i = 0; i < GameManager.DefaultCurrencyIds.Length; i++)
                EnsureCurrencyExists(GameManager.DefaultCurrencyIds[i], 0);


            SaveSystem.Save(this);
        }

/// <summary>
/// Gets the current experience points of the player.
/// </summary>
/// <returns>The amount of experience points.</returns>
        public int GetXP() => XP;

/// <summary>
/// Adds experience points to the player.
/// </summary>
/// <param name="amount">The amount of experience points to add.</param>
        public void AddXP(int amount)
        {
            XP += amount;
            RecalculateLevel();
        }


/// <summary>
/// Retrieves the amount of currency for the specified currency ID.
/// </summary>
/// <param name="currencyId">The ID of the currency to retrieve.</param>
/// <returns>The amount of currency associated with the given ID, or 0 if not found.</returns>
        public int GetCurrency(string currencyId)
        {
            return Currencies.TryGetValue(currencyId, out var amount) ? amount : 0;
        }

/// <summary>
/// Adds a specified amount of currency to the collection.
/// </summary>
/// <param name="currencyId">The ID of the currency to add.</param>
/// <param name="amount">The amount of currency to add.</param>
        public void AddCurrency(string currencyId, int amount)
        {
            if (amount <= 0)
                return;

            if (string.IsNullOrWhiteSpace(currencyId))
                return;

            if (!GameManager.IsValidCurrencyId(currencyId))
            {
                DebugManager.Warning($"unknown currencyId '{currencyId}'. Currency will be ignored.", "System");
                return;
            }

            if (Currencies == null)
                Currencies = new Dictionary<string, int>();

            // Key immer sicherstellen (auch wenn amount später <= 0 ist)
            if (!Currencies.ContainsKey(currencyId))
                Currencies[currencyId] = 0;       

            Currencies[currencyId] += amount;
        }

        public void EnsureCurrencyExists(string currencyId, int initialAmount = 0)
        {
            if (string.IsNullOrWhiteSpace(currencyId))
                return;

            if (!GameManager.IsValidCurrencyId(currencyId))
            {
                DebugManager.Warning($"unknown currencyId '{currencyId}'. Currency will be ignored.", "System");
                return;
            }

            if (Currencies == null)
                Currencies = new Dictionary<string, int>();

            if (Currencies.ContainsKey(currencyId))
                return;

            if (!Currencies.ContainsKey(currencyId))
            {
                if (initialAmount < 0) initialAmount = 0;
                Currencies[currencyId] = initialAmount;
            }
        }

        /// <summary>
        /// Attempts to spend a specified amount of currency.
        /// </summary>
        /// <param name="currencyId">The ID of the currency to spend.</param>
        /// <param name="amount">The amount of currency to spend.</param>
        /// <returns>True if the currency was successfully spent; otherwise, false.</returns>
        public bool SpendCurrency(string currencyId, int amount)
        {
            if (amount <= 0) return false;
            if (!CanSpend(currencyId, amount)) return false;

            Currencies[currencyId] -= amount;
            return true;
        }

/// <summary>
/// Checks if the specified amount of currency can be spent.
/// </summary>
/// <param name="currencyId">The identifier for the currency to check.</param>
/// <param name="amount">The amount of currency to check.</param>
/// <returns>True if the amount can be spent; otherwise, false.</returns>
        public bool CanSpend(string currencyId, int amount)
        {
            if (amount <= 0) return false;
            return (GetCurrency(currencyId) >= amount);
        }


/// <summary>
/// Processes a refund for the specified currency and amount.
/// </summary>
/// <param name="currencyId">The identifier for the currency to refund.</param>
/// <param name="amount">The amount of currency to refund.</param>
        public void Refund(string currencyId, int amount)
        {
            if (amount <= 0) return;
            AddCurrency(currencyId, amount);
        }

/// <summary>
/// Retrieves a list of heroes that are currently unlocked.
/// </summary>
/// <returns>A read-only list of unlocked hero identifiers.</returns>
        public IReadOnlyList<string> GetUnlockedHeroes()
            => UnlockedHeroes;

/// <summary>
/// Checks if the specified hero is unlocked.
/// </summary>
/// <param name="heroId">The identifier of the hero to check.</param>
/// <returns>True if the hero is unlocked; otherwise, false.</returns>
        public bool IsHeroUnlocked(string heroId)
            => !string.IsNullOrEmpty(heroId) && UnlockedHeroes.Contains(heroId);

/// <summary>
/// Unlocks the specified hero by its identifier.
/// </summary>
/// <param name="heroId">The identifier of the hero to unlock.</param>
/// <returns>True if the hero was successfully unlocked; otherwise, false.</returns>
        public bool UnlockHero(string heroId)
        {
            if (string.IsNullOrEmpty(heroId)) return false;
            if (UnlockedHeroes.Contains(heroId)) return false;
            UnlockedHeroes.Add(heroId);
            return true;
        }

/// <summary>
/// Locks the specified hero by its identifier.
/// </summary>
/// <param name="heroId">The identifier of the hero to lock.</param>
/// <returns>True if the hero was successfully locked; otherwise, false.</returns>
        public bool LockHero(string heroId)
        {
            if (string.IsNullOrEmpty(heroId)) return false;
            return UnlockedHeroes.Remove(heroId);
        }

/// <summary>
/// Ensures that the specified starter hero is unlocked.
/// </summary>
/// <param name="starterHeroId">The identifier of the starter hero.</param>
/// <returns>True if the hero was successfully unlocked; otherwise, false.</returns>
        public bool EnsureStarterHeroUnlocked(string starterHeroId)
        {
            if (string.IsNullOrEmpty(starterHeroId)) return false;
            if (UnlockedHeroes == null) UnlockedHeroes = new List<string>();
            if (UnlockedHeroes.Contains(starterHeroId)) return false;
            UnlockedHeroes.Add(starterHeroId);
            return true; // hat was geändert
        }


/// <summary>
/// Sets the progress for a specified map based on its difficulty.
/// </summary>
/// <param name="map">The identifier of the map.</param>
/// <param name="difficulty">The difficulty level of the map.</param>
        public void SetMapProgress(int map, MapDifficulty difficulty)
        {
                MapProgress[map] = (int)difficulty;
        }

/// <summary>
/// Retrieves the progress of a specified map.
/// </summary>
/// <param name="map">The identifier of the map.</param>
/// <returns>The progress value for the map, or 0 if not found.</returns>
        public int GetMapProgress(int map)
        {
            return MapProgress.TryGetValue(map, out var highest) ? highest : 0;
        }

        private void RecalculateLevel()
        {
            int xp = XP;
            int level = 1;
            int totalXpRequired = 0;
            int xpForNext = 0;

            while (true)
            {
                xpForNext = BalanceManager.GetXpForLevel(level);
                if (xp < totalXpRequired + xpForNext)
                    break;

                totalXpRequired += xpForNext;
                level++;
            }

            Level = level;

            XPInCurrentLevel = XP - totalXpRequired; // nicht XP überschreiben!
            XPToNextLevel = xpForNext;
            missingXP = xpForNext - XPInCurrentLevel;
            levelProgress = (float)XPInCurrentLevel / xpForNext;
            DebugManager.Log($"Player XP={XP}, Level={Level}", DebugManager.EDebugLevel.Debug, "Player");
            DebugManager.Log($"Player next Level {levelProgress:P2} - missing:{missingXP} ", DebugManager.EDebugLevel.Debug, "Player");
        }


        /// <summary>
        /// Prepares a snapshot of the current inventory by clearing and populating the inventory save list.
        /// </summary>
        [System.Obsolete("LEGACY: Use GameManager.MapDomainToProfile / MapProfileToDomain instead. Remove in Phase 4.", false)]
/// <summary>
/// Prepares a snapshot of the current inventory by clearing the existing data.
/// Logs the completion of the snapshot preparation.
/// </summary>
        public void PrepareInventorySnapshot()
        {
            InventorySave ??= new List<InventorySnapshot>();
            InventorySave.Clear();

            ////foreach (var inv in Inventories)
            ////{
            ////    if (inv == null) continue;
            ////    //DebugManager.Log($"inv: {inv.invID} -  {inv.GetAllItems().Count}");
            ////    var dict = inv.ToDictionary() ?? new Dictionary<string, int>();
            ////    InventorySave.Add(new InventorySnapshot { id = inv.invID, items = dict });
            ////}

            // kleine, robuste Logs
            DebugManager.Log("InventorySnapshot built:", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
            foreach (var s in InventorySave)
                DebugManager.Log($" - {s.id}: {s.items?.Count ?? 0}", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
        }

        // Nach dem Laden: Snapshot zurück in die Live-Inventare schieben
        /// <summary>
        /// Restores inventories from a saved snapshot if available.
        /// Initializes live inventories if none exist.
        /// </summary>
        [System.Obsolete("LEGACY: Do not use. Use Domain snapshot path. Remove in Phase 4.", false)]
/// <summary>
/// Restores inventories from a saved snapshot.
/// This method will do nothing if the snapshot is null.
/// </summary>
        public void RestoreInventoriesFromSnapshot()
        {
            if (InventorySave == null) return;

            // Hilfsresolver
            //Inventory GetById(string id)
            //{
            //    for (int i = 0; i < Inventories.Count; i++)
            //        if (string.Equals(Inventories[i].invID, id, StringComparison.Ordinal))
            //            return Inventories [i];
            //    return null;
            //}

            //int applied = 0;
            //foreach (var snap in InventorySave)
            //{
            //    if (string.IsNullOrEmpty(snap.id)) continue;
            //    var inv = GetById(snap.id);
            //    if (inv == null) continue;

            //    inv.FromDictionary(snap.items ?? new Dictionary<string, int>());
            //    applied++;
            //}

            //DebugManager.Log($"InventorySnapshot restored — applied:{applied}", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
        }

/// <summary>
/// Builds a ResearchSnapshot from the given ResearchState.
/// </summary>
/// <param name="state">The ResearchState to build the snapshot from.</param>
/// <returns>A ResearchSnapshot representing the state.</returns>
        //public ResearchSnapshot BuildResearchSnapshotFrom(CodexState state)
        //{
        //    var snap = new ResearchSnapshot();
        //    if (state == null) return snap;

        //    snap.activeNodeId = state.activeNodeId;
        //    snap.completedNodeIds.AddRange(state.completedNodeIds);

        //    foreach (var kv in state.perDeedProgress)
        //    {
        //        var p = kv.Value;
        //        var e = new NodeProgressEntry
        //        {
        //            nodeId = kv.Key,
        //            progress = new NodeProgressSave
        //            {
        //                waves = p.waves,
        //                mapsTotal = p.mapsTotal,
        //                killsGeneralWeighted = p.killsGeneralWeighted,
        //                eliteCount = p.eliteCount,
        //                bossCount = p.bossCount,
        //                mapsByDifficulty = new List<MapRequirement>(),
        //                killsByTagWeighted = new List<KillTagCount>(),
        //            }
        //        };
        //        if (p.mapsByDifficulty != null)
        //            foreach (var md in p.mapsByDifficulty)
        //                e.progress.mapsByDifficulty.Add(new MapRequirement { difficulty = md.Key, amount = md.Value });

        //        if (p.killsByTagWeighted != null)
        //            foreach (var t in p.killsByTagWeighted)
        //                e.progress.killsByTagWeighted.Add(new KillTagCount { enemyTag = t.Key, count = t.Value });

        //        snap.perNodeProgress.Add(e);
        //    }
        //    return snap;
        //}

/// <summary>
/// Restores research data from a snapshot into the given research state.
/// Updates the active node ID and clears progress data as needed.
/// </summary>
/// <param name="state">The research state to restore data into.</param>
/// <param name="snap">The research snapshot containing the data to restore.</param>
        //public void RestoreResearchInto(CodexState state, ResearchSnapshot snap)
        //{
        //    if (state == null) return;

        //    state.activeNodeId = snap?.activeNodeId;
        //    state.completedNodeIds.Clear();
        //    state.perDeedProgress.Clear();

        //    if (snap == null) return;

        //    foreach (var id in snap.completedNodeIds)
        //        state.completedNodeIds.Add(id);

        //    foreach (var e in snap.perNodeProgress)
        //    {
        //        var np = new DeedProgress
        //        {
        //            waves = e.progress.waves,
        //            mapsTotal = e.progress.mapsTotal,
        //            killsGeneralWeighted = e.progress.killsGeneralWeighted,
        //            eliteCount = e.progress.eliteCount,
        //            bossCount = e.progress.bossCount,
        //            mapsByDifficulty = new Dictionary<MapDifficulty, int>(),
        //            killsByTagWeighted = new Dictionary<string, int>(StringComparer.Ordinal),
        //        };
        //        if (e.progress.mapsByDifficulty != null)
        //            foreach (var md in e.progress.mapsByDifficulty) np.mapsByDifficulty[md.difficulty] = md.amount;
        //        if (e.progress.killsByTagWeighted != null)
        //            foreach (var t in e.progress.killsByTagWeighted) np.killsByTagWeighted[t.enemyTag ?? ""] = t.count;

        //        state.perDeedProgress[e.nodeId] = np;
        //    }
        //}

/// <summary>
/// Retrieves the hero progress data for the specified hero ID, or creates a new entry if it does not exist.
/// </summary>
/// <param name="heroId">The ID of the hero.</param>
/// <returns>The hero progress data associated with the hero ID, or null if the ID is empty.</returns>
        public HeroProgressData GetOrCreateHeroProgress(string heroId)
        {
            if (string.IsNullOrEmpty(heroId))
            {
                DebugManager.Error("[PlayerProfileDTO] GetOrCreateHeroProgress called with empty heroId.", "Hero");
                return null;
            }

            var hpd = HeroesData.FirstOrDefault(h => h.HeroId == heroId);

            if (hpd == null)
            {
                hpd = new HeroProgressData
                {
                    HeroId = heroId,
                    Level = 1,
                    CurrentXP = 0,
                    TotalXP = 0,
                    TotalOrbitPoints = 0,
                    UnspentOrbitPoints = 0,
                    UnlockedSockets = 0
                };
                HeroesData.Add(hpd);
                DebugManager.Log($"[Profile] Created new HeroProgress for {heroId}.", DebugManager.EDebugLevel.Debug, "Hero");
            }

            return hpd;
        }

/// <summary>
/// Updates the hero's progress based on the provided hero instance.
/// </summary>
/// <param name="inst">The hero instance containing progress data.</param>
        public void UpdateHeroProgressFromInstance(HeroInstance inst)
        {
            if (inst == null || inst.heroDef == null)
            {
                DebugManager.Error("[PlayerProfileDTO] UpdateHeroProgressFromInstance called with null.", "Hero");
                return;
            }

            var hpd = GetOrCreateHeroProgress(inst.heroDef.HeroId);
            if (hpd == null) return;

            hpd.Level = inst.Level;
            hpd.CurrentXP = inst.CurrentXP;
            hpd.TotalXP = inst.TotalXP;
            hpd.TotalOrbitPoints = inst.TotalOrbitPointsEarned;
            hpd.UnspentOrbitPoints = inst.UnspentOrbitPoints;
            hpd.UnlockedSockets = inst.UnlockedSockets;
        }

    }

    [Serializable]
    public struct InventorySlotSnapshot
    {
        public int slot;              // SlotIndex im InventoryInstance
        public string itemId;         // ItemDef id
        public int count;             // StackCount (instanced => 1)
        public string iteminstanceId;     // null/empty => nicht-instanced
    }

    [Serializable]
    public struct InventorySnapshot
    {
        public string id;                                 // z.B. "remains", "part", "rune", "module", "gear"

        // Legacy/Convenience (z.B. für UI/Counts):
        public Dictionary<string, int> items;             // flache Map (itemId -> count)

        // Neu: Slot-genau (wichtig für instanced Items, Positionen, instanceId):
        public List<InventorySlotSnapshot> slots;         // nur belegte Slots (empfohlen)

        public List<GearInstance> gearInstances;          // alle GearInstances die in 'slots' referenziert werden

        public List<SkillModuleInstance> skillModuleInstances; //: SkillModule variant payloads (instanceId = VariantKey)
    }
}
