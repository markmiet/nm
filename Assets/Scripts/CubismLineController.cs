using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubismLineController : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("Shader Params")]
    public Texture2D maintexuusi;
    public float tilecount = 64;
    public bool useTintColor = false;
    public Color tintcolor;
    public float jitter = 1.0f;
    public float tilesizevar = 1.0f;

    public bool useColornoise = false;
    public Color colornoise;
    public float rotation = 0.0f;


    private Renderer renderer;
    private Material material;

    public bool working = false;

    private ShaderPropertyBlockCache _cache;

    private Dictionary<string, int> _propIDs = new();

    void CacheShaderProperties(Material mat)
    {
        Shader shader = mat.shader;
        int count = shader.GetPropertyCount();
        for (int i = 0; i < count; i++)
        {
            string name = shader.GetPropertyName(i);
            _propIDs[name] = Shader.PropertyToID(name);
        }
    }

    void Start()
    {
        /*
         *     Properties
    {
        _MainTex ("Line Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _TileCount ("Tiles (per axis)", Float) = 32
        _Jitter ("Jitter (0..1)", Range(0,1)) = 0.3
        _Rotation ("Rotation (radians)", Range(0,6.28318)) = 1.0
        _ColorNoise ("Color Noise", Range(0,1)) = 0.1
        _Seed ("Seed", Float) = 0.0

        _TileSizeVar ("Tile Size Variation", Range(0,1)) = 0.2
    }
        */

        renderer = GetComponent<Renderer>();
        /*
        if (!working)
        {
            renderer.material = materiaalijotakaytetaankunworkingtappaeipaalla;
        }

        _cache = new ShaderPropertyBlockCache(renderer);
        material = renderer.material;
        CacheShaderProperties(material);
        if (maintexuusi != null)
        {
            _cache.SetTexture(_propIDs["_MainTex"], maintexuusi);
        }
        */

        AsetaMateriaali();

    }


    public Material workingTrueMaterial;

    public void AsetaMateriaali()
    {

        _cache = new ShaderPropertyBlockCache(renderer);
        
        if (working && workingTrueMaterial != null)
        {
            renderer.material = workingTrueMaterial;
            
        }
        
        material = renderer.material;
        CacheShaderProperties(material);
        if (maintexuusi != null)
        {
            _cache.SetTexture(_propIDs["_MainTex"], maintexuusi);
        }

    }


    // Update is called once per frame
    void Update()
    {

        if (maintexuusi != null)
        {
            _cache.SetTexture(_propIDs["_MainTex"], maintexuusi);
        }

        //AsetaMateriaali();

        if (!working)
        {
            return;
        }

        if (maintexuusi != null)
        {
            _cache.SetTexture(_propIDs["_MainTex"], maintexuusi);
        }
        else
        {

            _cache.SetTexture(_propIDs["_MainTex"], maintexuusi);
        }


        // material.SetFloat("_TileCount", tilecount);

        _cache.SetFloat(_propIDs["_TileCount"], tilecount);


        //material.SetColor("_Color", tintcolor);
        if (useTintColor)
            _cache.SetColor(_propIDs["_Color"], tintcolor);
        if (useColornoise)
            _cache.SetColor(_propIDs["_ColorNoise"], colornoise);



        _cache.SetFloat(_propIDs["_Jitter"], jitter);

        _cache.SetFloat(_propIDs["_TileSizeVar"], tilesizevar);



        _cache.SetFloat(_propIDs["_Rotation"], rotation);




    }
}
