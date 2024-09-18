using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalloJoukkioController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject pallo;
    private GameObject alus;
    public int pallojenmaara = 10;
    void Start()
    {
     
        

        
    }
 
    public Color gizmoColor = Color.green;
    public float gizmoRadius = 0.5f;

    // This method draws the Gizmo when the object is selected
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, gizmoRadius);
    }
    
 private bool pallottehty = false;

    // Update is called once per frame
    void Update()
    {

        //  GameObject instanssi= Instantiate(pallo);
        
        bool nakyvissa = IsObjectInView(Camera.main, transform);
        if (!nakyvissa)
        {
            return;
        }
        if (nakyvissa && !pallottehty)
        {
            pallottehty = true;
            for (int i=0;i<pallojenmaara;i++)
            {
                GameObject instanssi = Instantiate(pallo, new Vector3(
    transform.position.x+i*4.0f, transform.position.y, 0), Quaternion.identity);

              //  PalliController p = instanssi.GetComponent<PalliController>();
              //  p.alusGameObject = alus;
            }
        }

        
 

       // instanssi.GetComponent<Rigidbody2D>().velocity = ve;

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
