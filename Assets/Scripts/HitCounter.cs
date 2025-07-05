using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCounter : BaseController
{
    // Start is called before the first frame update
    public int hitThreshold = 100;
    public int hitCount = 0;

    public GameObject explosion;

    public GameObject osumanSavu;
    public float osumanSavunKesto = 0.5f;

    public void RegisterHit()
    {
        hitCount++;
        if (hitCount >= hitThreshold)
        {
            GameManager.Instance.kasvataHighScorea(gameObject);
            RajaytaChildrenit();
            Destroy(gameObject);
      
        }
    }
    public void RegisterHit(Vector2 contactPoint)
    {
        RegisterHit();
        if (teeExplosion && explosion != null)
        {
            GameObject instanssi2 = Instantiate(explosion, contactPoint, Quaternion.identity);
            Destroy(instanssi2, alivetime);

        }
        if (osumanSavu!=null)
        {
            GameObject instanssi2 = Instantiate(osumanSavu, contactPoint, Quaternion.identity);
            Destroy(instanssi2, osumanSavunKesto);
        }

    }



    private void RajaytaChildrenit()
    {
        //if (explosion!=null)
        // {
        Transform[] t =
        GetComponentsInChildren<Transform>();
        if (t != null)
        {
            foreach (Transform tt in t)
            {
                //GameObject instanssi2 = Instantiate(explosion, tt.transform.position, Quaternion.identity);
                ChildColliderReporter c =
                tt.gameObject.GetComponent<ChildColliderReporter>();
                if (c != null && teerajaytasprite)
                {
                    c.RajaytaSprite(tt.gameObject, 4, 4, 1.0f, alivetime);
                }
                if (teeExplosion && explosion != null)
                {
                    Vector2 keski=PalautaKaikkienCollidereidenKeskipiste(tt.gameObject);

                    if (keski!=Vector2.zero)
                    {
                        GameObject instanssi2 = Instantiate(explosion, keski, Quaternion.identity);
                        
                        Destroy(instanssi2, alivetime);

                    }
                    else
                    {
                      //  GameObject instanssi2 = Instantiate(explosion, transform.position, Quaternion.identity);
                    }
                   
                   //

                }

            }
        }

        //   RajaytaSprite(gameObject, 8, 8, 1.0f, 5.0f);
        //RajaytaSprite(go, rows, columns, explosionForce, alivetime, -1, false, 0, false, 0.0f, -0.2f, false, null);
        //RajaytaSprite(go, rows, columns, explosionForce, alivetime, -1, false, 0, false, 0.0f, -0.2f, false, null);
        if (teerajaytasprite)
        {
            RajaytaSprite(gameObject, rajahdysrowcol, rajahdysrowcol, rajahdysvoima, alivetime,
sirpalemass, teeBoxCollider2d, 0, false, gravityscale,
  0.0f, adddestroycontroller, explosion);
        }


        //RajaytaUudellaTavalla();

        // }
    }
    public int rajahdysrowcol = 16;
    public float rajahdysvoima = 0.1f;
    public float alivetime = 5.0f;
    public float sirpalemass = 1.0f;

    public float gravityscale = 0.5f;
    public bool adddestroycontroller = true;
    public bool teeBoxCollider2d = true;

    public bool teerajaytasprite = true;
    public bool teeExplosion = false;

}
