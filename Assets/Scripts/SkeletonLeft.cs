using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonLeft : BaseController
{
    // Start is called before the first frame update
    SkeletonController sc;
    void Start()
    {
       sc= GetComponentInParent<SkeletonController>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool left;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (IsGoingToBeDestroyed()) {
            return;
        }



        if (col.tag.Contains("alustag"))
        {
            return;
        }


        if (sc.vaihdasuuntaa && !sc.stoppaa && !col.tag.Contains("skeletonvihollinen") &&
            !col.tag.Contains("makitavihollinenammus") && (col.tag.Contains("vihollinen")
            || col.tag.Contains("tiili")))
        {
            sc.stoppaa = true;
            sc.stoppauslaskuri = 0.0f;
            sc.Tormatty(left);

        }
    }
}
