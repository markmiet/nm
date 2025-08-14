using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class OutlineSmokeEmitter : MonoBehaviour
{
    [Tooltip("Particle system used for smoke emission. Should be preconfigured (size, lifetime, material).")]
    public ParticleSystem smokeSystem;





    [Tooltip("Distance between smoke emissions along outline (in sprite units).")]
    public float spacing = 0.1f;

    [Tooltip("Random positional jitter in world units.")]
    public float jitter = 0.02f;

    [Tooltip("Scale range for each particle.")]
    public Vector2 sizeRange = new Vector2(0.5f, 1f);


    private Vector2 sizeRangeMinimi = new Vector2(0.01f, 0.01f);

    [Tooltip("Whether to emit only once at Start or repeatedly.")]
    public bool emitInStart = true;
    public bool emitInUpdate = true;

    [Tooltip("Emiting maksimi kokonaisaikaupdatessa")]
    public float emitInUpdateMaxLength = 10000;

    [Tooltip("If not once, emit every X seconds.")]
    public float emitInterval = 0.25f;

    SpriteRenderer sr;
    Sprite sprite;
    float nextEmit;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sprite = sr.sprite;

        if (smokeSystem == null)
            Debug.LogWarning("OutlineSmokeEmitter: No ParticleSystem assigned!");
    }

    void Start()
    {
        if (emitInStart)
            EmitOutlineSmoke();
    }

    private float emitstarttime = 0;

    void Update()
    {
        if (emitInUpdate && Time.time >= nextEmit)
        {
            EmitOutlineSmoke();
            nextEmit = Time.time + emitInterval;
            if (emitstarttime==0)
            {
                emitstarttime = Time.time;
            }

            if (Time.time - emitstarttime >= emitInUpdateMaxLength)
            {
                sizeRange *= 0.99f;//pienenee

                //emitInUpdate = false;

                if (sizeRange.x< sizeRangeMinimi.x || sizeRange.y<sizeRangeMinimi.y)
                {
                    emitInUpdate = false;
                }
            }
        }
        else if (!emitInUpdate && Time.time >= nextEmit)
        {
            if (smokeSystem!=null && smokeSystem.isPlaying)
            {
                //smokeSystem.Stop();
                smokeSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
                


            nextEmit = Time.time + emitInterval;
            //Debug.Log("ei eimit");
        }
    }

    void EmitOutlineSmoke2()
    {
        if (sprite == null || smokeSystem == null) return;

        if (!smokeSystem.isPlaying)
            smokeSystem.Play();

        int shapeCount = sprite.GetPhysicsShapeCount();
        ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams();

        for (int s = 0; s < shapeCount; s++)
        {
            List<Vector2> shape = new List<Vector2>();
            sprite.GetPhysicsShape(s, shape);

            if (shape.Count < 2) continue;

            for (int i = 0; i < shape.Count; i++)
            {
                Vector2 a = shape[i];
                Vector2 b = shape[(i + 1) % shape.Count];

                float edgeLen = Vector2.Distance(a, b);
                if (edgeLen <= Mathf.Epsilon) continue;

                int samples = Mathf.Max(1, Mathf.CeilToInt(edgeLen / spacing));

                for (int k = 0; k < samples; k++)
                {
                    float t = (k + 0.5f) / samples;
                    Vector2 localPos = Vector2.Lerp(a, b, t);

                    // Convert sprite local space to world space
                    Vector3 worldPos = sr.transform.TransformPoint(localPos);

                    // Add small random jitter
                    worldPos += (Vector3)(Random.insideUnitCircle * jitter);

                    // Configure particle parameters
                    ep.position = smokeSystem.transform.InverseTransformPoint(worldPos);
                    ep.startSize = Random.Range(sizeRange.x, sizeRange.y);
                    ep.startColor = Color.gray;
                    ep.rotation = Random.Range(0f, 360f);
                    ep.applyShapeToPosition = false;

                    smokeSystem.Emit(ep, 1);
                }
            }
        }
    }


    void EmitOutlineSmoke()
    {
        if (sprite == null || smokeSystem == null) return;

        if (!smokeSystem.isPlaying)
            smokeSystem.Play();

        int shapeCount = sprite.GetPhysicsShapeCount();
        ParticleSystem.EmitParams ep = new ParticleSystem.EmitParams();

        for (int s = 0; s < shapeCount; s++)
        {
            List<Vector2> shape = new List<Vector2>();
            sprite.GetPhysicsShape(s, shape);

            if (shape.Count < 2) continue;

            for (int i = 0; i < shape.Count; i++)
            {
                Vector2 a = shape[i];
                Vector2 b = shape[(i + 1) % shape.Count];

                // Scale the local points to account for Transform's localScale
                Vector2 scaledA = Vector2.Scale(a, sr.transform.lossyScale);
                Vector2 scaledB = Vector2.Scale(b, sr.transform.lossyScale);

                float edgeLen = Vector2.Distance(scaledA, scaledB);
                if (edgeLen <= Mathf.Epsilon) continue;

                int samples = Mathf.Max(1, Mathf.CeilToInt(edgeLen / spacing));

                for (int k = 0; k < samples; k++)
                {
                    float t = (k + 0.5f) / samples;

                    // Interpolate between scaled points
                    Vector2 localScaledPos = Vector2.Lerp(scaledA, scaledB, t);

                    // Convert to world space using TransformPoint (still needed for rotation + position)
                    Vector3 worldPos = sr.transform.TransformPoint(localScaledPos);

                    // Add jitter
                    worldPos += (Vector3)(Random.insideUnitCircle * jitter);

                    // Particle space
                    ep.position = smokeSystem.transform.InverseTransformPoint(worldPos);
                    ep.startSize = Random.Range(sizeRange.x, sizeRange.y);
                    ep.startColor = Color.gray;
                    ep.rotation = Random.Range(0f, 360f);
                    ep.applyShapeToPosition = false;

                    smokeSystem.Emit(ep, 1);
                }
            }
        }
    }


}
