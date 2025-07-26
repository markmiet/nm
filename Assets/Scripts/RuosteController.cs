using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuosteController : MonoBehaviour
{
    private int _DissolveAmount = Shader.PropertyToID("_DissolveAmount");
    private int _VerticalDissolve = Shader.PropertyToID("_VerticalDissolve");

    private int _OutlineThickness = Shader.PropertyToID("_OutlineThickness");
    private int _DissolveScale = Shader.PropertyToID("_DissolveScale");
    private int _OutLineColor = Shader.PropertyToID("_OutLineColor");
    private int _SpiralStrength = Shader.PropertyToID("_SpiralStrength");


    public float dissolveAmount = 1.0f;
    public float VerticalDissolve = 1.0f;
    public float OutlineThickness = 1.0f;

    public float DissolveScale = 1.0f;
    public Color OutLineColor = Color.blue;
    public float SpiralStrength = 1.0f;


    public float randomdissolveAmount = 0.0f;
    public float randomVerticalDissolve = 0.0f;
    public float randomOutlineThickness = 0.0f;

    public float randomDissolveScale = 0.0f;
    public float randomOutLineColor = 0.0f;
    public float randomSpiralStrength = 0.0f;


    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;

    /*
    _OutlineThickness
        _DissolveScale
        _OutLineColor
        _SpiralStrength
        */
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderers = GetComponentsInParent<SpriteRenderer>();
        _materials = new Material[_spriteRenderers.Length];
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;

        }
        /*
        dissolveAmount = dissolveAmount + dissolveAmount * (
        GenerateRandomFromPosition(-randomdissolveAmount, randomdissolveAmount) / 100.0f);


        VerticalDissolve = VerticalDissolve + VerticalDissolve * (
        GenerateRandomFromPosition(-randomVerticalDissolve, randomVerticalDissolve) / 100.0f);


        OutlineThickness = OutlineThickness + OutlineThickness * (
        GenerateRandomFromPosition(-randomOutlineThickness, randomOutlineThickness) / 100.0f);

        DissolveScale = DissolveScale + DissolveScale * (
        GenerateRandomFromPosition(-randomDissolveScale, randomDissolveScale) / 100.0f);


        SpiralStrength = SpiralStrength + SpiralStrength * (
        GenerateRandomFromPosition(-randomSpiralStrength, randomSpiralStrength) / 100.0f);
        */
        float arpa = GenerateRandomFromPosition(0, 100);

        if (arpa >= 100 - todennakoisyysJollaMuunnetaan)
        {
            Muunna();
        }



    }

    public float muunnossykli = 1.0f;
    private float laskuri = 0;
    public bool muunnaUpdatessa = false;

    public float todennakoisyysJollaMuunnetaan = 50.0f;

    /*
    public float hueVariance = 0.05f;  // max ±5% värivaihtelu
    public float saturationVariance = 0.1f;
    public float valueVariance = 0.1f;
    */
    // Update is called once per frame
    void Update()
    {
        /*
        // Satunnaista RGB ja Alpha ±20 %
        float newR = Mathf.Clamp01(original.r * (1f + GenerateRandomFromPosition(-randomOutLineColor/100.0f, randomOutLineColor / 100.0f)));
        float newG = Mathf.Clamp01(original.g * (1f + GenerateRandomFromPosition(-randomOutLineColor / 100.0f, randomOutLineColor / 100.0f)));
        float newB = Mathf.Clamp01(original.b * (1f + GenerateRandomFromPosition(-randomOutLineColor / 100.0f, randomOutLineColor / 100.0f)));
        float newA = Mathf.Clamp01(original.a * (1f + GenerateRandomFromPosition(-randomOutLineColor / 100.0f, randomOutLineColor / 100.0f)));

        Color uusvari = new Color(newR, newG, newB, newA);
        _materials[i].SetColor(_OutLineColor, uusvari);

        */
        if (muunnaUpdatessa)
        {

            laskuri += Time.deltaTime;

            if (laskuri >= muunnossykli)
            {
                laskuri = 0;

                Muunna();

                /*
                var renderer = GetComponent<SpriteRenderer>();
                Color baseColor = renderer.color;

                Color.RGBToHSV(baseColor, out float h, out float s, out float v);

                h = (h + GenerateRandomFromPosition(-hueVariance, hueVariance)) % 1f;
                if (h < 0f) h += 1f;

                s = Mathf.Clamp01(s * (1f + GenerateRandomFromPosition(-saturationVariance, saturationVariance)));
                v = Mathf.Clamp01(v * (1f + GenerateRandomFromPosition(-valueVariance, valueVariance)));

                Color newColor = Color.HSVToRGB(h, s, v);
                newColor.a = baseColor.a; // säilytä alpha

                renderer.color = newColor;
                */


            }
        }
    }

    public float hueAsteet = 0.05f;
    public float toinenrandomin = 0.2f;



    private void Muunna()
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            float dissolveAmountl = dissolveAmount + dissolveAmount * (
GenerateRandomFromPosition(-randomdissolveAmount, randomdissolveAmount) / 100.0f);


            float VerticalDissolvel = VerticalDissolve + VerticalDissolve * (
            GenerateRandomFromPosition(-randomVerticalDissolve, randomVerticalDissolve) / 100.0f);


            float OutlineThicknessl = OutlineThickness + OutlineThickness * (
            GenerateRandomFromPosition(-randomOutlineThickness, randomOutlineThickness) / 100.0f);

            float DissolveScalel = DissolveScale + DissolveScale * (
            GenerateRandomFromPosition(-randomDissolveScale, randomDissolveScale) / 100.0f);


            float SpiralStrengthl = SpiralStrength + SpiralStrength * (
            GenerateRandomFromPosition(-randomSpiralStrength, randomSpiralStrength) / 100.0f);




            _materials[i].SetFloat(_DissolveAmount, dissolveAmountl);
            _materials[i].SetFloat(_VerticalDissolve, VerticalDissolvel);
            _materials[i].SetFloat(_OutlineThickness, OutlineThicknessl);
            _materials[i].SetFloat(_DissolveScale, DissolveScalel);

            _materials[i].SetFloat(_SpiralStrength, SpiralStrengthl);


            Color original = OutLineColor;


            // Muunna väri HSV-tilaan
            Color.RGBToHSV(original, out float h, out float s, out float v);

            // Satunnaista hue ±10 astetta, saturation ja value ±20 %
            h = (h + GenerateRandomFromPosition(-hueAsteet, hueAsteet)) % 1f; // Hue-ympyrä on 0–1, 0.05 ~18 astetta
            if (h < 0f) h += 1f;

            s = Mathf.Clamp01(s * (1f + GenerateRandomFromPosition(-toinenrandomin / 100.0f, toinenrandomin / 100.0f)));//saturation
            v = Mathf.Clamp01(v * (1f + GenerateRandomFromPosition(-toinenrandomin / 100.0f, toinenrandomin / 100.0f)));//value

            // Muunna takaisin RGB:ksi
            /*
            h = 0.8f;
            s = 0.8f;
            v = 0.8f;
            */
            Color newColor = Color.HSVToRGB(h, s, v);
            newColor.a = original.a; // Säilytä alkuperäinen alpha

            // _materials[i].SetColor(_OutLineColor, newColor);
            //eri tapa alkaa

            float newR = Mathf.Clamp01(original.r * (1f + GenerateRandomFromPosition(-randomOutLineColor / 100.0f, randomOutLineColor / 100.0f)));
            float newG = Mathf.Clamp01(original.g * (1f + GenerateRandomFromPosition(-randomOutLineColor / 100.0f, randomOutLineColor / 100.0f)));
            float newB = Mathf.Clamp01(original.b * (1f + GenerateRandomFromPosition(-randomOutLineColor / 100.0f, randomOutLineColor / 100.0f)));
            float newA = Mathf.Clamp01(original.a * (1f + GenerateRandomFromPosition(-randomOutLineColor / 100.0f, randomOutLineColor / 100.0f)));

            Color uusvari = new Color(newR, newG, newB, newA);
            // Color rust = new Color(183f / 255f, 65f / 255f, 14f / 255f, 0.5f);
            _materials[i].SetColor(_OutLineColor, uusvari);
            //eri tapa loppuu

        }

    }

    float GenerateRandomFromPosition(float alaraja, float ylaraja)
    {
        if (muunnaUpdatessa)
        {
            return Random.Range(alaraja, ylaraja);
        }
        else
        {
            Vector2 position = transform.position;

            // Muunnetaan koordinaatit deterministiseksi siemeneksi
            int seed = Mathf.FloorToInt(position.x * 73856093) ^ Mathf.FloorToInt(position.y * 19349663);
            System.Random rng = new System.Random(seed);

            // Arvo välillä 0.0 – 1.0
            float luku = (float)rng.NextDouble();

            // Skaalataan arvo haluttuun väliin
            return Mathf.Lerp(alaraja, ylaraja, luku);
        }

    }

}
