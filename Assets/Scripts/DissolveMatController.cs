using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveMatController : BaseController
{

    [Header("Shader Params")]
    public Texture2D maintexuusi;

    // Update is called once per frame
    public float dissolveamount = 0.44f;
    public float dissolveScale = 113f;
    public float verficaldissolve = 0.0f;

    public float spiralStrength = 0.0f;

    [Range(0f, 360f)]
    public float rotationAngle = 0f; // degrees


    // Start is called before the first frame update
    //_DissolveAmount
    //_OutlineThickness
    //_DissolveScale
    //_OutLineColor
    //_SpiralStrength
    //_VerticalDissolve

    private Renderer _spriteRenderers;
    private Material _materials;
    private int _MainTex = Shader.PropertyToID("_MainTex");
    private int _DissolveAmount = Shader.PropertyToID("_DissolveAmount");
    private int _DissolveScale = Shader.PropertyToID("_DissolveScale");
    private int _Tiling = Shader.PropertyToID("_TilingArvo");
    private int _VerticalDissolve = Shader.PropertyToID("_VerticalDissolve");

    private int _OutLineColor = Shader.PropertyToID("_OutLineColor");

    private int _OutlineThickness = Shader.PropertyToID("_OutlineThickness");

    private int _SpiralStrength = Shader.PropertyToID("_SpiralStrength");


    private int _RotationAmount = Shader.PropertyToID("_RotationAmount");



    private ShaderPropertyBlockCache _cache;

    private Dictionary<string, int> _propIDs = new();
    //tehdään vielä

    //  public Color outlineColor;

    //  public float outlinethickness = 0.0f;
    void Start()
    {
        _spriteRenderers = GetComponent<Renderer>();

        _cache = new ShaderPropertyBlockCache(_spriteRenderers);

        // Clone materials to avoid shared material side effects
        //   _spriteRenderers.material = new Material(_spriteRenderers.material);
        //     _materials = _spriteRenderers.material;

        CacheShaderProperties(_spriteRenderers.material);

        alus = PalautaAlus();
        if (maaritaDissolveAmountRelativeToAlus)
        {
            maksimiDissolveAmount = dissolveamount;
          
                _cache.SetFloat(_propIDs["_DissolveAmount"], DissolveAmountRelativeToAlus());
            
        }

        dissisyklilaskuri = sykli;

    }


    public float sykli = 0.5f;
    private float dissisyklilaskuri = 0.0f;

    public float maksimiDissolveAmount;
    public bool maaritaDissolveAmountRelativeToAlus = false;

    public float distancemin = 0.0f;//dissolveamountuusi=0.0f
    public float distancemax = 5.0f;//dissolveamountuusi=0.44f

    private GameObject alus;

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



    //tempit

    /*
    private Texture2D exmaintexuusi;

    private float exdissolveamount = 0.44f;
    private float exdissolveScale = 113f;
    private float exverficaldissolve = 0.0f;

    private float exspiralStrength = 0.0f;


    private float exrotationAngle = 0f; // degrees
    */

    public Color outlineColor;
    public bool asetaoutlinecolor = false;
    private void Update()
    {
        try
        {
            dissisyklilaskuri += Time.deltaTime;

            if (dissisyklilaskuri>=sykli)
            {
                dissisyklilaskuri = 0.0f;
                if (maaritaDissolveAmountRelativeToAlus)
                {

                    _cache.SetFloat(_propIDs["_DissolveAmount"], DissolveAmountRelativeToAlus());
                }
                else
                {

                    _cache.SetFloat(_propIDs["_DissolveAmount"], dissolveamount);
                }

                if (outlineColor != null && asetaoutlinecolor)
                {
                    _cache.SetColor(_propIDs["_OutLineColor"], outlineColor);

                }

                _cache.SetFloat(_propIDs["_DissolveScale"], dissolveScale);
                _cache.SetFloat(_propIDs["_VerticalDissolve"], verficaldissolve);
                _cache.SetFloat(_propIDs["_SpiralStrength"], spiralStrength);
                _cache.SetFloat(_propIDs["_RotationAmount"], rotationAngle);

                if (maintexuusi != null)
                    _cache.SetTexture(_propIDs["_MainTex"], maintexuusi);
            }


        }

        catch (KeyNotFoundException)
        {
            Debug.LogWarning($"PropertyID xxx not found in _floats dictionary. {gameObject.name} ");
        }

    }

    private float DissolveAmountRelativeToAlus()
    {

        Vector2 distanceVec = transform.position - alus.transform.position;
        float distance = distanceVec.magnitude;

        // Factor goes 0 at distancemin → 1 at distancemax
        float t = Mathf.Clamp01((distance - distancemin) / (distancemax - distancemin));

        // Lerp from full dissolve to 0
        float dissolveamountuusi = Mathf.Lerp(maksimiDissolveAmount, 0f, t);
        return dissolveamountuusi;

    }

    /*
    void Updatevvv()
    {
        if (!Mathf.Approximately(dissolveamount, exdissolveamount))
        {
            _materials.SetFloat(_DissolveAmount, dissolveamount);
            exdissolveamount = dissolveamount;
        }

        if (!Mathf.Approximately(dissolveScale, exdissolveScale))
        {
            _materials.SetFloat(_DissolveScale, dissolveScale);
            exdissolveScale = dissolveScale;
        }

        if (!Mathf.Approximately(verficaldissolve, exverficaldissolve))
        {
            _materials.SetFloat(_VerticalDissolve, verficaldissolve);
            exverficaldissolve = verficaldissolve;
        }

        if (!Mathf.Approximately(spiralStrength, exspiralStrength))
        {
            _materials.SetFloat(_SpiralStrength, spiralStrength);
            exspiralStrength = spiralStrength;
        }

        if (maintexuusi != exmaintexuusi && maintexuusi != null)
        {
            _materials.SetTexture(_MainTex, maintexuusi);
            exmaintexuusi = maintexuusi;
        }

        if (!Mathf.Approximately(rotationAngle, exrotationAngle))
        {
            _materials.SetFloat(_RotationAmount, rotationAngle);
            exrotationAngle = rotationAngle;
        }
    }
    */

}
