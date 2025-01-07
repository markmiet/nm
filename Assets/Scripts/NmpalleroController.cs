using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NmpalleroController : BaseController
{
    // Start is called before the first frame update
    private SpriteRenderer sp;
    private CircleCollider2D bd;
    private PallerokokonaisuusController pc;
    public GameObject bonusprefab;

    public void setPallerokokonaisuusController(PallerokokonaisuusController p)
    {
        pc = p;
    }

    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        bd = GetComponent<CircleCollider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("alustag"))
        {



            IExplodable o =
col.gameObject.GetComponent<IExplodable>();
            if (o != null)
            {

                o.Explode();
            }
            else
            {
                //		Debug.Log("alus ja explode mutta ei ookkaan " + col.collider.tag);
            }

        }
        else if (col.CompareTag("ammustag"))
        {

            Explode();
            IExplodable o =
col.gameObject.GetComponent<IExplodable>();
            if (o != null)
            {
                o.Explode();
            }
            else
            {
                Debug.Log("alus ja explode mutta ei ookkaan " + col);
            }
        }

    }
    public void Explode()
    {
        bool onkokaikkiammuttu = pc.TarkistaOnkoAmmuttu();

        sp.enabled = false;
        bd.enabled = false;
        bool onkokaikkiammuttunyt = pc.TarkistaOnkoAmmuttu();

        if (!onkokaikkiammuttu && onkokaikkiammuttunyt)
        {
            Vector2 boxsize = new Vector2(sp.size.x, sp.size.y);
            Vector2 pos = transform.position;

            TeeBonus(bonusprefab, pos, boxsize,  1);
        }
        RajaytaSprite(gameObject, 3, 3, 1.0f, 1.0f);
      //  Destroy(gameObject);
    }
}
