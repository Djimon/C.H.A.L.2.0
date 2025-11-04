using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Localization
{
    public static class LocalizationManager
    {
        private static Dictionary<string, string> _dict;

/// <summary>
/// Loads localization data for the specified language code.
/// This data is used for translating keys into their corresponding values.
/// </summary>
/// <param name="languageCode">The code of the language to load.</param>
        public static void Load(string languageCode)
        {
            // JSON laden: { "Enemy_InsectSwarm_Name": "Insekten-Schwarm", ... }
            TextAsset json = Resources.Load<TextAsset>($"Localization/{languageCode}");
            _dict = JsonUtility.FromJson<LocalizationDict>(json.text).ToDictionary();
        }

/// <summary>
/// Translates the given key into its corresponding value.
/// If the key is not found, it returns the key itself as a fallback.
/// </summary>
/// <param name="key">The key to be translated.</param>
/// <returns>The translated value or the original key if not found.</returns>
        public static string Translate(string key)
        {
            if (_dict != null && _dict.TryGetValue(key, out var val))
                return val;
            return key; // fallback
        }
    }
}
