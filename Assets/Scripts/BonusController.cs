using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusController : BaseController
{
  //  public float rotationTime = 4f; // Time for a full 360-degree rotation
    //private float elapsedTime = 0f;

    // Start is called before the first frame update
    private AudioplayerController ad;
    /*
    private SpriteRenderer[] _spriteRenderers;
    private Material[] _materials;
    private int _GlowColor = Shader.PropertyToID("_GlowColor");
    private Color glowColor;
    public float glowSpeed = 0.1f;

    private Color[] _baseColors;
    private bool onko = false;
    */
    void Start()
    {
        ad = PalautaAudioplayerController();
        /*
        _spriteRenderers = GetComponentsInParent<SpriteRenderer>();
        _materials = new Material[_spriteRenderers.Length];

        _baseColors=new Color[_spriteRenderers.Length];
        for (int i = 0; i < _spriteRenderers.Length; i++)
        {
            // Clone materials to avoid shared material side effects
            _spriteRenderers[i].material = new Material(_spriteRenderers[i].material);
            _materials[i] = _spriteRenderers[i].material;

            glowColor = _spriteRenderers[i].material.GetColor(_GlowColor);
            if (glowColor!=null)
            {
                onko = true;
            }
            else
            {
                Debug.Log("ei ole");
            }

            float originalIntensity = glowColor.maxColorComponent;

            _baseColors[i] = glowColor / Mathf.Max(originalIntensity, 0.0001f);

        }
        */
    }
    void OnBecameInvisible()
    {
        //Debug.Log ("OnBecameInvisible");
        // Destroy the enemy
        //tuhoa = true;

        //Destroy(gameObject);
        //BaseDestroy();

    }

    public float intensityMin = 0.5f;
    public float intensityMax = 2f;
    public float pulseSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        /*

              // float intensity = Mathf.PingPong(Time.time * glowSpeed, pingpongylaraja) + pingponlisays;

               float lerpedIntensity = Mathf.Lerp(intensityMin, intensityMax, Mathf.PingPong(Time.time * pulseSpeed, 1f));

              // Color glowColoruus = glowColor * intensity;

               for (int i = 0; i < _materials.Length; i++)
               {
                   //@todo lerps glowcolor intensity over time
                   Color modulatedColor = _baseColors[i] * lerpedIntensity;
                   _materials[i].SetColor(_GlowColor, modulatedColor);
               }
               */

        Tuhoa(gameObject);
    }

    bool onkoAlukseenJoTormatty = false;
    public void OnTriggerEnter2D(Collider2D col)
    {

        if (IsGoingToBeDestroyed())
        {
            return;
        }
        
      //  Debug.Log($"This object's collider: {col.collider.name}");
      //  Debug.Log($"Other object's collider: {col.otherCollider.name}");
        if (!onkoAlukseenJoTormatty && col.CompareTag("alustag"))
        {
            onkoAlukseenJoTormatty = true;
            GetComponent<Collider2D>().enabled = false;//tämän pitäisi riittää
            RajaytaSprite(gameObject, 3, 3, 2.0f, 1f);
            //Destroy(this.gameObject);
            BaseDestroy();

            ad.BonusPlay();

            AlusController myScript = col.gameObject.GetComponent<AlusController>();
            if (myScript != null)
            {
                myScript.BonusCollected();

            }
            else
            {
                Debug.Log("aluscontrollerin skripti oli null");
            }

        }
    }

    /*

            void OnCollisionEnter2D(Collision2D col)
    {

        Debug.Log($"This object's collider: {col.collider.name}");
        Debug.Log($"Other object's collider: {col.otherCollider.name}");

        if (onkoAlukseenJoTormatty)
        {
            Debug.Log("on mahdollista");
            return;
        }
        if (col.otherCollider.tag=="alustag")
        {
            Debug.Log("on mika");
        }

        if (col.collider.tag == "alustag")
        {

            onkoAlukseenJoTormatty = true;

            // col.otherCollider.gameObject.SendMessage
            // col.otherCollider.gameObject.SendMessage("BonusCollected");
            ad.BonusPlay();

            AlusController myScript = col.otherCollider.gameObject.GetComponent<AlusController>();
            if (myScript != null)
            {
                myScript.BonusCollected();

            }
            else
            {
                Debug.Log("aluscontrollerin skripti oli null");
            }

            RajaytaSprite(gameObject, 3, 3, 2.0f, 2f);
            Destroy(this.gameObject);

            //  Debug.Log("on bonustaaaaaaaaa ");

            //   Debug.Log("bonus keratty");

            //pitää 

            // col.otherCollider


        }

    }
    */
}
