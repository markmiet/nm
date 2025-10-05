using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraGunLauncher : BaseController
{

    public GameObject ammo;
    public float syklivali = 10.0f;
    public int kappalemaarajokalaukaistaan = 1;
    private int kpl = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private float l = 0.0f;
    // Update is called once per frame
    void Update()
    {
        if (kpl>=kappalemaarajokalaukaistaan)
        {
            BaseDestroy();
            return;
        }

        l += Time.deltaTime;

        if (l>=syklivali)
        {

            GameObject go = Instantiate(ammo, PalautaAlus().transform.position, Quaternion.identity);
            //velocityn hoitaa tämä objekti

            //IgnoraaCollisiotVihollistenValilla(PalautaAlus(), go));
            l = 0.0f;
            kpl++;
        }
    }
}
