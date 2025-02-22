using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInfoController : MonoBehaviour
{

    Camera main;
    Kamera kamera;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        main = Camera.main;
        kamera=main.GetComponent<Kamera>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        //bool onko = IsObjectInOrthographicView(transform, main);

        bool onko = IsObjectInCameraView(transform.position);

        if (onko)
        {
            GameObject olemassa =
            kamera.cameraInfo;
            if (olemassa!=null)
            {
                bool onkotoinen=IsObjectInCameraView(olemassa.transform.position);
                if (!onkotoinen)
                {
                    kamera.cameraInfo = this.gameObject;
                }
                else if (transform.position.x>olemassa.transform.position.x)
                {
                    kamera.cameraInfo = this.gameObject;
                }
            }
            else
            {
                kamera.cameraInfo = this.gameObject;
            }
        }
    }

    public float scrollspeedx = 3.0f;
    public float scrollspeedy = 3.0f;

    public float rotationz = 0.0f;
    public bool stop;
    public float stoptime;


    public bool generoilisaavihollisia = false;
    public GameObject vihollinenjokageneroidaan;
    public int vihollismaaranrajaarvo = 1;
    public float generointivali = 5.0f;

    public bool naytasavu = false;
    public bool naytataustat = true;
    public GameObject savu;
    public GameObject tausta1;
    public GameObject tausta2;
    public GameObject tausta3;




    private bool IsObjectInOrthographicView(Transform target, Camera camera)
    {
        Vector3 position = camera.WorldToScreenPoint(target.position);
        return position.x >= 0 && position.x <= Screen.width &&
               position.y >= 0 && position.y <= Screen.height &&
               position.z > 0; // Ensure it's in front of the camera
    }

    private bool IsObjectInCameraView(Vector3 worldPosition)
    {
        Vector3 viewportPosition = main.WorldToViewportPoint(worldPosition);
        bool viewportissa= viewportPosition.x > 0 && viewportPosition.x < 1 &&
               viewportPosition.y > 0 && viewportPosition.y < 1 &&
               viewportPosition.z > 0; // Ensure the object is in front of the camera


        float x = worldPosition.x;
        float kamerax = main.transform.position.x;
        bool onkonakyvissa = spriteRenderer.isVisible;
        bool kameraxylix = kamerax >= x;

        if (viewportissa && onkonakyvissa && kameraxylix)
        {
            return true;
        }
        return false;
    }

}
