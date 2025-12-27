using CHAL.Systems.Inventory;
using CHAL.Systems.Items;
using CHAL.UI;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the inventory system for the demo, handling multiple inventory views.
/// </summary>
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

        _bagA = InventoryInstance.Create("bagA", _bagA.InvDef);
        
        _bagB = InventoryInstance.Create("bagB", _bagB.InvDef);


        _domain.RegisterInstance(_bagA);
        _domain.RegisterInstance(_bagB);

        // Test-Items (nutz deine IDs aus ItemRegistry)
        _domain.TryAdd(_bagA.instanceID, new ItemStackRef("part:eye", 7), out _);
        _domain.TryAdd(_bagA.instanceID, new ItemStackRef("remains:glitter_dust", 12), out _);
        _domain.TryAdd(_bagB.instanceID, new ItemStackRef("module:core", 3), out _);
        _domain.TryAdd(_bagB.instanceID, new ItemStackRef("remains:blood", 6), out _);

        // UI binden
        if (bagAView) bagAView.Bind(_domain, _bagA.instanceID, _bagA.InvDef.cols, _bagA.InvDef.rows);
        if (bagBView) bagBView.Bind(_domain, _bagB.instanceID, _bagB.InvDef.cols, _bagB.InvDef.rows);


    }
}
