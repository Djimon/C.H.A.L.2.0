using UnityEngine;
using UnityEngine.Rendering;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "Hero", menuName = "Data/Hero")]
/// <summary>
/// Represents a hero definition in the game, including identity, lore, and gameplay attributes.
/// </summary>
    public class HeroDef : ScriptableObject
    {
        [Header("Identity & Flavor")]
        public string HeroId;                // "Hero_Piercer_01"
        public string DisplayName;           // "Kaelen the Piercer"
        [TextArea] public string Lore;       // Flavourtext / Story

        [Header("Gameplay")]
        public ArchetypeDef Archetype;       // Verweis auf ArchetypeDef
        public ArmorClass Armorclass;        //TODO: use when handling gear-slots/inventory
        public int BaseHealth = 100;
        public float BaseDamage = 10f;
        public float BaseMovementSpeed = 2f;
        public float sightRange = 20f;

        [Header("Signature Passive")]
        public SkillModifierDef SignaturePassive;   // TODO: own ScriptableObject mit ModifierData (List?)


        [Header("Visuals")]
        public Sprite Portrait;
        public GameObject Prefab;            // 3D- oder 2D-Model für Ingame
        public AudioClip VoiceSample;        // optional

        public SkillModuleDef fallBackAttack;

    }
}
