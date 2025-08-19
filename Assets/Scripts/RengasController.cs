using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RengasController : BaseController
{
    private Transform spaceship;                  // Target to follow
    public WheelJoint2D leftWheel, rightWheel;   // Wheel joints
    public float moveThreshold = 0.5f;           // Min distance to start moving
    public float motorSpeed = 500f;              // Motor speed (positive or negative)
    public float maxTorque = 1000f;              // How strong the motor is

    private Rigidbody2D rb;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SetupMotors(0f); // Start with motors off
    }




    void FixedUpdate()
    {
        if (IsGoingToBeDestroyed())
        {
            return;
        }


        if (spaceship == null)
        {
            spaceship = PalautaAlus().transform;
        }

        float directionToTarget = spaceship.position.x - transform.position.x;

        // Check if we need to move (based on horizontal distance only)
        if (Mathf.Abs(directionToTarget) > moveThreshold)
        {
            //float speed = Mathf.Sign(directionToTarget) * -motorSpeed; // "-" because wheel joint spins backward to move forward

            float speed = Mathf.Sign(directionToTarget) * motorSpeed;   // no negative

            SetupMotors(speed);
        }
        else
        {
            SetupMotors(0f); // Stop moving
        }
    }

    void SetupMotors(float speed)
    {
        JointMotor2D motor = leftWheel.motor;
        motor.motorSpeed = speed;
        motor.maxMotorTorque = maxTorque;

        leftWheel.motor = motor;
        rightWheel.motor = motor;


        leftWheel.useMotor = speed != 0;
        rightWheel.useMotor = speed != 0;
    }
}



