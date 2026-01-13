using CHAL.Core;
using CHAL.Systems.Codex;
using UnityEngine;

namespace CHAL.UI
{

/// <summary>
/// Represents an object that can be clicked in the game.
/// </summary>
    public class ClickableObject : MonoBehaviour
    {
        private Renderer rend;
        private MaterialPropertyBlock mpb;

        public GameObject menuUI; // Hier dein MenÃ¼ zuweisen im Inspector

        void Awake()
        {
            rend = GetComponent<Renderer>();
            mpb = new MaterialPropertyBlock();

            if (!rend.sharedMaterial.HasProperty("_shimmerOn"))
            {
                DebugManager.Warning($"{name}: Material has no ShimmerOn effect!", "UI");
            }

            SetShimmer(false);
        }

/// <summary>
/// Handles the event when the mouse pointer enters a hover state.
/// </summary>
        public void OnHoverEnter()
        {
            SetShimmer(true);
        }

/// <summary>
/// Handles the event when the mouse pointer exits a hover state.
/// </summary>
        public void OnHoverExit()
        {
            SetShimmer(false);
        }

/// <summary>
/// Handles the click event for the UI.
/// </summary>
        public void OnClick()
        {
            // Wenn irgendein IngameUI sichtbar ist, ignorieren wir Klicks auf Weltobjekte komplett.

            var allUis = Object.FindObjectsByType<IngameUI>(FindObjectsSortMode.None); 
            for (int i = 0; i < allUis.Length; i++)
            {
                if (allUis[i] != null && allUis[i].IsVisible)
                    return;
            }


            if (menuUI != null)
            {
                var ui = menuUI.GetComponent<IngameUI>();
                var unlocked = true;
                if (ui.requiredFeatureID != "none")
                    unlocked = GameManager.Instance.ResearchUnlocks.IsUnlockedCraftingFeature(ui.requiredFeatureID);

                if (ui != null && unlocked)
                    ui.Show(true);

                SetShimmer(false);
            }
        }

        private void SetShimmer(bool on)
        {
            mpb.Clear();
            mpb.SetFloat("_shimmerOn", on ? 1f : 0f);
            rend.SetPropertyBlock(mpb);
        }
    }
}
