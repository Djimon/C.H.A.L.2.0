using CHAL.Core;
using CHAL.Systems.Research;
using UnityEngine;

namespace CHAL.UI
{

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
                DebugManager.Warning($"{name}: Material has no ShimmerOn effect!", "Visual");
            }

            SetShimmer(false);
        }

        public void OnHoverEnter()
        {
            SetShimmer(true);
        }

        public void OnHoverExit()
        {
            SetShimmer(false);
        }

        public void OnClick()
        {
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
