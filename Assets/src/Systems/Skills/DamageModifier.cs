using CHAL.Data;
using System;
using System.Collections.Generic;

[Serializable]
/// <summary>
/// Represents a modifier for damage calculations in the game.
/// Contains properties to define the type and target of the damage modifier.
/// </summary>
public class DamageModifier
{
    public string Id;

    // Welcher "Layer" im DMG-Modell
    public DamageModifierType Type;

    // Für Added, Increased, More: betroffener Typ (oder "Any")
    public DamageType TargetType;

    // Für Convert/Gain: Quelle + Ziel
    public DamageType SourceType;
    public DamageType DestinationType;

    // Wert-Bedeutung:
    // Added:        flacher Wert (z.B. 15 = +15 Schaden)
    // Convert/Gain: 0.3 = 30%
    // Increased:    0.2 = +20% increased
    // More:         0.3 = 30% more  -> Faktor = 1 + Value
    public float Value;

    // Für Tag-Filter wie bisher
    public List<SkillDeliveryTag> AppliesTo;

    // Hook behalten wir, damit z.B. "OnHit more Damage" geht
    public ModifierHook Hook = ModifierHook.None;
}

public enum DamageModifierType
{
    Added,      // +X flat damage of type T
    Convert,    // convert % of A -> B (no duplication)
    Gain,       // gain % of A as B (duplication)
    Increased,  // +X% increased (additive)
    More        // X% more (multiplicative)
}
