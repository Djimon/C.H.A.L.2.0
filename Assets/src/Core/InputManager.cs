using UnityEngine;

public class InputManager : MonoBehaviour
{
    private ClickableObject lastHovered;

    void Update()
    {
        ClickableObject clickable = null;

        // Ray von der Maus
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            if (hit.collider.CompareTag("clickableObject"))
            {
                clickable = hit.collider.GetComponent<ClickableObject>() 
                    ?? hit.collider.GetComponentInChildren<ClickableObject>();
            }

            //DebugManager.Log($"raycast hit: {hit.collider.name} found clikableObject? ({clickable.name})", DebugManager.EDebugLevel.Dev);
        }

        // Hover-Wechsel
        if (clickable != lastHovered)
        {
            if (lastHovered != null)
                lastHovered.OnHoverExit();

            if (clickable != null)
            {
                clickable.OnHoverEnter();
                DebugManager.Info("Hover Clickable");
            }
            
            lastHovered = clickable;
        }

        // Klick
        if (clickable!= null && Input.GetMouseButtonDown(0))
        {
            clickable.OnClick();
            DebugManager.Info("Clicked Clickable");
        }
    }
    
}
