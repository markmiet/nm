using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveAmountRelativetoAlusController : BaseController
{
    // Start is called before the first frame update
    private DissolveMatController p;
    private GameObject alus;

    private float dissolveoriginal;

    public GameObject bone8;

    void Start()
    {
        p = GetComponent<DissolveMatController>();
        if (p!=null)
        {
            dissolveoriginal = p.dissolveamount;
        }
        alus = PalautaAlus();
    }
    public float distancemin = 0.0f;//dissolveamountuusi=0.0f
    public float distancemax = 5.0f;//dissolveamountuusi=0.44f

    // Update is called once per frame
    void Update()
    {
        
        if (p!=null)
        {
            /*
            Vector2 distanceVec = bone8.transform.position - alus.transform.position;
            float distance = distanceVec.magnitude;

            // Small distance = dissolveoriginal, large distance = 0
            float dissolveamountuusi = Mathf.Lerp(dissolveoriginal, 0f, Mathf.InverseLerp(0f, distancemax, distance));

            // Clamp so it never goes above dissolveoriginal
            dissolveamountuusi = Mathf.Clamp(dissolveamountuusi, 0f, dissolveoriginal);

            Debug.Log("distance=" + distance + " dissolveamount=" + dissolveamountuusi);
            p.dissolveamount = dissolveamountuusi;
            */
            
            Vector2 distanceVec = bone8.transform.position - alus.transform.position;
            float distance = distanceVec.magnitude;

            // Factor goes 0 at distancemin → 1 at distancemax
            float t = Mathf.Clamp01((distance - distancemin) / (distancemax - distancemin));

            // Lerp from full dissolve to 0
            float dissolveamountuusi = Mathf.Lerp(dissolveoriginal, 0f, t);

            //Debug.Log($"distance={distance} dissolveamount={dissolveamountuusi}");
            p.dissolveamount = dissolveamountuusi;

        }
    }
}
