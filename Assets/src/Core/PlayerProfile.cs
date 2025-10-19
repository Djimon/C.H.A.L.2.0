using CHAL.Core;
using CHAL.Systems.Economy;
using CHAL.Systems.Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    [Serializable]
    public class PlayerProfile : IWallet
    {
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
        [NonSerialized] public List<Inventory> Inventories = new();
        // Persistenter Snapshot aller Inventare (nur für Save/Load)
        public List<InventorySnapshot> InventorySave = new();

        public void InitializePlayer(string name, Color[] colors)
        {
            playerName = name;
            playerColors = colors;

            var starterId = GameManager.Instance.starterHero != null ? GameManager.Instance.starterHero.HeroId : "TestHero";
            EnsureStarterHeroUnlocked(starterId);
            InitInventories();

            SaveSystem.Save(this);
        }

        private void InitInventories()
        {
            Inventories.Add(new("remains"));
            Inventories.Add(new("part")); 
            Inventories.Add(new("rune"));
            Inventories.Add(new("module"));
            Inventories.Add(new("gear"));
        }

        public int GetXP() => XP;

        public void AddXP(int amount)
        {
            XP += amount;
            RecalculateLevel();
        }


        public int GetCurrency(string currencyId)
        {
            return Currencies.TryGetValue(currencyId, out var amount) ? amount : 0;
        }

        public void AddCurrency(string currencyId, int amount)
        {
            if (amount <= 0) return;

            if (!Currencies.ContainsKey(currencyId))
                Currencies[currencyId] = 0;

            Currencies[currencyId] += amount;
        }

        public bool SpendCurrency(string currencyId, int amount)
        {
            if (amount <= 0) return false;
            if (!CanSpend(currencyId, amount)) return false;

            Currencies[currencyId] -= amount;
            return true;
        }

        public bool CanSpend(string currencyId, int amount)
        {
            if (amount <= 0) return false;
            return (GetCurrency(currencyId) >= amount);
        }


        public void Refund(string currencyId, int amount)
        {
            if (amount <= 0) return;
            AddCurrency(currencyId, amount);
        }

        public IReadOnlyList<string> GetUnlockedHeroes()
            => UnlockedHeroes;

        public bool IsHeroUnlocked(string heroId)
            => !string.IsNullOrEmpty(heroId) && UnlockedHeroes.Contains(heroId);

        public bool UnlockHero(string heroId)
        {
            if (string.IsNullOrEmpty(heroId)) return false;
            if (UnlockedHeroes.Contains(heroId)) return false;
            UnlockedHeroes.Add(heroId);
            return true;
        }

        public bool LockHero(string heroId)
        {
            if (string.IsNullOrEmpty(heroId)) return false;
            return UnlockedHeroes.Remove(heroId);
        }

        public bool EnsureStarterHeroUnlocked(string starterHeroId)
        {
            if (string.IsNullOrEmpty(starterHeroId)) return false;
            if (UnlockedHeroes == null) UnlockedHeroes = new List<string>();
            if (UnlockedHeroes.Contains(starterHeroId)) return false;
            UnlockedHeroes.Add(starterHeroId);
            return true; // hat was geändert
        }


        public void SetMapProgress(int map, MapDifficulty difficulty)
        {
                MapProgress[map] = (int)difficulty;
        }

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


        public void PrepareInventorySnapshot()
        {
            InventorySave ??= new List<InventorySnapshot>();
            InventorySave.Clear();

            foreach (var inv in Inventories)
            {
                if (inv == null) continue;
                DebugManager.Log($"inv: {inv.invID} -  {inv.GetAllItems().Count}");
                var dict = inv.ToDictionary() ?? new Dictionary<string, int>();
                InventorySave.Add(new InventorySnapshot { id = inv.invID, items = dict });
            }

            // kleine, robuste Logs
            DebugManager.Log("InventorySnapshot built:", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
            foreach (var s in InventorySave)
                DebugManager.Log($" - {s.id}: {s.items?.Count ?? 0}", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
        }

        // Nach dem Laden: Snapshot zurück in die Live-Inventare schieben
        public void RestoreInventoriesFromSnapshot()
        {
            if (InventorySave == null) return;

            // Falls z.B. frisch aus Menü geladen wurde: sicherstellen, dass Live-Inventare existieren
            if (Inventories.Count == 0)
                InitInventories();

            // Hilfsresolver
            Inventory GetById(string id)
            {
                for (int i = 0; i < Inventories.Count; i++)
                    if (string.Equals(Inventories[i].invID, id, StringComparison.Ordinal))
                        return Inventories [i];
                return null;
            }

            int applied = 0;
            foreach (var snap in InventorySave)
            {
                if (string.IsNullOrEmpty(snap.id)) continue;
                var inv = GetById(snap.id);
                if (inv == null) continue;

                inv.FromDictionary(snap.items ?? new Dictionary<string, int>());
                applied++;
            }

            DebugManager.Log($"InventorySnapshot restored — applied:{applied}", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
        }


    }

    [Serializable]
    public struct InventorySnapshot
    {
        public string id;                                 // z.B. "remains", "part", "rune", "module", "gear"
        public Dictionary<string, int> items;             // flache Map (itemId -> count)
    }
}