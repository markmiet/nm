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

    public GameObject bonus;
    public bool teebonus = false;
   // public bool teebonuschildrenille = false;
    
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

    public GameObject hitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan;
    public float kestoaikahitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan;
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
            if (teebonus && bonus!=null && lastkontantipointti!=Vector2.zero)
            {
                GameObject insbonus = Instantiate(bonus, lastkontantipointti, Quaternion.identity);
              

            }

            if (hitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan!=null)
            {
                GameObject inskeski = Instantiate(hitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan, transform.position, Quaternion.identity);
                Destroy(inskeski, kestoaikahitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan);
                ColliderExtremes c =
                GetComponent<ColliderExtremes>();
                if (c!=null)
                {
                    ColliderCorners corners = c.GetCorners();

                    GameObject ins = Instantiate(hitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan,
                        corners.lowerLeft, Quaternion.identity);
                    Destroy(ins, kestoaikahitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan);

                    GameObject ins2 = Instantiate(hitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan,
                        corners.lowerRight, Quaternion.identity);
                    Destroy(ins2, kestoaikahitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan);


                    GameObject ins3 = Instantiate(hitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan,
                        corners.upperLeft, Quaternion.identity);
                    Destroy(ins3, kestoaikahitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan);

                    GameObject ins4 = Instantiate(hitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan,
                        corners.upperRight, Quaternion.identity);
                    Destroy(ins4, kestoaikahitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan);


                }

            }
           // BaseDestroy();
            return true;
        }
        SaadaDissolveAmountVerrattunaOsumiin();
        return false;
    }
    //  private float lasttimeexplosion = 0;
    //  private float mindelayexplosions = 1.0f;



    private Vector2 lastkontantipointti = Vector2.zero;


    public bool RegisterHit(Vector2 contactPoint)
    {

        if (IsGoingToBeDestroyed())
        {
            return true;
        }
        lastkontantipointti = contactPoint;
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
             //   Debug.Log("rajaytys");

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

    public bool teekamerashake = false;
    public void Update()
    {
 


        if (tuhoajosollaanKameranVasemmallaPuolella)
        {
            TuhoaJosOllaanSiirrettyJonkunVerranKameranVasemmallePuolenSalliPieniAlitusJaYlitys(gameObject);

            OnkoOkToimiaUusi(gameObject);
            
        }

    }


    public float rajaytaspritenScaleFactorProsentti = 100.0f;


    public int childrenienmaaranrajoArvoJottaRajaytetaanVainJokaToinen = 15;

    private bool onkoChildreneitaNiinPaljonEttaRajaytaVainJokaToinen()
    {
        int maara =0;
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
                    maara++;
                }
                if (c != null && teerajaytaspriteuusiversio)
                {
                    maara++;
                }
            }
        }
        return maara > childrenienmaaranrajoArvoJottaRajaytetaanVainJokaToinen;
    }


    public void RajaytaChildrenit()
    {
        if (teekamerashake)
        {
            //     StartCoroutine(CameraShake.Instance.Shake(1f, 0.5f));
            CameraShake.Instance.ShakeYourAssBaby();


        }
        bool tuhoaminenkaynnissa = false;
        //if (explosion!=null)
        // {
        bool jokatoinen = onkoChildreneitaNiinPaljonEttaRajaytaVainJokaToinen();
        bool raj = true;
        Transform[] t =
        GetComponentsInChildren<Transform>();
        //bool childbonustehty = false;
        if (t != null)
        {
            foreach (Transform tt in t)
            {
         /*
                if (teebonuschildrenille && bonus!=null && !childbonustehty)
                {
                    GameObject insbonus = Instantiate(bonus, tt.position, Quaternion.identity);
                    childbonustehty = true;

                }
                */


                //GameObject instanssi2 = Instantiate(explosion, tt.transform.position, Quaternion.identity);
                ChildColliderReporter c =
                tt.gameObject.GetComponent<ChildColliderReporter>();
                if (c != null && teerajaytasprite)
                {
                    c.RajaytaSprite(tt.gameObject, 4, 4, 1.0f, alivetime);

                    // c.RajaytaSpriteUusiMonimutkaisin(tt.gameObject, 4,4, 1.0f, alivetime);

                    if (jokatoinen)
                    {
                        if (raj)
                            c.RajaytaSprite(tt.gameObject, 4, 4, 1.0f, alivetime);

                        raj = !raj;

                    }
                    else
                    {
                        c.RajaytaSprite(tt.gameObject, 4, 4, 1.0f, alivetime);
                    }
                }
                if (c != null && teerajaytaspriteuusiversio)
                {
                   // c.RajaytaSprite(tt.gameObject, 4, 4, 1.0f, alivetime);

                    //c.RajaytaSpriteUusiMonimutkaisin(tt.gameObject, 4, 4, 1.0f, alivetime);

                    if (jokatoinen)
                    {
                        if (raj)
                            c.RajaytaSpriteUusiMonimutkaisin(gameObject, uusirajaytyscolumns, uusirajaytysrows, rajahdysvoima, alivetime,
            rajaytaSpritenExplosion, rajaytaspritenviive, gameJostaRajaytyksenPistelasketaan, 36, teeBoxCollider2d, gravityscale, rajaytaspritenScaleFactorProsentti);

                        raj = !raj;
                    }
                    else
                    {
                        c.RajaytaSpriteUusiMonimutkaisin(gameObject, uusirajaytyscolumns, uusirajaytysrows, rajahdysvoima, alivetime,
rajaytaSpritenExplosion, rajaytaspritenviive, gameJostaRajaytyksenPistelasketaan, 36, teeBoxCollider2d, gravityscale, rajaytaspritenScaleFactorProsentti);

                    }



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
            tuhoaminenkaynnissa = true;
            RajaytaSprite(gameObject, rajahdysrowcol, rajahdysrowcol, rajahdysvoima, alivetime,
sirpalemass, teeBoxCollider2d, 0, false, gravityscale,
  0.0f, adddestroycontroller, explosion);
            //Destroy(gameObject);
        }
        if (teerajaytaspriteuusiversio)
        {
            tuhoaminenkaynnissa = true;
            //Debug.Log("RajaytaSpriteUusiMonimutkaisin " + Time.realtimeSinceStartup);
                 RajaytaSpriteUusiMonimutkaisin(gameObject, uusirajaytyscolumns, uusirajaytysrows, rajahdysvoima, alivetime,
                     rajaytaSpritenExplosion, rajaytaspritenviive, gameJostaRajaytyksenPistelasketaan,
                     36, teeBoxCollider2d, gravityscale, rajaytaspritenScaleFactorProsentti

                     );
            //Destroy(gameObject);

        }


        //RajaytaUudellaTavalla();

        // }
        if (!tuhoaminenkaynnissa)
        {
            BaseDestroy();
        }
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



    public float PalautaOsumaProsenttiNollaViiva1()
    {
        float prosentit = Mathf.Clamp01(hitCount / (float)hitThreshold);
        return prosentit;
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
