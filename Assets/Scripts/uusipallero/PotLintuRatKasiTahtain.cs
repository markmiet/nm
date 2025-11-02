using UnityEngine;
using UnityEngine.U2D.IK;

public class PotLintuRatKasiTahtain : BaseController
{
    [Header("IK Solvers")]
    [SerializeField] private LimbSolver2D walkSolver;
    [SerializeField] private LimbSolver2D aimSolver;

    [Header("Target Settings")]
    public Transform player;
    [SerializeField] private Transform walkTarget;
    [SerializeField] private Transform aimTarget;

    [Header("Blend Settings")]
    [Range(0f, 1f)] public float aimWeight = 0f; // 0 = walk, 1 = aim
    [SerializeField] private float blendSpeed = 2f;

    [Header("Detection")]
    public bool canSeePlayer = false;
    public Transform backofhead;
    public Transform eye;
    public float fieldofvisioninDegrees = 90;

    public float maxDist = 0.5f;//tämä vaikuttaa vain sen targetin kulkemisnopeuteen


    public float maxDistanceOfAimTargetFromTransformPosition = 1.0f;

    private float aimsolverinalkuweight = 1.0f;
    public void Start()
    {
        player = PalautaAlus().transform;
        aimsolverinalkuweight = aimSolver.weight;
    }


    public float stableDuration = 1f;

    private bool lastSeenState = false;
    private float stateChangeTime = -999f; // milloin viimeksi tila vaihtui

    public bool CanEnemySeePlayer()
    {
        bool immediateResult = ComputeImmediateVision();

        // Jos tila on eri kuin viimeksi nähty tila
        if (immediateResult != lastSeenState)
        {
            // Onko kulunut riittävästi aikaa viime vaihdosta?
            if (Time.time - stateChangeTime >= stableDuration)
            {
                // Hyväksytään muutos ja päivitetään tila
                lastSeenState = immediateResult;
                stateChangeTime = Time.time;
            }
            // Jos ei ole kulunut tarpeeksi aikaa, palautetaan edellinen tila
        }
        else
        {
            // Päivitä viime vaihtohetki, jotta viive alkaa alusta vasta kun muutos havaitaan
            stateChangeTime = Time.time;
        }

        return lastSeenState;
    }

    /// <summary>
    /// Laskee heti tämän framen perusteella, näkeekö vihollinen pelaajan.
    /// </summary>
    private bool ComputeImmediateVision()
    {
        // Suunta vihollisen silmistä pelaajaan
        Vector2 directionToPlayer = player.position - eye.position;

        // Vihollisen "eteenpäin" -suunta (oletus: vastakkainen kuin takaraivon suunta)
        Vector2 forwardDir = (eye.position - backofhead.position).normalized;

        // Onko pelaaja näkökentän sisällä
        float angle = Vector2.Angle(forwardDir, directionToPlayer);
        if (angle > fieldofvisioninDegrees * 0.5f)
            return false;

        // Tarkista esteet (raycast)
        float distanceToPlayer = directionToPlayer.magnitude;
        RaycastHit2D[] hit = Physics2D.RaycastAll(eye.position, directionToPlayer.normalized, distanceToPlayer);

        Debug.DrawRay(eye.position, directionToPlayer, Color.red);

        foreach (RaycastHit2D h in hit)
        {
            if (h.collider.tag.Contains("tiili"))
                return false; // Näköeste
        }

        return true; // Pelaaja näkyy
    }



    public bool CanEnemySeePlayerEicahc()
    {
        // Direction from enemy's eyes to the player
        Vector2 directionToPlayer = player.position - eye.position;

        // Forward direction (assuming the enemy’s forward is the opposite of backofhead direction)
        Vector2 forwardDir = (eye.position - backofhead.position).normalized;

        // Check if the player is within the field of view angle
        float angle = Vector2.Angle(forwardDir, directionToPlayer);
        if (angle > fieldofvisioninDegrees * 0.5f)
            return false; // player is outside vision cone

        // Check line of sight (no obstacles blocking view)
        float distanceToPlayer = directionToPlayer.magnitude;
        RaycastHit2D[] hit = Physics2D.RaycastAll(eye.position, directionToPlayer.normalized, distanceToPlayer);
        Debug.DrawRay(eye.position, player.position, Color.red);
        //Debug.DrawRay(eye.position, toPlayer, Color.red);
        foreach (RaycastHit2D h in hit)
        {
            if (h.collider.tag.Contains("tiili"))
            {
                // Something is blocking the view
                return false;
            }
        }



        // Player is within FOV and not blocked
        return true;
    }


    void Update()
    {
       // if (player == null || aimSolver == null || walkSolver == null) return;

        if (player == null || aimSolver == null) return;



        canSeePlayer = CanEnemySeePlayer();
        // Smoothly blend weight based on visibility
        float targetWeight = canSeePlayer ? aimsolverinalkuweight : 0f;
        aimWeight = Mathf.MoveTowards(aimWeight, targetWeight, Time.deltaTime * blendSpeed);
        if (walkSolver!=null)
            walkSolver.weight = 1f - aimWeight;
        aimSolver.weight = aimWeight;

        // Walk target (optional swing)
        /*
        if (walkTarget != null)
        {
            Vector3 walkPos = transform.position + transform.right * 0.5f + Vector3.up * 0.2f;
            //walkTarget.position = Vector3.Lerp(walkTarget.position, walkPos, Time.deltaTime * 4f);
        }
        */

        // Aim target logic
        if (aimTarget != null)
        {
            Vector3 aimPos;

            if (canSeePlayer)
            {
                aimPos = player.position;


                Vector3 dirrri = aimPos - transform.parent.transform.position;
                float dist = dirrri.magnitude;

                // Clamp how far the target can move
                if (dist > maxDistanceOfAimTargetFromTransformPosition)
                {
                    dirrri = dirrri.normalized * maxDistanceOfAimTargetFromTransformPosition;

                    // Final, clamped target position
                    aimPos = transform.parent.transform.position + dirrri;
                }



            }

            else
            {
                //
                //                aimPos = transform.position + transform.right * 0.5f + Vector3.up * 0.3f; // neutral pose
                aimPos = transform.position;// neutral pose


            }
            //@todo go from transform position towards aimPos max of   public float maxDistanceOfAimTargetFromTransformPosition = 1.0f;


            // Clamp how far the target can move

            Vector3 dir = aimPos - transform.position;
            if (dir.magnitude > maxDist)
                aimPos = transform.position + dir.normalized * maxDist;

            aimTarget.position = Vector3.Lerp(aimTarget.position, aimPos, Time.deltaTime * 5f);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (aimTarget == null) return;

        // Choose color based on detection (optional)
        Gizmos.color = canSeePlayer ? Color.red : Color.yellow;

        // Draw a small square (2D box) at the aim target position
        float boxSize = 0.1f;
        Vector3 pos = aimTarget.position;

        // Draw a wire square in 2D (on XY plane)
        Gizmos.DrawLine(pos + new Vector3(-boxSize, -boxSize, 0), pos + new Vector3(boxSize, -boxSize, 0));
        Gizmos.DrawLine(pos + new Vector3(boxSize, -boxSize, 0), pos + new Vector3(boxSize, boxSize, 0));
        Gizmos.DrawLine(pos + new Vector3(boxSize, boxSize, 0), pos + new Vector3(-boxSize, boxSize, 0));
        Gizmos.DrawLine(pos + new Vector3(-boxSize, boxSize, 0), pos + new Vector3(-boxSize, -boxSize, 0));

        // Optional: draw line from this object to the aim target
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, aimTarget.position);
    }
#endif


}
