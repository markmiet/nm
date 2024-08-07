using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : MonoBehaviour
{
    public float skrollimaara = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float deltaAika = Time.deltaTime;
        float maara = deltaAika * skrollimaara;

        transform.position = new Vector2(transform.position.x - maara, transform.position.y);



    }


    void OnCollisionEnter2D(Collision2D col)
    {
     

        if (col.collider.tag == "alustag")
        {
            

          //  Debug.Log("on bonustaaaaaaaaa ");



            Destroy (this.gameObject);
            //pitää 

           // col.otherCollider

            col.otherCollider.gameObject.SendMessage("BonusCollected");

        }


    }
}
