using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildColliderReporter : BaseController 
{
    private HitCounter parent;
    public GameObject explosion;

    void Start()
    {
        parent = GetComponentInParent<HitCounter>();
    }

    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        parent?.RegisterHit();
    }
    */

    public void RegisterHit(Vector2 contactPoint)
    {
        parent?.RegisterHit();
        if (explosion!=null)
        {
            GameObject instanssi2 = Instantiate(explosion, contactPoint, Quaternion.identity);
        }

        
    }
}
