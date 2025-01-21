using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiipiolioController : BaseController
{
    // Start is called before the first frame update
    public float siivetrotatemax = 5.0f;
    public float siivetrotatemin = -5.0f;
    private float rotationTime = 0f;       // Timer to control the rotation
    public float rotatetimeseconds = 2.0f;//sekkaa
    private GameObject alus;
    void Start()
    {
        BoxCollider2D[] b =
        GetComponentsInChildren<BoxCollider2D>();
        BoxCollider2D[] b2 =
        GetComponentsInChildren<BoxCollider2D>();
        foreach (BoxCollider2D bb in b)
        {
            foreach (BoxCollider2D bb2 in b2)
            {
                Physics2D.IgnoreCollision(bb, bb2, true);
            }
        }
        IgnoReCollisions();
        alus = PalautaAlus();


    }
    private void IgnoReCollisions()
    {
        BoxCollider2D[] b =
GetComponents<BoxCollider2D>();
        BoxCollider2D[] b2 =
        GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D bb in b)
        {
            foreach (BoxCollider2D bb2 in b2)
            {
                Physics2D.IgnoreCollision(bb, bb2, true);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        TuhoaMuttaAlaTuhoaJosOllaanEditorissa(gameObject);
        rotationTime += Time.deltaTime;
        float t = Mathf.PingPong(rotationTime / rotatetimeseconds, 1f);
        float currentRotationsiivet = Mathf.Lerp(siivetrotatemin, siivetrotatemax, t);

        transform.localRotation = Quaternion.Euler( 0, 0, currentRotationsiivet);  // Z-axis rotation (for 2D)
                                                                                  //  haukisiivet.transform.localPosition = uusloca;

        //kulma kohti alusta
        //
        /*
        if (alus!=null)
        {
            Vector3 direction = alus.transform.position - transform.position;
            // Calculate target rotation
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

            // Smoothly rotate towards the target
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        }
        */

    }



}
