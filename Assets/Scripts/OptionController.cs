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

    public void ammuNormilaukaus(GameObject ammusPrefab)
    {

        if (OnkoAmmustenMaaraAlleMaksimin())
        {
            Vector3 v3 =
    new Vector3(0.1f +
    m_Rigidbody2D.position.x, m_Rigidbody2D.position.y, 0);

            GameObject instanssi = Instantiate(ammusPrefab, v3, Quaternion.identity);
            // instanssi.
            //       instanssi.tag = "optionammustag";tama kommentoitu

            instanssi.GetComponent<Rigidbody2D>().velocity = new Vector2(20, 0);
            AmmusController ac = instanssi.GetComponent<AmmusController>();
            ac.SetOption(this.gameObject);
            aluksenluomienElossaOlevienAmmustenMaara++;
        }
    }
    private int aluksenluomienElossaOlevienAmmustenMaara = 0;
    public void VahennOptionLuomienElossaOlevienAmmustenMaaraa()
    {
        aluksenluomienElossaOlevienAmmustenMaara = aluksenluomienElossaOlevienAmmustenMaara - 1;
    }



    GameObject instanssiBulletAlas;
    GameObject instanssiBulletYlos;

    public void ammuAlaslaukaus(GameObject bulletPrefab)
    {
          if (instanssiBulletAlas == null)
        {
            Vector3 v3 =
new Vector3(0.1f +
m_Rigidbody2D.position.x, m_Rigidbody2D.position.y-0.0f, 0);



            instanssiBulletAlas = Instantiate(bulletPrefab, v3, Quaternion.identity);
            //instanssiBulletAlas.SendMessage("Alas", true);

            IAlas alas = instanssiBulletAlas.GetComponent<IAlas>();
            if (alas != null)
            {
                alas.Alas(true);
            }

                instanssiBulletAlas.GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, -2);


            instanssiBulletAlas.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        }

    }

    public void ammuYloslaukaus(GameObject bulletPrefab)
    {
        if (instanssiBulletYlos == null)
        {
            Vector3 v3 =
new Vector3(0.1f +
m_Rigidbody2D.position.x, m_Rigidbody2D.position.y, 0);



            instanssiBulletYlos = Instantiate(bulletPrefab, v3, Quaternion.identity);
            //instanssiBulletYlos.SendMessage("Alas", false);

            IAlas alas = instanssiBulletYlos.GetComponent<IAlas>();
            if (alas != null)
            {
                alas.Alas(false);
            }

            instanssiBulletYlos.GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, 2);

            instanssiBulletYlos.GetComponent<Rigidbody2D>().gravityScale = -1.0f;

        }

    }

}
