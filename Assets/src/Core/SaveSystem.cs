using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using CHAL.Data;

namespace CHAL.Core
{
    public static class SaveSystem
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

        public static void Save(PlayerProfile profile)
        {
            var json = JsonConvert.SerializeObject(profile, Formatting.Indented);
            File.WriteAllText(SavePath, json);
            Debug.Log($"Spielstand gespeichert: {SavePath}");
        }

        public static PlayerProfile Load()
        {
            if (!File.Exists(SavePath))
                return null;

            var json = File.ReadAllText(SavePath);
            return JsonConvert.DeserializeObject<PlayerProfile>(json);
        }
    }
}