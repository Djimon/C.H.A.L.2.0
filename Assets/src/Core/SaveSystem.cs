using BayatGames.SaveGameFree;
using CHAL.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

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

            DebugManager.Log($"SaveSystem: loaded ← {id}", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
            return p;
        }

        /// <summary>Für „Neu anfangen“: löscht das eine Profil komplett.</summary>
        public static bool DeleteProfileData()
        {
            ConfigureSaveGame();
            var id = FileId();
            if (!SaveGame.Exists(id)) return false;
            SaveGame.Delete(id);
            DebugManager.Log($"SaveSystem: deleted '{id}'", DebugManager.EDebugLevel.Dev, "Save", LogType.Log);
            return true;
        }
    }
}