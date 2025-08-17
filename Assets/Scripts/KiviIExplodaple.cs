using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiviIExplodaple : BaseController, IExplodable
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (explode && explodeHeti)
        {
            RajaytaSprite(gameObject, 4, 4, 1.0f, 0.8f);
            /*
            RajaytaSprite(gameObject, 4,4,0.01f,0.5f,0.1f,true,0,false,0.0f,
            0,false,null,false, "makitavihollinenexplodetag", "Default"
            );
            */
            if (prefabExplosion != null)
            {
                GameObject raj = Instantiate(
        prefabExplosion, transform.position, Quaternion.identity);
                Destroy(raj, 1.0f);
            }
            Destroy(gameObject);
        }
        else
        {
            TuhoaJosOllaanSiirrettyJonkunVerranKameranVasemmallePuolenSalliPieniAlitusJaYlitys(gameObject);
        }
    }


    public GameObject prefabExplosion;
    public bool explode = false;
    public bool explodeHeti = false;
    public void Explode()
    {
        if (!explode || explodeHeti)
        {
            return;
        }
        RajaytaSprite(gameObject, 8, 8, 4.0f, 0.8f);

        if (prefabExplosion != null)
        {
            GameObject raj = Instantiate(
    prefabExplosion, transform.position, Quaternion.identity);
            Destroy(raj, 1.0f);
        }

        Destroy(gameObject);

    }

    void OnBecameInvisible()
    {
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;
        //Explode();
        Destroy(gameObject);
    }

    public float nopeusjokapitaaYlittaaJottaKaikkituhoutuu = 1.0f;
    void OnCollisionEnter2D(Collision2D col)
    {
        //tiilivihollinentag on se oikea tiili
        //tämä onkin tiilivihollinenkivitag
        float relativeVelocity = col.relativeVelocity.magnitude;
       // Debug.Log("kivihomman relativeVelocity=" + relativeVelocity);

        if (relativeVelocity>= nopeusjokapitaaYlittaaJottaKaikkituhoutuu && col.collider.CompareTag("tiilivihollinenkiviexplodetag"))
        {
            JointBreakHandler j=
            transform.parent.GetComponentInChildren<JointBreakHandler>();
            if (j!=null)
            {
                //
                j.BreakAllJoints();
            }
        }

    }


}