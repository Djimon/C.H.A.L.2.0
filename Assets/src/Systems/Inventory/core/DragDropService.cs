using CHAL.Systems.Items;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHAL.Systems.Inventory
{
    /// Merkt Quelle als ItemMoveObject & baut MoveRequest. Keine Visuals, nur Funktion.
    public sealed class DragDropService
    {
        private readonly IInventoryDomain _domain;

        private ItemMoveObject _from;
        private bool _hasFrom = false;
        private bool _splitHalf;

        public bool HasFrom => _hasFrom;              // NEU: öffentlich lesbar
        public ItemMoveObject From => _from;          // NEU: Quelle – für Ghost-Text/Icon
        public bool IsSplit => _splitHalf;

        public DragDropService(IInventoryDomain domain) { _domain = domain; }

        //events
        public event Action<ItemStack,bool> OnBeginDrag;
        public event Action OnEndDrag;

        // “Pickup” (auch per RMB für Split möglich)
/// <summary>
/// Initiates the drag operation for an item.
/// </summary>
/// <param name="from">The item to be dragged.</param>
/// <param name="splitHalf">Indicates whether to split the item in half during the drag.</param>
        public void BeginDrag(ItemMoveObject from, bool splitHalf)
        {
            DebugManager.Log($"[Drag&Drop]: Begin", DebugManager.EDebugLevel.Dev, "Inventory");
            _from = from;
            _hasFrom = true;
            _splitHalf = splitHalf;

            var stack = _domain.Peek(from.instanceID, from.slot);
            if (stack.HasValue) OnBeginDrag?.Invoke(stack.Value, splitHalf);

        }


        // Abbruch (ESC, Rechtsklick außerhalb, etc.)
/// <summary>
/// Cancels the current action or operation.
/// </summary>
        public void Cancel()
        {
            _hasFrom = false;
            _splitHalf = false;

            OnEndDrag?.Invoke();
        }

/// <summary>
/// Attempts to drop an item onto the specified target object.
/// </summary>
/// <param name="to">The target item move object to drop onto.</param>
        public void TryDropOn(ItemMoveObject to)
        {
            if (!_hasFrom) return;

            bool sameSlot = _from.instanceID == to.instanceID && _from.slot == to.slot;

            if (sameSlot)
            {
                if (_splitHalf)
                {
                    // Split auf gleichem Slot macht keinen Sinn -> abbrechen
                    Cancel();
                }
                // sonst: Pickup bleibt „in der Hand“, Ghost bleibt sichtbar
                return;
            }

            var req = new MoveRequest
            {
                fromInventory = _from,
                toInventory = to,
                moveMode = _splitHalf ? MoveMode.Split : MoveMode.Move,
                amount = null
            };

            if (!_domain.TryMove(req, out var res))
            {
                DebugManager.Log($"[Drag&Drop]: Move fehlgeshclagen: {res.reason}", DebugManager.EDebugLevel.Dev, "Inventory");
                Cancel();
                return;
            }

            DebugManager.Log($"[Drag&Drop]: Move successful", DebugManager.EDebugLevel.Dev, "Inventory");

            // success
            Cancel();
        }
    }
}
