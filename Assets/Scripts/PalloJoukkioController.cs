using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloJoukkioController : BaseController
{
    // Start is called before the first frame update

    public GameObject pallo;
    private GameObject alus;
    public int pallojenmaara = 10;
    private int pallojennykymaara = 0;
    private float viimeisinx = 0.0f;
    void Start()
    {




    }

    public Color gizmoColor = Color.green;
    public float gizmoRadius = 0.5f;

    // This method draws the Gizmo when the object is selected

    public void Awake()
    {
        //FixedUpdate2();
    }

    private bool pallottehty = false;

    // Update is called once per frame
    void Update()
    {
        if (IsGoingToBeDestroyed())
        {
            return;
        }


        //  GameObject instanssi= Instantiate(pallo);

        bool nakyvissa = IsObjectInView(Camera.main, transform);
        if (!nakyvissa)
        {
            return;
        }
        if (nakyvissa && !pallottehty)
        {
            
     
            bool instanssiluotu = false;
            while (true)
            {

             
                //float xarvo = i * 4.0f;
                float yarvo = 0.0f;// transform.position.y;
                if (voikoInstantioida(viimeisinx, yarvo))
                {

                    Vector3 v3 =
new Vector3(
transform.position.x + viimeisinx, transform.position.y + yarvo, 0);

                    GameObject instanssi = Instantiate(pallo, v3, Quaternion.identity);
                    pallojennykymaara++;
                    instanssiluotu = true;
                }
                else
                {
                    //   Debug.Log("ei voi");
                }
                viimeisinx = viimeisinx + 1.0f;

                if (pallojennykymaara >= pallojenmaara)
                {
                    pallottehty = true;
                    viimeisinx = 0.0f;
                    break;
                }
                if (instanssiluotu)
                {
                    break;
                }
                //  PalliController p = instanssi.GetComponent<PalliController>();
                //  p.alusGameObject = alus;
            }
        }
        if (nakyvissa && pallottehty)
        {
 

                SpriteRenderer[] ss =
                GetComponentsInChildren<SpriteRenderer>();

                if (ss != null)
                {
                    foreach (SpriteRenderer s in ss)
                    {
                        if (s.isVisible)
                        {
                            return;
                        }
                    }
                }
              //  Destroy(gameObject);
            BaseDestroy();
           
        }



        // instanssi.GetComponent<Rigidbody2D>().velocity = ve;

    }

    public Vector2 alabocenter = new Vector2(0, 0);
    public Vector2 boxsizealhaalla = new Vector2(2, 2);

    public LayerMask layerMask;
    public bool voikoInstantioida(float pos, float posy)
    {
        Vector2 uusi = new Vector2(alabocenter.x + pos, alabocenter.y + posy);
        //return !onkoTagiaBoxissa("vihollinen", boxsizealhaalla, uusi, layerMask);
        return !onkoTagiaBoxissa("vihollinen", boxsizealhaalla, uusi, layerMask);



    }


    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);

        Gizmos.color = Color.blue;

        Gizmos.DrawWireCube((Vector2)transform.position + alabocenter, boxsizealhaalla);


    }



    bool IsObjectInView(Camera cam, Transform objTransform)
    {
        // Convert object's world position to viewport coordinates
        Vector3 viewportPoint = cam.WorldToViewportPoint(objTransform.position);

        // Check if the object is within the camera's view horizontally (X) and vertically (Y),
        // and whether it's in front of the camera (Z)
        return viewportPoint.x > 0 && viewportPoint.x < 1 &&
               viewportPoint.y > 0 && viewportPoint.y < 1 &&
               viewportPoint.z > 0;
    }
}
