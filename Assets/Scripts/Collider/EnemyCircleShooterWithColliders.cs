using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyCircleShooterWithColliders : MonoBehaviour
{
    [Header("Circle Settings")]
    public float minRadius = 1f;
    public float maxRadius = 3f;
    public float ringWidth = 0.5f;
    [Tooltip("Speed at which the circle grows from minRadius to maxRadius.")]
    public float growthSpeed = 1f; // units per second

    [Header("Pattern Settings")]
    public int filledAngle = 40;  // visible arc size
    public int emptyAngle = 40;   // gap size
    [Range(1f, 10f)] public float arcResolution = 5f; // degrees per vertex

    [Header("Arc Range")]
    public float angleMin = 90f;   // starting angle
    public float angleMax = 160f;  // ending angle

    [Header("Visuals")]
    public Material ringMaterial;  // assign in Inspector
    public Texture ringTexture;    // optional
    [Range(1f, 20f)] public float textureRepeat = 8f;

    [Header("Collider Fine-Tuning")]
    [Tooltip("Adjust how far inward the inner collider edge sits (0.5 = half ring width). Only affects collider.")]
    [Range(0f, 1f)] public float innerScale = 0.5f;

    [Header("Rotation Settings")]
    [Tooltip("Degrees per second rotation of the arcs.")]
    public float rotationSpeed = 90f;

    private float currentRadius;
    private float timeOffset;

    private readonly List<LineRenderer> arcRenderers = new List<LineRenderer>();
    private readonly List<PolygonCollider2D> arcColliders = new List<PolygonCollider2D>();
    private readonly List<Transform> arcTransforms = new List<Transform>(); // store transforms for rotation

    public TextureWrapMode texturewrapmode = TextureWrapMode.Repeat;
    public LineTextureMode texturemode = LineTextureMode.Tile;

    void Start()
    {
        currentRadius = minRadius; // start at minimum radius
        timeOffset = Random.value * Mathf.PI * 2f;
        GenerateArcs();
    }

    void Update()
    {
        // Grow radius smoothly from minRadius to maxRadius
        if (currentRadius < maxRadius)
        {
            currentRadius = Mathf.MoveTowards(currentRadius, maxRadius, growthSpeed * Time.deltaTime);
        }

        UpdateArcs();

        // Rotate arcs
        for (int i = 0; i < arcTransforms.Count; i++)
        {
            arcTransforms[i].Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    void GenerateArcs()
    {
        ClearArcs();

        float patternLength = filledAngle + emptyAngle;
        int totalArcs = Mathf.CeilToInt((angleMax - angleMin) / patternLength);

        for (int i = 0; i < totalArcs; i++)
        {
            float startAngle = angleMin + i * patternLength;
            if (startAngle >= angleMax) break;
            float endAngle = Mathf.Min(startAngle + filledAngle, angleMax);

            GameObject arcObj = new GameObject($"Arc_{i}");
            arcObj.transform.SetParent(transform, false);
            arcObj.transform.localPosition = Vector3.zero;
            arcObj.transform.localRotation = Quaternion.identity;
            arcObj.transform.localScale = Vector3.one;

            arcTransforms.Add(arcObj.transform);

            LineRenderer arcRenderer = arcObj.AddComponent<LineRenderer>();
            arcRenderer.useWorldSpace = false;
            arcRenderer.loop = false;
            arcRenderer.startWidth = ringWidth;
            arcRenderer.endWidth = ringWidth;
            arcRenderer.numCapVertices = 2;

            InitializeArcMaterial(arcRenderer);

            PolygonCollider2D poly = arcObj.AddComponent<PolygonCollider2D>();
            poly.isTrigger = false;

            arcRenderers.Add(arcRenderer);
            arcColliders.Add(poly);
        }
    }

    void UpdateArcs()
    {
        float patternLength = filledAngle + emptyAngle;
        float angle = angleMin;

        for (int i = 0; i < arcRenderers.Count; i++)
        {
            float startAngle = angle;
            if (startAngle >= angleMax) break;
            float endAngle = Mathf.Min(angle + filledAngle, angleMax);

            UpdateArcRenderer(arcRenderers[i], arcColliders[i], startAngle, endAngle);

            angle += patternLength;
        }
    }

    void UpdateArcRenderer(LineRenderer arcRenderer, PolygonCollider2D collider, float startAngle, float endAngle)
    {
        int steps = Mathf.CeilToInt((endAngle - startAngle) / arcResolution);

        // -------------------------
        // LineRenderer points (visual)
        // -------------------------
        List<Vector3> linePoints = new List<Vector3>();
        float lineRadius = currentRadius; // visuals always at currentRadius

        for (int s = 0; s <= steps; s++)
        {
            float a = Mathf.Lerp(startAngle, endAngle, s / (float)steps);
            float rad = a * Mathf.Deg2Rad;
            linePoints.Add(new Vector3(Mathf.Cos(rad) * lineRadius, Mathf.Sin(rad) * lineRadius, 0f));
        }

        arcRenderer.positionCount = linePoints.Count;
        arcRenderer.SetPositions(linePoints.ToArray());

        // Texture scaling
        float arcFraction = (endAngle - startAngle) / 360f;
        arcRenderer.material.mainTextureScale = new Vector2(arcFraction * textureRepeat, 1f);

        // -------------------------
        // PolygonCollider2D points (physics)
        // -------------------------
        float colliderInnerRadius = currentRadius - (ringWidth * innerScale);
        float colliderOuterRadius = currentRadius + (ringWidth * (1f - innerScale));

        List<Vector2> colliderPolygon = new List<Vector2>();

        // Outer edge (start -> end)
        for (int s = 0; s <= steps; s++)
        {
            float a = Mathf.Lerp(startAngle, endAngle, s / (float)steps);
            float rad = a * Mathf.Deg2Rad;
            colliderPolygon.Add(new Vector2(Mathf.Cos(rad) * colliderOuterRadius, Mathf.Sin(rad) * colliderOuterRadius));
        }

        // Inner edge (end -> start)
        for (int s = steps; s >= 0; s--)
        {
            float a = Mathf.Lerp(startAngle, endAngle, s / (float)steps);
            float rad = a * Mathf.Deg2Rad;
            colliderPolygon.Add(new Vector2(Mathf.Cos(rad) * colliderInnerRadius, Mathf.Sin(rad) * colliderInnerRadius));
        }

        collider.pathCount = 1;
        collider.SetPath(0, colliderPolygon.ToArray());
        collider.offset = Vector2.zero;
    }

    void InitializeArcMaterial(LineRenderer lr)
    {
        if (ringMaterial != null)
            lr.material = new Material(ringMaterial);
        else
            lr.material = new Material(Shader.Find("Sprites/Default"));

        if (ringTexture != null)
        {
            lr.material.mainTexture = ringTexture;
            lr.material.mainTexture.wrapMode = texturewrapmode;
        }

        lr.textureMode = texturemode;
    }

    void ClearArcs()
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        arcRenderers.Clear();
        arcColliders.Clear();
        arcTransforms.Clear();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var col in GetComponentsInChildren<PolygonCollider2D>())
        {
            if (col.pathCount == 0) continue;
            var path = col.GetPath(0);
            for (int i = 0; i < path.Length; i++)
            {
                Vector2 p1 = col.transform.TransformPoint(path[i]);
                Vector2 p2 = col.transform.TransformPoint(path[(i + 1) % path.Length]);
                Gizmos.DrawLine(p1, p2);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, minRadius);
        Gizmos.DrawWireSphere(transform.position, maxRadius);
    }
}
