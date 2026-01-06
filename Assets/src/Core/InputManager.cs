using CHAL.UI;
using UnityEngine;


namespace CHAL.Core
{

/// <summary>
/// Manages user input and interactions with clickable objects in the game.
/// </summary>
    public class InputManager : MonoBehaviour
    {
        private ClickableObject lastHovered;


        void Update()
        {
            HandleClickableObjects();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Make small Pause-Menue to decide to Continue or go bakc ot main-menu
                GameManager.Instance.GoToMainMenu();
            }

            //if (/*GameManager.Instance.CurrentState == GameState.Hideout &&*/ Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.O))
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.O))
            {
                var cheatmenu = FindFirstObjectByType<CheatMenuController>(FindObjectsInactive.Include);
                if (cheatmenu != null)
                {
                    cheatmenu.ToggleUI();
                }
                else
                {
                    DebugManager.Log("CheatMenuController not found in scene.", DebugManager.EDebugLevel.Dev, "Input", LogType.Warning);
                }
            }

        }

        private void HandleClickableObjects()
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
                    DebugManager.Log("Hover Clickable", DebugManager.EDebugLevel.Debug, "Input");
                }

                lastHovered = clickable;
            }

            // Klick
            if (clickable != null && Input.GetMouseButtonDown(0))
            {
                clickable.OnClick();
                DebugManager.Log("Clicked Clickable", DebugManager.EDebugLevel.Debug, "Input");
            }
        }
    }
}
