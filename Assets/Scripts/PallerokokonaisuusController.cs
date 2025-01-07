using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PallerokokonaisuusController : BaseController
{

    public GameObject prefab; // Assign a prefab in the Inspector
    public int numberOfFollowers = 5; // Total GameObjects including the leader
    public float moveSpeed = 1f; // Speed of the leader

    private float elapsedTime = 0f; // Tracks time for movement phases
    private enum MovementPhase { MoveLeft, MoveDown,MoveUp, Stop }
    private MovementPhase currentPhase = MovementPhase.MoveLeft;
    private Vector3 leaderDirection = Vector3.left; // Initial direction of the leader
    private List<GameObject> followers = new List<GameObject>();
    private Queue<Vector3> positions = new Queue<Vector3>();

    public int positionDelay = 100; // Delay in position units between followers

    public float xsekuntienmaara = 5.0f;
    public float ysekuntienmaara = 2.0f;
    private bool alaspain;
    public float offsetttiminneluodaan = 1.0f;
    private SpriteRenderer sp;
    public bool suorakulmainenliikkuminen = true;
    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        sp.enabled = false;
    }

    private bool tehty = false;

    private void Tee()
    {
        Vector3 uusi = new Vector3(transform.position.x + offsetttiminneluodaan, transform.position.y, transform.position.z);
        // Instantiate the leader and followers
        for (int i = 0; i < numberOfFollowers; i++)
        {
            GameObject obj = Instantiate(prefab, uusi, Quaternion.identity); // Start on the right
            obj.GetComponent<NmpalleroController>().setPallerokokonaisuusController(this);

            followers.Add(obj);
        }
        alaspain = IsInUpperPartOfScreen(gameObject);
        IgnoreCollisions(followers);
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
        Destroy(gameObject);
        foreach (GameObject c in followers)
        {
            Destroy(c);
        }
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

 
    private void MoveLeader()
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
        /*
        if (maara>= viivepositioissa * numberOfFollowers - 1)
        {
            base.TallennaSijaintiSailytaVainNkplViimeisinta(viivepositioissa * numberOfFollowers - 1,
                false, false, leader);

            for (int i = 1; i < numberOfFollowers; i++)
            {
                Vector3 pal = base.palautaScreenpositioneissa((i - 1) * viivepositioissa);
                if (pal != Vector3.zero)
                {
                    followers[i].transform.position = pal;
                }

            }
        }
        */


        // Save the leader's position
          positions.Enqueue(leader.transform.position);

        // Limit the queue size to store only necessary positions

        int maxQueueSize = (numberOfFollowers) * positionDelay;
        while (positions.Count > maxQueueSize)
        { 
            positions.Dequeue();
           // int koko = positions.Count;
           // Debug.Log("koko="+koko);
        }
    }

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

                followers[i].transform.position = targetPosition;
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
