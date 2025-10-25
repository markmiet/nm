using UnityEngine;
using UnityEngine.U2D.IK;

public class SkeletonAiming2DIK : BaseController
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

    public void Start()
    {
        player = PalautaAlus().transform;
    }



    private bool CanEnemySeePlayer()
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
        Debug.DrawRay(eye.position, player.position,Color.red);
        //Debug.DrawRay(eye.position, toPlayer, Color.red);
        foreach (RaycastHit2D h in hit) {
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
        if (player == null || aimSolver == null || walkSolver == null) return;
        canSeePlayer = CanEnemySeePlayer();
        // Smoothly blend weight based on visibility
        float targetWeight = canSeePlayer ? 1f : 0f;
        aimWeight = Mathf.MoveTowards(aimWeight, targetWeight, Time.deltaTime * blendSpeed);

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
                aimPos = player.position;
            else
            {
                //
                //                aimPos = transform.position + transform.right * 0.5f + Vector3.up * 0.3f; // neutral pose
                aimPos = transform.position;// neutral pose


            }


            // Clamp how far the target can move
            float maxDist = 0.5f;
            Vector3 dir = aimPos - transform.position;
            if (dir.magnitude > maxDist)
                aimPos = transform.position + dir.normalized * maxDist;

            aimTarget.position = Vector3.Lerp(aimTarget.position, aimPos, Time.deltaTime * 5f);
        }
    }


}
