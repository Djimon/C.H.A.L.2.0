using UnityEngine;
using UnityEngine.Rendering;

namespace CHAL.Data
{
    [CreateAssetMenu(fileName = "Hero", menuName = "Data/Hero")]
    public class HeroDef : ScriptableObject
    {
        [Header("Identity & Flavor")]
        public string HeroId;                // "Hero_Piercer_01"
        public string DisplayName;           // "Kaelen the Piercer"
        [TextArea] public string Lore;       // Flavourtext / Story

        [Header("Gameplay")]
        public ArchetypeDef Archetype;       // Verweis auf ArchetypeDef
        public int BaseHealth = 100;
        public float BaseMovementSpeed = 2f;
        public float sightRange = 20f;


        [Header("Visuals")]
        public Sprite Portrait;
        public GameObject Prefab;            // 3D- oder 2D-Model für Ingame
        public AudioClip VoiceSample;        // optional



    }
}
