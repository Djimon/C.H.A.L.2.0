using System;
using System.Collections.Generic;
using CHAL.Data;

namespace CHAL.Systems.Research
{
    public sealed class ResearchTreeCompiled
    {
        public readonly Dictionary<string, ResearchNodeDef> nodesById;
        public readonly Dictionary<string, (int lane, int stage)> posById;
        public readonly Dictionary<string, List<string>> parentsById;

        public ResearchTreeCompiled(
            Dictionary<string, ResearchNodeDef> nodesById,
            Dictionary<string, (int lane, int stage)> posById,
            Dictionary<string, List<string>> parentsById)
        {
            this.nodesById = nodesById;
            this.posById = posById;
            this.parentsById = parentsById;
        }
    }

    public static class ResearchTreeCompiler
    {
        public static ResearchTreeCompiled Compile(ResearchTreeDef tree)
        {
            var nodesById = new Dictionary<string, ResearchNodeDef>(StringComparer.Ordinal);
            var posById = new Dictionary<string, (int lane, int stage)>(StringComparer.Ordinal);
            var parentsById = new Dictionary<string, List<string>>(StringComparer.Ordinal);

            if (tree == null)
            {
                DebugManager.Log("ResearchTreeCompiler: tree=null", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Error);
                return new ResearchTreeCompiled(nodesById, posById, parentsById);
            }

            // Lane-Schleife
            for (int lane = 0; lane < tree.researchLanes.Count; lane++)
            {
                // Falls du ResearchTreeLane statt ResearchLane nutzt:
                var tl = tree.researchLanes[lane] as dynamic; // nur zur Illustration
                var stages = (tl is ResearchTreeLane rtl) ? rtl.stages : null;
                if (stages == null) continue;

                for (int stage = 0; stage < stages.Count; stage++)
                {
                    var stageRef = stages[stage];
                    if (stageRef?.nodes == null) continue;

                    foreach (var entry in stageRef.nodes)
                    {
                        if (entry?.node == null || string.IsNullOrWhiteSpace(entry.node.id))
                            continue;

                        var id = entry.node.id;

                        if (nodesById.ContainsKey(id))
                        {
                            DebugManager.Log($"ResearchTreeCompiler: doppelte Node-ID '{id}' im Tree.", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Error);
                            continue;
                        }

                        nodesById[id] = entry.node;
                        posById[id] = (lane, stage);

                        // Parents
                        var plist = new List<string>();
                        if (entry.parentRefs != null)
                        {
                            foreach (var pref in entry.parentRefs)
                            {
                                if (pref == null || string.IsNullOrWhiteSpace(pref.id)) continue;
                                plist.Add(pref.id);
                            }
                        }
                        parentsById[id] = plist;
                    }
                }
            }

            // Azyklizität / Stage-Ordnung validieren
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
                        DebugManager.Log($"ResearchTreeCompiler: Stage-Ordnung verletzt: Parent '{pid}' (stage {ppos.stage}) ≥ Child '{id}' (stage {stage}).",
                            DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Error);
                    }
                }
            }

            // Zyklus-Check (DFS)
            if (HasCycle(parentsById))
                DebugManager.Log("ResearchTreeCompiler: Zyklische Abhängigkeit im Tree.", DebugManager.EDebugLevel.Dev, "Research", UnityEngine.LogType.Error);

            return new ResearchTreeCompiled(nodesById, posById, parentsById);
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
