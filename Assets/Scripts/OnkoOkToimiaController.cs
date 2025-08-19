using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnkoOkToimiaController : BaseController
{

    public bool estaChilderienTormayksetToisiinsa = true;
    // Start is called before the first frame update
    void Start()
    {
        if (estaChilderienTormayksetToisiinsa)
        {
            IgnoraaChildienCollisiot();
        }
        
    }
    public bool voikotoimia = false;

    // Update is called once per frame
    public float viive = 0.5f;//@todoo randomisoi
    private float viivelaskuri = 0.0f;
    public bool toimiviiveella = false;
    void Update()
    {
        voikotoimia = OnkoOkToimiaUusiKasitteleMyosChildienRigidBodyt(gameObject);
        if (toimiviiveella)
        {
            if (voikotoimia)
            {
                viivelaskuri += Time.deltaTime;
            }
            if (voikotoimia && viivelaskuri < viive)
            {

                Rigidbody2D[] rigidbodies = gameObject.GetComponentsInChildren<Rigidbody2D>();
                foreach (Rigidbody2D r in rigidbodies)
                {
                    if (r != null)
                    {
                        r.constraints |= RigidbodyConstraints2D.FreezePositionY;//freeze y
                    }
                }
            }
            else if (voikotoimia && viivelaskuri >= viive)
            {
                Rigidbody2D[] rigidbodies = gameObject.GetComponentsInChildren<Rigidbody2D>();
                foreach (Rigidbody2D r in rigidbodies)
                {
                    if (r != null)
                    {
                        // Unlock Y position
                        r.constraints &= ~RigidbodyConstraints2D.FreezePositionY;//unlock freeze y
                    }
                }
            }

        }



    }

    
}
