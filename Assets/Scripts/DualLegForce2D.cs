using UnityEngine;
using UnityEngine.U2D.IK;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class DualLegForce2D : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Rigidbody2D rb;

    [Header("Ground Check")]
    public Transform leftFootPoint;
    public Transform rightFootPoint;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundMask;

    [Header("Force Settings")]
    [Tooltip("Base force applied per foot push.")]
    public float footForce = 15f;
    [Tooltip("Damping to reduce sliding.")]
    public float drag = 3f;

    [Header("Slope Settings")]
    [Tooltip("Maximum walkable slope angle in degrees.")]
    public float maxSlopeAngle = 40f;
    [Tooltip("Strength of automatic sliding when on steep slopes.")]
    public float slideForce = 5f;

    public float pushThreshold = 0.01f;
    public ForceMode2D forcemode = ForceMode2D.Force;

    private bool leftGrounded;
    private bool rightGrounded;

    private Vector2 leftSlopeDir = Vector2.right;
    private Vector2 rightSlopeDir = Vector2.right;
    private float leftSlopeAngle;
    private float rightSlopeAngle;




    [Header("Foot Locking")]
    public bool enableFootLock = true;
    public float footLockLerpSpeed = 8f;


    [Header("Animation Sync")]
    public bool syncAnimationSpeed = true;
    public float animationSpeedMultiplier = 0.15f;

    public Transform ok1Transform;


    public Transform vj1;
    public Transform vjalkatera;


    public Transform oj1;
    public Transform ojalkatera;


    public LimbSolver2D vj1LimbSolver2D;
    public LimbSolver2D vjalkateraLimbSolver2D;


    public LimbSolver2D oj1LimbSolver2D;
    public LimbSolver2D ojalkateraLimbSolver2D;

    public float footlockweight = 0.8f;

    public bool disableLegLimbSolvers = true;
    public bool disablejalkateraLegLimbSolvers = true;
    private Vector2 originalScale;

    public void Start()
    {
        originalScale = transform.localScale;
        
      //  Oikealle(nykysuuntaoikea);
    }

    private void Reset()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        float direction = transform.localScale.x >= 0 ? 1f : -1f;
        if (footForce == 0.0f)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            return;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
        /*
        if (rb.velocity.x < 0)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        */
        if (direction>=0)
        {
            if (rb.velocity.x < 0)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        else
        {
            if (rb.velocity.x > 0)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }


        float leftPush = animator.GetFloat("LeftFootPush");
        float rightPush = animator.GetFloat("RightFootPush");

        // === Ground check + slope detection ===
        RaycastHit2D leftHit = Physics2D.Raycast(leftFootPoint.position, Vector2.down, groundCheckDistance, groundMask);
        RaycastHit2D rightHit = Physics2D.Raycast(rightFootPoint.position, Vector2.down, groundCheckDistance, groundMask);

        leftGrounded = leftHit.collider != null;
        rightGrounded = rightHit.collider != null;

        if (!leftGrounded && !rightGrounded)
        {
            //pitäis tutkia mikä animaatio menossa, jos kävely niin vain sillin tämä:
            if (rb.velocity.y < -0.1f)
            {
                animator.speed = 0.1f;
            }
        }

        leftSlopeDir = leftGrounded ? new Vector2(leftHit.normal.y, -leftHit.normal.x).normalized : Vector2.right;
        rightSlopeDir = rightGrounded ? new Vector2(rightHit.normal.y, -rightHit.normal.x).normalized : Vector2.right;

        leftSlopeAngle = leftGrounded ? Vector2.Angle(leftHit.normal, Vector2.up) : 0f;
        rightSlopeAngle = rightGrounded ? Vector2.Angle(rightHit.normal, Vector2.up) : 0f;

        // Facing direction
        /*
        if (nykysuuntaoikea)
        {
            direction = 1.0f;
        }
        else
        {
            direction = -1.0f;
        }
        */
        
        /*
        if (transform.localScale.x >= 0)
        {
            ok1Transform.localScale = new Vector2(1, 1);
        }
        else
        {
            ok1Transform.localScale = new Vector2(-1, -1);
        }
        */
        // === Apply slope-aware movement ===
        ApplyFootForce(leftGrounded, leftPush, leftSlopeDir, leftSlopeAngle, leftFootPoint, direction, Color.red);
        ApplyFootForce(rightGrounded, rightPush, rightSlopeDir, rightSlopeAngle, rightFootPoint, direction, Color.blue);

        // === Apply slide on steep slopes ===
        HandleSlopeSlide(leftGrounded, leftSlopeAngle, leftSlopeDir);
        HandleSlopeSlide(rightGrounded, rightSlopeAngle, rightSlopeDir);

        // === Apply drag ===
        rb.velocity = new Vector2(rb.velocity.x * (1f - Time.fixedDeltaTime * drag), rb.velocity.y);

        // ===  locking ===
        //enableFootLock=false toimii paremmin
        if (enableFootLock)
        {
            //HandleFootLocking(leftFootPoint, ref leftLocked, ref leftLockPosition, leftGrounded, leftPush);
            //HandleFootLocking(rightFootPoint, ref rightLocked, ref rightLockPosition, rightGrounded, rightPush);

            // HandleFootLocking(vj1, ref leftLocked, ref leftLockPosition, leftGrounded, leftPush);
            //  HandleFootLocking(vjalkatera, ref leftLocked, ref leftLockPosition2, leftGrounded, leftPush);

            //            public LimbSolver2D vj1LimbSolver2D;
            //  public LimbSolver2D vjalkateraLimbSolver2D;

            //HandleFootLocking(ojalkatera, ref leftLocked, ref rightLockPosition2, rightGrounded, rightPush);


            //HandleFootLocking(rightFootPoint, ref rightLocked, ref rightLockPosition, rightGrounded, rightPush);

            if (leftGrounded)
            {
                vj1LimbSolver2D.weight = footlockweight;
                vjalkateraLimbSolver2D.weight = footlockweight;
                if (disableLegLimbSolvers)
                {
                    vj1LimbSolver2D.enabled = false;
                }


                // public bool disableLegLimbSolvers = true;
                // public bool disablejalkateraLegLimbSolvers = true;

                if (disablejalkateraLegLimbSolvers)
                {
                    vjalkateraLimbSolver2D.enabled = false;
                }


            }
            else
            {
                vj1LimbSolver2D.weight = 1.0f;
                vjalkateraLimbSolver2D.weight = 1.0f;
                vj1LimbSolver2D.enabled = true;
                vjalkateraLimbSolver2D.enabled = true;

                //@todoo parametrisoi noiden disablointi/enablointi muuttujalla

            }
            if (rightGrounded)
            {
                oj1LimbSolver2D.weight = footlockweight;
                ojalkateraLimbSolver2D.weight = footlockweight;
                if (disableLegLimbSolvers)
                {
                    oj1LimbSolver2D.enabled = false;
                }
                if (disablejalkateraLegLimbSolvers)
                {
                    ojalkateraLimbSolver2D.enabled = false;
                }
            }
            else
            {

                oj1LimbSolver2D.weight = 1.0f;
                ojalkateraLimbSolver2D.weight = 1.0f;
                oj1LimbSolver2D.enabled = true;

                ojalkateraLimbSolver2D.enabled = true;

            }

        }

        // === Animation speed sync ===
        if (syncAnimationSpeed)
        {
            float horizontalSpeed = Mathf.Abs(rb.velocity.x);
            animator.speed = Mathf.Clamp(horizontalSpeed * animationSpeedMultiplier, 0.1f, 3f);
        }



        // === Animator state info ===
        //    animator.SetBool("LeftGrounded", leftGrounded);
        //      animator.SetBool("RightGrounded", rightGrounded);
    }

    private void ApplyFootForce(bool grounded, float push, Vector2 slopeDir, float slopeAngle, Transform foot, float facing, Color debugColor)
    {
        if (!grounded || push <= pushThreshold)
            return;

        // Only walk if slope is within walkable range
        if (slopeAngle <= maxSlopeAngle)
        {
            Vector2 force = slopeDir * push * footForce * facing;
            rb.AddForce(force, forcemode);
            Debug.DrawRay(foot.position, force.normalized * 0.3f, debugColor);
        }
    }

    private void HandleSlopeSlide(bool grounded, float slopeAngle, Vector2 slopeDir)
    {
        if (!grounded || slopeAngle <= maxSlopeAngle)
            return;

        // Slide down slope
        Vector2 slideDirection = -slopeDir;
        rb.AddForce(slideDirection * slideForce, ForceMode2D.Force);
    }



    // === DEBUG GIZMOS ===
    private void OnDrawGizmosSelected()
    {
        DrawFootRayGizmo(leftFootPoint, leftGrounded, leftSlopeAngle);
        DrawFootRayGizmo(rightFootPoint, rightGrounded, rightSlopeAngle);
    }

    private void DrawFootRayGizmo(Transform foot, bool grounded, float slopeAngle)
    {
        if (!foot) return;

        // Raycast line
        Gizmos.color = grounded ? Color.white : new Color(1f, 1f, 1f, 0.3f);
        Gizmos.DrawLine(foot.position, foot.position + Vector3.down * groundCheckDistance);

        // Visual slope angle indicator (arc + text)
#if UNITY_EDITOR
        if (grounded)
        {
            // Base color changes if too steep
            Color slopeColor = slopeAngle > maxSlopeAngle ? Color.red : Color.green;
            UnityEditor.Handles.color = slopeColor;

            // Draw small arc showing slope angle
            Vector3 footPos = foot.position;
            UnityEditor.Handles.DrawSolidArc(
                footPos + Vector3.up * 0.05f,
                Vector3.forward,
                Vector3.up,
                -slopeAngle,
                0.2f
            );

            // Draw angle text
            UnityEditor.Handles.Label(footPos + Vector3.up * 0.15f, $"{slopeAngle:0.0}°");
        }
#endif
    }

    public bool nykysuuntaoikea = true;
    private void Oikealle(bool oikea)
    {
        /*
        if (!nykysuuntaoikea && oikea)
        {

            transform.localScale = new Vector2(Mathf.Abs(originalScale.x), originalScale.y);
        }
        else if (nykysuuntaoikea && !oikea)
        {
            transform.localScale = new Vector2(-Mathf.Abs(originalScale.x), originalScale.y);
        }
        */

        if (oikea)
        {

            transform.localScale = new Vector2(Mathf.Abs(originalScale.x), originalScale.y);
        }
        else 
        {
            transform.localScale = new Vector2(-Mathf.Abs(originalScale.x), originalScale.y);
        }
        nykysuuntaoikea = oikea;
    }

    public void LateUpdate()
    {
        //transform.localScale = new Vector2(-0.75f, 0.75f);
//        Oikealle(nykysuuntaoikea);

        Jalkalukko();
        //   transform.localScale = new Vector2(0.75f,- 0.75f);

        /*
            public Transform vj1;
    public Transform vjalkatera;


    public Transform oj1;
    public Transform ojalkatera;
    */

        //leftGrounded = leftHit.collider != null;
        //  rightGrounded = rightHit.collider != null;
        /*
        if (!leftGroundedprevious && leftGrounded)
        {
            oj1lock = oj1jalka;
        }
        if (leftGroundedprevious && leftGrounded)
        {
             oj1jalka = oj1lock;
        }

        if (!rightGroundedprevious && rightGrounded)
        {
            vj1lock = vj1jalka;
        }
        if (rightGroundedprevious && rightGrounded)
        {
            vj1jalka = vj1lock;
        }

        */

    }
    private bool leftGroundedprevious;
    private bool rightGroundedprevious;


    private Vector3 leftLockPos;
    private Vector3 rightLockPos;
    private bool leftLocked;
    private bool rightLocked;

    private void Jalkalukko()
    {
        if (vj1 == null || vjalkatera == null || oj1 == null || ojalkatera == null)
        {
            return;
        }
        // LEFT FOOT
        if (!leftGroundedprevious && leftGrounded)
        {
            // just landed — capture lock position
            leftLockPos = vj1.position;
            leftLocked = true;
        }
        else if (!leftGrounded)
        {
            leftLocked = false;
        }

        // RIGHT FOOT
        if (!rightGroundedprevious && rightGrounded)
        {
            // just landed — capture lock position
            rightLockPos = oj1.position;
            rightLocked = true;
        }
        else if (!rightGrounded)
        {
            rightLocked = false;
        }

        // Apply the locks
        if (leftLocked)
        {
            vj1.position = Vector3.Lerp(vj1.position, leftLockPos, Time.deltaTime * footLockLerpSpeed);
            vjalkatera.position = Vector3.Lerp(vjalkatera.position, leftLockPos, Time.deltaTime * footLockLerpSpeed);
        }

        if (rightLocked)
        {
            oj1.position = Vector3.Lerp(oj1.position, rightLockPos, Time.deltaTime * footLockLerpSpeed);
            ojalkatera.position = Vector3.Lerp(ojalkatera.position, rightLockPos, Time.deltaTime * footLockLerpSpeed);
        }

        // Store previous grounded states for next frame
        leftGroundedprevious = leftGrounded;
        rightGroundedprevious = rightGrounded;
    }

}
