using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHAL.Data
{
    /// <summary>
    /// A concrete, persisted gear item instance.
    /// Holds rolled implicits/affixes and references a static GearDef/GearData via ID.
    /// </summary>
    [Serializable]
    public sealed class GearInstance
    {
        [Header("Identity")]
        public string instanceId; // unique per item (GUID)
        public string gearItemId;  // reference to the static definition (ScriptableObject)

        [Header("Base Tier")]
        public GearBaseTier baseTier = GearBaseTier.T1;

        [Header("Rolled Mods (persisted)")]
        public List<ImplicitRoll> implicits = new List<ImplicitRoll>();
        public List<AffixRoll> affixes = new List<AffixRoll>();

        //public bool isIdentified = true;

        public static GearInstance CreateNew(string gearDefId, GearBaseTier baseTier)
        {
            return new GearInstance
            {
                instanceId = Guid.NewGuid().ToString("N"),
                gearItemId = gearDefId,
                baseTier = baseTier,
                implicits = new List<ImplicitRoll>(capacity: 3),
                affixes = new List<AffixRoll>(capacity: 3)
            };
        }

        public override string ToString()
        {
            return $"GearInstance(id={instanceId}, def={gearItemId}, tier={baseTier}, implicits={implicits?.Count ?? 0}, affixes={affixes?.Count ?? 0})";
        }


        public bool TryAddImplicit(ImplicitRoll roll, int maxAllowed)
        {
            if (implicits == null)
                implicits = new List<ImplicitRoll>(capacity: Math.Max(0, maxAllowed));

            if (maxAllowed <= 0) return false;
            if (implicits.Count >= maxAllowed) return false;

            implicits.Add(roll);
            return true;
        }

        public bool TryAddAffix(AffixRoll roll, int maxAllowed)
        {
            if (affixes == null)
                affixes = new List<AffixRoll>(capacity: Math.Max(0, maxAllowed));

            if (maxAllowed <= 0) return false;
            if (affixes.Count >= maxAllowed) return false;

            affixes.Add(roll);
            return true;
        }

    }

    /// <summary>
    /// Concrete rolled implicit on an item instance.
    /// References ImplicitDef by Id and stores the rolled value (and optional metadata).
    /// </summary>
    [Serializable]
    public struct ImplicitRoll
    {
        [Tooltip("Reference to ImplicitDef.Id")]
        public string implicitId;

        [Tooltip("Rolled value (flat or percent depends on ImplicitDef.ValueKind)")]
        public float value;

        [Tooltip("0 = Signature, 1 = Slot2, 2 = Slot3 (optional but useful for debugging/UI)")]
        public int slotIndex;

        [Tooltip("Optional: store the base tier used for the roll (debug/consistency)")]
        public GearBaseTier rolledFromTier;

        public ImplicitRoll(string implicitId, float value, int slotIndex, GearBaseTier rolledFromTier)
        {
            this.implicitId = implicitId;
            this.value = value;
            this.slotIndex = slotIndex;
            this.rolledFromTier = rolledFromTier;
        }
    }

    /// <summary>
    /// Placeholder for later affix crafting.
    /// Keep the same pattern as ImplicitRoll: reference by ID + rolled value(s).
    /// </summary>
    [Serializable]
    public struct AffixRoll
    {
        [Tooltip("Reference to AffixDef.Id (future)")]
        public string affixId;

        [Tooltip("Rolled value (interpretation is defined by AffixDef)")]
        public float value;

        [Tooltip("Optional: prefix/suffix grouping etc. (future)")]
        public int index;

        public AffixRoll(string affixId, float value, int index = 0)
        {
            this.affixId = affixId;
            this.value = value;
            this.index = index;
        }
    }

    [Serializable]
    public enum GearBaseTier
    {
        T1 = 1,
        T2 = 2,
        T3 = 3
    }
}
