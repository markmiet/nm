using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : BaseController
{
    public int laserkaytossamontakotuhotaan = 3;
    private int tuhottujenmaara;
    //eli ei rigidbodya ollenkaan joo
    //@todoo jatka

    public float nopeusx = 3.0f;

    public float damagemaarajokaaiheutataan = 1.0f;

    public GameObject osumaExplosion;

    // Start is called before the first frame update

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

    public void Explode()
    {
        //GameObject explosionIns = Instantiate(explosion, transform.position, Quaternion.identity);
        //	Destroy(explosionIns, 1.0f);


        //ammuksen massa oli 0.06

        if (tuhottujenVihollistenmaara >= laserkaytossamontakotuhotaan)
        {
            Destroy(gameObject);
        }


    }
    void OnBecameInvisible()
    {
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;
        //Explode();
        Destroy(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        float delta = Time.deltaTime;
        transform.position += new Vector3(delta * nopeusx, 0, 0f);
        
        if (OnkoKameranOikeallaPuolella(gameObject))
        {
            Destroy(gameObject);
        }
        /*
        else
        {
            TuhoaJosOikeallaPuolenKameraaTutkimuitakainEsimNopeus(gameObject,-10.0f);
        }
        */

    }

    //@todoooo vaihda ontriggeriksi


    private HashSet<Collider2D> alreadyTriggered = new HashSet<Collider2D>();
    private HashSet<GameObject> parentalreadyTriggered = new HashSet<GameObject>();
    public void OnTriggerEnter2D(Collider2D col)
    {



        if (col.tag.Contains("alustag"))
        {
            return;
        }



        if (col.tag.Contains("tiilivihollinen") || col.tag.Contains("eituhvih"))
        {
            tuhottujenVihollistenmaara = laserkaytossamontakotuhotaan;
            TeeOsumaExplosion();

            Explode();

        }

        else if (col.tag.Contains("vihollinen") && col.tag.Contains("explode"))
        {
            //tormattyviholliseen = true;
            //	Debug.Log("explodeeeeeeeeeeeeeeeee ");

            TeeOsumaExplosion();



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

                    Vector2 contactPoint = col.ClosestPoint(transform.position);
                    hitcounter.RegisterHit(contactPoint);
                }
                else
                {

                    ChildColliderReporter childColliderReporter = col.gameObject.GetComponent<ChildColliderReporter>();
                    if (childColliderReporter != null)
                    {
                        //LisaaTuhottujenMaaraa(col.gameObject);
                        tuhottujenVihollistenmaara = laserkaytossamontakotuhotaan;
                        Vector2 contactPoint = col.ClosestPoint(transform.position);
                        childColliderReporter.RegisterHit(contactPoint);

                    }
                    else
                    {

                        SkeletonController sc = col.gameObject.GetComponent<SkeletonController>();
                        if (sc != null)
                        {
                            GetComponent<Collider2D>().enabled = false;

                            Vector2 contactPoint = col.ClosestPoint(transform.position);

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
                                bool rajahtiko = damageMahdollinen.AiheutaDamagea(damagemaarajokaaiheutataan);
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
        if (!tuhotut.Contains(go))
        {
            tuhottujenVihollistenmaara++;
            tuhotut.Add(go);
        }


    }

    public void TeeOsumaExplosion()
    {
               if (osumaExplosion!=null)
        {
            GameObject explosionIns = Instantiate(osumaExplosion, transform.position, Quaternion.identity);
            Destroy(explosionIns, 1.0f);
        }
    }

    private bool aluksenluoma = false;
    public GameObject alus;

    public GameObject option;
    public void OnDestroy()
    {
        //System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(true);
        //Debug.Log(stackTrace.ToString());

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
