using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    private Renderer rend;
    private MaterialPropertyBlock mpb;

    public GameObject menuUI; // Hier dein Menü zuweisen im Inspector

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();

        if (!rend.sharedMaterial.HasProperty("_shimmerOn"))
        {
            DebugManager.Warning($"{name}: Matieral hat kein ShimmerOn-Effekt!","Visual");
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
            if (ui != null)
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
