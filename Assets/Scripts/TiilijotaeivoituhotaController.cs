using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiilijonkavoituhotaController : BaseController /*, IExplodable*/
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //TuhoaJosVaarassaPaikassa(gameObject);
        Tuhoa(gameObject);

    }
    public float explosionForce = 10f; // Base force magnitude

    public void Explode()
    {
        Destroy(gameObject);

        RajaytaSprite(gameObject, 10, 10, explosionForce, 0.3f);
        //mitä jos vain aluksen ampumilla voi siirtää
        //ei optioneilla niin ei tule ongelmaa
        //tai sitten erillinen option painike jolla optiot saa siirrettyä suoraan eteen :)
        //eli togle painike optioille
        //vai oisko antibonusbutton jonka ottamalla kaikki bonukset häviääkin pois
        //yleensä vitutus mutta joskus hyöty

        //eikun se autofire button on off
    }

}
