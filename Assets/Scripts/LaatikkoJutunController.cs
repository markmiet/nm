using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaatikkoJutunController : BaseController
{
    // Start is called before the first frame update

    public GameObject kohtajostaLaukaistaan;
    public GameObject laukaistavaAsia;
    public float laukaisuvali = 1.0f;
    private float laukaisusyklilaskuri = 0.0f;
    public Vector2 laukaisuVelocity = new Vector2(0, -1);
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!OnkoOkToimiaUusi(gameObject))
        {
            return;
        }
        TuhoaJosOllaanSiirrettyJonkunVerranKameranVasemmallePuolenSalliPieniAlitusJaYlitys(gameObject);

        if (kohtajostaLaukaistaan!=null && laukaistavaAsia && laukaisuvali>0.0f)
        {
            laukaisusyklilaskuri += Time.deltaTime;
            if (laukaisusyklilaskuri >= laukaisuvali)
            {
                GameObject instanssi=Instantiate(laukaistavaAsia, kohtajostaLaukaistaan.transform.position, Quaternion.identity);
                IgnoraaCollisiotVihollistenValilla(instanssi, gameObject);
                GameObject tiili=GameObject.Find("Tilemap");

                IgnoraaCollisiotVihollistenValilla(tiili, instanssi);

                Rigidbody2D r =
                instanssi.GetComponent<Rigidbody2D>();
                r.velocity = laukaisuVelocity;
                laukaisusyklilaskuri = 0;
            }
        }
    }
}
