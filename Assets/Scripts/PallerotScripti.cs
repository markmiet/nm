using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PallerotScripti : BaseController
{

    // Start is called before the first frame update
    void Start()
    {
        IgnoreChildCollisions(transform);

    }

    // Update is called once per frame
 
    public void Update()
    {
        TuhoaJosVaarassaPaikassa(gameObject);

        /*
        SpriteRenderer[] ss =
        GetComponentsInChildren<SpriteRenderer>();

        if (ss!=null)
        {
            foreach (SpriteRenderer s in ss)
            {
                if (s.isVisible)
                {
                    return;
                }
            }
        }
        Destroy(gameObject);

        */
        //gameObject.transform.ge
        /*
        if (IsObjectLeftOfCamera(gameObject,10.0f))
        {
            Destroy(gameObject);
        }
        
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform child = gameObject.transform.GetChild(i);
            GameObject childObject = child.gameObject;
            bool vasemmalla=IsObjectLeftOfCamera(childObject,10.0f);
            if (!vasemmalla)
            {
                
                return;
            }
            Debug.Log("ei vasen"+child.transform.position.x+" kamerax="+ Camera.main.transform.position.x);

            // Debug.Log("Child GameObject: " + childObject.name);
        }
        Destroy(gameObject);
        */

    }






}
