using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakitaVihollinenAmmusScripti : BaseController, IExplodable
{

    public GameObject prefap;

    private Rigidbody2D m_Rigidbody2D;

    // Start is called before the first frame update
    //private float spawnTime;
    //public float kauankoViholliseentormayksetIgnorataan = 0.5f;

    public bool kaannaObjektiLiikkeenSuuntaiseksi = false;

    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        //spawnTime = Time.time;
        if (kaannaObjektiLiikkeenSuuntaiseksi)
        {
            Vector2 velocity = m_Rigidbody2D.velocity;
            if (velocity.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }
    /*
	private bool VihollinenCollideIgnore()
    {
		if (Time.time-spawnTime>=kauankoViholliseentormayksetIgnorataan)
        {
			return false;
        }
		return true;
    }
	*/

    // Update is called once per frame



    public float speedjonkaalletuhotaan = 0.5f;

    /*
	private bool TuhoaJosKameranAlla()
	{
		bool alla = IsObjectDownOfCamera(gameObject);
		if (alla)
		{
			Destroy(gameObject);
			//Debug.Log("alla tuhoa");
			return true;
		}
		return false;


	}
	*/
    public float maksimiaikaMinkaVoiOllaHengissa = 60.0f;
    //private float checkInterval = 0.3f;
    //private float nextCheckTime;
    //public float nopeusjonkaalletuhotaan;
    void Update()
    {

//        Tuhoa(prefap,gameObject, speedjonkaalletuhotaan);

        if (prefap != null)
        {
            bool tuhoutui=TuhoaKunElamisenAikaRajaTayttyyTaiHidastuuLiikaa(prefap, gameObject, maksimiaikaMinkaVoiOllaHengissa, speedjonkaalletuhotaan);

        }
        
        else
        {
            Tuhoa(gameObject, speedjonkaalletuhotaan);
        }

        /*
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval;
            float speed = m_Rigidbody2D.velocity.magnitude;
        //	Debug.Log("speed="+speed);
            if (speed <= speedjonkaalletuhotaan)
            {
            //	Debug.Log("Projectile too slow, destroying...");
                //Destroy(gameObject);
                Explode();
                return;
            }

        }
        TuhoaJosVaarassaPaikassa(gameObject);
        TuhoaJosEiKamerassa(gameObject);
        */

        if (kaannaObjektiLiikkeenSuuntaiseksi)
        {
            Vector2 velocity = m_Rigidbody2D.velocity;
            if (velocity.sqrMagnitude > 0.001f)
            {
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }


    void OnBecameInvisible()
    {
        //	Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;
        hengissaoloaika = 0.0f;
        ObjectPoolManager.Instance.ReturnToPool(prefap, gameObject);
        //Destroy(gameObject);
    }
    public float damagemaarajokaaiheutetaan = 1.0f;

    void OnCollisionEnter2D(Collision2D col)
    {
        /*
		//      Debug.Log("tagi=" + col.collider.tag);
		if (col.collider.CompareTag("alustag")) {
			//Destroy (col.gameObject);
			//Debug.Log("game over");

			//Animator aa = col.gameObject.GetComponent<Animator>();
			// aa.SetBool("explode", true);
			//  tuhoa = true;
			//Destroy(gameObject, 0.2f);

			Explode();

			// col.gameObject.SendMessage("Explode");
			IExplodable ex = col.gameObject.GetComponent<IExplodable>();
			if (ex!=null)
            {
				ex.Explode();
            }
			else
            {
				IDamagedable o =
col.gameObject.GetComponent<IDamagedable>();
				if (o != null)
				{
					o.AiheutaDamagea(damagemaarajokaaiheutetaan);


				}
				else
				{
					//		Debug.Log("alus ja explode mutta ei ookkaan " + col.collider.tag);
				}
			}

		}
		else 
		*/
        if (col.collider.tag.Contains("alus"))
        {
            //Destroy(gameObject);
            hengissaoloaika = 0.0f;
            ObjectPoolManager.Instance.ReturnToPool(prefap, gameObject);

            //Explode();

        }
        else if (col.collider.tag.Contains("tiili"))
        {
            //Destroy(gameObject);
            Explode();

        }
        else if (col.collider.CompareTag("makitavihollinenexplodetag") &&
            col.gameObject.transform == gameObject.transform.parent
            )
        {
        }
        else if (col.collider.tag.Contains("ammus"))
        {

        }
        else if (col.collider.tag.Contains("vihollinen"))
        {
            //Destroy (col.gameObject);
            //Destroy(gameObject);
            //Debug.Log ("dame over");
            //just continue
            //Destroy(gameObject);
            if (/*col.collider.gameObject!=creator*/ !OnkoSamaaKokonaisuutta(col.collider.gameObject, creator))
            {
                Explode();
            }

        }
    }


    private bool OnkoSamaaKokonaisuutta(GameObject g1, GameObject g2)
    {
        if (g1 == g2)
        {
            return true;
        }
        //m.SetCreator(this.gameObject);


        if (g1 == null || g2 == null) return false;
        bool ret = g1.transform.root == g2.transform.root;
        return ret;
    }


    public float alivetimeRajahdyksenJalkeen = 0.5f;
    public void Explode()
    {
        if (explosion != null)
        {
            GameObject explosionIns = Instantiate(explosion, transform.position, Quaternion.identity);
            //	RajaytaSprite(gameObject, 3, 3, 1.0f, alivetimeRajahdyksenJalkeen);

            //  Destroy(gameObject);
            //



            //RajaytaSprite(gameObject, 3, 3, 2.0f, 1.2f);
            Destroy(explosionIns, 1.0f);
        }
        //0.5f

        hengissaoloaika = 0.0f;
        ObjectPoolManager.Instance.ReturnToPool(prefap, gameObject);

        //Destroy(gameObject);


    }
    public GameObject explosion;


    private GameObject creator;
    public void SetCreator(GameObject c)
    {
        creator = c;
        Collider[] allColliders = gameObject.GetComponentsInChildren<Collider>();

        Collider[] allColliders2 = c.GetComponentsInChildren<Collider>();

        foreach(Collider c1 in allColliders)
        {
            foreach (Collider c2 in allColliders2)
            {
                Physics.IgnoreCollision(c1, c2);
            }
        }

        IgnoraaCollisiotVihollistenValilla(c, gameObject);


    }



}
