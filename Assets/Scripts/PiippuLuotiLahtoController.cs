using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiippuLuotiLahtoController : BaseController
{
    public GameObject luoti;
    public float ammusVoima = 2.0f;
    public float tulinopeus = 2.0f;
    private float seuraavaTuliAika = 0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!OnkoOkToimiaUusi(gameObject.transform.parent.gameObject))
        {
            return;
        }

        if (Time.time >= seuraavaTuliAika)
        {
            Ammu();
            seuraavaTuliAika = Time.time + tulinopeus;
        }
    }
    void Ammu()
    {

        if (luoti == null)
        {
            return;
        }

        Vector2 ve = new Vector2(ammusVoima, 0);
        //GameObject Instantiate(ammus);



        GameObject instanssi = Instantiate(luoti, transform.position, Quaternion.identity);
        IgnoraaCollisiotVihollistenValilla(instanssi, gameObject);

        GameObject obj = PalautaTilemap();
        IgnoraaCollisiotVihollistenValilla(instanssi, obj);

        //   PalliController p = instanssi.GetComponent<PalliController>();
        //   p.alusGameObject = alusGameObject;

        instanssi.GetComponent<Rigidbody2D>().velocity = ve;
        if (instanssi.GetComponent<PiippuluotiController>()!=null)
            instanssi.GetComponent<PiippuluotiController>().ve = ve;

    }

}
