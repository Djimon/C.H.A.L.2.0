using CHAL.Systems.Inventory;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    public class PlayerProfile
    {
        // --- Meta ---
        public DateTime LastSaveTime;            // Für Autosave / Debug

        // --- Charakter-Fortschritt ---
        public int XP;                           // Gesamt-XP
        public int Level;                        // optional: aus XP berechnet

        // --- Currencies ---
        public Dictionary<string, int> Currencies = new();
        // Beispiel: { "gold" -> 1234, "dna" -> 50 

        // --- Items ---
        public Inventory Remains = new("remains");
        public Inventory Parts = new("part");
        public Inventory Runes = new("rune");
        public Inventory Modules = new("module");

        // Map Progress
        public Dictionary<int, Dictionary<MapDifficulty, int>> MapProgress = new();
        // Setzen: SetMapProgress(1,MapDifficulty.easy,9)
        // Abfragen:  GetMapProgress(1, MapDifficulty.medium)

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


        public void SetMapProgress(int map, MapDifficulty difficulty, int wave)
        {
            if (!MapProgress.ContainsKey(map))
                MapProgress[map] = new Dictionary<MapDifficulty, int>();

            MapProgress[map][difficulty] = wave;
        }

        public int GetMapProgress(int map, MapDifficulty difficulty)
        {
            if (MapProgress.TryGetValue(map, out var diffDict) &&
                diffDict.TryGetValue(difficulty, out var wave))
                return wave;

            return 0; // Default = noch nicht gespielt
        }

        private void RecalculateLevel()
        {
            // Beispiel: 100 XP pro Level
            Level = XP / 100;
        }

    }
}