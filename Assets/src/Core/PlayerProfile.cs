using CHAL.Core;
using CHAL.Systems.Inventory;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CHAL.Data
{
    [Serializable]
    public class PlayerProfile
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

        // --- Currencies ---
        public Dictionary<string, int> Currencies = new();
        // Beispiel: { "gold" -> 1234, "dna" -> 50 

        // --- Items ---
        public Inventory Remains = new("remains");
        public Inventory Parts = new("part");
        public Inventory Runes = new("rune");
        public Inventory Modules = new("module");

        // Map Progress
        //first int is MapNo, 
        //second int is highest difficulty succeded
        public Dictionary<int,int> MapProgress = new();
        // Setzen: SetMapProgress(1,MapDifficulty.easy,9)
        // Abfragen:  GetMapProgress(1, MapDifficulty.medium)

        public void InitializePlayer(string name, Color[] colors)
        { 
            playerName = name;
            playerColors = colors;

            SaveSystem.Save(this);
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
            if (!Currencies.ContainsKey(currencyId))
                Currencies[currencyId] = 0;

            Currencies[currencyId] += amount;
        }

        public bool SpendCurrency(string currencyId, int amount)
        {
            if (GetCurrency(currencyId) < amount)
                return false;

            Currencies[currencyId] -= amount;
            return true;
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

    }
}