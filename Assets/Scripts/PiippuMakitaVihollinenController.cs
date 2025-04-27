using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiippuMakitaVihollinenController : BaseController, IExplodable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explode()
    {
        GameObject parentObject = transform.parent?.gameObject;

        if (parentObject != null)
        {
            //Debug.Log("Parent GameObject: " + parentObject.name);
            IExplodable ie = parentObject.GetComponent<IExplodable>();
            if (ie!=null)
            {
                ie.Explode();
            }
        }
        else
        {
            //Debug.Log("This GameObject has no parent.");
        }
    }

    public float damagemaarajokaaiheutetaan = 1.0f;





}
