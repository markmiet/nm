using System.Collections;
using UnityEngine;

public class DirectionSpriteSwitcher : MonoBehaviour
{
    public enum State
    {
        WalkLeft,
        IdleCenter,
        WalkRight
    }

    [Header("stateJokahalutaanAsettaa (for debugging)")]
    public State stateJokahalutaanAsettaa;

    [Header("Current State (for debugging)")]
    public State currentState;


    [Header("Child GameObjects")]
    public GameObject walkLeft;
    public GameObject idleCenter;
    public GameObject walkRight;

    [Header("Movement Settings")]
    public float idleThreshold = 0.1f; // prevents flickering

    private Rigidbody2D rb;
    private Animator leftAnim;
    private Animator centerAnim;
    private Animator rightAnim;
    public DualLegForce2D dualLegForce2Doikea;
    public DualLegForce2D dualLegForce2Dvasen;


    public float dualLegForce2DoikeaFootForceOriginal;
    public float dualLegForce2DvasenFootForceOriginal;
    public float walkanimspeed = 0.75f;
    public float idelanimspeed = 2.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        leftAnim = walkLeft.GetComponent<Animator>();
        centerAnim = idleCenter.GetComponent<Animator>();
        rightAnim = walkRight.GetComponent<Animator>();
        rightAnim.SetBool("keski", false);
        leftAnim.SetBool("vasen2keski", false);
        dualLegForce2DoikeaFootForceOriginal = dualLegForce2Doikea.footForce;
        dualLegForce2DvasenFootForceOriginal = dualLegForce2Dvasen.footForce;
    //    SetState(stateJokahalutaanAsettaa, true );
    }

    void Update()
    {
        /*
        float moveX = rb != null ? rb.velocity.x : Input.GetAxisRaw("Horizontal");

        if (moveX > idleThreshold)
            SetState(State.WalkRight);
        else if (moveX < -idleThreshold)
            SetState(State.WalkLeft);
        else
            SetState(State.IdleCenter);
        */
        SetState(stateJokahalutaanAsettaa, false);
    }

    void SetState(State newState,bool force)
    {
        if (!force && currentState == newState) 
            return; // no change needed
                    //vaihdetaan positio

        Vector3 position = Vector3.zero;
       /*
        switch (currentState)
        {
            case State.WalkLeft:
                position = walkLeft.transform.position;
                
                // leftAnim?.SetBool("isWalking", true);
                break;

            case State.IdleCenter:
                position = walkRight.transform.position;

                //centerAnim?.SetBool("isWalking", false);
                //leftAnim?.SetBool("isWalking", false);
                
                break;

            case State.WalkRight:
                position = walkRight.transform.position;

                //walkRight.SetActive(true);
                //rightAnim?.SetBool("isWalking", true);
                break;
        }
        */

        if (walkLeft.active)
        {
            position = walkLeft.transform.position;
        }
        else if (walkRight.active)
        {
            position = walkRight.transform.position;
        }

        State previousState = currentState;

        currentState = newState;
        stateJokahalutaanAsettaa = newState;
        // deactivate all first
        walkLeft.SetActive(false);
        idleCenter.SetActive(false);
        walkRight.SetActive(false);

        // activate one based on state
        switch (currentState)
        {
            case State.WalkLeft:
                walkLeft.SetActive(true);
                walkLeft.transform.position = position;
                dualLegForce2Dvasen.footForce = dualLegForce2DvasenFootForceOriginal;
                leftAnim.speed = walkanimspeed;
                // leftAnim?.SetBool("isWalking", true);
                break;

            case State.IdleCenter:
                idleCenter.transform.position = position;
                dualLegForce2Doikea.footForce = 0.0f;
                dualLegForce2Dvasen.footForce = 0.0f;
                if (previousState== State.WalkRight)
                {
                    walkRight.SetActive(true);
                    rightAnim.SetBool("keski", true);
                    leftAnim.SetBool("vasen2keski", false);
                    rightAnim.speed = idelanimspeed;
                    leftAnim.speed = idelanimspeed;

                }
                else if (previousState == State.WalkLeft)
                {
                    walkLeft.SetActive(true);
                    rightAnim.SetBool("keski", false);
                    leftAnim.SetBool("vasen2keski", true);
                    leftAnim.speed = idelanimspeed;
                    rightAnim.speed = idelanimspeed;
                }


                //idleCenter.SetActive(true);



                //       walkRight.SetActive(true);


                //centerAnim?.SetBool("isWalking", false);
                //leftAnim?.SetBool("isWalking", false);
                //rightAnim?.SetBool("isWalking", false);
                break;

            case State.WalkRight:
                walkRight.SetActive(true);
                walkRight.transform.position = position;
                dualLegForce2Doikea.footForce = dualLegForce2DoikeaFootForceOriginal;
                rightAnim.speed = walkanimspeed;
                //rightAnim?.SetBool("isWalking", true);
                break;
        }
    }



    public void ChangeState(State state)
    {
        Debug.Log("Animation event received — switching to WalkLeft");
       // SetState(state,false);
        StartCoroutine(DeferredChangeState(state));
    }

    private IEnumerator DeferredChangeState(State newState)
    {
        yield return new WaitForEndOfFrame();
        SetState(newState, false);
    }

}
