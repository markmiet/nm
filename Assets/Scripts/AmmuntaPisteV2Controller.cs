using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmuntaPisteV2Controller : BaseController
{
    // Start is called before the first frame update
    public GameObject[] ammus;
    public float laukaisusykli = 1.0f;

    private float laukaisusyklilaskuri = 0.0f;
    

    private GameObject PalautaObjekti()
    {
        int randomIndex = Random.Range(0, ammus.Length);

        GameObject a = ammus[randomIndex];
        return a;
    }

    private Rigidbody2D ok;

    void Start()
    {
        ok = GetComponentInParent<Rigidbody2D>();

    }
   
    void Update()
    {
        if (ok!=null)
        {
            if (!ok.simulated)
            {
                return;
            }
        }

        //@todo create logic
          if (!OnkoOkToimiaUusi(gameObject)) {
            return;
        }

        laukaisusyklilaskuri += Time.deltaTime;

        if (laukaisusyklilaskuri>=laukaisusykli )
        {
            laukaisusyklilaskuri = 0.0f;

            //
            GameObject ins = Instantiate(PalautaObjekti(), transform.position, Quaternion.identity);
            IgnoraaCollisiotVihollistenValilla(ins, transform.parent.gameObject);
   


        }
    }
}
