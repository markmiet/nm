using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : BaseController
{
  //  public Color cloneColor;
    public int laserkaytossamontakotuhotaan = 3;
    private int tuhottujenmaara;
    //eli ei rigidbodya ollenkaan joo
    //@todoo jatka

    public float nopeusx = 3.0f;

    public float damagemaarajokaaiheutataan = 1.0f;

    public GameObject osumaExplosion;

    // Start is called before the first frame update

    public Vector2 alkuvelocity = Vector2.zero;

    public void SetOption(GameObject p_option)
    {
        option = p_option;
    }

    public void SetAluksenluoma(bool arvo)
    {
        aluksenluoma = arvo;
    }

    void Start()
    {

    }

    public void Explode(bool teeklooni)
    {
        //GameObject explosionIns = Instantiate(explosion, transform.position, Quaternion.identity);
        //	Destroy(explosionIns, 1.0f);


        if (IsGoingToBeDestroyed())
        {
            return;
        }

        //ammuksen massa oli 0.06

        if (tuhottujenVihollistenmaara >= laserkaytossamontakotuhotaan)
        {
            if (!olenklooni && teeklooni)
            {
                lisaaAluksenLuomi();
                GameObject klooni = Instantiate(gameObject);
                klooni.GetComponent<LaserController>().SetAluksenluoma(aluksenluoma);
                klooni.GetComponent<LaserController>().SetOption(option);

                klooni.GetComponent<LaserController>().olenklooni = true;

                klooni.GetComponent<Rigidbody2D>().velocity = alkuvelocity;
                SpriteRenderer sr = klooni.GetComponent<SpriteRenderer>();


               // Color originalColor = sr.color;

                // Set alpha to half
                //originalColor.a *= 0.1f;

                // Apply new color
              //  sr.color = cloneColor;


                //vaihdetaanko klooni puolta pienemmäksi :) hihii tai väriä sehän on cooli tai jotain hih
            }
            BaseDestroy();
            //Destroy(gameObject);
            
        }
    }
    public bool olenklooni = false;
    void OnBecameInvisible()
    {
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;
        //Explode();
      //  Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {

        if (IsGoingToBeDestroyed())
        {
            return;
        }
        if (tuhottujenVihollistenmaara >= laserkaytossamontakotuhotaan)
        {
            //Destroy(gameObject);
            BaseDestroy();
            return;
        }

        //  float delta = Time.deltaTime;
        //    transform.position += new Vector3(delta * nopeusx, 0, 0f);

        TuhoaAmmukset(gameObject);

        /*
        if (OnkoKameranOikeallaPuolella(gameObject))
        {
            Destroy(gameObject);
        }
        */

        /*
        else
        {
            TuhoaJosOikeallaPuolenKameraaTutkimuitakainEsimNopeus(gameObject,-10.0f);
        }
        */

    }

    //@todoooo vaihda ontriggeriksi


    private HashSet<Collision2D> alreadyTriggered = new HashSet<Collision2D>();
    private HashSet<GameObject> parentalreadyTriggered = new HashSet<GameObject>();

    private void DestroyKaikki(GameObject go)
    {
        /*
        if (go.transform.parent!=null)
        {
            Destroy(go.transform.parent.gameObject);
        }
        else
        {
            Destroy(go);
        }
        */
    }


    void OnCollisionEnter2D(Collision2D col)
    {


        if (IsGoingToBeDestroyed())
        {
            return;
        }

        if (tuhottujenVihollistenmaara >= laserkaytossamontakotuhotaan)
        {
            //Destroy(gameObject);
            BaseDestroy();
            return;
        }

        if (col.collider.tag.Contains("alustag"))
        {
            return;
        }

        Vector2 contactPoint = col.GetContact(0).point;

        // Vector2 contactPoint = col.ClosestPoint(transform.position);

        if (col.collider.tag.Contains("tiilivihollinen") || col.collider.tag.Contains("eituhvih"))
        {
            tuhottujenVihollistenmaara = laserkaytossamontakotuhotaan;
            TeeOsumaExplosion(contactPoint);

            Explode(false);

        }

        else if (col.collider.tag.Contains("vihollinen") && col.collider.tag.Contains("explode"))
        {
            Debug.Log("viholliseen tormatty lasercontroller " + olenklooni);

            bool teeklooni = true;
            //tormattyviholliseen = true;
            //	Debug.Log("explodeeeeeeeeeeeeeeeee ");
            TeeOsumaExplosion(contactPoint);

            LisaaTuhottujenMaaraa(col.gameObject);


            if (col.gameObject != null)
            {
                //             if (alreadyTriggered.Contains(col))
                //               return;

                alreadyTriggered.Add(col);




                if (col.gameObject.transform.parent != null)
                {
                    HitCounter hc = col.gameObject.transform.parent.gameObject.GetComponent<HitCounter>();
                    if (hc == null)
                    {
                        if (parentalreadyTriggered.Contains(col.gameObject.transform.parent.gameObject))
                        {
                            //                     return;
                        }
                        parentalreadyTriggered.Add(col.gameObject.transform.parent.gameObject);
                    }
                }

                HitCounter hitcounter = col.gameObject.GetComponent<HitCounter>();

                if (hitcounter != null)
                {
                    //LisaaTuhottujenMaaraa(col.gameObject);
                    bool tuhoutuiko = hitcounter.RegisterHit(contactPoint);
                    if (tuhoutuiko)
                    {
                        IgnoraaCollisiotVihollistenValilla(gameObject, col.gameObject);
                        DestroyKaikki(col.gameObject);
                    }
                    else
                    {
                        teeklooni = false;
                    }
                }
                else
                {

                    ChildColliderReporter childColliderReporter = col.gameObject.GetComponent<ChildColliderReporter>();
                    if (childColliderReporter != null)
                    {
                        //  LisaaTuhottujenMaaraa(col.gameObject);
                        //tuhottujenVihollistenmaara = laserkaytossamontakotuhotaan;

                        bool tuhoituko=
                            childColliderReporter.RegisterHit(contactPoint,col.collider.gameObject);
                        if (!tuhoituko)
                        {
                            teeklooni = false;
                        }

                    }
                    else
                    {

                        SkeletonController sc = col.gameObject.GetComponent<SkeletonController>();
                        if (sc != null)
                        {
                            GetComponent<Collider2D>().enabled = false;


                            //Vector2 contactPoint = col.GetContact(0).point;
                            bool rajahtiko = sc.Explode(contactPoint);
                            if (rajahtiko)
                            {
                                GameManager.Instance.kasvataHighScorea(col.gameObject);
                                Collider2D ss =
                                col.gameObject.GetComponent<Collider2D>();
                                if (ss != null)
                                {
                                    ss.enabled = false;
                                }
                                //col.enabled = false;
                                IgnoraaCollisiotVihollistenValilla(gameObject, col.gameObject);

                            }
                            else
                            {
                                teeklooni = false;
                            }

                            //tuhottujenVihollistenmaara++;
                            //    tuhottujenVihollistenmaara = laserkaytossamontakotuhotaan;
                            //LisaaTuhottujenMaaraa(col.gameObject);


                        }
                        else
                        {

                            IDamagedable damageMahdollinen = col.gameObject.GetComponent<IDamagedable>();
                            if (damageMahdollinen != null)
                            {
                                bool rajahtiko = damageMahdollinen.AiheutaDamagea(damagemaarajokaaiheutataan, contactPoint);
                                if (rajahtiko)
                                {
                                    GameManager.Instance.kasvataHighScorea(col.gameObject);
                                }
                                else
                                {
                                    teeklooni = false;
                                }
                                //tuhottujenVihollistenmaara++;
                                //      tuhottujenVihollistenmaara = laserkaytossamontakotuhotaan;
                                //LisaaTuhottujenMaaraa(col.gameObject);
                                IgnoraaCollisiotVihollistenValilla(gameObject, col.gameObject);

                            }
                            else
                            {

                                IExplodable o =
                col.gameObject.GetComponent<IExplodable>();
                                if (o != null)
                                {
                                    o.Explode();
                                    //tuhottujenVihollistenmaara++;
                                    GameManager.Instance.kasvataHighScorea(col.gameObject);
                                    Collider2D ss =
                                col.gameObject.GetComponent<Collider2D>();
                                    if (ss != null)
                                    {
                                        ss.enabled = false;
                                    }
                                    IgnoraaCollisiotVihollistenValilla(gameObject, col.gameObject);

                                    //        LisaaTuhottujenMaaraa(col.gameObject);

                                }
                                else
                                {
                                    //Debug.Log("vihollinen ja explode mutta ei ookkaan " + col.tag);
                                    //makitavihollinen vasen

                                    IExplodable parentin = col.gameObject.GetComponentInParent<IExplodable>();
                                    if (parentin != null)
                                    {
                                        if (col.gameObject.transform.parent != null)
                                        {
                                            Collider2D[] childCollid = col.gameObject.transform.parent.GetComponentsInChildren<Collider2D>();
                                            if (childCollid != null)
                                            {
                                                foreach (Collider2D m in childCollid)
                                                {
                                                    m.enabled = false;
                                                }
                                            }

                                        }
                                        parentin.Explode();
                                        IgnoraaCollisiotVihollistenValilla(gameObject, col.gameObject);

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

                                    Collider2D[] childCollid2 = col.gameObject.transform.GetComponentsInChildren<Collider2D>();
                                    if (childCollid2 != null)
                                    {
                                        foreach (Collider2D m in childCollid2)
                                        {
                                            m.enabled = false;
                                        }
                                    }


                                    //      LisaaTuhottujenMaaraa(col.gameObject);

                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //	Debug.Log("gameobjekcti null");

            }
            Explode(teeklooni);
        }

        //Explode(0.0f);



    }



    /*
        public void OnTriggerEnter2D(Collider2D col)
        {



            if (col.tag.Contains("alustag"))
            {
                return;
            }


            Vector2 contactPoint = col.ClosestPoint(transform.position);

            if (col.tag.Contains("tiilivihollinen") || col.tag.Contains("eituhvih"))
            {
                tuhottujenVihollistenmaara = laserkaytossamontakotuhotaan;
                TeeOsumaExplosion(contactPoint);

                Explode();

            }

            else if (col.tag.Contains("vihollinen") && col.tag.Contains("explode"))
            {
                //tormattyviholliseen = true;
                //	Debug.Log("explodeeeeeeeeeeeeeeeee ");
                TeeOsumaExplosion(contactPoint);



                if (col.gameObject != null)
                {
                    if (alreadyTriggered.Contains(col))
                        return;

                    alreadyTriggered.Add(col);




                    if (col.gameObject.transform.parent != null)
                    {
                        HitCounter hc = col.gameObject.transform.parent.gameObject.GetComponent<HitCounter>();
                        if (hc == null)
                        {
                            if (parentalreadyTriggered.Contains(col.gameObject.transform.parent.gameObject))
                            {
                                return;
                            }
                            parentalreadyTriggered.Add(col.gameObject.transform.parent.gameObject);
                        }
                    }

                    HitCounter hitcounter = col.gameObject.GetComponent<HitCounter>();

                    if (hitcounter != null)
                    {

                        hitcounter.RegisterHit(contactPoint);
                    }
                    else
                    {

                        ChildColliderReporter childColliderReporter = col.gameObject.GetComponent<ChildColliderReporter>();
                        if (childColliderReporter != null)
                        {
                            //LisaaTuhottujenMaaraa(col.gameObject);
                            tuhottujenVihollistenmaara = laserkaytossamontakotuhotaan;
                            childColliderReporter.RegisterHit(contactPoint);

                        }
                        else
                        {

                            SkeletonController sc = col.gameObject.GetComponent<SkeletonController>();
                            if (sc != null)
                            {
                                GetComponent<Collider2D>().enabled = false;


                                //Vector2 contactPoint = col.GetContact(0).point;
                                bool rajahtiko = sc.Explode(contactPoint);
                                if (rajahtiko)
                                {
                                    GameManager.Instance.kasvataHighScorea(col.gameObject);
                                    Collider2D ss =
                                    col.gameObject.GetComponent<Collider2D>();
                                    if (ss != null)
                                    {
                                        ss.enabled = false;
                                    }
                                    //col.enabled = false;
                                }

                                //tuhottujenVihollistenmaara++;
                                tuhottujenVihollistenmaara = laserkaytossamontakotuhotaan;
                                //LisaaTuhottujenMaaraa(col.gameObject);


                            }
                            else
                            {

                                IDamagedable damageMahdollinen = col.gameObject.GetComponent<IDamagedable>();
                                if (damageMahdollinen != null)
                                {
                                    bool rajahtiko = damageMahdollinen.AiheutaDamagea(damagemaarajokaaiheutataan, contactPoint);
                                    if (rajahtiko)
                                    {
                                        GameManager.Instance.kasvataHighScorea(col.gameObject);
                                    }
                                    //tuhottujenVihollistenmaara++;
                                    tuhottujenVihollistenmaara = laserkaytossamontakotuhotaan;
                                    //LisaaTuhottujenMaaraa(col.gameObject);

                                }
                                else
                                {

                                    IExplodable o =
                    col.gameObject.GetComponent<IExplodable>();
                                    if (o != null)
                                    {
                                        o.Explode();
                                        //tuhottujenVihollistenmaara++;
                                        GameManager.Instance.kasvataHighScorea(col.gameObject);
                                        Collider2D ss =
                                    col.gameObject.GetComponent<Collider2D>();
                                        if (ss != null)
                                        {
                                            ss.enabled = false;
                                        }

                                        LisaaTuhottujenMaaraa(col.gameObject);

                                    }
                                    else
                                    {
                                        Debug.Log("vihollinen ja explode mutta ei ookkaan " + col.tag);
                                        //makitavihollinen vasen

                                        IExplodable parentin = col.gameObject.GetComponentInParent<IExplodable>();
                                        if (parentin != null)
                                        {
                                            if (col.gameObject.transform.parent != null)
                                            {
                                                Collider2D[] childCollid = col.gameObject.transform.parent.GetComponentsInChildren<Collider2D>();
                                                if (childCollid != null)
                                                {
                                                    foreach (Collider2D m in childCollid)
                                                    {
                                                        m.enabled = false;
                                                    }
                                                }

                                            }
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

                                        Collider2D[] childCollid2 = col.gameObject.transform.GetComponentsInChildren<Collider2D>();
                                        if (childCollid2 != null)
                                        {
                                            foreach (Collider2D m in childCollid2)
                                            {
                                                m.enabled = false;
                                            }
                                        }


                                        LisaaTuhottujenMaaraa(col.gameObject);

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //	Debug.Log("gameobjekcti null");

                }
                Explode();
            }

            //Explode(0.0f);



        }
    */
    /**
	void OnCollisionEnter2D(Collision2D col)
	{





		if (col.collider.tag.Equals("alustag"))
		{
			return;
		}



		if (col.collider.tag.Contains("tiilivihollinen") || col.collider.tag.Contains("hammasvihollinen"))
		{
			tuhottujenVihollistenmaara = laserkaytossamontakotuhotaan;
			Explode();
		}

		else if (col.collider.tag.Contains("vihollinen") && col.collider.tag.Contains("explode"))
		{
			//tormattyviholliseen = true;
			//	Debug.Log("explodeeeeeeeeeeeeeeeee ");




			if (col.gameObject != null)
			{
				
				SkeletonController sc = col.gameObject.GetComponent<SkeletonController>();
				if (sc != null)
				{
					sc.Explode(col);
					//tuhottujenVihollistenmaara++;
					LisaaTuhottujenMaaraa(col.gameObject);

				}
				else
				{

					IDamagedable damageMahdollinen = col.gameObject.GetComponent<IDamagedable>();
					if (damageMahdollinen != null)
					{
						damageMahdollinen.AiheutaDamagea(damagemaarajokaaiheutataan);
						//tuhottujenVihollistenmaara++;
						LisaaTuhottujenMaaraa(col.gameObject);

					}
					else
					{

						IExplodable o =
		col.gameObject.GetComponent<IExplodable>();
						if (o != null)
						{
							o.Explode();
							//tuhottujenVihollistenmaara++;
							LisaaTuhottujenMaaraa(col.gameObject);

						}
						else
						{
							Debug.Log("vihollinen ja explode mutta ei ookkaan " + col.collider.tag);

						}
					}

				}

			}
			else
			{
				//	Debug.Log("gameobjekcti null");

			}
			Explode();
		}

		//Explode(0.0f);

	}
	**/

    private HashSet<GameObject> tuhotut = new HashSet<GameObject>();
    private int tuhottujenVihollistenmaara = 0;
    private void LisaaTuhottujenMaaraa(GameObject go)
    {
        //     if (!tuhotut.Contains(go))
        //   {
        tuhottujenVihollistenmaara++;
        tuhotut.Add(go);
        // }


    }

    public float osumaexplosionkestoaika = 0.5f;

    public void TeeOsumaExplosion(Vector2 contactPoint)
    {
        if (osumaExplosion != null)
        {
            //GameObject explosionIns = Instantiate(osumaExplosion, contactPoint, Quaternion.identity);

           GameObject explosionIns = ObjectPoolManager.Instance.GetFromPool(osumaExplosion, contactPoint, Quaternion.identity);

           ObjectPoolManager.Instance.ReturnToPool(explosionIns, osumaexplosionkestoaika);

            //Destroy(explosionIns, 1.0f);
        }
    }

    private bool aluksenluoma = false;
    public GameObject alus;

    public GameObject option;
    public override void OnDestroyPoolinlaittaessa()
    {
        //System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(true);
        //Debug.Log(stackTrace.ToString());

     //   Debug.Log("Destroy olen klooni="+olenklooni);


        if (aluksenluoma && alus != null)
        {
            //aluscontrollerin 
            alus.GetComponent<AlusController>().VahennaaluksenluomienElossaOlevienAmmustenMaaraa();
        }
        else if (option != null)
        {
            if (option.GetComponent<OptionController>() != null)
            {
                option.GetComponent<OptionController>().VahennOptionLuomienElossaOlevienAmmustenMaaraa();

            }
            else
            {

                Debug.Log("mitaa");
            }

        }
    }

    private void lisaaAluksenLuomi()
    {
        if (aluksenluoma && alus != null)
        {
            //aluscontrollerin 
            alus.GetComponent<AlusController>().LisaaaluksenluomienElossaOlevienAmmustenMaaraa();
        }
        else if (option != null)
        {
            if (option.GetComponent<OptionController>() != null)
            {
                option.GetComponent<OptionController>().LisaaaOptionLuomienElossaOlevienAmmustenMaaraa();

            }
            else
            {
                Debug.Log("mitaa");
            }
        }
    }



    /*
    private void OnCollisionEnter2D(Collision2D col)
    {
        //   collision.enabled = false;

        BoxCollider2D[] childCollid2 = GetComponents<BoxCollider2D>();
        if (childCollid2 != null)
        {
            foreach (BoxCollider2D m in childCollid2)
            {
                if (!m.isTrigger)
                {
                  //  m.enabled = false;
                }
            }
        }
        
    }
    */

}
