using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutGlowShaderOmController : MonoBehaviour
{

    [ColorUsage(true, true)]
    public Color clowvari;

    private int _GlowColor = Shader.PropertyToID("_GlowColor");

    private SpriteRenderer _spriteRenderers;
    //private Material instancedMaterial;
    // Start is called before the first frame update
    public float salku = 0.1f;
    public float sloppu = 0.99f;

    private int _SubiYla = Shader.PropertyToID("_SubiYla");

    private int _SubiALa = Shader.PropertyToID("_SubiALa");



    
    void Start()
    {
        _spriteRenderers = GetComponent<SpriteRenderer>();

        //  Material klooni=new M_spriteRenderers.material);
        // _spriteRenderers.material = new Material(_spriteRenderers.material);


        Material instancedMaterial  = new Material(_spriteRenderers.material);
        _spriteRenderers.material = instancedMaterial;

       // _materials = _spriteRenderers.material;

        //        _materials[i] = _spriteRenderers[i].material;


       // _spriteRenderers[i].material = new Material(_spriteRenderers[i].material);
       // _materials[i] = _spriteRenderers[i].material;
    }

    // Update is called once per frame
    void Update()
    {
        _spriteRenderers.material.SetColor(_GlowColor, clowvari);

        _spriteRenderers.material.SetFloat(_SubiYla, salku);
        _spriteRenderers.material.SetFloat(_SubiALa, sloppu);
        //spriteRenderer.material.SetColor(GlowColorID, glowColor);



    }
}
