using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IhmematoParticleController : MonoBehaviour
{

    private ShaderPropertyBlockCache _cache;

    private Dictionary<string, int> _propIDs = new();

    private int _CameraFarFadeDistance = Shader.PropertyToID("_CameraFarFadeDistance");


    public float cameraFarFadeDistanceMax = 10;
    public float cameraFarFadeDistanceMin = 1;

    private HitCounter hc;

    //Material m;


    // Start is called before the first frame update
    void Start()

    {
        hc = GetComponent<HitCounter>();

        ParticleSystem ps = GetComponent<ParticleSystem>();
        ParticleSystemRenderer renderer = ps.GetComponent<ParticleSystemRenderer>();
        _cache = new ShaderPropertyBlockCache(renderer);

       // m = renderer.material;

        CacheShaderProperties(renderer.material);
    }



    void CacheShaderProperties(Material mat)
    {
        Shader shader = mat.shader;
        int count = shader.GetPropertyCount();
        for (int i = 0; i < count; i++)
        {
            string name = shader.GetPropertyName(i);
            _propIDs[name] = Shader.PropertyToID(name);

            Debug.Log("name=" + name);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (hc!=null)
        {
            float prossanollaviivayksi=
            hc.PalautaOsumaProsenttiNollaViiva1();

            //public float cameraFarFadeDistanceMax = 10;
            //public float cameraFarFadeDistanceMin = 1;

             float laske = Mathf.Lerp(cameraFarFadeDistanceMin, cameraFarFadeDistanceMax, prossanollaviivayksi);

            _cache.SetFloat(_propIDs["_CameraFadingEnabled"], 1.0f);


            _cache.SetFloat(_propIDs["_CameraFarFadeDistance"], laske);

            /*
            var psr = GetComponent<ParticleSystemRenderer>();
            var block = new MaterialPropertyBlock();
            psr.GetPropertyBlock(block);
            block.SetFloat("_CameraFarFadeDistance", laske);
            */
            // psr.SetPropertyBlock(block);
            //m.SetInt("_CameraFadingEnabled", 1);
           
           // m.SetFloat("_CameraFarFadeDistance", laske);
        }

    }
}
