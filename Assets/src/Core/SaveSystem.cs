using CHAL.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace CHAL.Core
{
    public static class SaveSystem
    {
        private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

        public static void Save(PlayerProfile profile)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");

            // XP, Gold, DNA
            sb.AppendLine($"  \"xp\": {profile.XP},");
            sb.AppendLine($"  \"gold\": {profile.GetCurrency("gold")},");
            sb.AppendLine($"  \"crystal\": {profile.GetCurrency("crystal")},");

            // MapProgress
            sb.AppendLine("  \"map_progress\":[");
            var keys = new List<int>(profile.MapProgress.Keys);
            keys.Sort();
            for (int i = 0; i < keys.Count; i++)
            {
                int map = keys[i];
                int diff = profile.MapProgress[map];
                string comma = (i < keys.Count - 1) ? "," : "";
                sb.AppendLine($"    {{ \"{map}\": \"{diff}\" }}{comma}");
            }

            sb.AppendLine("  ],");

            // Inventories
            WriteInventory(sb, "remains", profile.Remains.ToDictionary());
            sb.AppendLine(",");
            WriteInventory(sb, "parts", profile.Parts.ToDictionary());
            sb.AppendLine(",");
            WriteInventory(sb, "runes", profile.Runes.ToDictionary());
            sb.AppendLine(",");
            WriteInventory(sb, "modules", profile.Modules.ToDictionary());


            //last entry 
            profile.LastSaveTime = DateTime.Now;
            string lastSaveStr = profile.LastSaveTime.ToString("o");
            // LastSaveTime
            sb.AppendLine($"  \"last_savetime\": \"{lastSaveStr}\"");

            sb.AppendLine("\n}");


            File.WriteAllText(SavePath, sb.ToString());
            Debug.Log($"Save written: {SavePath}");
        }

        private static void WriteInventory(StringBuilder sb, string name, Dictionary<string, int> dict)
        {
            sb.AppendLine($"  \"{name}\": [");
            int c = 0;
            foreach (var kv in dict)
            {
                c++;
                string comma = (c < dict.Count) ? "," : "";
                sb.AppendLine($"    {{ \"{kv.Key}\" : {kv.Value} }}{comma}");
            }
            sb.Append("  ]");
        }

        public static PlayerProfile Load()
        {
            if (!File.Exists(SavePath))
                return null;

            string json = File.ReadAllText(SavePath);

            var profile = new PlayerProfile();
            // Sehr rudimentärer Parser: wir extrahieren per String-Suche
            // (für robustere Variante: SimpleJSON oder Newtonsoft nehmen)

            string lst = ExtractString(json, "\"last_savetime\"");
            if (!string.IsNullOrEmpty(lst) && DateTime.TryParse(lst, null, System.Globalization.DateTimeStyles.RoundtripKind, out var dt))
            {
                profile.LastSaveTime = dt;
            }

            profile.XP = ExtractInt(json, "\"xp\"");
            profile.AddCurrency("gold", ExtractInt(json, "\"gold\""));
            profile.AddCurrency("crystal", ExtractInt(json, "\"crystal\""));

            profile.MapProgress = ExtractMapProgress(json);
            profile.Remains.FromDictionary(ExtractInventory(json, "remains"));
            profile.Parts.FromDictionary(ExtractInventory(json, "parts"));
            profile.Runes.FromDictionary(ExtractInventory(json, "runes"));
            profile.Modules.FromDictionary(ExtractInventory(json, "modules"));

            Debug.Log($"Save loaded: {SavePath}");
            return profile;
        }

        private static int ExtractInt(string json, string key)
        {
            int idx = json.IndexOf(key);
            if (idx < 0) return 0;
            int colon = json.IndexOf(":", idx) + 1;
            int comma = json.IndexOfAny(new char[] { ',', '\n' }, colon);
            string num = json.Substring(colon, comma - colon).Trim();
            int.TryParse(num, out int result);
            return result;
        }

        private static string ExtractString(string json, string key)
        {
            int idx = json.IndexOf(key);
            if (idx < 0) return null;
            int colon = json.IndexOf(":", idx) + 1;
            int quote1 = json.IndexOf("\"", colon);
            if (quote1 < 0) return null;
            int quote2 = json.IndexOf("\"", quote1 + 1);
            if (quote2 < 0) return null;
            return json.Substring(quote1 + 1, quote2 - quote1 - 1);
        }

        private static Dictionary<int, int> ExtractMapProgress(string json)
        {
            var result = new Dictionary<int, int>();

            int start = json.IndexOf("\"map_progress\"");
            if (start < 0) return result;

            int arrayStart = json.IndexOf("[", start);
            int arrayEnd = json.IndexOf("]", arrayStart);
            if (arrayStart < 0 || arrayEnd < 0) return result;

            string inner = json.Substring(arrayStart, arrayEnd - arrayStart);

            // Matches: { "1": "3" } oder { "1": 3 }
            var rx = new System.Text.RegularExpressions.Regex(@"\{\s*""(\d+)""\s*:\s*""?(\d+)""?\s*\}");
            var matches = rx.Matches(inner);
            foreach (System.Text.RegularExpressions.Match m in matches)
            {
                if (int.TryParse(m.Groups[1].Value, out var map) &&
                    int.TryParse(m.Groups[2].Value, out var diff))
                {
                    result[map] = diff;
                }
            }
            return result;
        }

        private static Dictionary<string, int> ExtractInventory(string json, string key)
        {
            var result = new Dictionary<string, int>();
            int start = json.IndexOf($"\"{key}\"");
            if (start < 0) return result;
            int arrayStart = json.IndexOf("[", start);
            int arrayEnd = json.IndexOf("]", arrayStart);
            string inner = json.Substring(arrayStart, arrayEnd - arrayStart);

            string[] entries = inner.Split(new[] { '{', '}', ',', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var e in entries)
            {
                var line = e.Trim();
                if (line.StartsWith("\""))
                {
                    var parts = line.Replace("\"", "").Split(':');
                    if (parts.Length == 2)
                    {
                        string id = parts[0].Trim();
                        int.TryParse(parts[1], out int count);
                        result[id.Replace("\"", "").Trim()] = count;
                    }
                }
            }
            return result;
        }
    }
}