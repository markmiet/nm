using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PommiController : BaseController,  IExplodable
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
    void Update()
    {
        TuhoaMuttaAlaTuhoaJosOllaanEditorissa(gameObject);
        delta += Time.deltaTime;

        if (delta> rajahdysaika)
        {
            Explode();
            delta = 0.0f;
        }
    }
    public float force = 1;
    public float alive = 2;
    public  float massa = 1;
    public int rows = 5;
    public int cols = 5;

    public void Explode()
    {

        RajaytaSprite(gameObject, rows,cols, force, alive, massa, true);
        /* 
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
          }

       */

        

              GameObject instanssi = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(instanssi,2);

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the explosion radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}

