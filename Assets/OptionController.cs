using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionController : MonoBehaviour
{

    public GameObject alusGameObject;


    private Rigidbody2D aluksenm_Rigidbody2D;


    private Rigidbody2D m_Rigidbody2D;
    public int maxPositions;

    public int jarjestysnro;//lahtee ykkosesta


    public int monenkopositionPaahanMennaanLahimmillaan;

    private List<Vector3> positions = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {


        alusGameObject = GameObject.FindGameObjectWithTag("alustag");

    

        aluksenm_Rigidbody2D = alusGameObject.GetComponent<Rigidbody2D>();
        m_Rigidbody2D =GetComponent<Rigidbody2D>(); 
    }

    Vector3 aluksenviimekerrankohta = new Vector3(0, 0, 0);
    private bool onkoAlusLiikkunutViimekerrasta(Vector3 aluksennykykohta)
    {
        return aluksennykykohta.x != aluksenviimekerrankohta.x || aluksennykykohta.y != aluksenviimekerrankohta.y; 
    }

    // Update is called once per frame
    void Update()
    {



    }

    void FixedUpdate()
    {


        // if (onkoAlusLiikkunutViimekerrasta(aluksenm_Rigidbody2D.position))
        // {

        //        m_Rigidbody2D.position.Set(aluksenm_Rigidbody2D.position.x, aluksenm_Rigidbody2D.position.y);

        // transform.position.Set(aluksenm_Rigidbody2D.position.x, aluksenm_Rigidbody2D.position.y,0);

        //transform.position.Set(1.0f,1.0f,0.0f);


        //     }
        if  (alusGameObject!=null &&  onkoAlusLiikkunutViimekerrasta(alusGameObject.transform.position))
        {
            positions.Add(alusGameObject.transform.position);

            // Ensure the list does not exceed the maximum number of positions
            if (positions.Count > maxPositions*jarjestysnro)
            {
                positions.RemoveAt(0); // Remove the oldest position
            }




            Vector3 vector3 = positions[0];

            transform.position = vector3;

            aluksenviimekerrankohta = alusGameObject.transform.position;
        }
        // {
        //    aluksenviimekerrankohta = aluksenm_Rigidbody2D.position;
        //    alusGameObject.transform
        //    return;
        //}

        //positions.Add(aluksenm_Rigidbody2D.position);

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
m_Rigidbody2D.position.x, m_Rigidbody2D.position.y, 0);



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
