using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildColliderReporter : BaseController 
{
    private HitCounter parent;
    public GameObject explosion;
    private HitCounter tamaHitCounter;

    protected virtual void Start()
    {
        parent = GetComponentInParent<HitCounter>();
        tamaHitCounter = GetComponent<HitCounter>();
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
        tamaHitCounter?.RegisterHit();
        if (explosion!=null)
        {
            GameObject instanssi2 = Instantiate(explosion, contactPoint, Quaternion.identity);
        }       
    }

    public int PalautaHittienMaara()
    {
        if (parent != null)
        {
            return parent.hitThreshold;
        }
        else if (tamaHitCounter != null)
        {
            return tamaHitCounter.hitThreshold;
        }
        return 1;
    }

}
