using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HaukiController : BaseController
{
    public float rotatemax = 45.0f;
    public float rotatemin = -45.0f;

    public float rotatetimeseconds = 2.0f;//sekkaa
    private float rotationTime = 0f;       // Timer to control the rotation

    public float liikex=- 0.1f;
    private Rigidbody2D m_Rigidbody2D;
    private SpriteRenderer m_SpriteRenderer;

    private GameObject alus;

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_Rigidbody2D.simulated = false;
        GameObject[] allObstacles = GameObject.FindGameObjectsWithTag("alustag");
        foreach (GameObject obstacles in allObstacles)
        {
            alus = obstacles;
        }

    }

    private bool OnkoOkToimia()
    {
        return m_SpriteRenderer.isVisible;
    }

    // Update is called once per frame
    void Update()
    {
        if (!OnkoOkToimiaUusi(gameObject))
        {
            return;
        }
        m_Rigidbody2D.simulated = true;
        rotationTime += Time.deltaTime;

        // Calculate the t value for interpolation. It oscillates between 0 and 1.
        float t = Mathf.PingPong(rotationTime / rotatetimeseconds, 1f);

        // Interpolate between rotatemin and rotatemax based on t
        float currentRotation = Mathf.Lerp(rotatemin, rotatemax, t);

        // Apply the rotation to the object's local rotation around the Z axis (for 2D) or Y axis (for 3D)
        transform.localRotation = Quaternion.Euler(0, 0, currentRotation);  // Z-axis rotation (for 2D)

       // transform.position.x = transform.position.x + liikex;

        // For 3D objects, you can change it to: Quaternion.Euler(0, currentRotation, 0) for Y-axis rotation
    }

    public void FixedUpdate()
    {
        if (!OnkoOkToimiaUusi(gameObject))
        {
            return;
        }
        // transform.position = new Vector2(transform.position.x + liikex, transform.position.y);
        bool ylos = AlusYlhaalla();
        bool alas = AlusAlhaalla();
        if (ylos)
        {
            m_Rigidbody2D.velocity = new Vector2(liikex, 1.0f);
        }
        else if (alas)
        {
            m_Rigidbody2D.velocity = new Vector2(liikex, -1.0f);
        }
        else
        {
            m_Rigidbody2D.velocity = new Vector2(liikex, 0.0f);
        }

    }

    public bool AlusYlhaalla()
    {
        return alus.transform.position.y > transform.position.y;
    }

    public bool AlusAlhaalla()
    {
        return alus.transform.position.y < transform.position.y;
    }


    public void Explode()
    {
        Destroy(gameObject);
    }



    public bool tuhoaJosOnBecameInvisible = true;

    void OnBecameInvisible()
    {
        //MJM 18.12.2023 OTA POIS KOMMENTEISTA
        if (tuhoaJosOnBecameInvisible)
        {
            Destroy(gameObject);
        }
    }
}
