using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaapaalloRunkoController : BaseController
{
    // Start is called before the first frame update
    public GameObject vasensilmapaikka;
    public GameObject oikeasilmapaikka;
    public GameObject nenapaikka;

    public GameObject silmahehkuParticle;
    public GameObject ammus;
    public float ammuntasykli = 1.0f;
    void Start()
    {
        if (silmahehkuParticle!=null)
        {
            //Instantiate(silmahehkuParticle, vasensilmapaikka.transform.position, Quaternion.identity);
            GameObject inst = Instantiate(silmahehkuParticle, vasensilmapaikka.transform.position, silmahehkuParticle.transform.rotation, vasensilmapaikka.transform);

            inst.transform.localPosition = Vector3.zero;
            inst.transform.localScale = silmahehkuParticle.transform.localScale;

            ParticleSystem ps = inst.GetComponent<ParticleSystem>();
            ps.Play();
        }



    }
    private float lasku = 0;
    // Update is called once per frame
    void Update()
    {
        lasku += Time.deltaTime;

        if (lasku>=ammuntasykli)
        {
            lasku = 0;
            Vector2 vv = palautaAmmuksellaVelocityVector(PalautaAlus(), 5.0f);
            if (ammus != null)
            {
                GameObject instanssihylsy = Instantiate(ammus, vasensilmapaikka.transform.position, Quaternion.identity);
                IgnoraaCollisiotVihollistenValilla(instanssihylsy, gameObject);
                Rigidbody2D rb = instanssihylsy.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = vv;
                }
            }
        }
    }
}
