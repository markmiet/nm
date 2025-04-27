using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : BaseController
{
    public float rotationTime = 4f; // Time for a full 360-degree rotation
    private float elapsedTime = 0f;

    // Start is called before the first frame update
    private AudioplayerController ad;
    void Start()
    {
        ad = FindObjectOfType<AudioplayerController>();
        

    }

    void OnBecameInvisible()
    {
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Calculate the Y rotation angle (360 degrees)
        float angle = (elapsedTime / rotationTime) * 360f;

        // Apply rotation
      //  transform.rotation = Quaternion.Euler(0f, angle, 0f);

        // Reset elapsed time after a full rotation (optional)
        if (elapsedTime >= rotationTime)
        {
            elapsedTime = 0f;
        }



    }

    bool onkoAlukseenJoTormatty = false;
    public void OnTriggerEnter2D(Collider2D col)
    {
        
      //  Debug.Log($"This object's collider: {col.collider.name}");
      //  Debug.Log($"Other object's collider: {col.otherCollider.name}");
        if (!onkoAlukseenJoTormatty && col.CompareTag("alustag"))
        {
            onkoAlukseenJoTormatty = true;
            GetComponent<Collider2D>().enabled = false;//tämän pitäisi riittää
            RajaytaSprite(gameObject, 3, 3, 2.0f, 1f);
            Destroy(this.gameObject);
            ad.BonusPlay();

            AlusController myScript = col.gameObject.GetComponent<AlusController>();
            if (myScript != null)
            {
                myScript.BonusCollected();

            }
            else
            {
                Debug.Log("aluscontrollerin skripti oli null");
            }

        }
    }

    /*

            void OnCollisionEnter2D(Collision2D col)
    {

        Debug.Log($"This object's collider: {col.collider.name}");
        Debug.Log($"Other object's collider: {col.otherCollider.name}");

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
            ad.BonusPlay();

            AlusController myScript = col.otherCollider.gameObject.GetComponent<AlusController>();
            if (myScript != null)
            {
                myScript.BonusCollected();

            }
            else
            {
                Debug.Log("aluscontrollerin skripti oli null");
            }

            RajaytaSprite(gameObject, 3, 3, 2.0f, 2f);
            Destroy(this.gameObject);

            //  Debug.Log("on bonustaaaaaaaaa ");

            //   Debug.Log("bonus keratty");

            //pitää 

            // col.otherCollider


        }

    }
    */
}
