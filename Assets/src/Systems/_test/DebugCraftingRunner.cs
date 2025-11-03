using System.Text;
using UnityEngine;
using CHAL.Systems.Inventory;       // IInventoryDomain, ItemStack
using CHAL.Systems.Crafting;        // RecipeDef
using CHAL.Core;
using CHAL.Data;


public sealed class CraftingDebugRunner : MonoBehaviour
{
    [Header("Data")]
    public CraftingCatalog catalog;
    public int recipeIndex = 0;

    [Tooltip("InstanceId deines Material-Inventars")]
    public string materialsInventoryId = "player_parts";
    public string outputInventoryId = "All_Inventory";

    [Header("Options")]
    public bool runOnStart = true;
    public bool simulateCurrencyMissing = false;
    public int grantCrafts = 1;

    private InventoryDomain _inv;
    private IWallet _wallet;

    void Awake()
    {

        GameManager.Instance.TestInitInventory();

        _inv = GameManager.Instance.Inventory;
        _wallet = GameManager.Instance.Profile;
    }

    void Start()
    {
        GameManager.Instance.EnsureInstance("player_parts", PlayerInventoryType.Part);
        GameManager.Instance.EnsureInstance("All_Inventory", PlayerInventoryType.all);


        if (!runOnStart) return;
        RunOnce();
    }

    [ContextMenu("RunOnce")]
    public void RunOnce()
    {
        var recipe = catalog.recipes[recipeIndex];
        var wallet = simulateCurrencyMissing ? new WalletProxyMissing(_wallet) : _wallet;

        DebugManager.DebugLog($"[CraftTest] Recipe: {NameOf(recipe)}");
        PrintPreview(recipe);

        if (CraftingService.TryCraftToInventory(recipe, _inv,  _wallet, outputInventoryId, out var reason))
        {
            DebugManager.DebugLog($"[CraftTest] SUCCESS -> Output placed into '{outputInventoryId}'");
        }
        else
        {
            DebugManager.Warning($"[CraftTest] FAIL -> {reason}");
        }

        // nach Commit/Fail erneut den Zustand zeigen
        PrintPreview(recipe);
    }

    [ContextMenu("GrantRequirements")]
    public void GrantRequirements()
    {
        var recipe = catalog.recipes[recipeIndex];
        var preview = CraftingService.GetPreview(recipe, outputInventoryId, _inv,  _wallet);

        // 1) Materials auffüllen
        //foreach (var m in preview.materials)
        //{
        //    int need = m.required * grantCrafts;
        //    int missing = need - m.playerAmount;
        //    if (missing <= 0) continue;

        //    var ok = _inv.TryAdd(materialsInventoryId, new ItemStack(m.itemId, missing), out var tx);
        //    if (!ok)
        //    {
        //        Debug.LogWarning($"[Grant] TryAdd failed for {m.itemId} (missing={missing}).");
        //    }
        //    else
        //    {
        //        Debug.Log($"[Grant] +{missing} {m.itemId} (now >= {need}).");
        //    }
        //}

        // 2) Currency auffüllen
        //foreach (var c in preview.currencies)
        //{
        //    int need = c.required * grantCrafts;
        //    int missing = need - c.playerAmount;
        //    if (missing <= 0) continue;

        //    _wallet.Refund(c.currencyId, missing); // Debug: add currency via refund
        //    Debug.Log($"[Grant] +{missing} {c.currencyId} (now >= {need}).");
        //}

        // 3) Kontrolle
        var after = CraftingService.GetPreview(recipe, outputInventoryId, _inv,  _wallet);
        DebugManager.DebugLog($"[Grant] canCraft={after.canCraft} for x{grantCrafts} crafts.");
    }

    private void PrintPreview(RecipeDef recipe)
    {
        var prev = CraftingService.GetPreview(recipe, outputInventoryId, _inv,  _wallet);
        var sb = new StringBuilder();

        sb.AppendLine($"[Preview] canCraft={prev.canCraft}");
        sb.AppendLine("  Materials:");
        //foreach (var m in prev.materials)
        //    sb.AppendLine($"    - {m.itemId}: need {m.required} / have {m.playerAmount} {(m.enough ? "" : "<MISSING>")}");

        sb.AppendLine("  Currency:");
        //foreach (var c in prev.currencies)
        //    sb.AppendLine($"    - {c.currencyId}: need {c.required} / have {c.playerAmount} {(c.enough ? "" : "<MISSING>")}");

        DebugManager.DebugLog(sb.ToString());
    }

    private static string NameOf(RecipeDef r) => string.IsNullOrEmpty(r.displayKey) ? r.name : r.displayKey;

    // Optional: simuliert „Currency fehlt/Spend schlägt fehl“, um Rollback zu testen
    private sealed class WalletProxyMissing : IWallet
    {
        private readonly IWallet _inner;
        public WalletProxyMissing(IWallet inner) { _inner = inner; }
        public int GetCurrency(string id) => 0;
        public bool CanSpend(string id, int amt) => false;
        public bool SpendCurrency(string id, int amt) => false; // erzwingt Fail NACH Materialabbuchung
        public void Refund(string id, int amt) => _inner.Refund(id, amt);
    }
}
