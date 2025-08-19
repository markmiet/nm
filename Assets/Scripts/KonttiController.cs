using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KonttiController : BaseController
{
    /*
    [NoScaleOffset] _NoiseTex("NoiseTex", 2D) = "white" {}
        [NoScaleOffset] _MainTex("MainTex", 2D) = "white" {}
        _RustAmout("RustAmout", Float) = 0.5
        _RustColor("RustColor", Color) = (0.8566037, 0.7935706, 0.8201109, 0)
        _UVScale("UVScale", Vector) = (1, 1, 0, 0)
        _Rotation("Rotation", Float) = 0
        _TilingOffset("TilingOffset", Vector) = (0.1, 0, 0, 0)
        */
    private int _NoiseTex = Shader.PropertyToID("_NoiseTex");
    private int _MainTex = Shader.PropertyToID("_MainTex");

    private int _RustAmout = Shader.PropertyToID("_RustAmout");
    private int _RustColor = Shader.PropertyToID("_RustColor");
    private int _UVScale = Shader.PropertyToID("_UVScale");
    private int _Rotation = Shader.PropertyToID("_Rotation");
    private int _TilingOffset = Shader.PropertyToID("_TilingOffset");


    public float RustAmout = 0.5f;
    public Color RustColor = Color.red;
    public Vector2 UVScale = new Vector2(1, 1);//1,1 normi koko 2,2 on puolet siit‰ normikoosta joten skaalautuu v‰‰rin p‰in...
    public float Rotation = 0;
    public Vector2 TilingOffset = new Vector2(0, 0);//t‰‰ on siis sijainti
                                                    //x - niin vasemmalle
                                                    //


    public float RustAmoutRandom = 0;
    public float RustColorRandom = 0;
    public float UVScaleRandom = 0;
    public float RotationRandom = 0;
    public float TilingOffsetRandom = 0;

    public Texture NoiseTex;
    
    public bool muunnaUpdatessa = true;





    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderers = GetComponentsInParent<SpriteRenderer>();
        _materials = new Material[_spriteRenderers.Length];
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            _materials[i] = _spriteRenderers[i].material;

        }
    }
    public float muunnossykli = 1.0f;
    private float laskuri = 0;
    // Update is called once per frame
    void Update()
    {
        if (IsGoingToBeDestroyed())
        {
            return;
        }

        if (muunnaUpdatessa)
        {

            laskuri += Time.deltaTime;

            if (laskuri >= muunnossykli)
            {
                laskuri = 0;

                Muunna();
            }
        }
    }
    private void Muunna()
    {
        for (int i = 0; i < _materials.Length; i++)
        {
            float RustAmoutL = RustAmout + RustAmout * (
GenerateRandomFromPosition(-RustAmoutRandom, RustAmoutRandom) / 100.0f);


            _materials[i].SetFloat(_RustAmout, RustAmoutL);

            _materials[i].SetColor(_RustColor, RustColor);

            _materials[i].SetTexture(_NoiseTex, NoiseTex);
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

            // Arvo v‰lill‰ 0.0 ñ 1.0
            float luku = (float)rng.NextDouble();

            // Skaalataan arvo haluttuun v‰liin
            return Mathf.Lerp(alaraja, ylaraja, luku);
        }

    }



    public static Texture2D OverlayTexture(Texture2D mainTexture, Texture2D overlayTexture, Color32 overlayTint)
    {
        // Copy the texture on the GPU for speed
        var outTexture = new Texture2D(mainTexture.width, mainTexture.height, mainTexture.format, true);
        Graphics.CopyTexture(mainTexture, outTexture);

        var outTexturePixels32 = outTexture.GetPixels32();
        var overlayTexturePixels32 = overlayTexture.GetPixels32();

        for (int i = 0; i < outTexturePixels32.Length; i++)
        {
            overlayTexturePixels32[i].r = (byte)(overlayTexturePixels32[i].r * overlayTint.r / 255);
            overlayTexturePixels32[i].g = (byte)(overlayTexturePixels32[i].g * overlayTint.g / 255);
            overlayTexturePixels32[i].b = (byte)(overlayTexturePixels32[i].b * overlayTint.b / 255);
            overlayTexturePixels32[i].a = (byte)(overlayTexturePixels32[i].a * overlayTint.a / 255);

            outTexturePixels32[i] = Color32.Lerp(outTexturePixels32[i], overlayTexturePixels32[i], (float)overlayTexturePixels32[i].a / 255);
        }

        outTexture.SetPixels32(outTexturePixels32);
        outTexture.Apply();

        return outTexture;
    }
}
