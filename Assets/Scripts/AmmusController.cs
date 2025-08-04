using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmusController : BaseController, IExplodable
{
    private Animator m_Animator;
    //private bool tuhoaViivella = false;
    //private bool tuhoa = false;
    //private bool tuhoamistoimenpiteetkaynnistetty = false;
    private int tormaysmaara = 0;

    private Rigidbody2D m_Rigidbody2D;

    private Renderer m_Renderer;
    private bool ollutnakyvissa = false;
    private bool ollutEinakyvissa = false;
    private bool firstime = true;

    public GameObject explosion;
    // Start is called before the first frame update


    private bool aluksenluoma = false;
    public GameObject alus;


    public GameObject option;
    public void SetOption(GameObject p_option)
    {
        option = p_option;
    }

    public void SetAluksenluoma(bool arvo)
    {
        aluksenluoma = arvo;
    }



    private AudioplayerController ad;
    void Start()
    {
        ad = FindObjectOfType<AudioplayerController>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        m_Animator = GetComponent<Animator>();
        m_Renderer = GetComponent<Renderer>();
        //audiosourceexplode.Play();

        //	audiosourceexplode = GetComponent<AudioSource>();



    }

    // Update is called once per frame
    /*
	void Update ()
	{

		firstime = false;
	}
	

	private bool TuhoaJosKameranAlla()
	{
		bool alla = IsObjectDownOfCamera(gameObject);
		if (alla)
		{
			Destroy(gameObject);
			Debug.Log("alla tuhoa");
			return true;
		}
		return false;


	}
	*/
    public float nopeusjonkaalleTuhoutuu = 0.2f;

    public void Update()
    {
        if (tormattyviholliseen)
        {
            Destroy(gameObject);
            return;
        }

        if (OnkoKameranOikeallaPuolella(gameObject))
        {
            Destroy(gameObject);
        }
        else
        {
            TuhoaJosOikeallaPuolenKameraaTutkimuitakainEsimNopeus(gameObject, nopeusjonkaalleTuhoutuu);
        }


        //
        if (true)
        {
            return;
        }

        //Debug.Log ("vauhti x=" + m_Rigidbody2D.velocity.x);
        //Debug.Log ("vauhti y=" + m_Rigidbody2D.velocity.y);
        /*
		if (Mathf.Abs (m_Rigidbody2D.velocity.x) <= nopeusjonkaalleTuhoutuu && Mathf.Abs (m_Rigidbody2D.velocity.y) <= nopeusjonkaalleTuhoutuu) {
			Debug.Log("ammuksen vauhti liian pieni tuhoa");
			Destroy (gameObject);
			//   Explode(0.0f);
			tuhoa = true;

            return;
		}
		
        float speed = m_Rigidbody2D.velocity.magnitude;
        //	Debug.Log("alusammuksen nopeus=" + speed);

        if (speed <= nopeusjonkaalleTuhoutuu && !laserkaytossa)
        {

            Destroy(gameObject);
        }

        //	TuhoaJosVaarassaPaikassa(gameObject);
        TuhoaJosEiKamerassa(gameObject);


        */

    }




    private float annaRandomiKulmaAmmukselleTormayksenJalkeen()
    {
        float f = Random.Range(0f, 360f);
        //Debug.Log ("f=" + f);
        return f;


    }
    private bool tormattyviholliseen = false;


    public float damagemaarajokaaiheutataan = 1.0f;
    private HashSet<GameObject> parentalreadyTriggered = new HashSet<GameObject>();
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag.Equals("alustag"))
        {
            return;
        }

        if (tormattyviholliseen)
        {
            return;
        }
        Vector2 contactPoint = col.GetContact(0).point;
        if (col.collider.tag.Contains("tiilivihollinen") || col.collider.tag.Contains("eituhvih"))
        {
            Explode(contactPoint, col.collider.tag);
            return;
        }

        else if (col.collider.tag.Contains("vihollinen") && col.collider.tag.Contains("explode"))
        {
            IgnoraaCollisiotVihollistenValilla(gameObject, col.gameObject);
            tormattyviholliseen = true;
            //	Debug.Log("explodeeeeeeeeeeeeeeeee ");

            if (col.gameObject != null)
            {
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

                   // Vector2 contactPoint = col.GetContact(0).point;
                    hitcounter.RegisterHit(contactPoint);
                }
                else
                {
                    ChildColliderReporter childColliderReporter = col.gameObject.GetComponent<ChildColliderReporter>();
                    if (childColliderReporter != null)
                    {

                     
                        childColliderReporter.RegisterHit(contactPoint);

                    }
                    else
                    {

                        SkeletonController sc = col.gameObject.GetComponent<SkeletonController>();
                        if (sc != null)
                        {
                            //col.collider.enabled = false;
                            GetComponent<Collider2D>().enabled = false;
                           // Vector2 contactPoint = col.GetContact(0).point;

                            bool rajahtiko = sc.Explode(contactPoint);
                            if (rajahtiko)
                            {
                                Collider2D ss = col.gameObject.GetComponent<Collider2D>();
                                if (ss != null)
                                {
                                    ss.enabled = false;
                                }
                                GameManager.Instance.kasvataHighScorea(col.gameObject);
                            }
                            //tuhottujenVihollistenmaara++;
                            col.otherCollider.enabled = false;

                        }
                        else
                        {

                            IDamagedable damageMahdollinen = col.gameObject.GetComponent<IDamagedable>();
                            if (damageMahdollinen != null)
                            {
                               // Vector2 contactPoint = col.GetContact(0).point;
                                bool rajahtiko = damageMahdollinen.AiheutaDamagea(damagemaarajokaaiheutataan, contactPoint);
                                if (rajahtiko)
                                {
                                    GameManager.Instance.kasvataHighScorea(col.gameObject);
                                }
                                //tuhottujenVihollistenmaara++;
                            }
                            else
                            {
                               // Debug.Log("osauma=" + col.gameObject);
                                IExplodable o =
                col.gameObject.GetComponent<IExplodable>();
                                if (o != null)
                                {
                                    col.collider.enabled = false;
                                    GetComponent<Collider2D>().enabled = false;

                                    if (col.gameObject!=null)
                                    GameManager.Instance.kasvataHighScorea(col.gameObject);
                                    o.Explode();
                                    //tuhottujenVihollistenmaara++;

                                }
                                else
                                {
                                    Debug.Log("vihollinen ja explode mutta ei ookkaan " + col.collider.tag);
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


                                        GameManager.Instance.kasvataHighScorea(col.gameObject);
                                        parentin.Explode();


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
            Explode(contactPoint, col.collider.tag);
        }

        //Explode(0.0f);

    }



    void OnBecameInvisible()
    {
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;
        //Explode();
        Destroy(gameObject);
    }

    public void Explode()
    {
        Explode(transform.position,null);
    }
    public string[] erikoistagitjotkaeiaiheutaammusrajahdysta;

    public void Explode(Vector2 contanctpoint,string tag)
    {
        if (explosion!=null)
        {
            bool rajayta = true;
            if (tag!=null && erikoistagitjotkaeiaiheutaammusrajahdysta!=null && erikoistagitjotkaeiaiheutaammusrajahdysta.Length>0)
            {
                foreach (string t in erikoistagitjotkaeiaiheutaammusrajahdysta)
                {
                    if (tag.Contains(t))
                    {
                        rajayta = false;
                        break;
                    }
                }
            }
            if (rajayta)
            {
                GameObject explosionIns = Instantiate(explosion, contanctpoint, Quaternion.identity);
                Destroy(explosionIns, rajaytyskestoaika);
            }

        }
        /*
        if (rajaytaspriteexplodenjalkeenpistatamatrueksi)
        {
            RajaytaSprite(gameObject, rajaytysrows, rajaytyscols, rajaytysvoima, rajaytyskestoaika);
        }
        */

        //ammuksen massa oli 0.06

         Destroy(gameObject);

    }




    public bool rajaytaspriteexplodenjalkeenpistatamatrueksi = false;

    public int rajaytysrows = 3;
    public int rajaytyscols = 3;

    public float rajaytysvoima = 5.0f;
    public float rajaytyskestoaika = 1.0f;


    /*
	bool IsVisibleFrom2 (this Renderer parent, Camera camera)
	{
		Plane [] planes = GeometryUtility.CalculateFrustumPlanes (camera);
		return GeometryUtility.TestPlanesAABB (planes, parent.bounds);
	}
	*/



    /*
	public void Explode(float viive)
    {
		Destroy(gameObject);

		GameObject explosionIns = Instantiate(explosion, transform.position, Quaternion.identity);

        Destroy(explosionIns, 1.0f);

//        Destroy(gameObject,viive);
		


	}
	*/


    public void Destroy()
    {
        Debug.Log("no niin");
    }

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


    public void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("col=" + col);
    }

 }