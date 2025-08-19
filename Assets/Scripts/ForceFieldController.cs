using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]

//[ExecuteAlways]
public class ForceFieldController : BaseController
{
    // Start is called before the first frame update
    private ParticleSystem particleSystem;

    public GameObject valo;


    public int hittienmaaraJokaKestetaan = 10;
    public int hittienmaara = 0;

    //0.8,0,1, 0.8
    // public bool partikkelitEnabloituna = true;
    ParticleSystem.MainModule main;
    public float alphaValueAloitusArvo = 0.5f;
    public float alphaValueLopetusArvo = 0.1f;
    public float alphaValueNykyarvo = 0.5f;

    public float simulationspeedAloitusArvo = 5f;
    public float simulationspeedLopetusArvo = 1f;
    public float simulationspeedNykyarvo = 5f;



    private GameObject alus;
    public bool onkotoiminnassa = false;

    void Start()
    {
        Debug.Log("start ff " + gameObject.name);
        particleSystem = GetComponent<ParticleSystem>();
        // main = particleSystem.main;
        // var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        // if (renderer != null && renderer.material != null)
        // {
        //    Color color = renderer.material.color;
        //    renderer.material.color = new Color(color.r, color.g, color.b, alphaValueAloitusArvo);
        //}
        alus = PalautaAlus();
        renderoija = particleSystem.GetComponent<ParticleSystemRenderer>();
        VaihdaAlphaa();
       // SetOnkotoiminnassa(IsOnkotoiminnassa());
        // SetActive(false);

    }
    private ParticleSystemRenderer renderoija;
    // Update is called once per frame
    void Update()
    {


        //float alphaValueNykyarvo = Mathf.Lerp(alphaValueAloitusArvo, alphaValueLopetusArvo, (float)hittienmaara / hittienmaaraJokaKestetaan);
        /*
        main = particleSystem.main;
      
        if (renderer != null && renderer.material != null)
        {
            Color color = renderer.material.color;
            renderer.material.color = new Color(color.r, color.g, color.b, alphaValueNykyarvo);
        }
        */

        if (hittienmaara + 1 >= hittienmaaraJokaKestetaan)
        {
            //vilkuta
            timer += Time.deltaTime;

            if (timer >= blinkInterval)
            {
                timer = 0f;
                visible = !visible;

                Color color = renderoija.material.color;
                float newAlpha = visible ? 1f : 0f;

                renderoija.material.color = new Color(color.r, color.g, color.b, newAlpha);
            }
        }

    }

    public float blinkInterval = 0.2f;

    private float timer = 0f;
    private bool visible = true;


    /*
    if (partikkelitEnabloituna)
    {
        if (!particleSystem.isPlaying)
        {
            particleSystem.Play();
        }


    }
    else
    {
        particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
    */





    /*
    void OnParticleCollision(GameObject other)
    {
        if (other.tag.Contains("vihollinen") && !other.tag.Contains("tiili"))
        {
            IExplodable ex = other.gameObject.GetComponent<IExplodable>();
            if (ex != null)
            {
                ex.Explode();
            }
            else
            {
                Destroy(other.gameObject.transform.parent);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //haukisilmavihollinenexplodetag

        //  col.otherCollider t‰‰ on alus

        //explodetag
        if (col.collider.tag.Contains("hauki") || col.collider.tag.Contains("tiili") ||
            col.collider.tag.Contains("pyoroovi") || col.collider.tag.Contains("laatikkovihollinenexplodetag"))
        {

        }
        else if (col.collider.tag.Contains("makitavihollinenexplodetag"))
        {
            PiippuMakitaVihollinenController p = col.gameObject.GetComponent<PiippuMakitaVihollinenController>();
            if (p)
            {
                if (col.gameObject.transform.parent!=null)
                {
                    IExplodable ex = col.gameObject.transform.parent.GetComponent<IExplodable>();
                    if (ex!=null)
                    {
                        ex.Explode();
                    }
                    else
                    {
                        Destroy(col.gameObject.transform.parent);
                    }
                    
                }

            }
            else
            {
                IExplodable ex = col.gameObject.GetComponent<IExplodable>();
                if (ex != null)
                {
                    ex.Explode();
                }
                else
                {
                    Destroy(col.gameObject.transform.parent);
                }
            }
            PoltaValo();

        }

        else if (col.collider.tag.Contains("vihollinen"))
        {
            IExplodable ex = col.gameObject.GetComponent<IExplodable>();
            if (ex != null)
            {
                ex.Explode();
            }
            else
            {
                Destroy(col.gameObject.transform.parent);
            }
            PoltaValo();


        }

    }
    */

    private HashSet<Collider2D> alreadyTriggered = new HashSet<Collider2D>();
    private HashSet<GameObject> parentalreadyTriggered = new HashSet<GameObject>();

    public float damagemaarajokaaiheutataan = 10.0f;

    public int maksimihittienmaara = 1;

    //vaihda t‰m‰kin normicolladeriksi
    //     IgnoraaCollisiotVihollistenValilla(gameObject, col.gameObject);
    //pakko olla triggeri?

    void OnCollisionEnter2D(Collision2D col)
    {

        Debug.Log("ff coll" + col.otherCollider.name);
    }

        public void OnTriggerEnter2D(Collider2D col)
    {
        if (IsGoingToBeDestroyed())
        {
            return;
        }

        if (col.tag.Contains("eituhvih"))
        {

            //return;
        }
        Vector2 thisPosition = transform.position;
        Vector2 otherPosition = col.transform.position;

        Vector2 estimatedContactPoint = (thisPosition + otherPosition) / 2f;
        //ei ole tarkka
        //kysymys kuuluu miksi trigger miksei muu

        /*
        if (col.tag.Contains("tiili") && col.gameObject.GetComponent<VaihtoController>()!=null)
        {
            VaihtoController v = col.gameObject.GetComponent<VaihtoController>();
            if (v.bulletholemode)
            {
                
                Rigidbody2D myRb = GetComponentInParent<Rigidbody2D>();
                Rigidbody2D otherRb = col.attachedRigidbody;

                Vector2 myVelocity = myRb ? myRb.velocity : Vector2.zero;
                Vector2 otherVelocity = otherRb ? otherRb.velocity : Vector2.zero;

                Vector2 relativeVelocity = myVelocity - otherVelocity;

                //public void BulletHole(Vector2 relvel, Vector2 hitPoint, GameObject go)
                Debug.Log("estim=" + estimatedContactPoint);
                v.BulletHole(relativeVelocity, estimatedContactPoint, gameObject);

            }
        }

        else 
        */
        if (col.tag.Contains("vihollinen") && !col.tag.Contains("tiili"))
        {
            //if (alreadyTriggered.Contains(col))
              //  return;

            alreadyTriggered.Add(col);


            IDamagedable damageMahdollinen = col.gameObject.GetComponent<IDamagedable>();
            if (damageMahdollinen != null)
            {

                bool rajahtiko = damageMahdollinen.AiheutaDamagea(damagemaarajokaaiheutataan, estimatedContactPoint);
                if (rajahtiko)
                {
                    GameManager.Instance.kasvataHighScorea(col.gameObject);
                }
                //tuhottujenVihollistenmaara++;

                //tuhottujenVihollistenmaara++;
                //LisaaTuhottujenMaaraa(col.gameObject);
                //Destroy(gameObject);
                BaseDestroy();
                return;

            }

            if (col.gameObject.transform.parent != null)
            {
                HitCounter hc = col.gameObject.transform.parent.gameObject.GetComponent<HitCounter>();
                if (hc == null)
                {
                  //  if (parentalreadyTriggered.Contains(col.gameObject.transform.parent.gameObject))
                 //   {
                   //     return;
                  //  }
                    parentalreadyTriggered.Add(col.gameObject.transform.parent.gameObject);
                }
            }

            HitCounter hitcounter = col.gameObject.GetComponent<HitCounter>();

            if (hitcounter != null)
            {
                //col.enabled = false;
                /*
                Vector2 thisPosition = transform.position;
                Vector2 otherPosition = col.transform.position;

                Vector2 estimatedContactPoint = (thisPosition + otherPosition) / 2f;
                */

                int maara = Mathf.Min(hitcounter.hitThreshold, maksimihittienmaara);

                /**/

                for (int i = 0; i < maara; i++)
                {
                    hitcounter.RegisterHit(estimatedContactPoint);
                    hittienmaara++;
                }
                //Destroy(col.gameObject);


                //hitcounter.RegisterHit(estimatedContactPoint);
                //alus.GetComponent<AlusController>().AsetaForceFieldiButtonEnabloiduksi();
                //SetOnkotoiminnassa(false);

                if (hittienmaara > hittienmaaraJokaKestetaan)
                {
                    // gameObject.SetActive(false);
                    //
                    //  alus.GetComponent<AlusController>().AsetaForceFieldiButtonEnabloiduksi();
                    //SetOnkotoiminnassa(false);
                    //Destroy(gameObject);
                    BaseDestroy();

                }
                return;
                
            }
            ChildColliderReporter childColliderReporter = col.gameObject.GetComponent<ChildColliderReporter>();
            if (childColliderReporter != null)
            {
                //col.enabled = false;
                /*
                Vector2 thisPosition = transform.position;
                Vector2 otherPosition = col.transform.position;
                Vector2 estimatedContactPoint = (thisPosition + otherPosition) / 2f;
                */
                //n‰in saadaan r‰j‰ht‰m‰‰n
                int maara = childColliderReporter.PalautaVaadittuHittienMaara();//t‰m‰ kuluttaa aina sen verran mit‰ vaaditaan ko. objekti tuhoamiseen

                maara = Mathf.Min(maara, maksimihittienmaara);

                for (int i=0;i<maara;i++)
                {
                    int nykymaara = childColliderReporter.PalautaNykyinenOsumienMaara();
                    int vaaditut = childColliderReporter.PalautaVaadittuHittienMaara();
                    if (nykymaara==vaaditut)
                    {
                        break;
                    }
                    
                    childColliderReporter.RegisterHit(estimatedContactPoint,col.gameObject);
                    hittienmaara++;
                    if (hittienmaara >= hittienmaaraJokaKestetaan)
                    {
                        break;
                    }
                }
                //hittienmaara += maara;
                if (hittienmaara > hittienmaaraJokaKestetaan)
                {
                    // gameObject.SetActive(false);
                    //
                    //  alus.GetComponent<AlusController>().AsetaForceFieldiButtonEnabloiduksi();
                    //SetOnkotoiminnassa(false);
                  //  Destroy(gameObject);
                    BaseDestroy();
                }

                //alus.GetComponent<AlusController>().AsetaForceFieldiButtonEnabloiduksi();
                //SetOnkotoiminnassa(false);

                return;


            }


           

            SkeletonController sc = col.gameObject.GetComponent<SkeletonController>();
            if (sc != null)
            {
                //col.collider.enabled = false;
                GetComponent<Collider2D>().enabled = false;


                sc.TeeLopulllinenRajaytys();
                GameManager.Instance.kasvataHighScorea(col.gameObject);
                //tuhottujenVihollistenmaara++;
                //LisaaTuhottujenMaaraa(col.gameObject);
                // alus.GetComponent<AlusController>().AsetaForceFieldiButtonEnabloiduksi();
                //Destroy(gameObject);
                BaseDestroy();

                //SetOnkotoiminnassa(false);

            }
            else
            {
                //mites t‰m‰ damage



                IExplodable o =
        col.gameObject.GetComponent<IExplodable>();
                if (o != null)
                {
                    o.Explode();
                    GameManager.Instance.kasvataHighScorea(col.gameObject);
                }
                else
                {
                    Debug.Log("vihollinen ja explode mutta ei ookkaan " + col.tag);

                    IExplodable parentin = col.gameObject.GetComponentInParent<IExplodable>();
                    if (parentin != null)
                    {
                        parentin.Explode();
                        GameManager.Instance.kasvataHighScorea(col.gameObject);

                    }
                    Collider2D ssparent = col.gameObject.GetComponentInParent<Collider2D>();
                    if (ssparent != null)
                    {
                        ssparent.enabled = false;
                    }

                    Collider2D ss =
                        col.gameObject.GetComponent<Collider2D>();
                    if (ss != null)
                    {
                        ss.enabled = false;
                    }
                    //Destroy(col.gameObject);

                }
                hittienmaara++;
                PoltaValo();
                
                Debug.Log("hittienm‰‰r‰=" + hittienmaara);
                //t‰h‰n se himmennys eli partikkelien m‰‰r‰‰‰ v‰hennet‰‰n tms
                if (hittienmaara > hittienmaaraJokaKestetaan)
                {
                    // gameObject.SetActive(false);
                    //
                    //alus.GetComponent<AlusController>().AsetaForceFieldiButtonEnabloiduksi();
                    //SetOnkotoiminnassa(false);
                    //Destroy(gameObject);
                    BaseDestroy();

                }
            }
        }

    }

    /*
    public void SetOnkotoiminnassa(bool active)
    {
        //gameObject.SetActive(active);
        onkotoiminnassa = active;

        Collider2D[] sd =
        GetComponents<Collider2D>();

        foreach (Collider2D c in sd)
        {
            c.enabled = active;
        }

        if (active)
        {
            alphaValueNykyarvo = alphaValueAloitusArvo;
            hittienmaara = 0;
            PoltaValo();

            if (!particleSystem.isPlaying)
            {
                particleSystem.Play();
            }
        }
        else
        {
            //particleSystem.Stop();

            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
    */
    /*
    public bool IsOnkotoiminnassa()
    {
        return onkotoiminnassa;
    }
    */

    private void PoltaValo()
    {
       // if (true)
       //     return;

        //valo.GetComponent<AlusLightController>().SetExplosionLights();
        VaihdaAlphaa();


        //float alpha = newColor.a;
        //ParticleSystem.MainModule main = particleSystem.main; // Get a fresh MainModule reference

        //Color newColor = main.startColor.color;
        //float alpha = newColor.a - 1;
        //newColor.a = alpha; // Set new alpha

        //  Color color = main.startColor.color;
        //  float alpha255 = color.a - 0.5f;
        //  Debug.Log($"Alpha: {alpha255}"); // Debugging

        // Modify alpha (if needed) and set it back
        //   color.a = alpha255;
        //     main.startColor = new ParticleSystem.MinMaxGradient(color); // Apply updated color

        // if (particleSystem.isPlaying)
        // {
        //particleSystem.Stop();
        //particleSystem.Play();
        // }
    }

    private void VaihdaAlphaa()
    {


        if (particleSystem==null)
        {
            return;
        }
        alphaValueNykyarvo = Mathf.Lerp(alphaValueAloitusArvo, alphaValueLopetusArvo, (float)hittienmaara / hittienmaaraJokaKestetaan);

        main = particleSystem.main;
        //var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        if (renderoija != null && renderoija.material != null)
        {
            Color color = renderoija.material.color;
            renderoija.material.color = new Color(color.r, color.g, color.b, alphaValueNykyarvo);
        }
        /*
            public float simulationspeedAloitusArvo = 5f;
    public float simulationspeedLopetusArvo = 1f;
    public float simulationspeedNykyarvo = 5f;
    */

        simulationspeedNykyarvo = Mathf.Lerp(simulationspeedAloitusArvo, simulationspeedLopetusArvo, (float)hittienmaara / hittienmaaraJokaKestetaan);

        main.simulationSpeed = simulationspeedNykyarvo;
    }
}