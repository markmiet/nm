using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class WingVibration : MonoBehaviour
{
    [Header("Varinan asetukset")]
    public float frequency = 80f;       // V‰rin‰n taajuus
    public float alphaStrength = 0.5f;  // Kuinka paljon alpha vaihtelee
    public float distortionAmount = 0.02f; // UV-koordinaattien siirto

    private Renderer sr;
    private Material mat;
    private Color baseColor;
    private Vector2 baseOffset;

    void Start()
    {
        sr = GetComponent<Renderer>();
        // Luodaan materiaalista kopio, jotta muutokset eiv‰t vaikuta muihin objekteihin
        mat = Instantiate(sr.material);
        sr.material = mat;

        baseColor = mat.color;

        if (mat.HasProperty("_MainTex"))
            baseOffset = mat.GetTextureOffset("_MainTex");
    }

    void Update()
    {
        float t = Time.time * frequency;

        // 1. Alpha v‰risee siniaallon mukaan
        float alphaVariation = (Mathf.Sin(t) + 1f) * 0.5f * alphaStrength;
        Color c = baseColor;
        c.a = Mathf.Clamp01(baseColor.a - alphaVariation);
        mat.color = c;

        // 2. UV-koordinaattien pieni v‰‰ristys
        if (mat.HasProperty("_MainTex"))
        {
            float xOffset = Mathf.Sin(t) * distortionAmount;
            float yOffset = Mathf.Cos(t) * distortionAmount;
            mat.SetTextureOffset("_MainTex", baseOffset + new Vector2(xOffset, yOffset));
        }
    }
}
