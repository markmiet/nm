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


    // Update is called once per frame
    void Update()
    {
        Vector3 alusScreenCoordinaateissa =aluscontroller.palautaViimeinenSijaintiScreenpositioneissa(jarjestysnro);

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(alusScreenCoordinaateissa);

        transform.position = worldPosition;

    }

    void FixedUpdate()
    {


    }


    public void ammuNormilaukaus(GameObject ammusPrefab)
    {
        Vector3 v3 =
new Vector3(0.1f +
m_Rigidbody2D.position.x  , m_Rigidbody2D.position.y, 0);

        GameObject instanssi = Instantiate(ammusPrefab, v3, Quaternion.identity);
        // instanssi.
        instanssi.tag = "optionammustag";

        instanssi.GetComponent<Rigidbody2D>().velocity = new Vector2(20, 0);
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
            instanssiBulletAlas.SendMessage("Alas", true);
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
            instanssiBulletYlos.SendMessage("Alas", false);
            instanssiBulletYlos.GetComponent<Rigidbody2D>().velocity = new Vector2(0.1f, 2);

            instanssiBulletYlos.GetComponent<Rigidbody2D>().gravityScale = -1.0f;

        }

    }

}
