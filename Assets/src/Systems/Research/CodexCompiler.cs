using CHAL.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Research
{
    public sealed class CompiledCodex
    {
        public readonly Dictionary<string, CodexDeedDef> nodesById;
        public readonly Dictionary<string, (int lane, int stage)> posById;
        public readonly Dictionary<string, List<string>> parentsById;

        public CompiledCodex(
            Dictionary<string, CodexDeedDef> nodesById,
            Dictionary<string, (int lane, int stage)> posById,
            Dictionary<string, List<string>> parentsById)
        {
            this.nodesById = nodesById;
            this.posById = posById;
            this.parentsById = parentsById;
        }
    }

    public static class CodexCompiler
    {
/// <summary>
/// Compiles a research tree definition into a compiled research tree.
/// </summary>
/// <param name="tree">The research tree definition to compile.</param>
/// <returns>A compiled representation of the research tree.</returns>
        public static CompiledCodex Compile(CodexDef tree)
        {
            var nodesById = new Dictionary<string, CodexDeedDef>(StringComparer.Ordinal);
            var posById = new Dictionary<string, (int lane, int stage)>(StringComparer.Ordinal);
            var parentsById = new Dictionary<string, List<string>>(StringComparer.Ordinal);

            if (tree == null)
            {
                DebugManager.Log("ResearchTreeCompiler: tree=null", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Error);
                return new CompiledCodex(nodesById, posById, parentsById);
            }

            var lanes = tree.codexChapters; // <- WICHTIG: der echte Baum!
            if (lanes == null || lanes.Count == 0)
            {
                DebugManager.Log("ResearchTreeCompiler: researchTreeLanes ist leer.", DebugManager.EDebugLevel.Dev, "Research", LogType.Warning);
                return new CompiledCodex(nodesById, posById, parentsById);
            }

            for (int lane = 0; lane < lanes.Count; lane++)
            {
                var laneDef = lanes[lane];
                if (laneDef?.stages == null) continue;

                for (int stage = 0; stage < laneDef.stages.Count; stage++)
                {
                    var stageRef = laneDef.stages[stage];
                    if (stageRef?.deedSlots == null) continue;

                    foreach (var entry in stageRef.deedSlots)
                    {
                        if (entry?.deed == null || string.IsNullOrWhiteSpace(entry.deed.id))
                            continue;

                        var id = entry.deed.id;

                        if (nodesById.ContainsKey(id))
                        {
                            DebugManager.Log($"ResearchTreeCompiler: doppelte Node-ID '{id}' im Tree.", DebugManager.EDebugLevel.Dev, "Research", LogType.Error);
                            continue;
                        }

                        nodesById[id] = entry.deed;
                        posById[id] = (lane, stage);

                        var plist = new List<string>();

                        parentsById[id] = plist;
                    }
                }
            }

            // AzyklizitÃ¤t / Stage-Ordnung validieren
            foreach (var kv in parentsById)
            {
                var id = kv.Key;
                var (lane, stage) = posById[id];

                foreach (var pid in kv.Value)
                {
                    if (!posById.TryGetValue(pid, out var ppos))
                    {
                        DebugManager.Log($"ResearchTreeCompiler: Parent '{pid}' von '{id}' fehlt im Tree.", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Error);
                        continue;
                    }
                    if (ppos.stage >= stage)
                    {
                        DebugManager.Log($"ResearchTreeCompiler: Stage-Ordnung verletzt: Parent '{pid}' (stage {ppos.stage}) â‰¥ Child '{id}' (stage {stage}).",
                            DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Error);
                    }
                }
            }

            // Zyklus-Check (DFS)
            if (HasCycle(parentsById))
                DebugManager.Log("ResearchTreeCompiler: Zyklische AbhÃ¤ngigkeit im Tree.", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Error);

            return new CompiledCodex(nodesById, posById, parentsById);
        }

        private static bool HasCycle(Dictionary<string, List<string>> parentsById)
        {
            var visited = new HashSet<string>();
            var stack = new HashSet<string>();
            bool Dfs(string id)
            {
                if (!visited.Add(id)) return false;
                stack.Add(id);
                if (parentsById.TryGetValue(id, out var ps))
                {
                    foreach (var pid in ps)
                    {
                        if (!visited.Contains(pid) && Dfs(pid)) return true;
                        if (stack.Contains(pid)) return true;
                    }
                }
                stack.Remove(id);
                return false;
            }

            foreach (var id in parentsById.Keys)
                if (Dfs(id)) return true;
            return false;
        }
    }
}
