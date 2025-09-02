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

    public GameObject[] objektitjoihinCollideIgnore;

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
                //GameObject instanssi=Instantiate(laukaistavaAsia, kohtajostaLaukaistaan.transform.position, Quaternion.identity);
                GameObject instanssi= ObjectPoolManager.Instance.GetFromPool(laukaistavaAsia,
                    kohtajostaLaukaistaan.transform.position, Quaternion.identity);
               // instanssi.GetComponent<BaseController>().SetPreFap(laukaistavaAsia);



                IgnoraaCollisiotVihollistenValilla(instanssi, gameObject);
                //  GameObject tiili=GameObject.Find("Tilemap");

                

                foreach (GameObject g in objektitjoihinCollideIgnore)
                {
                    IgnoraaCollisiotVihollistenValilla(g, instanssi);
                    foreach (Transform child in g.transform)
                    {
                        GameObject childObject = child.gameObject;
                       // Debug.Log("Child: " + childObject.name);

                        IgnoraaCollisiotVihollistenValilla(childObject, instanssi);

                    }

                    
                }

                //

                Rigidbody2D r =
                instanssi.GetComponent<Rigidbody2D>();
                r.velocity = laukaisuVelocity;
                laukaisusyklilaskuri = 0;
            }
        }
    }
}
