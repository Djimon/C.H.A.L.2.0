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


        // “Pickup” (auch per RMB für Split möglich)
        public void BeginDrag(ItemMoveObject from, bool splitHalf)
        {
            DebugManager.Log($"[Drag&Drop]: Begin", DebugManager.EDebugLevel.Dev, "Inventory");
            _from = from;
            _hasFrom = true;
            _splitHalf = splitHalf;
        
        }


        // Abbruch (ESC, Rechtsklick außerhalb, etc.)
        public void Cancel()
        {
            _hasFrom = false;
            _splitHalf = false;
        }

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

            DebugManager.Log($"[Drag&Drop]: Move erfolgreich", DebugManager.EDebugLevel.Dev, "Inventory");

            // success
            Cancel();
        }
    }
}