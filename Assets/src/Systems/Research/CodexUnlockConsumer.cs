using CHAL.Core;
using CHAL.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Systems.Codex
{
    /// <summary>
    /// Übersetzt Unlocks in echte Gameplay-Effekte.
    /// Registry bleibt Registry (Flags), Consumer macht die Auswirkungen.
    /// </summary>
    public sealed class CodexUnlockConsumer
    {
        private readonly CodexService _codexService;

        public CodexUnlockConsumer(CodexService codexService)
        {
            _codexService = codexService ?? throw new ArgumentNullException(nameof(codexService));
        }

        public void Apply(string deedId, IReadOnlyList<CodexUnlock> unlocks)
        {
            if (unlocks == null || unlocks.Count == 0)
                return;

            // 2) Gameplay Effekte
            for (int i = 0; i < unlocks.Count; i++)
                ApplyOne(unlocks[i]);

            DebugManager.DevLog($"Deed '{deedId}' unlocked {unlocks.Count} features.", "Codex");
        }

        private void ApplyOne(CodexUnlock u)
        {
            switch (u.unlockType)
            {
                case CodexUnlockTypes.CodexSlots:
                    ApplyCodexSlots(u);
                    break;

                // Weitere UnlockTypes später hier ergänzen.
                default:
                    break;
            }
        }

        private void ApplyCodexSlots(CodexUnlock u)
        {
            int amount = ParseAmountOrDefault(u.targetId, 1);

            for (int k = 0; k < amount; k++)
            {
                if (!_codexService.TryUnlockNextFocusSlot(out var reason))
                {
                    DebugManager.Log($"CodexSlots unlock blocked: {reason}",
                        DebugManager.EDebugLevel.Dev, "Research", LogType.Log);
                    break;
                }
            }
        }

        private static int ParseAmountOrDefault(string s, int def)
        {
            if (string.IsNullOrWhiteSpace(s)) return def;
            if (!int.TryParse(s, out var v)) return def;
            return Mathf.Max(1, v);
        }
    }
}
