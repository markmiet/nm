using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PommiController : BaseController, IExplodable
{

    [SerializeField] private float explosionRadius = 5f;   // Radius of the explosion
    [SerializeField] private float explosionForce = 500f; // Force of the explosion
    [SerializeField] private LayerMask affectedLayers;    // Layers that can be affected by the explosion


    // Start is called before the first frame update
    void Start()
    {

    }

    public float rajahdysaika = 3.0f;
    public GameObject explosion;

    // Update is called once per frame
    private float delta = 0.0f;
    private bool rajaytetty = false;
    void Update()
    {
        if (IsGoingToBeDestroyed())
        {
            return;
        }

        Tuhoa(gameObject);
        delta += Time.deltaTime;

        if (!rajaytetty && delta > rajahdysaika)
        {
            rajaytetty = true;
            Explode();
            delta = 0.0f;
        }
    }
    public float force = 1;
    public float alive = 2;
    public float massa = 1;
    public int rows = 5;
    public int cols = 5;

    public void Explode()
    {

        RajaytaSprite(gameObject, rows, cols, force, alive, massa, true);
        /* 
       */
        /*
            Camera targetCamera = Camera.main;

        // Calculate camera bounds in world space
        Vector2 cameraPos = targetCamera.transform.position;
        float height = targetCamera.orthographicSize * 2;
        float width = height * targetCamera.aspect;

        Bounds cameraBounds = new Bounds(cameraPos, new Vector3(width, height, 0));

        // Optional: visualize bounds
        Debug.DrawLine(cameraBounds.min, cameraBounds.max, Color.green, 5f);

        List<GameObject> visibleObjects = new List<GameObject>();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (!obj.activeInHierarchy)
                continue;

            Vector2 objPos = obj.transform.position;

            // Check if the object's position is inside the camera bounds
            Collider2D col = obj.GetComponent<Collider2D>();
            if (col && cameraBounds.Intersects(col.bounds))
            {
                visibleObjects.Add(obj);
            }

            //    if (cameraBounds.Contains(objPos))
            // {
            //     visibleObjects.Add(obj);
            // }
        }

        Debug.Log("Visible 2D Objects: " + visibleObjects.Count);
        foreach (GameObject go in allObjects)
        {
            if (!IsInLayerMask(go))
            {
                continue;
            }
            */
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, affectedLayers);

          foreach (Collider2D collider in colliders)
          {
            
              Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();

              if (rb != null)
              {
                  // Calculate direction from the explosion center to the object
                  Vector2 direction = (rb.position - (Vector2)transform.position).normalized;

                  // Apply force to the Rigidbody2D
                  rb.AddForce(direction * explosionForce);
              }
              
              
            IExplodable ii =
            collider.gameObject.GetComponent<IExplodable>();
            if (ii!=null)
            {
                for (int i=0;i<1;i++)
                {
                    ii.Explode();
                }

            }

            IDamagedable dama= collider.gameObject.GetComponent<IDamagedable>();
            if (dama != null)
            {
                Vector2 contactPoint = collider.gameObject.transform.position;
                dama.AiheutaDamagea(1, contactPoint);
            }

            ChildColliderReporter child = collider.gameObject.GetComponent<ChildColliderReporter>();
            if (child != null)
            {
                Vector2 contactPoint = collider.gameObject.transform.position;
                for (int i=0;i<1;i++)
                {
                    child.RegisterHit(contactPoint,collider.gameObject);
                }
                
            }
            

        }




        GameObject instanssi = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(instanssi, 2);

        //  Destroy(gameObject);
        BaseDestroy();
    }

    bool IsInLayerMask(GameObject obj)
    {
        return ((affectedLayers.value & (1 << obj.layer)) != 0);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the explosion radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

