using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KarkiControler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "tiilitag")
        {
        //   Destroy(transform.parent);
         
            //Destroy (col.gameObject)
        }
    }


}
