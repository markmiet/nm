using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankkiController : HitCounter
{



        //public float 


        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!OnkoOkToimiaUusi(gameObject))
        {
            return;
        }
    }
}
