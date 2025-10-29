using UnityEngine;

public class PingPongRotateCapsule : BaseController
{
    // Maximum rotation angles for each axis
    public float maxRotationX = 45f;
    public float maxRotationY = 60f;
    public float maxRotationZ = 90f;

    // Speed of rotation for each axis
    public float speedX = 1f;
    public float speedY = 1f;
    public float speedZ = 1f;

    public Vector2 velocity;
    public bool asetaVelocity = false;

    private Rigidbody2D r;
    private void Start()
    {
        r = GetComponent<Rigidbody2D>();
        if (r!=null && asetaVelocity)
        {
            r.velocity = velocity;
        }
    }

    void Update()
    {
        /*
        if (!OnkoOkToimiaUusi(gameObject))
        {
            return;
        }
        */
        
        float rotX = Mathf.Sin(Time.time * speedX) * maxRotationX;
        float rotY = Mathf.Sin(Time.time * speedY) * maxRotationY;
        float rotZ = Mathf.Sin(Time.time * speedZ) * maxRotationZ;

        transform.localRotation = Quaternion.Euler(rotX, rotY, rotZ);
        if (r != null && asetaVelocity)
        {
            r.velocity = velocity;
        }
        
    }
}
