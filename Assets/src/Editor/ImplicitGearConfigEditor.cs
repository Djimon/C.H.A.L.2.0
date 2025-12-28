#if UNITY_EDITOR
using CHAL.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[Obsolete("LEGACY Editor: kept for reference. New system uses BalanceConfig + ImplicitRegistryDef.", false)]
[CustomEditor(typeof(ImplicitGearTypeConfig))]
/// <summary>
/// Provides a custom editor for the implicit gear type configuration.
/// </summary>
public class ImplicitGearTypeConfigEditor : UnityEditor.Editor
{
    private string pastedGrid = "";
    private string status = "";

    // feste Gear-Spaltenreihenfolge fÃ¼r Templates/Export
    private static readonly GearType[] TemplateGears =
    {
        GearType.Head, GearType.Chest, GearType.Gloves,
        GearType.Legs, GearType.Boots, GearType.Amulet
    };

/// <summary>
/// Draws the custom inspector GUI for the component.
/// </summary>
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(
            "LEGACY CONFIG (Deprecated)\n" +
            "This asset is no longer used by runtime logic.\n" +
            "New system: BalanceConfig role weights + slot pool weights + ImplicitRegistryDef/ImplicitDef.\n" +
            "Kept only to reuse the old matrix editor workflow as reference.",
            MessageType.Warning);

        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Paste Grid â†’ Apply", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "Input GearType-Implicit-Matrix - Format (TSV):\n" +
            "ImplicitID | Head | Chest | Gloves | Legs | Boots | Amulet\n" +
            "Values: Integer (Weights). default = 0.",
            MessageType.None);

        EditorGUILayout.LabelField("Templates (Copy to Clipboard)", EditorStyles.boldLabel);
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Copy blank template"))
                CopyBlankTemplateToClipboard((ImplicitGearTypeConfig)target);

            if (GUILayout.Button("Copy template from asset"))
                CopyFromAssetToClipboard((ImplicitGearTypeConfig)target);
        }

        EditorGUILayout.Space(6);

        pastedGrid = EditorGUILayout.TextArea(pastedGrid, GUILayout.MinHeight(140));

        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Parse & Apply"))
                ApplyFromGrid((ImplicitGearTypeConfig)target, pastedGrid);

            if (GUILayout.Button("Clear"))
            {
                pastedGrid = "";
                status = "";
            }
        }


        if (!string.IsNullOrEmpty(status))
            EditorGUILayout.HelpBox(status, MessageType.Info);
    }

    // ===================== Templates =====================

    private void CopyBlankTemplateToClipboard(ImplicitGearTypeConfig asset)
    {
        var ids = GatherIdsFromAsset(asset);
        var header = "ID\t" + string.Join("\t", TemplateGears.Select(g => g.ToString()));
        var rows = new List<string> { header };

        if (ids.Count == 0)
        {
            // Kein IDs im Asset â†’ nur Header kopieren (du kannst die ID-Spalte manuell befÃ¼llen)
            status = "no IDs found in the Asset, no copy.";
            EditorGUIUtility.systemCopyBuffer = string.Join("\n", rows);
            return;
        }

        foreach (var id in ids)
            rows.Add(id + "\t" + string.Join("\t", Enumerable.Repeat("0", TemplateGears.Length)));

        EditorGUIUtility.systemCopyBuffer = string.Join("\n", rows);
        status = $"Copied blank template (0-values) to Clipboard. rows: {ids.Count}.";
    }

    private void CopyFromAssetToClipboard(ImplicitGearTypeConfig asset)
    {
        if (asset == null) { status = "Kein Asset ausgewÃ¤hlt."; return; }
        EnsureAllGearTypesExist(asset);

        // id -> gear -> weight (aus den Pools)
        var weights = BuildWeightMap(asset);

        // union: alle IDs aus Asset (= Gather) + evtl. IDs, die nur in einzelnen Pools existieren
        var ids = GatherIdsFromAsset(asset);
        foreach (var id in weights.Keys)
            ids.Add(id); // HashSet-Effekt via GatherIdsFromAsset? Wir nutzen hier Liste -> distinct gleich unten

        var orderedIds = ids.Distinct(StringComparer.Ordinal).OrderBy(x => x, StringComparer.Ordinal).ToList();

        var header = "ID\t" + string.Join("\t", TemplateGears.Select(g => g.ToString()));
        var rows = new List<string> { header };

        foreach (var id in orderedIds)
            rows.Add(BuildLineForId(id, weights));

        EditorGUIUtility.systemCopyBuffer = string.Join("\n", rows);
        status = $"copied template (from Asset) to Clipboard. rows: {orderedIds.Count}.";
    }

    private static string BuildLineForId(string id, Dictionary<string, Dictionary<GearType, int>> weights)
    {
        var vals = new List<string>(TemplateGears.Length);
        if (weights.TryGetValue(id, out var perGear))
        {
            foreach (var g in TemplateGears)
                vals.Add(perGear != null && perGear.TryGetValue(g, out var v) ? v.ToString() : "0");
        }
        else
        {
            for (int i = 0; i < TemplateGears.Length; i++) vals.Add("0");
        }
        return id + "\t" + string.Join("\t", vals);
    }

    // ===================== Apply =====================

    private void ApplyFromGrid(ImplicitGearTypeConfig asset, string text)
    {
        try
        {
            var (headers, rows) = ParseTSV(text);
            var colToGear = MapColumnsToGearTypes(headers);

            if (colToGear.Count == 0)
                throw new Exception("Keine gÃ¼ltigen GearType-Header gefunden. Erwartet: Head, Chest, Gloves, Legs, Boots, Amulet.");

            // ID -> (GearType -> Weight)
            var perIdWeights = new Dictionary<string, Dictionary<GearType, int>>(StringComparer.Ordinal);

            foreach (var row in rows)
            {
                if (row.Length == 0) continue;

                string id = Safe(row, 0).Trim();
                if (string.IsNullOrEmpty(id)) continue;

                if (!perIdWeights.TryGetValue(id, out var dict))
                    perIdWeights[id] = dict = new Dictionary<GearType, int>();

                for (int c = 1; c < row.Length && c < headers.Length; c++) // Gewichte ab Spalte 1
                {
                    if (!colToGear.TryGetValue(c, out var gt)) continue;
                    if (!int.TryParse(Safe(row, c), out int val)) val = 0;
                    dict[gt] = Mathf.Max(0, val);
                }
            }

            Undo.RecordObject(asset, "Apply Implicit Weights");
            EnsureAllGearTypesExist(asset);

            foreach (var kv in perIdWeights)
            {
                string id = kv.Key;

                foreach (var pair in kv.Value)
                {
                    var gt = pair.Key;
                    int w = pair.Value;

                    int poolIdx = asset.Pools.FindIndex(p => p.GearType == gt);
                    if (poolIdx < 0)
                    {
                        asset.Pools.Add(new GearTypePool { GearType = gt, Entries = new List<ImplicitWeight>() });
                        poolIdx = asset.Pools.Count - 1;
                    }

                    var pool = asset.Pools[poolIdx];
                    if (pool.Entries == null) pool.Entries = new List<ImplicitWeight>();

                    int idx = pool.Entries.FindIndex(e => string.Equals(e.ImplicitId, id, StringComparison.Ordinal));
                    if (idx < 0)
                        pool.Entries.Add(new ImplicitWeight { ImplicitId = id, Weight = w });
                    else
                    {
                        var e = pool.Entries[idx];
                        e.Weight = w;
                        pool.Entries[idx] = e;
                    }

                    asset.Pools[poolIdx] = pool; // struct zurÃ¼ckschreiben
                }
            }

            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            status = $"Applied: {perIdWeights.Count} IDs.";
        }
        catch (Exception ex)
        {
            status = "Error: " + ex.Message;
        }
    }

    // ===================== Helpers =====================

    private static (string[] headers, List<string[]> rows) ParseTSV(string text)
    {
        var lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length == 0) throw new Exception("Keine Daten.");
        string[] headers = lines[0].Split('\t');
        var rows = new List<string[]>();
        for (int i = 1; i < lines.Length; i++) rows.Add(lines[i].Split('\t'));
        return (headers, rows);
    }

    private static string Safe(string[] row, int i) => (i >= 0 && i < row.Length) ? row[i] : "";

    private static string Norm(string s) => (s ?? "").Trim().ToLowerInvariant();

    private static Dictionary<int, GearType> MapColumnsToGearTypes(string[] headers)
    {
        var map = new Dictionary<int, GearType>();
        for (int i = 0; i < headers.Length; i++)
        {
            switch (Norm(headers[i]))
            {
                case "gloves": map[i] = GearType.Gloves; break;
                case "head": map[i] = GearType.Head; break;
                case "legs": map[i] = GearType.Legs; break;
                case "chest": map[i] = GearType.Chest; break;
                case "boots": map[i] = GearType.Boots; break;
                case "amulet": map[i] = GearType.Amulet; break;
            }
        }
        return map;
    }

    private static void EnsureAllGearTypesExist(ImplicitGearTypeConfig asset)
    {
        if (asset.Pools == null) asset.Pools = new List<GearTypePool>();
        var present = new HashSet<GearType>();
        foreach (var p in asset.Pools) present.Add(p.GearType);
        foreach (GearType gt in Enum.GetValues(typeof(GearType)))
            if (!present.Contains(gt))
                asset.Pools.Add(new GearTypePool { GearType = gt, Entries = new List<ImplicitWeight>() });
    }

    private static HashSet<string> GatherIdsFromAsset(ImplicitGearTypeConfig asset)
    {
        var set = new HashSet<string>(StringComparer.Ordinal);
        if (asset?.Pools != null)
        {
            foreach (var pool in asset.Pools)
            {
                if (pool.Entries == null) continue;
                foreach (var e in pool.Entries)
                {
                    if (!string.IsNullOrEmpty(e.ImplicitId))
                        set.Add(e.ImplicitId.Trim());
                }
            }
        }
        return set;
    }

    private static Dictionary<string, Dictionary<GearType, int>> BuildWeightMap(ImplicitGearTypeConfig asset)
    {
        var map = new Dictionary<string, Dictionary<GearType, int>>(StringComparer.Ordinal);
        if (asset?.Pools == null) return map;

        foreach (var pool in asset.Pools)
        {
            if (pool.Entries == null) continue;
            foreach (var e in pool.Entries)
            {
                var id = (e.ImplicitId ?? "").Trim();
                if (string.IsNullOrEmpty(id)) continue;

                if (!map.TryGetValue(id, out var dict))
                    map[id] = dict = new Dictionary<GearType, int>();

                dict[pool.GearType] = Mathf.Max(0, e.Weight);
            }
        }
        return map;
    }
}
#endif
