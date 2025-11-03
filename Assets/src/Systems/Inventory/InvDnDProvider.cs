using UnityEngine;

namespace CHAL.Systems.Inventory
{

/// <summary>
/// Provides functionality for inventory drag and drop operations.
/// Inherits from MonoBehaviour to integrate with Unity's game object lifecycle.
/// </summary>
    public class InvDnDProvider : MonoBehaviour
    {
        public IInventoryDomain domain; // per Inspector/Bootstrap setzen
        private DragDropService _service;

        public DragDropService Service
        {
            get
            {
                if (_service == null && domain != null)
                    _service = new DragDropService(domain);
                return _service;
            }
        }

        private void OnValidate()
        {
            // Domain kann im Editor nachgezogen werden; Service bei Bedarf neu aufbauen
            if (domain != null && _service == null)
                _service = new DragDropService(domain);


        }
    }
}
