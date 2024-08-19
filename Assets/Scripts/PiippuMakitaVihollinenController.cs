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


    void OnTriggerEnter2D(Collider2D col)
    {
      //  Debug.Log("PiippuMakitaVihollinenController on OnTriggerEnter2D ");


        if (col.CompareTag("alustag"))
        {
            Debug.Log("OnTriggerEnter2D estaPyoriminen(true) ");
            col.gameObject.SendMessage("Explode");
        }

    }
}
