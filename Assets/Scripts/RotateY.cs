using UnityEngine;

public class RotateY : MonoBehaviour
{
    public float speed = 2f;        // how fast it pulses
    public float scaleAmount = 0.3f; // max size change
    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        // Ping-pong with sine wave between -1 and 1
        float pulse = Mathf.Sin(Time.time * speed) * scaleAmount;

        // Apply scale to X and Y (leave Z as is for 2D)
        transform.localScale = startScale + new Vector3(pulse, pulse, 0f);
    }
}
