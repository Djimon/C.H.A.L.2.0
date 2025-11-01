# global.InventoryDemoBootstrap

_Automatically generated/updated from `Assets/src/Systems/_test/demo_InvenotryBootstrap.cs`._

```text
1) Purpose
- Defines InventoryDemoBootstrap (a MonoBehaviour) to initialize a simple inventory demo.
- Creates an InventoryDomain and two InventoryInstance bags (bagA and bagB), registers them with the domain, preloads test items, and binds two InventoryView UI components to display them.
- Uses CHAL.Systems.Inventory, CHAL.Systems.Items, and CHAL.UI.

2) Public API
- Namespace/module: global (no explicit namespace)

- Types
  - public class InventoryDemoBootstrap : MonoBehaviour
    - Public fields
      - InventoryView bagAView; // UI binding for bag A
      - InventoryView bagBView; // UI binding for bag B

3) Key Behavior & Side Effects
- Awake (Unity lifecycle):
  - _domain = new InventoryDomain();
  - _bagA = InventoryInstance.Create("bagA", _bagA.InvDef);
  - _bagB = InventoryInstance.Create("bagB", _bagB.InvDef);
  - _domain.RegisterInstance(_bagA);
  - _domain.RegisterInstance(_bagB);
  - _domain.TryAdd(_bagA.instanceID, new ItemStack("part:eye", 7), out _);
  - _domain.TryAdd(_bagA.instanceID, new ItemStack("remains:glitter_dust", 12), out _);
  - _domain.TryAdd(_bagB.instanceID, new ItemStack("module:core", 3), out _);
  - _domain.TryAdd(_bagB.instanceID, new ItemStack("remains:blood", 6), out _);
  - UI binding:
    - if (bagAView) bagAView.Bind(_domain, _bagA.instanceID, _bagA.InvDef.cols, _bagA.InvDef.rows);
    - if (bagBView) bagBView.Bind(_domain, _bagB.instanceID, _bagB.InvDef.cols, _bagB.InvDef.rows);

4) Constraints & Failure Modes
- Potential runtime issue:
  - _bagA.InvDef and _bagB.InvDef are accessed before _bagA/_bagB are assigned, which can cause a NullReferenceException at Awake time.
- TryAdd calls ignore the out result (they discard success/failure).
- UI binding is conditional on bagAView/bagBView being non-null; otherwise binding is skipped.
- Exact behavior/shape of InventoryDomain, InventoryInstance, ItemStack, and InvDef are not defined in this file.

5) Example
- Not derivable from this file (no standalone usage example provided).

6) Unknowns
- Definitions and behavior of:
  - CHAL.Systems.Inventory types: InventoryDomain, InventoryInstance, InvDef, ItemStack, and how TryAdd/instanceID work beyond usage here.
  - CHAL.UI.InventoryView.Bind signature and runtime effects.
  - The structure/content of ItemStack (beyond constructor usage) and the meaning of item IDs like "part:eye".
  - The exact InvDef fields (cols, rows) and their semantics.
```
