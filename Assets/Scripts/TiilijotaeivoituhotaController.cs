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
        //mit� jos vain aluksen ampumilla voi siirt��
        //ei optioneilla niin ei tule ongelmaa
        //tai sitten erillinen option painike jolla optiot saa siirretty� suoraan eteen :)
        //eli togle painike optioille
        //vai oisko antibonusbutton jonka ottamalla kaikki bonukset h�vi��kin pois
        //yleens� vitutus mutta joskus hy�ty

        //eikun se autofire button on off
    }

}
