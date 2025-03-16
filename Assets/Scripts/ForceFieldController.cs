using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]

//[ExecuteAlways]
public class ForceFieldController : MonoBehaviour
{
    // Start is called before the first frame update
    private ParticleSystem particleSystem;

    public GameObject valo;


    public int hittienmaaraJokaKestetaan = 10;
    private int hittienmaara = 0;


    public bool partikkelitEnabloituna = true;
    ParticleSystem.MainModule main;
    public float alphaValueAloitusArvo = 0.5f;
    public float alphaValueLopetusArvo = 0.1f;
    public float alphaValueNykyarvo = 0.5f;
    void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
       // main = particleSystem.main;
       // var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
       // if (renderer != null && renderer.material != null)
       // {
        //    Color color = renderer.material.color;
        //    renderer.material.color = new Color(color.r, color.g, color.b, alphaValueAloitusArvo);
        //}
        VaihdaAlphaa();

    }

    // Update is called once per frame
    void Update()
    {
        if (partikkelitEnabloituna)
        {
            if (!particleSystem.isPlaying)
            {
                particleSystem.Play();
            }


        }
        else
        {
            particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
    /*
    void OnParticleCollision(GameObject other)
    {
        if (other.tag.Contains("vihollinen") && !other.tag.Contains("tiili"))
        {
            IExplodable ex = other.gameObject.GetComponent<IExplodable>();
            if (ex != null)
            {
                ex.Explode();
            }
            else
            {
                Destroy(other.gameObject.transform.parent);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //haukisilmavihollinenexplodetag

        //  col.otherCollider t‰‰ on alus

        //explodetag
        if (col.collider.tag.Contains("hauki") || col.collider.tag.Contains("tiili") ||
            col.collider.tag.Contains("pyoroovi") || col.collider.tag.Contains("laatikkovihollinenexplodetag"))
        {

        }
        else if (col.collider.tag.Contains("makitavihollinenexplodetag"))
        {
            PiippuMakitaVihollinenController p = col.gameObject.GetComponent<PiippuMakitaVihollinenController>();
            if (p)
            {
                if (col.gameObject.transform.parent!=null)
                {
                    IExplodable ex = col.gameObject.transform.parent.GetComponent<IExplodable>();
                    if (ex!=null)
                    {
                        ex.Explode();
                    }
                    else
                    {
                        Destroy(col.gameObject.transform.parent);
                    }
                    
                }

            }
            else
            {
                IExplodable ex = col.gameObject.GetComponent<IExplodable>();
                if (ex != null)
                {
                    ex.Explode();
                }
                else
                {
                    Destroy(col.gameObject.transform.parent);
                }
            }
            PoltaValo();

        }

        else if (col.collider.tag.Contains("vihollinen"))
        {
            IExplodable ex = col.gameObject.GetComponent<IExplodable>();
            if (ex != null)
            {
                ex.Explode();
            }
            else
            {
                Destroy(col.gameObject.transform.parent);
            }
            PoltaValo();


        }

    }
    */
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Contains("vihollinen")  && !col.tag.Contains("tiili"))
        {
           

            IExplodable o =
    col.gameObject.GetComponent<IExplodable>();
            if (o != null)
            {
                o.Explode();
            }
            else
            {
                Debug.Log("vihollinen ja explode mutta ei ookkaan " + col.tag);
                Destroy(col.gameObject);
            }
            PoltaValo();
            hittienmaara++;
            //t‰h‰n se himmennys eli partikkelien m‰‰r‰‰‰ v‰hennet‰‰n tms
            if (hittienmaara>= hittienmaaraJokaKestetaan)
            {
                gameObject.SetActive(false);
            }
        }

    }
    private void PoltaValo()
    {
        valo.GetComponent<AlusLightController>().SetExplosionLights();
        VaihdaAlphaa();
        //float alpha = newColor.a;
        //ParticleSystem.MainModule main = particleSystem.main; // Get a fresh MainModule reference

        //Color newColor = main.startColor.color;
        //float alpha = newColor.a - 1;
        //newColor.a = alpha; // Set new alpha

        //  Color color = main.startColor.color;
        //  float alpha255 = color.a - 0.5f;
        //  Debug.Log($"Alpha: {alpha255}"); // Debugging

        // Modify alpha (if needed) and set it back
        //   color.a = alpha255;
        //     main.startColor = new ParticleSystem.MinMaxGradient(color); // Apply updated color

        // if (particleSystem.isPlaying)
        // {
        //particleSystem.Stop();
        //particleSystem.Play();
        // }
    }

    private void VaihdaAlphaa()
    {
        alphaValueNykyarvo = Mathf.Lerp(alphaValueAloitusArvo, alphaValueLopetusArvo, (float)hittienmaara / hittienmaaraJokaKestetaan);

        main = particleSystem.main;
        var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        if (renderer != null && renderer.material != null)
        {
            Color color = renderer.material.color;
            renderer.material.color = new Color(color.r, color.g, color.b, alphaValueNykyarvo);
        }
    }
}