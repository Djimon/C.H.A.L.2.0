using CHAL.Systems.Inventory;
using UnityEngine;

public class InvDnDProvider : MonoBehaviour
{
    public IInventoryDomain domain; // per Inspector/Bootstrap setzen
    private DragDropService _service;

    public  DragDropService Service
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
