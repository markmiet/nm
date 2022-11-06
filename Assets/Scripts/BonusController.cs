using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : MonoBehaviour
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
     

        if (col.collider.tag == "alustag")
        {
            

            Debug.Log("on bonustaaaaaaaaa ");



            Destroy (this.gameObject);
            //pitää 

           // col.otherCollider

            col.otherCollider.gameObject.SendMessage("BonusCollected");

        }


    }
}
