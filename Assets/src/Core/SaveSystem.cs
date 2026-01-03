using BayatGames.SaveGameFree;
using CHAL.Data;
using CHAL.Systems.Research;
using CHAL.Systems.Stats;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;

namespace CHAL.Core
{
    public static class SaveSystem
    {
        // ---- Einstellungen ----
        private static GameSaveConfig _cfg;

        private static GameSaveConfig Cfg
        {
            get
            {
                if (_cfg == null)
                    _cfg = Resources.Load<GameSaveConfig>("Config/GameSaveConfig");
                return _cfg;
            }
        }

        private static void ConfigureSaveGame()
        {
            if (Cfg == null)
            {
                DebugManager.Log("SaveSystem: GameSaveConfig not found at Resources/Config/GameSaveConfig", DebugManager.EDebugLevel.Dev, "Save", LogType.Error);
                return;
            }
            SaveGame.Encode = Cfg.ShouldEncodeRuntime();
            SaveGame.EncodePassword = Cfg.encodePassword ?? string.Empty;
            // SaveGame.SavePath bleibt Default (persistentDataPath)
        }

        // ---- API: Save/Load für PlayerProfile ----
        private static string FileId()
        {
            return Cfg != null ? Cfg.ResolveFileIdRuntime() : "profiles/main/profile.json";
        }

/// <summary>
/// Saves the specified player profile to the game storage.
/// </summary>
/// <param name="profile">The player profile to save.</param>
        public static void Save(PlayerProfile profile)
        {
            if (profile == null)
            {
                DebugManager.Log("SaveSystem.Save: profile is null", DebugManager.EDebugLevel.Dev, "Save", LogType.Warning);
                return;
            }
            ConfigureSaveGame();

            // Wenn der GameManager bereits einen Domain-Snapshot (Slots + Instances) gebaut hat,
            // dürfen wir ihn hier nicht überschreiben.
            if (profile.InventorySave == null || profile.InventorySave.Count == 0)
            {
                var gm = GameManager.Instance;
                if (gm != null && ReferenceEquals(gm.Profile, profile) && gm.InventoryReady)
                {
                    gm.MapDomainToProfile();
                }
                else
                {
                    DebugManager.Log(
                        "SaveSystem.Save: InventorySave is empty and no InventoryDomain snapshot is available. Saving without inventory slots.",
                        DebugManager.EDebugLevel.Production, "Save", LogType.Warning);
                }
            }

            


            profile.LastSaveTime = DateTime.UtcNow;
            var id = FileId();

            // Inventories immer separat speichern (auch wenn sie leer sind)
            SaveInventories(profile.profileId, profile.InventorySave);

            // InventorySave soll NICHT mit ins Profil-File serialisiert werden,
            // damit profile.json schlank bleibt.
            var backupInventories = profile.InventorySave;
            profile.InventorySave = null;

            SaveGame.Save(id, profile);

            // Runtime wieder herstellen
            profile.InventorySave = backupInventories;

            DebugManager.Log($"SaveSystem: saved → {id}", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
        }

/// <summary>
/// Loads the player profile from the save game file.
/// </summary>
/// <returns>The loaded PlayerProfile, or null if no file exists.</returns>
        public static PlayerProfile Load()
        {
            ConfigureSaveGame();

            var id = FileId();
            if (!SaveGame.Exists(id))
            {
                DebugManager.Log($"SaveSystem.Load: no file at '{id}'", DebugManager.EDebugLevel.Dev, "Save", LogType.Warning);
                return null;
            }

            //Revert: direkt PlayerProfile laden
            var p = SaveGame.Load<PlayerProfile>(id);
            if (p == null)
            {
                DebugManager.Log($"SaveSystem.Load: failed to read '{id}'", DebugManager.EDebugLevel.Dev, "Save", LogType.Error);
                return null;
            }

            //HeroData checken:
            DebugManager.Log($"[SaveSystem.Load] HeroesData={p.HeroesData.Count}", DebugManager.EDebugLevel.Test, "Hero");
            for (int i = 0; i < p.HeroesData.Count; i++)
            {
                var h = p.HeroesData[i];
                DebugManager.Log($"  - {h.HeroId}: L{h.Level} XP={h.CurrentXP} TotalXP={h.TotalXP}",
                    DebugManager.EDebugLevel.Test, "Hero");
            }

            p.profileId = CurrentProfileId();

            // Inventories aus separater Datei nachladen
            p.InventorySave = LoadInventories(p.profileId);
            // Wichtig: Profile.InventorySave wird im SaveSystem.Load() aus inventory_v1.json gefüllt.
            // Die eigentliche Überführung in den InventoryDomain passiert erst hier:
            // - BootstrapInventoryDomain(): erzeugt Player- und Hero-Inventories (hero:{HeroId}:gear/sockets)
            // - MapProfileToDomain(): füllt alle Instanzen aus Profile.InventorySave
            // da spassiert im GameManager bei ContinueGame() oder StartNewGame()

            DebugManager.Log($"SaveSystem: loaded ← {id}", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
            return p;
        }

        /// <summary>Für „Neu anfangen“: löscht das eine Profil komplett.</summary>
        public static bool DeleteProfileData(string profileId)
        {
            ConfigureSaveGame();
            var pid = string.IsNullOrWhiteSpace(profileId) ? CurrentProfileId() : profileId;

            var ids = new[]
            {
                ProfileFileId(pid),
                ResearchFileId(pid),
                StatisticsFileId(pid),
                InventoryFileId(pid),
            };

            bool deletedAny = false;

            foreach (var id in ids)
            {
                if (!SaveGame.Exists(id)) continue;
                SaveGame.Delete(id);
                deletedAny = true;
                DebugManager.Log($"DeleteProfileData: deleted '{id}'", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
            }

            return deletedAny;
        }

        private static string ProfileFileId(string profileId)
        {
            // gleiche Struktur wie FileId() (standard: "profiles/main/profile.json")
            return $"profiles/{profileId}/profile.json";
        }

        /// <summary>
        /// Saves a research snapshot associated with the specified profile ID.
        /// If the profile ID is empty, the current profile ID is used.
        /// </summary>
        /// <param name="profileId">The ID of the profile to save the research snapshot for.</param>
        /// <param name="snap">The research snapshot to save.</param>
        public static void SaveResearch(string profileId, ResearchSnapshot snap)
        {
            ConfigureSaveGame(); // Encoder/Passwort etc. aus GameSaveConfig

            var pid = string.IsNullOrWhiteSpace(profileId) ? CurrentProfileId() : profileId;
            var id = ResearchFileId(pid);

            SaveGame.Save(id, snap ?? new ResearchSnapshot());

            DebugManager.Log($"SaveResearch → {id}", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
        }

/// <summary>
/// Loads a research snapshot based on the provided profile ID.
/// If the profile ID is empty, the current profile ID is used.
/// </summary>
/// <param name="profileId">The ID of the profile to load research for.</param>
/// <returns>A ResearchSnapshot object containing the loaded data.</returns>
        public static ResearchSnapshot LoadResearch(string profileId)
        {
            ConfigureSaveGame();

            var pid = string.IsNullOrWhiteSpace(profileId) ? CurrentProfileId() : profileId;
            var id = ResearchFileId(pid);

            if (!SaveGame.Exists(id))
            {
                DebugManager.Log($"LoadResearch: no file at '{id}', returning empty snapshot.", DebugManager.EDebugLevel.Dev, "Research", LogType.Warning);
                return new ResearchSnapshot();
            }

            var snap = SaveGame.Load<ResearchSnapshot>(id) ?? new ResearchSnapshot();
            DebugManager.Log($"LoadResearch ← {id}", DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
            return snap;
        }

/// <summary>
/// Deletes the research data associated with the specified profile ID.
/// Returns true if the deletion was successful; otherwise, false.
/// </summary>
/// <param name="profileId">The ID of the profile whose research data is to be deleted.</param>
/// <returns>True if the research data was deleted; otherwise, false.</returns>
        public static bool DeleteResearch(string profileId)
        {
            ConfigureSaveGame();
            var id = ResearchFileId(string.IsNullOrWhiteSpace(profileId) ? "main" : profileId);
            if (!SaveGame.Exists(id)) return false;
            SaveGame.Delete(id);
            DebugManager.Log($"DeleteResearch: {id}", DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
            return true;
        }

/// <summary>
/// Saves the statistics for a specified profile ID.
/// If the snapshot is null, a new empty snapshot is saved.
/// </summary>
/// <param name="profileId">The ID of the profile to save statistics for.</param>
/// <param name="snapshot">The statistics snapshot to save.</param>
        public static void SaveStatistics(string profileId, StatisticsSnapshot snapshot)
        {
            ConfigureSaveGame();

            var pid = string.IsNullOrWhiteSpace(profileId) ? CurrentProfileId() : profileId;
            var id = StatisticsFileId(pid);

            SaveGame.Save(id, snapshot ?? new StatisticsSnapshot());

            DebugManager.Log($"SaveStatistics → {id}", DebugManager.EDebugLevel.Dev, "Save");
        }

/// <summary>
/// Loads the statistics for a given profile ID.
/// Returns an empty snapshot if no statistics file exists.
/// </summary>
/// <param name="profileId">The ID of the profile to load statistics for.</param>
/// <returns>A StatisticsSnapshot containing the loaded statistics.</returns>
        public static StatisticsSnapshot LoadStatistics(string profileId)
        {
            ConfigureSaveGame();

            var pid = string.IsNullOrWhiteSpace(profileId) ? CurrentProfileId() : profileId;
            var id = StatisticsFileId(pid);

            if (!SaveGame.Exists(id))
            {
                DebugManager.Log($"LoadStatistics: no file at '{id}', returning empty snapshot.",
                    DebugManager.EDebugLevel.Dev, "Stats", LogType.Warning);
                return new StatisticsSnapshot();
            }

            var snap = SaveGame.Load<StatisticsSnapshot>(id) ?? new StatisticsSnapshot();
            DebugManager.Log($"LoadStatistics ← {id}", DebugManager.EDebugLevel.Dev, "Save");
            return snap;
        }


        /// <summary>
        /// Saves the inventory snapshots for a specified profile ID in a separate file.
        /// </summary>
        public static void SaveInventories(string profileId, List<InventorySnapshot> snapshots)
        {
            ConfigureSaveGame();

            var pid = string.IsNullOrWhiteSpace(profileId) ? CurrentProfileId() : profileId;
            var id = InventoryFileId(pid);

            // Leere Liste als Fallback, damit die Datei immer ein valides Array enthält
            var data = snapshots ?? new List<InventorySnapshot>();

            SaveGame.Save(id, data);

            DebugManager.Log($"SaveInventories → {id} (count={data.Count})",
                DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
        }

        /// <summary>
        /// Loads inventory snapshots for a given profile ID from the separate inventory file.
        /// Returns an empty list if no file exists.
        /// </summary>
        public static List<InventorySnapshot> LoadInventories(string profileId)
        {
            ConfigureSaveGame();

            var pid = string.IsNullOrWhiteSpace(profileId) ? CurrentProfileId() : profileId;
            var id = InventoryFileId(pid);

            if (!SaveGame.Exists(id))
            {
                DebugManager.Log($"LoadInventories: no file at '{id}', returning empty list.",
                    DebugManager.EDebugLevel.Dev, "Save", LogType.Warning);
                return new List<InventorySnapshot>();
            }

            var list = SaveGame.Load<List<InventorySnapshot>>(id) ?? new List<InventorySnapshot>();
            DebugManager.Log($"LoadInventories ← {id} (count={list.Count})",
                DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
            return list;
        }

        /// <summary>
        /// Deletes the inventory save file for the given profile ID.
        /// </summary>
        public static bool DeleteInventories(string profileId)
        {
            ConfigureSaveGame();

            var pid = string.IsNullOrWhiteSpace(profileId) ? CurrentProfileId() : profileId;
            var id = InventoryFileId(pid);

            if (!SaveGame.Exists(id)) return false;

            SaveGame.Delete(id);
            DebugManager.Log($"DeleteInventories: deleted '{id}'",
                DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
            return true;
        }


        private static string InventoryFileId(string profileId)
        {
            // Eigene Datei für Inventory-Snapshots
            return $"profiles/{profileId}/inventory_v1.json";
        }


        private static string ResearchFileId(string profileId)
        {
            // Gleiche Struktur wie beim Profil – SaveGame speichert unter persistentDataPath/<id>
            return $"profiles/{profileId}/research_v1.json";
        }

        private static string StatisticsFileId(string profileId)
        {
            // Eigene Datei für Statistik-Snapshots
            return $"profiles/{profileId}/statistics_v1.json";
        }

        /// <summary>
        /// Retrieves the current profile ID from a file path.
        /// If no valid ID is found, it returns "main".
        /// </summary>
        /// <returns>The current profile ID as a string.</returns>
        public static string CurrentProfileId()
        {
            var id = FileId(); // z. B. "profiles/main/profile.json"
                               // Robust parsen:
                               //  - nimmt das Segment nach "profiles/"
                               //  - schneidet "/profile.json" ab
            const string anchor = "profiles/";
            int i = id.IndexOf(anchor, StringComparison.Ordinal);
            if (i < 0) return "main";
            i += anchor.Length;
            int j = id.IndexOf('/', i);
            if (j < 0) j = id.Length;
            var pid = id.Substring(i, j - i);
            return string.IsNullOrWhiteSpace(pid) ? "main" : pid;
        }

    }
}
