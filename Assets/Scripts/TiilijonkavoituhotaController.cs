using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiilijonkavoituhotaController : BaseController /*, IExplodable*/
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float explosionForce = 10f; // Base force magnitude

    public void Explode()
    {
        Destroy(gameObject);

        RajaytaSprite(gameObject, 10, 10, explosionForce, 0.3f);
    }

}
