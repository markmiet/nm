// RingBoxColliderGenerator.cs
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
[DisallowMultipleComponent]
public class RingBoxColliderGenerator : MonoBehaviour
{
    [Header("Source sprite (optional)")]
    public SpriteRenderer spriteRenderer;

    [Header("Ring radii (world units)")]
    public float outerRadius = 1f;
    public float innerRadius = 0.6f;

    [Header("Collider pieces")]
    public float desiredArcLength = 0.1f;
    public float radialThickness = 0.1f;
    public float overlapFactor = 1.05f;

    [Header("Options")]
    public bool alignTangent = true;
    public string generatedPrefix = "RingBox_";
    public bool isTrigger = false;

    [Tooltip("If true, generated colliders are merged by CompositeCollider2D on this object")]
    public bool useComposite = true;

    [Header("Debug / auto")]
    public bool tryUseSpriteBounds = true;

    void OnValidate()
    {
        outerRadius = Mathf.Max(0.01f, outerRadius);
        innerRadius = Mathf.Clamp(innerRadius, 0f, outerRadius - 0.001f);
        desiredArcLength = Mathf.Max(0.001f, desiredArcLength);
        radialThickness = Mathf.Clamp(radialThickness, 0.001f, outerRadius - innerRadius + 0.001f);
        overlapFactor = Mathf.Max(1f, overlapFactor);
    }

    public void Generate()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (tryUseSpriteBounds && spriteRenderer != null)
        {
            float srOuterMax = Mathf.Max(spriteRenderer.bounds.extents.x, spriteRenderer.bounds.extents.y);
            outerRadius = srOuterMax;
        }

        RemoveGenerated();

        float avgRadius = (outerRadius + innerRadius) * 0.5f;
        float circumference = 2f * Mathf.PI * avgRadius;
        int count = Mathf.Max(3, Mathf.CeilToInt(circumference / desiredArcLength));
        float arcLength = circumference / count;
        float boxLength = arcLength * overlapFactor;
        float boxHeight = radialThickness;

        // Setup Composite if enabled
        if (useComposite)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Static;
            }

            CompositeCollider2D comp = GetComponent<CompositeCollider2D>();
            if (comp == null)
            {
                comp = gameObject.AddComponent<CompositeCollider2D>();
                comp.geometryType = CompositeCollider2D.GeometryType.Polygons;
            }
        }

        // Create segments
        for (int i = 0; i < count; i++)
        {
            float t = (float)i / count;
            float angle = t * Mathf.PI * 2f;
            Vector3 localPos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * avgRadius;

            GameObject go = new GameObject(generatedPrefix + i);
            go.transform.SetParent(this.transform, false);
            go.transform.localPosition = localPos;
            float deg = angle * Mathf.Rad2Deg;
            go.transform.localRotation = alignTangent
                ? Quaternion.Euler(0f, 0f, deg + 90f)
                : Quaternion.Euler(0f, 0f, deg);

            BoxCollider2D bc = go.AddComponent<BoxCollider2D>();
            bc.size = new Vector2(boxLength, boxHeight);
            bc.offset = Vector2.zero;
            bc.isTrigger = isTrigger;

            if (useComposite)
                bc.usedByComposite = true;

            GeneratedMarker marker = go.AddComponent<GeneratedMarker>();
            marker.generator = this;
        }

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            EditorUtility.SetDirty(this.gameObject);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(this.gameObject.scene);
        }
#endif
    }

    public void RemoveGenerated()
    {
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform c = transform.GetChild(i);
            GeneratedMarker m = c.GetComponent<GeneratedMarker>();
            if (m != null && m.generator == this)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    DestroyImmediate(c.gameObject);
                else
                    Destroy(c.gameObject);
#else
                Destroy(c.gameObject);
#endif
            }
        }
    }
}

public class GeneratedMarker : MonoBehaviour
{
    public RingBoxColliderGenerator generator;
}
