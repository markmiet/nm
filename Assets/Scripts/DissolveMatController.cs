using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveMatController : MonoBehaviour
{
    // Start is called before the first frame update
    //_DissolveAmount
    //_OutlineThickness
    //_DissolveScale
    //_OutLineColor
    //_SpiralStrength
    //_VerticalDissolve

    private SpriteRenderer _spriteRenderers;
    private Material _materials;
    private int _MainTex = Shader.PropertyToID("_MainTex");
    private int _DissolveAmount = Shader.PropertyToID("_DissolveAmount");
    private int _DissolveScale = Shader.PropertyToID("_DissolveScale");
    private int _Tiling = Shader.PropertyToID("_TilingArvo");
    private int _VerticalDissolve = Shader.PropertyToID("_VerticalDissolve");
    void Start()
    {
        _spriteRenderers = GetComponent<SpriteRenderer>();


            // Clone materials to avoid shared material side effects
            _spriteRenderers.material = new Material(_spriteRenderers.material);
            _materials = _spriteRenderers.material;
       

    }

    // Update is called once per frame
    public float dissolveamount = 0.44f;
    public float dissolveScale = 113f;
    public float verficaldissolve = 0.0f;

    void Update()
    {
        _materials.SetFloat(_DissolveAmount, dissolveamount);
        _materials.SetFloat(_DissolveScale, dissolveScale);
        _materials.SetFloat(_VerticalDissolve, verficaldissolve);
        

    }
}
