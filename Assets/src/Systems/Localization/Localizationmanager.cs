using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Localization
{
    public static class LocalizationManager
    {
        private static Dictionary<string, string> _dict;

        public static void Load(string languageCode)
        {
            // JSON laden: { "Enemy_InsectSwarm_Name": "Insekten-Schwarm", ... }
            TextAsset json = Resources.Load<TextAsset>($"Localization/{languageCode}");
            _dict = JsonUtility.FromJson<LocalizationDict>(json.text).ToDictionary();
        }

        public static string Translate(string key)
        {
            if (_dict != null && _dict.TryGetValue(key, out var val))
                return val;
            return key; // fallback
        }
    }
}