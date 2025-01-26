using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageMittariLineRendererController : BaseController
{
    // Start is called before the first frame update
    //private LineRenderer lineRenderer;
    void Start()
    {
     //   lineRenderer = GetComponent<LineRenderer>();
     //   lineRenderer.positionCount = 2; // Two points for a single line
    }

    // Update is called once per frame
    void Update()
    {
        // lineRenderer.SetPosition(0,)
    }

    public void SetDamage(float currentDamage, float maxDamage, Camera sormikamera)
    {
        Vector3 min = GetOhjausCameraMinWorldPosition(sormikamera);
        Vector3 max = GetOhjausCameraMaxWorldPosition(sormikamera);

        if (currentDamage > maxDamage)
        {
            currentDamage = maxDamage;
        }
        //100

        //      float kerroin = 100 / maxDamage;
        //        float nykyarvo = currentDamage * kerroin;

        float arvo = currentDamage / maxDamage;

        float leveys = Mathf.Abs(max.x - min.x);

        float laskettuleveys = arvo*leveys;


        transform.localScale= new Vector3(arvo, transform.localScale.y, transform.localScale.y);

    }


}