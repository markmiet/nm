using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlaosaScripti : MonoBehaviour
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
        Debug.Log("alaosa collidoi");   
        if (col.collider.tag == "tiilitag")
        {
            //   Destroy(transform.parent);

            //Destroy (col.gameObject)

            transform.parent.gameObject.SendMessage("Liu");
        }
    }

}
