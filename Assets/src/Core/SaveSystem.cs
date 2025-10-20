using BayatGames.SaveGameFree;
using CHAL.Data;
using CHAL.Systems.Research;
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

        public static void Save(PlayerProfile profile)
        {
            if (profile == null)
            {
                DebugManager.Log("SaveSystem.Save: profile is null", DebugManager.EDebugLevel.Dev, "Save", LogType.Warning);
                return;
            }
            ConfigureSaveGame();

            profile.PrepareInventorySnapshot();
            profile.LastSaveTime = DateTime.UtcNow;
            var id = FileId();
            
            SaveGame.Save(id, profile);

            DebugManager.Log($"SaveSystem: saved → {id}", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
        }

        public static PlayerProfile Load()
        {
            ConfigureSaveGame();

            var id = FileId();
            if (!SaveGame.Exists(id))
            {
                DebugManager.Log($"SaveSystem.Load: no file at '{id}'", DebugManager.EDebugLevel.Dev, "Save", LogType.Warning);
                return null;
            }

            // ⬇️ Revert: direkt PlayerProfile laden
            var p = SaveGame.Load<PlayerProfile>(id);
            if (p == null)
            {
                DebugManager.Log($"SaveSystem.Load: failed to read '{id}'", DebugManager.EDebugLevel.Dev, "Save", LogType.Error);
                return null;
            }

            p.RestoreInventoriesFromSnapshot();

            p.profileId = CurrentProfileId();

            //var snap = LoadResearch("");
            //p.RestoreResearchInto(p.ResearchRuntime, snap);

            DebugManager.Log($"SaveSystem: loaded ← {id}", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
            return p;
        }

        /// <summary>Für „Neu anfangen“: löscht das eine Profil komplett.</summary>
        public static bool DeleteProfileData(string profileId)
        {
            ConfigureSaveGame();
            var pid = string.IsNullOrWhiteSpace(profileId) ? CurrentProfileId() : profileId;
            var id = ResearchFileId(pid);
            if (!SaveGame.Exists(id)) return false;
            SaveGame.Delete(id);
            DebugManager.Log($"SaveSystem: deleted '{id}'", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
            return true;
        }

        public static void SaveResearch(string profileId, ResearchSnapshot snap)
        {
            ConfigureSaveGame(); // Encoder/Passwort etc. aus GameSaveConfig

            var pid = string.IsNullOrWhiteSpace(profileId) ? CurrentProfileId() : profileId;
            var id = ResearchFileId(profileId);

            SaveGame.Save(id, snap ?? new ResearchSnapshot());

            DebugManager.Log($"SaveResearch → {id}", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
        }

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

        public static bool DeleteResearch(string profileId)
        {
            ConfigureSaveGame();
            var id = ResearchFileId(string.IsNullOrWhiteSpace(profileId) ? "main" : profileId);
            if (!SaveGame.Exists(id)) return false;
            SaveGame.Delete(id);
            DebugManager.Log($"DeleteResearch: {id}", DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
            return true;
        }

        private static string ResearchFileId(string profileId)
        {
            // Gleiche Struktur wie beim Profil – SaveGame speichert unter persistentDataPath/<id>
            return $"profiles/{profileId}/research_v1.json";
        }

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