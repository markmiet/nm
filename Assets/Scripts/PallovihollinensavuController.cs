using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PallovihollinensavuController : BaseController
{

    private ParticleSystem particleSystem;
    private BoxCollider2D boxCollider;
    // Start is called before the first frame update

    public float savungamadeMaara = 0.1f;

    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Align the BoxCollider2D initially
        AlignColliderWithParticleSystem();
    }

    // Update is called once per frame
    void Update()
    {
        // Continuously align the BoxCollider2D with the Particle System
      //  AlignColliderWithParticleSystem();

    }

    public float widthScaleFactor = 0.5f;  // Scale factor for width
    public float heightScaleFactor = 0.6f; // Scale factor for height


    private void AlignColliderWithParticleSystem()
    {
        if (particleSystem == null || boxCollider == null) return;

        // Get the bounds of the Particle System's renderer
        Renderer particleRenderer = particleSystem.GetComponent<Renderer>();
        Bounds rendererBounds = particleRenderer.bounds;

        // Scale down the bounds separately for width and height
        float adjustedWidth = rendererBounds.size.x * widthScaleFactor;
        float adjustedHeight = rendererBounds.size.y * heightScaleFactor;

        // Adjust BoxCollider2D position and size
        boxCollider.offset = transform.InverseTransformPoint(rendererBounds.center);
        boxCollider.size = new Vector2(adjustedWidth, adjustedHeight);
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (true)
            return;

        if (IsGoingToBeDestroyed())
        {
            return;
        }

        if (col.CompareTag("alustag"))
        {

            AlusController o =
col.gameObject.GetComponent<AlusController>();
            if (o != null)
            {
                //kuumaa
                // o.Explode();
         //       Debug.Log("OnTriggerEnter2D Still in trigger: ");
                o.Savua();
          //      o.AiheutaDamagea(savungamadeMaara);

            }

        }
       
    }


    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("alustag"))
        {

            AlusController o =
col.gameObject.GetComponent<AlusController>();
            if (o != null)
            {
                //kuumaa
                // o.Explode();
                //         Debug.Log("OnTriggerStay2D Still in trigger: ");

                   o.Savua();
               // o.AiheutaDamagea(savungamadeMaara);

            }

        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
     //   Debug.Log("Exited trigger: " + other.name);
    }


}