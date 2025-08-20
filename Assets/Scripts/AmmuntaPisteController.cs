using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmuntaPisteController : BaseController
{
    // Start is called before the first frame update
    public GameObject ammus;
    public float laukaisusykli = 1.0f;

    private float laukaisusyklilaskuri = 0.0f;
    

    void Start()
    {
        
    }
   
    void Update()
    {
        //@todo create logic


        laukaisusyklilaskuri += Time.deltaTime;

        if (laukaisusyklilaskuri>=laukaisusykli )
        {
            laukaisusyklilaskuri = 0.0f;

            //
            GameObject ins = Instantiate(ammus, transform.position, Quaternion.identity);
            IgnoraaCollisiotVihollistenValilla(ins, transform.parent.gameObject);
   


        }
    }
}
