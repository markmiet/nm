using UnityEngine;

public class OscillateRedChannel : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float originalRed;
    private float originalGreen;

    private float time;

    // Speed of the oscillation
    public float speed = 1f;

    public float range = 0.2f;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalRed = spriteRenderer.color.r;
            originalGreen = spriteRenderer.color.g;
        }
    }

    void Update()
    {
        if (spriteRenderer == null) return;

        time += Time.deltaTime * speed;

        // Calculate oscillation from -0.05 to +0.05
        float offset = Mathf.Sin(time) * range;

        // New red value, clamped between 0 and 1
        float newRed = Mathf.Clamp01(originalRed + offset);

        float newgreen = Mathf.Clamp01(originalGreen + offset);


        // Apply new red value, keeping other color channels the same
        Color color = spriteRenderer.color;
        color.r = newRed;
        color.g = newgreen;

        spriteRenderer.color = color;
    }
}
