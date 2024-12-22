using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiippuMakitaVihollinenController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float damagemaarajokaaiheutetaan = 1.0f;

    void OnTriggerEnter2D(Collider2D col)
    {
      //  Debug.Log("PiippuMakitaVihollinenController on OnTriggerEnter2D ");


        if (col.CompareTag("alustag"))
        {

            IDamagedable o =
col.gameObject.GetComponent<IDamagedable>();
            if (o != null)
            {
                o.AiheutaDamagea(damagemaarajokaaiheutetaan);


            }
            else
            {
                //		Debug.Log("alus ja explode mutta ei ookkaan " + col.collider.tag);
            }
        }




    }
}
