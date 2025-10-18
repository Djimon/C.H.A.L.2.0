#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
// Annahme: DebugManager liegt unter CHAL.Core; ggf. Namespace anpassen.
using CHAL.Core;

namespace CHAL.Data
{
    public static class ResearchValidator
    {
        [MenuItem("Tools/CHAL/Validate Research Data")]
        public static void ValidateAll()
        {
            int errorCount = 0;
            int warnCount = 0;

            void LogError(string msg, UnityEngine.Object ctx = null)
            {
                errorCount++;
                // DebugManager.Log(string msg, EDebugLevel level = Debug, string tag = "System", LogType logType = Log, Color? customColor = null);
                DebugManager.Log(msg, DebugManager.EDebugLevel.Dev, "Research", LogType.Error);
                if (ctx != null) EditorGUIUtility.PingObject(ctx);
            }

            void LogWarning(string msg, UnityEngine.Object ctx = null)
            {
                warnCount++;
                DebugManager.Log(msg, DebugManager.EDebugLevel.Dev, "Research", LogType.Warning);
                if (ctx != null) EditorGUIUtility.PingObject(ctx);
            }

            void LogInfo(string msg)
            {
                DebugManager.Log(msg, DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
            }

            // --- Tree laden ---
            var treeGuids = AssetDatabase.FindAssets("t:ResearchTreeDef");
            ResearchTreeDef tree = null;
            if (treeGuids.Length == 0)
            {
                LogError("Kein ResearchTreeDef gefunden. Lege ein Asset über Create ▸ Research ▸ Tree an.");
            }
            else
            {
                if (treeGuids.Length > 1)
                    LogWarning($"Es wurden {treeGuids.Length} ResearchTreeDef-Assets gefunden. Es wird nur das erste geprüft.");

                var path = AssetDatabase.GUIDToAssetPath(treeGuids[0]);
                tree = AssetDatabase.LoadAssetAtPath<ResearchTreeDef>(path);
                if (tree == null)
                    LogError("Das gefundene ResearchTreeDef konnte nicht geladen werden.");
            }

            // --- Nodes laden ---
            var nodeGuids = AssetDatabase.FindAssets("t:ResearchNodeDef");
            var nodes = new List<ResearchNodeDef>(nodeGuids.Length);
            foreach (var g in nodeGuids)
            {
                var p = AssetDatabase.GUIDToAssetPath(g);
                var def = AssetDatabase.LoadAssetAtPath<ResearchNodeDef>(p);
                if (def != null) nodes.Add(def);
            }
            if (nodes.Count == 0)
                LogWarning("Keine ResearchNodeDef-Assets gefunden.");

            // --- Tree-Struktur-Checks ---
            int laneCount = tree != null ? tree.researchLanes.Count : 0;
            if (tree != null)
            {
                if (laneCount <= 0)
                    LogError("ResearchTreeDef: researchLanes ist leer. Mindestens 1 Lane erforderlich.", tree);

                int baseCount = tree.laneBaseX != null ? tree.laneBaseX.Count : 0;
                if (baseCount != laneCount)
                    LogError($"ResearchTreeDef: laneBaseX.Count ({baseCount}) != researchLanes.Count ({laneCount}).", tree);

                for (int i = 0; i < laneCount; i++)
                {
                    if (string.IsNullOrWhiteSpace(tree.researchLanes[i].laneName))
                        LogWarning($"ResearchTreeDef: Lane {i} hat einen leeren Namen.", tree);
                }

                if (tree.nodeWidth <= 0 || tree.nodeHeight <= 0 || tree.stageStepY <= 0)
                    LogError("ResearchTreeDef: nodeWidth/nodeHeight/stageStepY müssen > 0 sein.", tree);
            }

            // --- Node-Checks ---
            var byId = new Dictionary<string, ResearchNodeDef>(StringComparer.Ordinal);
            foreach (var n in nodes)
            {
                // ID
                if (string.IsNullOrWhiteSpace(n.id))
                {
                    LogError($"Node '{n.name}' hat leere ID.", n);
                }
                else if (byId.ContainsKey(n.id))
                {
                    LogError($"Doppelte Node-ID '{n.id}' in '{byId[n.id].name}' und '{n.name}'.", n);
                }
                else
                {
                    byId.Add(n.id, n);
                }

                // Lane bounds
                if (tree != null)
                {
                    if (n.lane < 0 || n.lane >= laneCount)
                        LogError($"Node '{n.id}': lane {n.lane} ist außerhalb 0..{laneCount - 1}.", n);
                }

                // Stage
                if (n.stage < 0)
                    LogError($"Node '{n.id}': stage < 0 ist ungültig (nutze 10er-Raster: 10,20,...).", n);

                // Unlock-Mapping
                //TODO

                // Requirements basic
                if (n.requirements == null || n.requirements.IsEmpty())
                    LogWarning($"Node '{n.id}': Requirements sind leer (V1 erlaubt – prüfen, ob gewünscht).", n);

                // Parents Existenz & Stage-Ordnung
                if (n.parents != null)
                {
                    foreach (var pid in n.parents)
                    {
                        if (string.IsNullOrWhiteSpace(pid))
                        {
                            LogWarning($"Node '{n.id}': leerer Parent-Eintrag.", n);
                            continue;
                        }
                        if (!byId.TryGetValue(pid, out var p))
                        {
                            LogError($"Node '{n.id}': unbekannter Parent '{pid}'.", n);
                            continue;
                        }
                        if (p.stage >= n.stage)
                            LogError($"Stage-Ordnung verletzt: Parent '{p.id}' (stage {p.stage}) ≥ Child '{n.id}' (stage {n.stage}).", n);
                    }
                }
            }

            // --- Zyklus-Check (DFS) ---
            if (byId.Count > 0)
            {
                bool HasCycle()
                {
                    var visited = new HashSet<string>();
                    var stack = new HashSet<string>();

                    bool Dfs(string id)
                    {
                        if (!visited.Add(id)) return false;
                        stack.Add(id);

                        var node = byId[id];
                        if (node.parents != null)
                        {
                            foreach (var pid in node.parents)
                            {
                                if (!byId.ContainsKey(pid)) continue; // bereits als Fehler markiert
                                if (!visited.Contains(pid) && Dfs(pid)) return true;
                                if (stack.Contains(pid)) return true;
                            }
                        }

                        stack.Remove(id);
                        return false;
                    }

                    foreach (var id in byId.Keys)
                        if (Dfs(id)) return true;
                    return false;
                }

                if (HasCycle())
                    LogError("Zyklische Abhängigkeit gefunden (Parents müssen azyklisch sein).");
            }

            // --- Lesbarkeit: Kollisionen pro (lane,stage) ---
            var groups = nodes.GroupBy(n => (n.lane, n.stage))
                              .OrderBy(g => g.Key.lane)
                              .ThenBy(g => g.Key.stage);
            foreach (var g in groups)
            {
                var list = g.OrderBy(n => n.id, StringComparer.Ordinal).ToList();
                if (list.Count > 6) // Richtwert für UI-Lesbarkeit
                    LogWarning($"Viele Nodes in (lane {g.Key.lane}, stage {g.Key.stage}) → {list.Count}. Lesbarkeit prüfen.", list[0]);
            }

            // --- Zusammenfassung ---
            LogInfo($"Research Validation abgeschlossen. Errors: {errorCount}, Warnings: {warnCount}, Nodes geprüft: {nodes.Count}.");
        }
    }
}
#endif
