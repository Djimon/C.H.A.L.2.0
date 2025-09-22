using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    private Renderer rend;
    private MaterialPropertyBlock mpb;

    public GameObject menuUI; // Hier dein Menü zuweisen im Inspector
    public Material hoverMatieral;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();

        if (hoverMatieral != null)
        {
            // Achtung: sharedMaterial = alle Objekte teilen sich das Asset
            // material = Instanz für dieses Objekt
            if (rend.sharedMaterial != hoverMatieral)
            {
                rend.sharedMaterial = hoverMatieral;
            }
        }
        else
        {
            Debug.LogWarning($"{name}: hoverMatieral ist nicht zugewiesen!", this);
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
            menuUI.SetActive(true);
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
