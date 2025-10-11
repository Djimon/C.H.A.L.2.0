using CHAL.Systems.Inventory;
using CHAL.Systems.Items;
using CHAL.UI;
using UnityEngine;

public class InventoryDemoBootstrap : MonoBehaviour
{
    public InventoryView bagAView;
    public InventoryView bagBView;

    private InventoryDomain _domain;
    private InventoryInstance _bagA;
    private InventoryInstance _bagB;

    void Awake()
    {
        _domain = new InventoryDomain();

        int Rows = 3;
        int Cols = 4;

        var def = new InventoryDef { TypeId = "demo:bag", NameKey = "Demo", cols = 4, rows = 3, defaultMaxStackPerSlot = 5 };
        _bagA = InventoryInstance.Create("bagA", def);

        var def2 = new InventoryDef {
                TypeId = "demo:bag2", 
                NameKey = "Demo 2", 
                cols = Cols,
                rows = Rows,
                defaultMaxStackPerSlot = 4,
                globalSlotFilter = new SlotFilter { AllowedItemTypes = new[] {ItemType.Remains} }};
        
        _bagB = InventoryInstance.Create("bagB", def2);


        _domain.RegisterInstance(_bagA);
        _domain.RegisterInstance(_bagB);

        // Test-Items (nutz deine IDs aus ItemRegistry)
        _domain.TryAdd(_bagA.instanceID, new ItemStack("part:antler", 7), out _);
        _domain.TryAdd(_bagA.instanceID, new ItemStack("remains:glitter_dust", 12), out _);
        _domain.TryAdd(_bagB.instanceID, new ItemStack("module:core", 3), out _);
        _domain.TryAdd(_bagB.instanceID, new ItemStack("remains:blood", 6), out _);

        // UI binden
        if (bagAView) bagAView.Bind(_domain, _bagA.instanceID, def.cols, def.rows);
        if (bagBView) bagBView.Bind(_domain, _bagB.instanceID, def2.cols, def2.rows);


    }
}
