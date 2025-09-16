using CHAL.Data;
using CHAL.Systems;
using CHAL.Systems.Items;
using CHAL.Systems.Loot;
using UnityEngine;

public class LootRulesDebug : MonoBehaviour
{
    //LootRules
    public string[] enemyTags = new[] { "insect", "swarm", "lvl3", "boss" };

    //LootBudgetCalc
    public int level = 3;
    public float difficulty = 1.0f; // easy=0.9, normal=1.0, hard=1.2
    [Header("Enemy Composition")]
    public int spawns = 12;
    public int normals = 4;
    public int magics = 1;
    public int elites = 0;
    public int bosses = 1;
    public int champions = 0;

    //UnluckyProtection
    private UnluckyProtection manager;

    //Softcap_Modulator
    //================
    [Header("BudgetModulator")]
    public int Budget = 100;   // Budget
    public int U_budget_Used = 80;    // bisher verbrauchtes Budget
    public int vi_item_Value = 40;  // Item-Wert


    void Start()
    {
        //LOOT RULES
        //===========
        ItemRegistry.Instance.Reload(); // falls nicht schon geladen
        var svc = new LootRulesService();
        svc.LoadAll();

        var merged = svc.GetMergedForTags(enemyTags);
        //Debug.Log($"Merged drops: {merged.drops.Count} | minDrops={merged.minDrops} maxDrops={merged.maxDrops}");

        foreach (var d in merged.drops)
        {
            var c = d.chance.HasValue ? d.chance.Value.ToString("0.##") : $"[{string.Join(",", d.chancesArray)}]";
            Debug.Log($"- {d.itemId} (rarity={d.rarity}, v={d.lootValue}) chance={c} qty={d.quantity}");
        }
        foreach (var kv in merged.rarityGuarantees)
            Debug.Log($"Guarantee: {kv.Key} >= {kv.Value}");


        var tags = new[] { "insect", "lvl3", "swarm" };
        var extras = svc.GetSecretDrops(tags);
        Debug.Log($"Gefundene SecretDrops: {extras.Count}");
        foreach (var lt in extras)
            Debug.Log($"{lt.itemId} : chance: {lt.chance}, qty: {lt.quantity}");

        //LOOT_BUDGET
        //==========
        int B = LootBudgetCalculator.CalculateBudget(
            spawns, normals, magics, elites, bosses, champions, level, difficulty);

        Debug.Log($"Wave Budget (Lvl {level}, diff {difficulty}): {B}");

        //UNLUCKY_PROT
        //===========
        manager = new UnluckyProtection();

        // Beispiel: Rare Base 20%
        float pBase = 20f;

        for (int i = 0; i < 10; i++)
        {
            float mult = manager.GetMultiplier(Rarity.Rare);
            float pEff = pBase * mult;
            Debug.Log($"Fail {i}: Mult={mult:0.00}, ChanceEff={pEff:0.0}%");
            manager.OnFail(Rarity.Rare); // simuliert Fehlschlag
            Debug.Log("Nach Drop → " + manager.DebugInfo());
        }

        // Dann Drop erzwingen → Reset
        manager.OnDrop(Rarity.Rare);
        Debug.Log("Nach Drop → " + manager.DebugInfo());

        //Softcap_Modulator
        //=================

        var rarity = Rarity.Rare;
        float M = LootBudgetModulator.GetModifier(U_budget_Used, vi_item_Value, B, rarity);
        Debug.Log($"Budget={B}, Used={U_budget_Used}, Item={vi_item_Value}, Rarity={rarity} → Modifier={M:0.00}");

    }
}

