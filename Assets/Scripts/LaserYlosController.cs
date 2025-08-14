using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserYlosController : BaseController
{
    // Start is called before the first frame update

    /*
    public float tilinx = 1.0f;
    public float tiliyx = 1.0f;

    public float offsetx = 0.0f;
    public float offsety = 0.0f;
    */
    public float offsetkerto = 2.0f;
    private Renderer rend;

    public bool muutax = false;
    public bool muutay = false;

    private LaserController lc;
    private SpriteRenderer sr;
    void Start()
    {

         rend = GetComponent<Renderer>();
        lc = GetComponent<LaserController>();
        sr = GetComponent<SpriteRenderer>();

    }

    public Vector2 texturescale = new Vector2(1, 1);

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = rend.material.GetTextureOffset("_MainTex");
        if (muutax)
            offset.x += Time.deltaTime * offsetkerto;

        if (muutay)
            offset.y += Time.deltaTime * offsetkerto;
        
        if (muutay || muutax)
        {
            rend.material.SetTextureOffset("_MainTex", offset);
            rend.material.SetTextureScale("_MainTex", texturescale);

        }

        
        //rend.material.SetColor("_Color", vari);
        /*
        Material mat = rend.material; // sharedMaterial to avoid runtime instance
        Shader shader = mat.shader;

        int propertyCount = shader.GetPropertyCount();
        Debug.Log($"Shader: {shader.name}  Properties: {propertyCount}");

        for (int i = 0; i < propertyCount; i++)
        {
            string propName = shader.GetPropertyName(i);
            var propType = shader.GetPropertyType(i);
            Debug.Log($"{i}: {propName} ({propType})");
        }
        */
        if (lc!=null)
        {
            if (lc.olenklooni)
            {
                sr.color = kloonivari;
            }
            else
            {
                sr.color = originelliVari;
            }
        }

    }
    public Color originelliVari = Color.blue;
    public Color kloonivari = Color.red;
}
