using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour
{

    public GameObject alusGameObject;


    private Rigidbody2D aluksenm_Rigidbody2D;


    private Rigidbody2D m_Rigidbody2D;
    public int maxPositionMaaraMikaAluksestaTallennetaan;

    public int jarjestysnro;//lahtee ykkosesta



    private AlusController aluscontroller;
    // Start is called before the first frame update
    void Start()
    {


        alusGameObject = GameObject.FindGameObjectWithTag("alustag");

    

        aluksenm_Rigidbody2D = alusGameObject.GetComponent<Rigidbody2D>();
        m_Rigidbody2D =GetComponent<Rigidbody2D>();


         aluscontroller=alusGameObject.GetComponent<AlusController>();



    }

    private int PalautaAmmustenMaksimimaara()
    {
        return aluscontroller.ammustenmaksimaaraProperty;

    }

    //public Transform player; // Reference to the player's spaceship


    private Vector3 targetPosition;

    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 alusScreenCoordinaateissa =aluscontroller.palautaViimeinenSijaintiScreenpositioneissa(jarjestysnro);

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(alusScreenCoordinaateissa);

        transform.position = worldPosition;
        */
        

        bool even = jarjestysnro % 2 == 0;
        Vector2 testi;
        if (even)
        {
             testi = new Vector2(1, -1);
        }
        else
        {
             testi = new Vector2(1, 1);

        }
        if (jarjestysnro==1)
        {
            testi = new Vector2(1, -1);
        }
        else if (jarjestysnro == 2)
        {
            testi = new Vector2(1, 1);
        }
        else if (jarjestysnro == 3)
        {
            testi = new Vector2(1, -2);
        }
        else if (jarjestysnro == 4)
        {
            testi = new Vector2(1, 2);
        }
        else if (jarjestysnro == 5)
        {
            testi = new Vector2(2, 0);
        }

        //Vector2.left

        //  targetPosition = (Vector2)alusGameObject.transform.position + testi * aluscontroller.optiotfollowDistance* jarjestysnro;
        targetPosition = (Vector2)alusGameObject.transform.position + testi * aluscontroller.optiotfollowDistance;


        // Smoothly move the Option Ball towards the target position
        transform.position = Vector2.Lerp(transform.position, targetPosition, aluscontroller.optiotmoveSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {


    }

    private bool OnkoAmmustenMaaraAlleMaksimin()
    {
        int maksimi = PalautaAmmustenMaksimimaara();
        // int nykymaara = palautaAmmustenMaara();
        int nykymaara = aluksenluomienElossaOlevienAmmustenMaara;
        return nykymaara < maksimi;
    }

    private bool OnkoAlasAmmustenMaaraAlleMaksimin()
    {
        int maksimi = PalautaAmmustenMaksimimaara();
        // int nykymaara = palautaAmmustenMaara();
        int nykymaara = alasammusaluksenluomienElossaOlevienAmmustenMaara;
        return nykymaara < maksimi;
    }

    private bool OnkoYlosAmmustenMaaraAlleMaksimin()
    {
        int maksimi = PalautaAmmustenMaksimimaara();
        // int nykymaara = palautaAmmustenMaara();
        int nykymaara = ylosammusaluksenluomienElossaOlevienAmmustenMaara;
        return nykymaara < maksimi;
    }

    public void ammuNormilaukaus(GameObject ammusPrefab,Vector2 aa)
    {

        if (OnkoAmmustenMaaraAlleMaksimin())
        {
            Vector3 v3 =
    new Vector3(0.1f +
    m_Rigidbody2D.position.x, m_Rigidbody2D.position.y, 0);
            GameObject instanssi=
            ObjectPoolManager.Instance.GetFromPool(ammusPrefab, v3, Quaternion.identity);


            //GameObject instanssi = Instantiate(ammusPrefab, v3, Quaternion.identity);
            // instanssi.
            //       instanssi.tag = "optionammustag";tama kommentoitu

            instanssi.GetComponent<Rigidbody2D>().velocity =aa;
            AmmusController ac = instanssi.GetComponent<AmmusController>();
            if (ac!=null)
            {
                ac.SetOption(this.gameObject);
                aluksenluomienElossaOlevienAmmustenMaara++;
            }
            else
            {
                LaserController lc= instanssi.GetComponent<LaserController>();
                lc.alkuvelocity = aa;
                if (lc!=null)
                {
                    lc.SetOption(this.gameObject);
                    aluksenluomienElossaOlevienAmmustenMaara++;
                }
            }

        }
    }
    private int aluksenluomienElossaOlevienAmmustenMaara = 0;
    public void VahennOptionLuomienElossaOlevienAmmustenMaaraa()
    {
        aluksenluomienElossaOlevienAmmustenMaara = aluksenluomienElossaOlevienAmmustenMaara - 1;
    }

    public void LisaaaOptionLuomienElossaOlevienAmmustenMaaraa()
    {
        aluksenluomienElossaOlevienAmmustenMaara = aluksenluomienElossaOlevienAmmustenMaara + 1;
    }


    private int alasammusaluksenluomienElossaOlevienAmmustenMaara = 0;
    public void VahennOptionLuomienElossaOlevienAlasAmmustenMaaraa()
    {
        alasammusaluksenluomienElossaOlevienAmmustenMaara = alasammusaluksenluomienElossaOlevienAmmustenMaara - 1;
    }

    public void LisaaaOptionLuomienElossaOlevienAlasAmmustenMaaraa()
    {
        alasammusaluksenluomienElossaOlevienAmmustenMaara = alasammusaluksenluomienElossaOlevienAmmustenMaara + 1;
    }


    private int ylosammusaluksenluomienElossaOlevienAmmustenMaara = 0;
    public void VahennOptionLuomienElossaOlevienYlosAmmustenMaaraa()
    {
        ylosammusaluksenluomienElossaOlevienAmmustenMaara = ylosammusaluksenluomienElossaOlevienAmmustenMaara - 1;
    }

    public void LisaaaOptionLuomienElossaOlevienYlosAmmustenMaaraa()
    {
        ylosammusaluksenluomienElossaOlevienAmmustenMaara = ylosammusaluksenluomienElossaOlevienAmmustenMaara + 1;
    }


    //GameObject instanssiBulletAlas;
    // GameObject instanssiBulletYlos;

    public void ammuAlaslaukaus(GameObject bulletPrefab,Vector2 bulletinalkuvelocity,float bulletgravityscale)
    {
        //   if (instanssiBulletAlas == null ||
        //       instanssiBulletAlas.GetComponent<BaseController>().IsGoingToBeDestroyed()
        //     )
        // {
        if (OnkoAlasAmmustenMaaraAlleMaksimin())
        {

            Vector3 v3 =
new Vector3(0.1f +
m_Rigidbody2D.position.x, m_Rigidbody2D.position.y - 0.0f, 0);


            /*
            instanssiBulletYlos.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, bulletinalkuvelocityy);

            instanssiBulletYlos.GetComponent<Rigidbody2D>().gravityScale = -bulletingravityscale;

            */


            //instanssiBulletAlas = Instantiate(bulletPrefab, v3, Quaternion.identity);

            GameObject instanssiBulletAlas = ObjectPoolManager.Instance.GetFromPool(bulletPrefab, v3, Quaternion.identity);
            alasammusaluksenluomienElossaOlevienAmmustenMaara++;


            instanssiBulletAlas.GetComponent<BulletScript>().SetAluksenluoma(false);
            instanssiBulletAlas.GetComponent<BulletScript>().SetOption(gameObject);


            //instanssiBulletAlas.SendMessage("Alas", true);

            instanssiBulletAlas.GetComponent<BulletScript>().alas = true;

            /*
            IAlas alas = instanssiBulletAlas.GetComponent<IAlas>();
            if (alas != null)
            {
                alas.Alas(true);
            }
            */

            instanssiBulletAlas.GetComponent<Rigidbody2D>().velocity = bulletinalkuvelocity;


            instanssiBulletAlas.GetComponent<Rigidbody2D>().gravityScale = bulletgravityscale;
            //}
        }
    }

    public void ammuYloslaukaus(GameObject bulletPrefab, Vector2 bulletinalkuvelocity, float bulletgravityscale)
    {
        // if (instanssiBulletYlos == null ||
        //       instanssiBulletYlos.GetComponent<BaseController>().IsGoingToBeDestroyed())
        // {

        if (OnkoYlosAmmustenMaaraAlleMaksimin())
        {

            Vector3 v3 =
new Vector3(0.1f +
m_Rigidbody2D.position.x, m_Rigidbody2D.position.y, 0);

            ylosammusaluksenluomienElossaOlevienAmmustenMaara++;


            //instanssiBulletYlos = Instantiate(bulletPrefab, v3, Quaternion.identity);


            GameObject instanssiBulletYlos = ObjectPoolManager.Instance.GetFromPool(bulletPrefab, v3, Quaternion.identity);

            instanssiBulletYlos.GetComponent<BulletScript>().SetAluksenluoma(false);
            instanssiBulletYlos.GetComponent<BulletScript>().SetOption(gameObject);

            instanssiBulletYlos.GetComponent<BulletScript>().alas = false;

            //instanssiBulletYlos.SendMessage("Alas", false);

            /*
            IAlas alas = instanssiBulletYlos.GetComponent<IAlas>();
            if (alas != null)
            {
                alas.Alas(false);
            }
            */

            instanssiBulletYlos.GetComponent<Rigidbody2D>().velocity = bulletinalkuvelocity;

            instanssiBulletYlos.GetComponent<Rigidbody2D>().gravityScale = bulletgravityscale;

            // }
        }
    }

}
