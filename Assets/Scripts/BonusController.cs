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

      //  transform.position = new Vector2(transform.position.x - maara, transform.position.y);



    }

    bool onkoAlukseenJoTormatty = false;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (onkoAlukseenJoTormatty)
        {
            Debug.Log("on mahdollista");
            return;
        }
        if (col.otherCollider.tag=="alustag")
        {
            Debug.Log("on mika");
        }

        if (col.collider.tag == "alustag")
        {
    
            onkoAlukseenJoTormatty = true;

            // col.otherCollider.gameObject.SendMessage
            // col.otherCollider.gameObject.SendMessage("BonusCollected");

            Destroy(this.gameObject);
            AlusController myScript = col.otherCollider.gameObject.GetComponent<AlusController>();
            if (myScript!=null)
            {
                myScript.BonusCollected();
            }
            else
            {
              //  Debug.Log("aluscontrollerin skripti oli null");
            }

            //  Debug.Log("on bonustaaaaaaaaa ");

         //   Debug.Log("bonus keratty");

            //pitää 

           // col.otherCollider


        }


    }
}
