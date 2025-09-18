#if UNITY_EDITOR
using CHAL.Data;
using CHAL.Systems.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RuneForgeConfig))]
public class RuneForgeConfigEditor : Editor
{
    private List<bool> runeFoldouts = new List<bool>();
    public override void OnInspectorGUI()
    {
        var config = (RuneForgeConfig)target;

        if (config.entries == null)
            config.entries = new List<RuneForgeEntry>();

        var remains = ItemRegistry.Instance.GetAllItemsByType("remains").ToList();
        var runes = ItemRegistry.Instance.GetAllItemsByType("rune").ToList();

        if (remains.Count == 0 || runes.Count == 0)
        {
            EditorGUILayout.HelpBox("Keine Remains oder Runes in der ItemRegistry gefunden!", MessageType.Warning);
        }

        // Iteriere durch alle Forge-Entries
        for (int e = 0; e < config.entries.Count; e++)
        {
            var entry = config.entries[e];

            if (entry.runes == null)
                entry.runes = new List<RuneChance>();

            // Stelle sicher, dass Foldouts-Liste synchron bleibt
            while (runeFoldouts.Count < entry.runes.Count)
                runeFoldouts.Add(true);

            EditorGUILayout.BeginVertical("box");

            // Remain-Dropdown
            int remainIndex = -1;
            if (entry.remain != null)
                remainIndex = remains.IndexOf(entry.remain);

            remainIndex = EditorGUILayout.Popup("Remain",
                remainIndex,
                remains.Select(i => i.itemId).ToArray());

            if (remainIndex >= 0 && remainIndex < remains.Count)
                entry.remain = remains[remainIndex];

            // RuneChances mit Foldouts
            for (int i = 0; i < entry.runes.Count; i++)
            {
                var rc = entry.runes[i];
                if (rc == null) continue;

                runeFoldouts[i] = EditorGUILayout.Foldout(runeFoldouts[i], $"RuneChance {i + 1}", true);

                if (runeFoldouts[i])
                {
                    EditorGUI.indentLevel++;

                    // Rune Dropdown
                    int runeIndex = -1;
                    if (rc.rune != null)
                        runeIndex = runes.IndexOf(rc.rune);

                    runeIndex = EditorGUILayout.Popup("Rune",
                        runeIndex,
                        runes.Select(r => r.itemId).ToArray());

                    if (runeIndex >= 0 && runeIndex < runes.Count)
                        rc.rune = runes[runeIndex];

                    // Weight Slider
                    rc.weight = EditorGUILayout.Slider("Weight", rc.weight, 0f, 1f);

                    // Entfernen-Button
                    if (GUILayout.Button("Remove Rune"))
                    {
                        entry.runes.RemoveAt(i);
                        runeFoldouts.RemoveAt(i);
                        i--;
                        EditorGUI.indentLevel--;
                        continue;
                    }

                    EditorGUI.indentLevel--;
                }
            }

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+ Rune"))
            {
                entry.runes.Add(new RuneChance());
                runeFoldouts.Add(true);
            }
            if (GUILayout.Button("- Remain"))
            {
                config.entries.RemoveAt(e);
                e--;
                break;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("+ Remain"))
        {
            config.entries.Add(new RuneForgeEntry { runes = new List<RuneChance>() });
        }

        if (GUI.changed)
            EditorUtility.SetDirty(config);
    }
}
#endif
