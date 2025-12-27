// File: Assets/src/CHAL/Systems/Gear/GearRoller.cs
using CHAL.Data;
using System;
using System.Collections.Generic;

namespace CHAL.Systems.Items
{
    public sealed class GearRoller
    {
        private readonly GameBalanceConfig _balance;
        private readonly ImplicitRegistry _implicits;

        // Reuse buffers (avoid allocs)
        private readonly List<ImplicitDef> _candidates = new(32);

        public GearRoller(GameBalanceConfig balance, ImplicitRegistry implicits)
        {
            _balance = balance;
            _implicits = implicits;
        }

        public List<ImplicitRoll> RollImplicits(
            GearType gearType,
            ArmorClass armorClass,
            GearBaseTier baseTier,
            System.Random rng,
            List<ImplicitRoll> outRolls = null)
        {
            outRolls ??= new List<ImplicitRoll>(3);
            outRolls.Clear();

            if (_balance == null || _implicits == null)
            {
                DebugManager.Error("[GearRoller] Missing balance or implicits registry.", "System");
                return outRolls;
            }

            var caps = _balance.gear.slotCapsByTier.GetCaps(baseTier);
            var maxImplicits = caps.maxImplicits;
            if (maxImplicits <= 0) return outRolls;

            var mainPool = GetMainPool(armorClass);
            GetOtherPools(mainPool, out var pool2, out var pool3);

            var slotsToRoll = Math.Min(maxImplicits, 3);

            for (int slot = 0; slot < slotsToRoll; slot++)
            {
                var pw = slot == 0 ? _balance.gear.slot1PoolWeights :
                         slot == 1 ? _balance.gear.slot2PoolWeights : _balance.gear.slot3PoolWeights;
                pw.Normalize();

                var pool = PickPoolMixed(pw, mainPool, pool2, pool3, rng);
                var roleW = GetRoleWeights(gearType, signature: slot == 0);
                var role = PickRole(roleW, rng);

                var roll = RollOneImplicit(
                    gearType, pool, role, baseTier,
                    slotIndex: slot, rng: rng,
                    excludeIds: outRolls);

                if (roll.HasValue)
                    outRolls.Add(roll.Value);
            }

            return outRolls;
        }

        private ImplicitRoll? RollOneImplicit(GearType gearType, ImplicitPool pool, ImplicitRole role, GearBaseTier baseTier, int slotIndex, Random rng, List<ImplicitRoll> excludeIds)
        {
            _implicits.GetCandidates(pool, role, gearType, _candidates);
            if (_candidates.Count == 0) return null;

            // Remove already-used implicit IDs (avoid duplicates on the same item)
            for (int i = _candidates.Count - 1; i >= 0; i--)
            {
                var cand = _candidates[i];
                if (cand == null) { _candidates.RemoveAt(i); continue; }

                var id = cand.Id;
                if (string.IsNullOrEmpty(id)) { _candidates.RemoveAt(i); continue; }

                if (ContainsImplicit(excludeIds, id))
                    _candidates.RemoveAt(i);
            }

            if (_candidates.Count == 0) return null;

            var picked = PickImplicitWeighted(_candidates, rng);
            if (picked == null) return null;

            var val = RollValue(picked, baseTier, rng);

            return new ImplicitRoll(
                implicitId: picked.Id,
                value: val,
                slotIndex: slotIndex,
                rolledFromTier: baseTier
            );
        }

        private static bool ContainsImplicit(List<ImplicitRoll> list, string id)
        {
            for (int i = 0; i < list.Count; i++)
                if (string.Equals(list[i].implicitId, id, StringComparison.Ordinal))
                    return true;
            return false;
        }

        private static ImplicitDef PickImplicitWeighted(List<ImplicitDef> cands, System.Random rng)
        {
            // Equal-weight by default because ImplicitDef.Weight defaults to 1.
            double sum = 0;
            for (int i = 0; i < cands.Count; i++)
                sum += Math.Max(0.0, cands[i].customWeight);

            if (sum <= 0.00001)
            {
                // fallback: uniform
                return cands[rng.Next(0, cands.Count)];
            }

            var r = rng.NextDouble() * sum;
            double acc = 0;
            for (int i = 0; i < cands.Count; i++)
            {
                acc += Math.Max(0.0, cands[i].customWeight);
                if (r <= acc)
                    return cands[i];
            }

            return cands[cands.Count - 1];
        }

        private static float RollValue(ImplicitDef def, GearBaseTier baseTier, System.Random rng)
        {
            var range = baseTier switch
            {
                GearBaseTier.T1 => def.Ranges.Tier1,
                GearBaseTier.T2 => def.Ranges.Tier2,
                GearBaseTier.T3 => def.Ranges.Tier3,
                _ => def.Ranges.Tier1
            };

            var u = (float)rng.NextDouble();
            return range.Min + (range.Max - range.Min) * u;
        }

        private GameBalanceConfig.GearRoleWeights GetRoleWeights(GearType gearType, bool signature)
        {
            // Lookup in balance table; fallback: mild Defense bias
            var list = _balance.gear.roleWeightsByGearType;
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].gearType != gearType) continue;
                    return signature ? list[i].signature : list[i].normal;
                }
            }

            return new GameBalanceConfig.GearRoleWeights { defense = 50, offense = 35, utility = 15 };
        }

        private static ImplicitRole PickRole(GameBalanceConfig.GearRoleWeights w, System.Random rng)
        {
            var d = Math.Max(0, w.defense);
            var o = Math.Max(0, w.offense);
            var u = Math.Max(0, w.utility);
            var sum = d + o + u;
            if (sum <= 0) return ImplicitRole.Defense;

            var r = rng.Next(0, sum);
            if (r < d) return ImplicitRole.Defense;
            r -= d;
            if (r < o) return ImplicitRole.Offense;
            return ImplicitRole.Utility;
        }

        private static ImplicitPool PickPoolMixed(
            GameBalanceConfig.SlotPoolWeights w,
            ImplicitPool main,
            ImplicitPool pool2,
            ImplicitPool pool3,
            System.Random rng)
        {
            var r = rng.NextDouble();
            var acc = 0.0;

            acc += w.main;
            if (r < acc) return main;

            acc += w.neutral;
            if (r < acc) return ImplicitPool.Neutral;

            acc += w.pool2;
            if (r < acc) return pool2;

            return pool3;
        }

        private static ImplicitPool GetMainPool(ArmorClass armorClass)
        {
            return armorClass switch
            {
                ArmorClass.Heavy => ImplicitPool.Melee,
                ArmorClass.Medium => ImplicitPool.Ranged,
                ArmorClass.Light => ImplicitPool.Caster,
                _ => ImplicitPool.Melee
            };
        }

        private static void GetOtherPools(ImplicitPool main, out ImplicitPool pool2, out ImplicitPool pool3)
        {
            // "the other two" besides main
            if (main == ImplicitPool.Melee) { pool2 = ImplicitPool.Ranged; pool3 = ImplicitPool.Caster; return; }
            if (main == ImplicitPool.Ranged) { pool2 = ImplicitPool.Melee; pool3 = ImplicitPool.Caster; return; }
            pool2 = ImplicitPool.Melee; pool3 = ImplicitPool.Ranged;
        }
    }
}
