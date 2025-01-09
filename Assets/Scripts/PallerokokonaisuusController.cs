using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PallerokokonaisuusController : BaseController
{

    public GameObject prefab; // Assign a prefab in the Inspector
    public int numberOfFollowers = 5; // Total GameObjects including the leader
    public float moveSpeed = 1f; // Speed of the leader

    private float elapsedTime = 0f; // Tracks time for movement phases
    public enum MovementPhase { MoveLeft, MoveRight, MoveDown, MoveUp, Stop, MoveLeftUp,MoveLeftDown, Sin }
    private MovementPhase currentPhase;
    private Vector3 leaderDirection = Vector3.zero; // Initial direction of the leader
    private List<GameObject> followers = new List<GameObject>();
    private Queue<Vector3> positions = new Queue<Vector3>();

    public int positionDelay = 100; // Delay in position units between followers

   // public float xsekuntienmaara = 5.0f;
   // public float ysekuntienmaara = 2.0f;
   // private bool alaspain;
    public float offsetttiminneluodaan = 1.0f;
    private SpriteRenderer sp;
  //  public bool suorakulmainenliikkuminen = true;

    public MovementPhase[] patternMovementPhase;
    private int movennumero = 0;
    public float[] movekesto;
    public float sinfrequency = 2f;
    public float sinamplitude = 0.5f;
    private float rotationTime = 0f;       // Timer to control the rotation
   // public float rotatetimeseconds = 2.0f;//sekkaa

  //  public float nopeusx = -0.01f;
  //  public float nopeusy = 0.01f;
    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        sp.enabled = false;
        currentPhase= MovementPhase.MoveLeft;
    }

    private void Randomize()
    {
        if (movekesto==null || movekesto.Length!= patternMovementPhase.Length)
        {
            for (int i = 0; i < movekesto.Length; i++)
            {
                movekesto[i] = 1;
            }
        }
        float randomNumber = Random.Range(1- randomisointiprossa, 1+ randomisointiprossa);
        //randomisoidaan kestoaika vahan niin 
        for (int i=0;i<movekesto.Length;i++)
        {
            movekesto[i] = movekesto[i] * randomNumber;
        }


        sinfrequency = sinfrequency * randomNumber;
        sinamplitude = sinamplitude * randomNumber;
    }
    public float randomisointiprossa = 0.25f;

    private bool tehty = false;


    private void Tee()
    {
        Vector3 uusi = new Vector3(transform.position.x + offsetttiminneluodaan, transform.position.y, transform.position.z);
        // Instantiate the leader and followers
        for (int i = 0; i < numberOfFollowers; i++)
        {
            GameObject obj = Instantiate(prefab, uusi, Quaternion.identity); // Start on the right
            NmpalleroController aa = obj.GetComponent<NmpalleroController>();
            if (aa!=null)
            {
                aa.setPallerokokonaisuusController(this);
            }
            

            followers.Add(obj);
        }
        //alaspain = IsInUpperPartOfScreen(gameObject);
        IgnoreCollisions(followers);
        Randomize();
        tehty = true; 
    }



    void FixedUpdate()
    {
        if (IsGameObjectVisible())
        {
            if (!tehty)
            {
                Tee();
            }
            MoveLeader();
            UpdateFollowers();
            TarkistaOnkoAmmuttu();
        }
    }

    public void OnBecameInvisible()
    {
       
        foreach (GameObject c in followers)
        {
            if (c != null)
            {
                Destroy(c);
            }
        }
        Destroy(gameObject);
    }
    public bool TarkistaOnkoAmmuttu()
    {
        bool kaikkiammuttu = true;
        foreach (GameObject c in  followers)
        {
  
            if (c.GetComponent<SpriteRenderer>().enabled)
            {
                kaikkiammuttu = false;
                break;
            }
        }
        return kaikkiammuttu;
    }

    private MovementPhase Palautaseuraava()
    {
        return patternMovementPhase[movennumero];
    }

    float CalculatePingPongRotation(float min, float max, float time, float duration)
    {
        float t = Mathf.PingPong(time / duration, 1f);
        return Mathf.Lerp(min, max, t);
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

        if (movennumero>= maksimimove)
        {
            movennumero = 0;
        }
        mk = movekesto[movennumero];

        currentPhase = patternMovementPhase[movennumero];
        GameObject leader = followers[0];

        // Update movement phase based on elapsed time
        switch (currentPhase)
        {
            case MovementPhase.Sin:

                float sinYMovement = Mathf.Sin(rotationTime * sinfrequency) * sinamplitude;


                //    transform.position += new Vector3(-1, delta * ( sinYMovement), 0f);


                //  leaderDirection = Vector3.zero;

                leaderDirection = new Vector3(-1, delta * (sinYMovement), 0f);


                break;
            
            case MovementPhase.MoveLeft:
                leaderDirection = Vector3.left;
                rotationTime = 0;


                break;

            case MovementPhase.MoveRight:
                leaderDirection = Vector3.right;

                rotationTime = 0;

                break;
            case MovementPhase.MoveDown:
                    leaderDirection = Vector3.down; // Stop movement

                rotationTime = 0;

                break;

            case MovementPhase.MoveUp:
                leaderDirection = Vector3.up; // Stop movement
                rotationTime = 0;


                break;
            case MovementPhase.MoveLeftDown:
                leaderDirection = new Vector3(-1, -1, 0);
                rotationTime = 0;

                break;

            case MovementPhase.MoveLeftUp:
                leaderDirection = new Vector3(-1, 1, 0);
                rotationTime = 0;

                break;

            case MovementPhase.Stop:
                return; // No more movement
        }

        // Move the leader and track time

            leader.transform.position += leaderDirection * moveSpeed * Time.deltaTime;
          
        
        elapsedTime += Time.deltaTime;
        // Save the leader's position
        positions.Enqueue(leader.transform.position);

        // Limit the queue size to store only necessary positions

        int maxQueueSize = (numberOfFollowers) * positionDelay;
        while (positions.Count > maxQueueSize)
        {
            positions.Dequeue();

        }
    }



    /*
    private void MoveLeaderSimple()
    {
   
        GameObject leader = followers[0];

        // Update movement phase based on elapsed time
        switch (currentPhase)
        {
            case MovementPhase.MoveLeft:
                if (elapsedTime >= xsekuntienmaara )
                {
                    if (alaspain)
                    {
                        currentPhase = MovementPhase.MoveDown;
                        elapsedTime = 0f;
                        if (suorakulmainenliikkuminen)
                        {
                            leaderDirection = Vector3.down;
                        }
                        else
                        {
                            leaderDirection = new Vector3(-1, -1, 0);
                        }
                    }
                    else
                    {
                        currentPhase = MovementPhase.MoveUp;
                        elapsedTime = 0f;
                        if (suorakulmainenliikkuminen)
                        {
                            leaderDirection = Vector3.up;
                        }
                        else
                        {
                            leaderDirection = new Vector3(-1, 1, 0);
                        }
                    }
                }
                break;

            case MovementPhase.MoveDown:
                if (elapsedTime >= ysekuntienmaara )
                {
                    currentPhase = MovementPhase.MoveLeft;
                    elapsedTime = 0f;
                    
                        leaderDirection = Vector3.left; // Stop movement
                    
                    
                }
                break;

            case MovementPhase.MoveUp:
                if (elapsedTime >= ysekuntienmaara)
                {
                    currentPhase = MovementPhase.MoveLeft;
                    elapsedTime = 0f;
                    
                        leaderDirection = Vector3.left; // Stop movement
                   
                }
            break;


            case MovementPhase.Stop:
                return; // No more movement
        }

        // Move the leader and track time
        leader.transform.position += leaderDirection * moveSpeed * Time.deltaTime;
        elapsedTime += Time.deltaTime;

        // Save the leader's position
          positions.Enqueue(leader.transform.position);

        // Limit the queue size to store only necessary positions

        int maxQueueSize = (numberOfFollowers) * positionDelay;
        while (positions.Count > maxQueueSize)
        { 
            positions.Dequeue();

        }
    }
    */

                    private void UpdateFollowers()
    {
        // Update each follower's position based on the queue
        for (int i = 1; i < numberOfFollowers; i++)
        {
            int index = positionDelay * i;

            if (positions.Count > index)
            {
             
                //Vector3 targetPosition = positions.ToArray()[positions.Count - 1 - index];
                Vector3 targetPosition = positions.ToArray()[ index];
                if (followers[i]!=null)
                {
                    followers[i].transform.position = targetPosition;
                }
                
            }
        }
    }

    public bool IsInUpperPartOfScreen(GameObject target)
    {
        // Convert the GameObject's world position to screen space
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);

        // Get the screen height
        float screenHeight = Screen.height;

        // Return true if the object is in the upper half of the screen
        return screenPosition.y > screenHeight / 2;
    }
}
