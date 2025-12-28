// File: Assets/src/CHAL/Systems/Gear/GearRoller.cs
using CHAL.Data;
using System;
using System.Collections.Generic;

namespace CHAL.Systems.Items
{
    public sealed class GearRoller
    {
        private readonly GameBalanceConfig _balance;
        private readonly GearModRegistry _mods;

        // Reuse buffers (avoid allocs)
        private readonly List<ImplicitDef> _implicitCandidates = new(32);
        private readonly List<AffixDef> _affixCandidates = new(64);

        public GearRoller(GameBalanceConfig balance, GearModRegistry mods)
        {
            _balance = balance;
            _mods = mods;
        }

        // ==========================
        // IMPLICITS
        // ==========================
        public List<ImplicitRoll> RollImplicits(
            GearType gearType,
            ArmorClass armorClass,
            GearBaseTier baseTier,
            System.Random rng,
            List<ImplicitRoll> outRolls = null)
        {
            outRolls ??= new List<ImplicitRoll>(3);
            outRolls.Clear();

            if (_balance == null || _mods == null)
            {
                DebugManager.Error("[GearRoller] Missing balance or GearModRegistry.", "System");
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
                    gearType: gearType,
                    pool: pool,
                    role: role,
                    baseTier: baseTier,
                    slotIndex: slot,
                    rng: rng,
                    exclude: outRolls);

                if (roll.HasValue)
                    outRolls.Add(roll.Value);
            }

            return outRolls;
        }

        private ImplicitRoll? RollOneImplicit(
            GearType gearType,
            ImplicitPool pool,
            ImplicitRole role,
            GearBaseTier baseTier,
            int slotIndex,
            Random rng,
            List<ImplicitRoll> exclude)
        {
            _mods.GetImplicitCandidates(gearType: gearType,poolMaskAsInt: (int)pool, roleAsInt: (int)role, buffer: _implicitCandidates);
            if (_implicitCandidates.Count == 0) return null;

            // avoid duplicates on same item
            for (int i = _implicitCandidates.Count - 1; i >= 0; i--)
            {
                var cand = _implicitCandidates[i];
                if (cand == null) { _implicitCandidates.RemoveAt(i); continue; }

                var id = cand.ImplicitId;
                if (string.IsNullOrEmpty(id)) { _implicitCandidates.RemoveAt(i); continue; }

                if (ContainsImplicit(exclude, id))
                    _implicitCandidates.RemoveAt(i);
            }

            if (_implicitCandidates.Count == 0) return null;

            var picked = PickImplicitWeighted(_implicitCandidates, rng);
            if (picked == null) return null;

            var value = RollValue(picked, baseTier, rng);

            return new ImplicitRoll(
                implicitId: picked.ImplicitId,
                value: value,
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
            double sum = 0;
            for (int i = 0; i < cands.Count; i++)
                sum += Math.Max(0.0, cands[i].customWeight);

            if (sum <= 0.00001)
                return cands[rng.Next(0, cands.Count)];

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
            var list = _balance.gear.roleWeightsByGearType;
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].gearType != gearType) continue;
                    return signature ? list[i].signature : list[i].normal;
                }
            }

            // fallback
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
            if (main == ImplicitPool.Melee) { pool2 = ImplicitPool.Ranged; pool3 = ImplicitPool.Caster; return; }
            if (main == ImplicitPool.Ranged) { pool2 = ImplicitPool.Melee; pool3 = ImplicitPool.Caster; return; }
            pool2 = ImplicitPool.Melee; pool3 = ImplicitPool.Ranged;
        }

        // ==========================
        // AFFIXES
        // ==========================

        public List<AffixRoll> RollAffixes(
            GearType gearType,
            GearBaseTier baseTier,
            System.Random rng,
            AffixFamily? chosenFamily = null,
            List<AffixRoll> outRolls = null)
        {
            outRolls ??= new List<AffixRoll>(4);
            outRolls.Clear();

            if (_balance == null || _mods == null)
            {
                DebugManager.Error("[GearRoller] Missing balance or GearModRegistry.", "System");
                return outRolls;
            }

            var caps = _balance.gear.slotCapsByTier.GetCaps(baseTier);
            var maxAffixes = caps.maxAffixes;
            if (maxAffixes <= 0) return outRolls;

            for (int slotIndex = 0; slotIndex < maxAffixes; slotIndex++)
            {
                var fam = chosenFamily ?? PickAffixFamily(gearType, rng);

                var roll = RollOneAffix(
                    gearType: gearType,
                    baseTier: baseTier,
                    family: fam,
                    slotIndex: slotIndex,
                    rng: rng,
                    exclude: outRolls);

                if (roll.HasValue)
                    outRolls.Add(roll.Value);
            }

            return outRolls;
        }

        private AffixRoll? RollOneAffix(
            GearType gearType,
            GearBaseTier baseTier,
            AffixFamily family,
            int slotIndex,
            System.Random rng,
            List<AffixRoll> exclude)
        {
            _mods.GetAffixCandidates(family, gearType, _affixCandidates);
            if (_affixCandidates.Count == 0) return null;

            for (int i = _affixCandidates.Count - 1; i >= 0; i--)
            {
                var cand = _affixCandidates[i];
                if (cand == null) { _affixCandidates.RemoveAt(i); continue; }

                var id = (cand.AffixId ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(id)) { _affixCandidates.RemoveAt(i); continue; }

                // no duplicate affix ids on same item
                if (!_balance.gear.affixRules.allowDuplicateAffixId && ContainsAffix(exclude, id))
                {
                    _affixCandidates.RemoveAt(i);
                    continue;
                }

                // category cap (counts via registry lookups; no AffixRoll schema changes needed)
                if (!IsCategoryAllowed(gearType, cand.Category, exclude))
                {
                    _affixCandidates.RemoveAt(i);
                    continue;
                }
            }

            if (_affixCandidates.Count == 0) return null;

            var picked = PickAffixWeighted(_affixCandidates, rng);
            if (picked == null) return null;

            var value = RollValue(picked, baseTier, rng);

            // GearInstance.AffixRoll currently: (affixId, value, index)
            return new AffixRoll(picked.AffixId, value, slotIndex, baseTier);
        }

        private bool IsCategoryAllowed(GearType gearType, AffixCategory category, List<AffixRoll> existing)
        {
            if (category == AffixCategory.None)
                return false;

            // No rule => allowed
            var cap = _balance.gear.affixRules.categoryCaps.GetCap(gearType, category, fallbackIfMissing: 99);
            if (cap >= 99) return true;

            if (cap <= 0) return false;

            int count = 0;
            for (int i = 0; i < existing.Count; i++)
            {
                var id = existing[i].affixId;
                if (string.IsNullOrEmpty(id)) continue;

                if (_mods.TryGetAffix(id, out var def) && def != null && def.Category == category)
                    count++;
            }

            return count < cap;
        }

        private static bool ContainsAffix(List<AffixRoll> list, string id)
        {
            for (int i = 0; i < list.Count; i++)
                if (string.Equals(list[i].affixId, id, StringComparison.Ordinal))
                    return true;
            return false;
        }

        private static AffixDef PickAffixWeighted(List<AffixDef> cands, System.Random rng)
        {
            double sum = 0;
            for (int i = 0; i < cands.Count; i++)
                sum += Math.Max(0.0, cands[i].customWeight);

            if (sum <= 0.00001)
                return cands[rng.Next(0, cands.Count)];

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

        private static float RollValue(AffixDef def, GearBaseTier baseTier, System.Random rng)
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

        private AffixFamily PickAffixFamily(GearType gearType, System.Random rng)
        {
            var w = _balance.gear.defaultAffixFamilyWeights;

            var list = _balance.gear.affixFamilyWeightsByGearType;
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].gearType != gearType) continue;
                    w = list[i].weights;
                    break;
                }
            }

            w.Normalize();

            var r = rng.NextDouble();
            var acc = 0.0;

            acc += w.core;
            if (r < acc) return AffixFamily.Core;

            acc += w.defensive;
            if (r < acc) return AffixFamily.Defensive;

            acc += w.synergy;
            if (r < acc) return AffixFamily.Synergy;

            return AffixFamily.Utility;
        }
    }
}
