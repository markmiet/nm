using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonKoiraController : MonoBehaviour
{
    //[SerializeField]
    //   public bool hyppaa = false;
    //   public bool laskeudu = false;

    // Start is called before the first frame update
    private Animator animator;
    private Rigidbody2D rb;
    public GameObject torso;


    private float startingZRotation;
    public float rotationLimit = 10;    // 10% of the starting rotation


    // public Transform leftFoot,rightFoot; // Assign foot GameObject (e.g., LeftFoot, RightFoot)
    // public float footOffset = 0.1f;
    // public float raycastDistance = 1f;
    // public LayerMask groundLayer;
    //public float lerpSpeed = 5f; // Smooth movement
    void Start()

    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        startingZRotation = transform.rotation.eulerAngles.z;
        GetAnimationStates();
    }


    public float forceAmount = 5f;       // Total force to apply

    public float maxforce = 55.0f;

    private float forceAnnettu = 0.0f;

    public float laskeutumisviive = 0.1f;
    private float laskeutumisekestolaskuri = 0.0f;

    void Update2()
    {

        //if (hyppaa)
        //{
        //animator.SetBool("hyppaa", true);
        //ollaankohyppaamassa = true;
        // rigidbody2D.AddForce(new Vector2(0, 10));

        //rb.velocity = new Vector2(rb.velocity.x, 3);

        // elapsedTime = 0.0f;

        //elapsedTime += Time.deltaTime;
        //}
        /*
        if (hyppaa &&  elapsedTime > hypynloppu)
        {
            elapsedTime = 0.0f;
           // ollaankohyppaamassa = false;
            hyppaa = false;
            hyppyvoimaannettu = false;


        }
        */
        /*
            if (hyppaa && elapsedTime>=hypynalku && elapsedTime<=hypynloppu)
        {
            float deltaForce = (forceAmount / (hypynloppu- hypynalku)) * Time.deltaTime;

            // Apply force
            rb.AddForce(Vector2.up * deltaForce);

            // Track how much force has been applied and time elapsed
            forceApplied += deltaForce;

            //float remainingForce = forceAmount - forceApplied;
            //if (remainingForce > 0)
            //{
              //  rb.AddForce(Vector2.up * remainingForce);
            //}

        }
            */

        //if (hyppaa && !hyppyvoimaannettu && elapsedTime<=hypynalku)
        //{
        //    rb.AddForce(Vector2.up * forceAmount);
        //    hyppyvoimaannettu = true;
        //}

        bool maassa = IsMaassa();
        if (OnkoHyppaamassaLoppuvaiheissa() && rb.velocity.y < 0)
        {
            //   Debug.Log("The Rigidbody2D is moving downward.");
            //hyppaa = false;
            //laskeudu = true;
            //   animator.SetBool("hyppaa", false);
            //   animator.SetBool("laskeudu", true);

            //   laskeutumisekestolaskuri += Time.deltaTime;
            // Debug.Log("laskeutuminen aloitetaan");
        }
        if (OnkoHyppaamassaLoppuvaiheissa() && rb.velocity.y < 0 && laskeutumisekestolaskuri >= laskeutumisviive)
        {
            Debug.Log("The Rigidbody2D viie kuluunut");
            //hyppaa = false;
            //laskeudu = true;
            //   animator.SetBool("hyppaa", false);
            //   animator.SetBool("laskeudu", true);

        }
        if (maassa)
        {
            // animator.SetBool("laskeudu", false);
            // laskeutumisekestolaskuri = 0.0f;
        }
        animator.SetBool("maassa", maassa);

        //animator.SetBool("hyppaa", hyppaa);
        //animator.SetBool("laskeudu", laskeudu); Raycast

        if (OnkoHyppaamassa() && currentstate == CharacterState.Jumping3)
        {
            // float deltaForce = (forceAmount / (hypynloppu - hypynalku)) * Time.deltaTime;
            float deltaForce = forceAmount * Time.deltaTime;

            if (forceAnnettu < maxforce)
            {
                rb.AddForce(Vector2.up * deltaForce);
                rb.AddForce(Vector2.left * deltaForce);
            }
            forceAnnettu += deltaForce;


        }
        else
        {
            forceAnnettu = 0.0f;

        }



        float currentZRotation = NormalizeAngle(transform.rotation.eulerAngles.z);

        // Calculate clamped rotation range
        float minRotation = startingZRotation - rotationLimit / 2;
        float maxRotation = startingZRotation + rotationLimit / 2;

        // Clamp the Z rotation
        float clampedZRotation = Mathf.Clamp(currentZRotation, minRotation, maxRotation);
        //katotaas torson rotation
        transform.rotation = Quaternion.Euler(
        transform.rotation.eulerAngles.x,
        transform.rotation.eulerAngles.y,
        clampedZRotation
        );
    }

    private bool OnkoHyppaamassa()
    {
        if (currentstate == CharacterState.Jumping1 ||
            currentstate == CharacterState.Jumping2 ||
            currentstate == CharacterState.Jumping3 ||
            currentstate == CharacterState.Jumping4 ||
            currentstate == CharacterState.Jumping5 ||
            currentstate == CharacterState.Jumping6 ||
            currentstate == CharacterState.Jumping7)
        {
            return true;
        }
        return false;
    }
    private bool OnkoHyppaamassaLoppuvaiheissa()
    {
        //currentstate == CharacterState.Jumping5 ||

        if (
            currentstate == CharacterState.Jumping6 ||
            currentstate == CharacterState.Jumping7)
        {
            return true;
        }
        return false;
    }


    public CharacterState currentstate;

    //public bool tuotahyppyvoimaa = false;

    public enum CharacterState
    {
        Konttaus0, //0
        Jumping1, //1
        Jumping2,//2
        Jumping3,//3
        Jumping4,//hyppyvoima 4
        Jumping5,//hyppyvoima5 
        Jumping6, //6
        Jumping7,//7
        Laskeudu8,//9
        Laskeudu9,//10
        Laskeudu10,//11
        Laskeudu11,//12
        Laskeudu12,//13
        Laskeudu13,//14

    }
    private float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180)
            angle -= 360;
        else if (angle < -180)
            angle += 360;

        return angle;
    }
    public bool IsMaassa()
    {

        NilkkaController[] nc =
        GetComponentsInChildren<NilkkaController>();

        if (nc != null)
        {
            foreach (NilkkaController n in nc)
            {
                if (n.IsMaassa())
                {
                    return true;
                }
            }
        }
        return false;

    }


    /*
    private bool maassa = false;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        CircleCollider2D circleCollider = collision.collider.GetComponent<CircleCollider2D>();

        if (circleCollider != null)
        {
            Debug.Log("Collision with a CircleCollider2D!");

            if (collision.collider.tag.Contains("tiili"))
            {
                maassa = true;
            }
            else
            {
                maassa = false;
            }
        }
        else
        {
            Debug.Log("Collision with a non-CircleCollider2D object.");
            maassa = false;
        }
    }
    */

    public bool hyppaa;
    public bool laskeudu;
    public bool nousepuolipystyyn;
    public bool kavelepuolipystyssa;
    public bool konttaa;
    public bool nousepystyyn;
    public bool idlaa;
    public bool maassa;

    //public string[] animationStates; // Assign animation state names in the Inspector

    private List<string> animationStates = new List<string>(); // Stores animation states



    public float transitionDuration = 0.2f; // Transition time in seconds
    public float cooldownTime = 1.0f; // Cooldown before transitioning to the next state

    private float lastTransitionTime = 0f; // Tracks the last transition time


    void Updateaaaaa()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0 is the layer index
        Debug.Log("Current State: " + stateInfo.fullPathHash);
        if (stateInfo.IsName("MyStateName"))
        {
            Debug.Log("The animator is in MyStateName.");
        }
        float progress = stateInfo.normalizedTime % 1; // Ensure it stays within 0 - 1
        Debug.Log("Animation Progress: " + (progress * 100) + "%");

        bool maassa = IsMaassa();
        animator.SetBool("maassa", maassa);
        animator.SetBool("hyppaa", hyppaa);
        animator.SetBool("nousepuolipystyyn", nousepuolipystyyn);
        animator.SetBool("kavelepuolipystyssa", kavelepuolipystyssa);
        animator.SetBool("konttaa", konttaa);
        animator.SetBool("nousepystyyn", nousepystyyn);
        animator.SetBool("idlaa", idlaa);
      //  AdjustFootPosition(leftFoot);
     //   AdjustFootPosition(rightFoot);

    }
    void GetAnimationStates()
    {
        if (animator == null || animator.runtimeAnimatorController == null) return;

        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

        foreach (AnimationClip clip in controller.animationClips)
        {
            if (!animationStates.Contains(clip.name))
            {
                animationStates.Add(clip.name);
            }
        }

        Debug.Log("Loaded Animation States: " + string.Join(", ", animationStates));
    }
    void Update()
    {
        /*
        if (animator != null && animationStates.Count > 0)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // Base layer (index 0)


            // Check if the current animation has finished AND cooldown has passed
            if (stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0) && Time.time >= lastTransitionTime + cooldownTime)
            {
                // Pick a random next state
                string nextState = animationStates[Random.Range(0, animationStates.Count)];

                // Crossfade to the next state smoothly
                animator.CrossFade(nextState, transitionDuration);

                // Update the last transition time
                lastTransitionTime = Time.time;

                Debug.Log("Transitioning to: " + nextState);
            }
        }
        */

        maassa = IsMaassa();

        if (maassa)
        {
            hyppaa = true;
            nousepuolipystyyn = true;
            konttaa = true;
            nousepystyyn = true;
            idlaa = true;
            kavelepuolipystyssa = true;
            laskeudu = false;
        }
        else
        {
            hyppaa = false;
            nousepuolipystyyn = false;
            konttaa = false;
            nousepystyyn = false;
            idlaa = false;
            kavelepuolipystyssa = false;
            laskeudu = true;
        }
        //laskeudu = hyppaa;
        /*
        //animator.SetBool("maassa", maassa);
        animator.SetBool("hyppaa", hyppaa);
        animator.SetBool("nousepuolipystyyn", nousepuolipystyyn);
        animator.SetBool("kavelepuolipystyssa", kavelepuolipystyssa);
        animator.SetBool("konttaa", konttaa);
        animator.SetBool("nousepystyyn", nousepystyyn);
        animator.SetBool("idlaa", idlaa);

        animator.SetBool("laskeudu", laskeudu);
        */

        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0); // Base layer (index 0)

        // Check if animation finished and cooldown has passed
        if (currentStateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0) && Time.time >= lastTransitionTime + cooldownTime)
        {
            string nextState = GetNextValidState();

            if (!string.IsNullOrEmpty(nextState))
            {
                animator.CrossFade(nextState, transitionDuration);
                lastTransitionTime = Time.time;
                Debug.Log("Transitioning to: " + nextState);
            }
        }


        /*

        AnimatorStateInfo currentStateInfo = animator.GetCurrentAnimatorStateInfo(0); // Base layer (index 0)

        // Check if the current animation has finished and cooldown has passed
        if (currentStateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0) && Time.time >= lastTransitionTime + cooldownTime)
        {

            string nextState = GetNextValidState(currentStateInfo);

            if (!string.IsNullOrEmpty(nextState))
            {
                if (nextState.Equals("konttaa"))
                {
                    //float deltaForce = forceAmount * Time.deltaTime;
                    rb.AddForce(Vector2.left * 10);
                }
                animator.SetBool(nextState, true);

                animator.CrossFade(nextState, transitionDuration);
                lastTransitionTime = Time.time;
                Debug.Log("Transitioning to: " + nextState);
            }
        }
        */

        if (OnkoHyppaamassa() && currentstate == CharacterState.Jumping3)
        {
            // float deltaForce = (forceAmount / (hypynloppu - hypynalku)) * Time.deltaTime;
            float deltaForce = forceAmount * Time.deltaTime;

            if (forceAnnettu < maxforce)
            {
                rb.AddForce(Vector2.up * deltaForce);
                rb.AddForce(Vector2.left * deltaForce);
            }
            forceAnnettu += deltaForce;


        }
        else
        {
            forceAnnettu = 0.0f;

        }

        float minRotation = startingZRotation - rotationLimit / 2;
        float maxRotation = startingZRotation + rotationLimit / 2;

        float currentZRotation = NormalizeAngle(transform.rotation.eulerAngles.z);

        // Clamp the Z rotation
        float clampedZRotation = Mathf.Clamp(currentZRotation, minRotation, maxRotation);
        //katotaas torson rotation
        transform.rotation = Quaternion.Euler(
        transform.rotation.eulerAngles.x,
        transform.rotation.eulerAngles.y,
        clampedZRotation
        );
    }


    /*
    void AdjustFootPosition(Transform footTarget)
    {
        //RaycastHit2D[] hitsit = Physics2D.RaycastAll(footTarget.position + Vector3.up * 0.5f, Vector2.down, raycastDistance, groundLayer);
        RaycastHit2D[] hitsit = Physics2D.RaycastAll(footTarget.position , Vector2.down, raycastDistance, groundLayer);


        //RaycastHit2D[] hitsit = Physics2D.RaycastAll(transform.position, direction, distance);
        if (hitsit != null & hitsit.Length > 0)
        {
            foreach (RaycastHit2D hit in hitsit)
            {

                if (hit.collider != null && hit.collider.tag.Contains("tiili"))
                {
                    //Vector3 targetPosition = hit.point + (Vector2.up * footOffset);
                    //7footTarget.position = Vector3.Lerp(footTarget.position, targetPosition, Time.deltaTime * lerpSpeed);
                  
                    
                    
                      footTarget.position = new Vector2(footTarget.position.x, hit.point.y + footOffset);

                  //  footTarget.position = Vector3.Lerp(footTarget.position, targetPosition, Time.deltaTime * lerpSpeed);

                   // Vector3 targetPosition = hit.point + (Vector2.up * footOffset);
                   // footTarget.position = Vector3.Lerp(footTarget.position, targetPosition, Time.deltaTime * lerpSpeed);

                }
                else
                {
                    //  footTarget.position = Vector3.Lerp(footTarget.position, originalPosition, Time.deltaTime * lerpSpeed);
            //        footTarget.position = Vector3.Lerp(footTarget.position, originalPosition, Time.deltaTime * lerpSpeed);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(leftFoot.position , leftFoot.position + Vector3.down * raycastDistance);
        //Gizmos.color = Color.white;
        //Gizmos.DrawLine(rightFoot.position , rightFoot.position + Vector3.down * raycastDistance);

    }
    */


    string GetNextValidState()
    {
        List<string> possibleStates = new List<string>();

        AnimatorTransitionInfo transitionInfo = animator.GetAnimatorTransitionInfo(0);

        // If there's an active transition, get the destination state
        if (transitionInfo.fullPathHash != 0)
        {
            possibleStates.Add(transitionInfo.fullPathHash.ToString()); // Use state name instead if accessible
        }

        // Randomly select a valid transition
        if (possibleStates.Count > 0)
        {
            return possibleStates[Random.Range(0, possibleStates.Count)];
        }

        return null; // No valid transitions found
    }


    string GetNextValidState(AnimatorStateInfo currentStateInfo)
    {
        List<string> possibleTransitions = new List<string>();

        // Get the Animator Controller (Requires RuntimeAnimatorController)
        AnimatorControllerParameter[] parameters = animator.parameters;


        foreach (AnimatorControllerParameter param in parameters)
        {
            // Find transitions that are valid from the current state
            //(param.type == AnimatorControllerParameterType.Trigger ||
            if ( (param.type == AnimatorControllerParameterType.Bool) /* && animator.GetBool(param.name)*/ )
            {
                possibleTransitions.Add(param.name);
            }
        }

        // If there are valid transitions, pick a random one
        if (possibleTransitions.Count > 0)
        {
            return possibleTransitions[Random.Range(0, possibleTransitions.Count)];
        }

        return null; // No valid transition found
    }

}