using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NmpalleroController : BaseController, IExplodable
{
    // Start is called before the first frame update
    private SpriteRenderer sp;
    private Collider2D[] bd;
    private PallerokokonaisuusController pc;
    public GameObject bonusprefab;
    public void setPallerokokonaisuusController(PallerokokonaisuusController p)
    {
        pc = p;
    }

    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        bd = GetComponents<Collider2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
        //TuhoaMuttaAlaTuhoaJosOllaanEditorissa(gameObject);
        if (IsGoingToBeDestroyed())
        {
            return;
        }

        TuhoaJosOllaanSiirrettyJonkunVerranKameranVasemmallePuolenSalliPieniAlitusJaYlitys(gameObject);
        

        MoveLeader();
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        /*
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
        else if (col.CompareTag("ammustag") || col.CompareTag("forcefield"))
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
               // Debug.Log("alus ja explode mutta ei ookkaan " + col);
            }
            
        }
        */

    }

    public float explosionforce = 0.5f;
    public float explosiontime = 0.5f;
    public float gravityscale = 2.0f;
    public void Explode()
    {
        //tämä on siis ok
        if (pc==null)
        {
           // Destroy(gameObject);
            BaseDestroy();
            return;
        }
        //(gameObject, 3, 3, 1.0f, 0.5f);
        //        RajaytaSpriteUusiMonimutkaisin(gameObject, uusirajaytyscolumns, uusirajaytysrows, explosionforce, explosiontime);//tämä tekee jo basedestroyn

        RajaytaSpriteUusiMonimutkaisin(gameObject, uusirajaytyscolumns, uusirajaytysrows, explosionforce, explosiontime, null,
            explosiontime, null, 36, false, gravityscale
            ) ;//tämä tekee jo basedestroyn
        /*
        GameObject go,
    int rows,
    int columns,
    float explosionForce,
    float aliveTime,
    GameObject fadeexplosion = null,
    float fadeDuration = 0.5f,
    GameObject gameObjectjostalasketaanPosition = null,
    int maksimimaara = 36, bool teerigidbody = true
            */
        //BaseDestroy(gameObject);
        bool onkokaikkiammuttu = pc.TarkistaOnkoAmmuttu();

        /*
       // sp.enabled = false;
        ParticleSystem[] pa =
        GetComponentsInChildren<ParticleSystem>();
        if (pa!=null)
        {
            foreach(ParticleSystem p in pa)
            {
                Destroy(p);  
            }
        }


        foreach(Collider2D c in bd)
        {
            c.enabled = false;
        }
       
        bool onkokaikkiammuttunyt = pc.TarkistaOnkoAmmuttu();
        */
        if (onkokaikkiammuttu/* &&onkokaikkiammuttunyt */)
        {
            //Vector2 boxsize = new Vector2(sp.size.x, sp.size.y);
            Vector2 pos = transform.position;
            if (pc.bonusPrefabit!=null && pc.bonusPrefabit.Length!=0)
            {
                foreach (GameObject g in pc.bonusPrefabit)
                {
                    TeeBonusReal(g, pos,1, true);
                }
            }
            else if (bonusprefab!=null)
            {

                TeeBonusReal(bonusprefab, pos, 1,false);
            }
            
        }
   
     //   Destroy(gameObject);
    }



    public float moveSpeed = 1f; // Speed of the leader

    private float elapsedTime = 0f; // Tracks time for movement phases
    public enum MovementPhase { MoveLeft, MoveRight, MoveDown, MoveUp, Stop, MoveLeftUp, MoveLeftDown, Sin }

    private PallerokokonaisuusController.MovementPhase currentPhase;

    public PallerokokonaisuusController.MovementPhase[] patternMovementPhase;
    private int movennumero = 0;
    public float[] movekesto;
    public float sinfrequency = 2f;
    public float sinamplitude = 0.5f;
    private float rotationTime = 0f;       // Timer to control the rotation
                                           //  public LayerMask collisionLayer;
                                           // public float rotatetimeseconds = 2.0f;//sekkaa


    public float randomisointiprossa = 0.25f;

    private Vector3 leaderDirection = Vector3.zero; // Initial direction of the leader

    private void Randomize()
    {
        if (movekesto == null || movekesto.Length != patternMovementPhase.Length)
        {
            for (int i = 0; i < movekesto.Length; i++)
            {
                movekesto[i] = 1;
            }
        }
        float randomNumber = Random.Range(1 - randomisointiprossa, 1 + randomisointiprossa);
        //randomisoidaan kestoaika vahan niin 
        for (int i = 0; i < movekesto.Length; i++)
        {
            movekesto[i] = movekesto[i] * randomNumber;
        }


        sinfrequency = sinfrequency * randomNumber;
        sinamplitude = sinamplitude * randomNumber;
    }


    private void MoveLeader()
    {
        float delta = Time.deltaTime;
        rotationTime += delta;
        int maksimimove = movekesto.Length;
        float mk = movekesto[movennumero];
        if (elapsedTime >= mk)
        {
            elapsedTime = 0f;
            movennumero++;
        }

        if (movennumero >= maksimimove)
        {
            movennumero = 0;
        }
        mk = movekesto[movennumero];

        currentPhase = patternMovementPhase[movennumero];


        GameObject leader = gameObject;

        if (leader == null)
        {
            return;
        }
        // Update movement phase based on elapsed time
        switch (currentPhase)
        {
            case PallerokokonaisuusController.MovementPhase.Sin:

                float sinYMovement = Mathf.Sin(rotationTime * sinfrequency) * sinamplitude;


                //    transform.position += new Vector3(-1, delta * ( sinYMovement), 0f);


                //  leaderDirection = Vector3.zero;

                leaderDirection = new Vector3(-1, delta * (sinYMovement), 0f);


                break;

            case PallerokokonaisuusController.MovementPhase.MoveLeft:
                leaderDirection = Vector3.left;
                rotationTime = 0;


                break;

            case PallerokokonaisuusController.MovementPhase.MoveRight:
                leaderDirection = Vector3.right;

                rotationTime = 0;

                break;
            case PallerokokonaisuusController.MovementPhase.MoveDown:
                leaderDirection = Vector3.down; // Stop movement

                rotationTime = 0;

                break;

            case PallerokokonaisuusController.MovementPhase.MoveUp:
                leaderDirection = Vector3.up; // Stop movement
                rotationTime = 0;


                break;
            case PallerokokonaisuusController.MovementPhase.MoveLeftDown:
                leaderDirection = new Vector3(-1, -1, 0);
                rotationTime = 0;

                break;

            case PallerokokonaisuusController.MovementPhase.MoveLeftUp:
                leaderDirection = new Vector3(-1, 1, 0);
                rotationTime = 0;

                break;

            case PallerokokonaisuusController.MovementPhase.Stop:
                return; // No more movement
        }

        // Move the leader and track time

        leader.transform.position += leaderDirection * moveSpeed * Time.deltaTime;
        // Vaista(delta, collisionLayer);

        elapsedTime += Time.deltaTime;
        // Save the leader's position
    }



}
