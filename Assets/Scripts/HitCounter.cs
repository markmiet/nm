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

    public bool rekisteroihittivainjosobjektillaEiOleJointtia = false;

    private DissolveMatController p;
    private float dissolveoriginal;
    public void Start()
    {
        p = GetComponent<DissolveMatController>();
        if (p != null)
        {
            dissolveoriginal = p.dissolveamount;
        }
        SaadaDissolveAmountVerrattunaOsumiin();
    }


    private bool JointEhtoToteutuu()
    {
        if (!rekisteroihittivainjosobjektillaEiOleJointtia)
        {
            return true;
        }
        Joint2D[] j =
        GetComponents<Joint2D>();
        foreach (Joint2D jj in j)
        {
            if (jj.enabled && jj.connectedBody != null)
            {
                return false;
            }
        }

        return true;

    }


    public bool RegisterHit()
    {

        if (IsGoingToBeDestroyed())
        {
            return true;
        }

        if (!JointEhtoToteutuu())
        {
            return false;
        }
        KaynnistaParticleEmitti();

        hitCount++;
        if (hitCount >= hitThreshold)
        {
            if (gameObject != null)
                GameManager.Instance.kasvataHighScorea(gameObject);
            RajaytaChildrenit();
            Destroy(gameObject);
            return true;
        }
        SaadaDissolveAmountVerrattunaOsumiin();
        return false;
    }
    //  private float lasttimeexplosion = 0;
    //  private float mindelayexplosions = 1.0f;

    public bool RegisterHit(Vector2 contactPoint)
    {

        if (IsGoingToBeDestroyed())
        {
            return true;
        }
        bool ret = RegisterHit();
        if (teeExplosion && explosion != null)
        {
            KaynnistaParticleEmitti();

            if (explosion != null)
            {
                // if (Time.time- lasttimeexplosion > mindelayexplosions)
                // {
                GameObject instanssi2 = Instantiate(explosion, contactPoint, Quaternion.identity);

                // lasttimeexplosion = Time.time;
                float kesto = Mathf.Min(alivetime, 0.5f);
                Destroy(instanssi2, kesto);
                // }
                Debug.Log("rajaytys");

            }



        }
        if (osumanSavu != null)
        {
            GameObject instanssi2 = Instantiate(osumanSavu, contactPoint, Quaternion.identity);
            Destroy(instanssi2, osumanSavunKesto);
        }
        SaadaDissolveAmountVerrattunaOsumiin();
        return ret;

    }

    public bool tuhoajosollaanKameranVasemmallaPuolella = true;
    public void Update()
    {

        if (tuhoajosollaanKameranVasemmallaPuolella)
        {

            if (gameObject == null)
            {
                Debug.Log("no nulli");
            }
            TuhoaJosOllaanSiirrettyJonkunVerranKameranVasemmallePuolenSalliPieniAlitusJaYlitys(gameObject);


        }

    }


    public void RajaytaChildrenit()
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
                    Vector2 keski = PalautaKaikkienCollidereidenKeskipiste(tt.gameObject);

                    if (keski != Vector2.zero)
                    {
                        GameObject instanssi2 = Instantiate(explosion, keski, Quaternion.identity);
                        float kesto = Mathf.Min(alivetime, 0.5f);
                        Destroy(instanssi2, kesto);


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
            //Destroy(gameObject);
        }
        if (teerajaytaspriteuusiversio)
        {
            Debug.Log("RajaytaSpriteUusiMonimutkaisin " + Time.realtimeSinceStartup);
                 RajaytaSpriteUusiMonimutkaisin(gameObject, uusirajaytyscolumns, uusirajaytysrows, rajahdysvoima, alivetime,
                     rajaytaSpritenExplosion, rajaytaspritenviive, gameJostaRajaytyksenPistelasketaan);
            //Destroy(gameObject);

        }

        //RajaytaUudellaTavalla();

        // }
    }

    public GameObject gameJostaRajaytyksenPistelasketaan;

    public int rajahdysrowcol = 16;
    public float rajahdysvoima = 0.1f;
    public float alivetime = 5.0f;
    public float sirpalemass = 1.0f;

    public float gravityscale = 0.5f;
    public bool adddestroycontroller = true;
    public bool teeBoxCollider2d = true;

    public bool teerajaytasprite = true;
    public bool teeExplosion = false;


    public GameObject rajaytaSpritenExplosion;


    [Tooltip("Eli kun ollaan FadeSlicessa ja ollaan instantioitu rajahdys, tama maaraa ajan jonka explosion kestaa")]
    public float rajaytaspritenviive = 0.5f;

    public bool saadaDissolveamount = false;


    public bool teerajaytaspriteuusiversio = false;

    private void SaadaDissolveAmountVerrattunaOsumiin()
    {
        if (saadaDissolveamount && p != null)
        {
            //float prosentit = ( (float)hitCount / (float)hitThreshold) * 100.0f;
            float prosentit = Mathf.Clamp01(hitCount / (float)hitThreshold) * 100f;
            float maksimidissolve = dissolveoriginal;
            float minimidissolve = 0.0f;
            float uusiarvo = (prosentit / 100.0f) * maksimidissolve;
            p.dissolveamount = uusiarvo;
        }
    }


    public bool kaynnistaParticleEmit = false;



    private Vector2 originellisizerange = Vector2.zero;
    public void KaynnistaParticleEmitti()
    {
        if (kaynnistaParticleEmit)
        {
            OutlineSmokeEmitter o =
            GetComponent<OutlineSmokeEmitter>();
            if (o != null)
            {
                o.emitInUpdate = true;
                //sizeRange

                float prosentit = Mathf.Clamp01(hitCount / (float)hitThreshold);
                //@todoo lipulla
                if (originellisizerange.x == 0 && originellisizerange.y == 0)
                {
                    originellisizerange = o.sizeRange;
                }

                Vector2 uusi = originellisizerange * prosentit;
                o.sizeRange = uusi;

            }
        }
    }

}
