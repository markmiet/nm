using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(SpriteSkin))]
public class SpriteMeshExploderGroupedFade : MonoBehaviour
{
    public SpriteSkin spriteSkin;
    public int rows = 10;
    public int cols = 10;
    public float explodeForce = 1f;
    public float fadeOutDelay = 1.5f;
    public float fadeDuration = 1.5f;
    public bool destroyOriginal = true;
    public float maxAngularVelocity = 360f;
    public Material meshMaterial;

    public bool rajayta = false;
    public bool rajaytetty = false;

    private SpriteRenderer sr;
    private List<GameObject> palaset = new List<GameObject>();

    void Awake()
    {
        if (spriteSkin == null) spriteSkin = GetComponent<SpriteSkin>();
        sr = spriteSkin.GetComponent<SpriteRenderer>();
    }

    public bool implode = false;

    public bool implodetehty = false;
    void Update()
    {
        if (rajayta && !rajaytetty)
        {
            rajaytetty = true;
            Explode();

        }

        if (implode && !implodetehty)
        {
            implodetehty = true;
            ImplodeAll(2f);
        }
    }

    public void Explode()
    {
        StartCoroutine(ExplodeRoutine());
    }

    private IEnumerator ExplodeRoutine()
    {
        Sprite sprite = sr.sprite;

        if (!spriteSkin.HasCurrentDeformedVertices())
        {
            Debug.LogWarning("Deformoidut vertexit eivät ole saatavilla.");
            yield break;
        }

        Vector3[] deformedVerts = spriteSkin.GetDeformedVertexPositionData().ToArray();
        ushort[] uTriangles = sprite.triangles;
        int[] triangles = uTriangles.Select(x => (int)x).ToArray();
        Vector2[] uvs = sprite.uv;

        Bounds bounds = sprite.bounds;
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        float stepX = (max.x - min.x) / cols;
        float stepY = (max.y - min.y) / rows;

        List<int>[] triangleGroups = new List<int>[rows * cols];
        for (int i = 0; i < triangleGroups.Length; i++)
            triangleGroups[i] = new List<int>();

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 avg = (deformedVerts[triangles[i]] +
                           deformedVerts[triangles[i + 1]] +
                           deformedVerts[triangles[i + 2]]) / 3f;

            int colIndex = Mathf.Clamp(Mathf.FloorToInt((avg.x - min.x) / stepX), 0, cols - 1);
            int rowIndex = Mathf.Clamp(Mathf.FloorToInt((avg.y - min.y) / stepY), 0, rows - 1);
            int cellIndex = rowIndex * cols + colIndex;

            triangleGroups[cellIndex].Add(triangles[i]);
            triangleGroups[cellIndex].Add(triangles[i + 1]);
            triangleGroups[cellIndex].Add(triangles[i + 2]);
        }

        foreach (var group in triangleGroups)
        {
            if (group.Count == 0) continue;

            List<Vector3> verts = new List<Vector3>();
            List<Vector2> pieceUVs = new List<Vector2>();
            List<int> tris = new List<int>();
            Dictionary<int, int> vertexMap = new Dictionary<int, int>();

            foreach (int triIndex in group)
            {
                if (!vertexMap.TryGetValue(triIndex, out int newIndex))
                {
                    newIndex = verts.Count;
                    vertexMap[triIndex] = newIndex;
                    verts.Add(deformedVerts[triIndex]);
                    pieceUVs.Add(uvs[triIndex]);
                }
                tris.Add(vertexMap[triIndex]);
            }

            GameObject piece = new GameObject("MeshPiece");
            piece.transform.position = spriteSkin.transform.position;
            piece.transform.rotation = spriteSkin.transform.rotation;
            piece.transform.localScale = spriteSkin.transform.lossyScale;

            MeshFilter mf = piece.AddComponent<MeshFilter>();
            MeshRenderer mr = piece.AddComponent<MeshRenderer>();
            Mesh mesh = new Mesh();
            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            mesh.SetUVs(0, pieceUVs);
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mf.mesh = mesh;

            if (meshMaterial != null)
                mr.material = meshMaterial;

            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            mpb.SetColor("_Color", sr.sharedMaterial.color);
            mr.SetPropertyBlock(mpb);

            PolygonCollider2D collider = piece.AddComponent<PolygonCollider2D>();
            collider.pathCount = tris.Count / 3;
            for (int t = 0; t < tris.Count; t += 3)
            {
                Vector2[] path = new Vector2[3];
                for (int v = 0; v < 3; v++)
                    path[v] = new Vector2(verts[tris[t + v]].x, verts[tris[t + v]].y);
                collider.SetPath(t / 3, path);
            }

            Rigidbody2D rb = piece.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1.0f;
          //  rb.mass = 0.0001f;
            Vector2 dir = (Random.insideUnitCircle + Vector2.up * 0.3f).normalized;
            rb.AddForce(dir * explodeForce, ForceMode2D.Impulse);
            rb.angularVelocity = Random.Range(-maxAngularVelocity, maxAngularVelocity);

            // --- Fade + data komponentit ---
            MeshPieceFade fade = piece.AddComponent<MeshPieceFade>();
            fade.fadeOutDelay = fadeOutDelay;
            fade.fadeDuration = fadeDuration;
            fade.Initialize(mr, mpb);

            MeshPieceData data = piece.AddComponent<MeshPieceData>();
            data.SaveStartTransform(piece.transform);
            palaset.Add(piece);
        }
        /* */
        if (destroyOriginal)
            Destroy(spriteSkin.gameObject);
        else
            sr.enabled = false;
       

        yield return null;
    }

    // --- Implosion + fade-in ---
    public void ImplodeAll(float duration)
    {
        foreach (var piece in palaset)
        {
            if (piece == null) continue;

            Rigidbody2D rb = piece.GetComponent<Rigidbody2D>();
            if (rb != null) rb.isKinematic = true;

            MeshPieceFade fade = piece.GetComponent<MeshPieceFade>();
            if (fade != null) fade.StopAllCoroutines(); // lopeta fade-out jos meneillään

            StartCoroutine(piece.GetComponent<MeshPieceData>().ImplodeAndFadeIn(duration));
        }
    }
}

// --- Fade & destroy (räjähdys) ---
public class MeshPieceFade : MonoBehaviour
{
    public float fadeOutDelay = 1.5f;
    public float fadeDuration = 1.5f;
    private MeshRenderer mr;
    private MaterialPropertyBlock mpb;

    public void Initialize(MeshRenderer renderer, MaterialPropertyBlock block)
    {
        mr = renderer;
        mpb = block;
        StartCoroutine(FadeAndDestroy());
    }

    private IEnumerator FadeAndDestroy()
    {
        yield return new WaitForSeconds(fadeOutDelay);

        float timer = 0f;
        Color color = mpb.GetColor("_Color");

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            mpb.SetColor("_Color", color);
            mr.SetPropertyBlock(mpb);
            yield return null;
        }

        Destroy(gameObject);
    }
}

// --- Tallennetaan alkuperäinen sijainti + rotaatio + implosion fade-in ---
public class MeshPieceData : MonoBehaviour
{
    public Vector3 startPosition;
    public Quaternion startRotation;

    public void SaveStartTransform(Transform t)
    {
        startPosition = t.position;
        startRotation = t.rotation;
    }

    public IEnumerator ImplodeAndFadeIn(float duration)
    {
        float timer = 0f;
        Vector3 initialPos = transform.position;
        Quaternion initialRot = transform.rotation;

        MeshRenderer mr = GetComponent<MeshRenderer>();
        MaterialPropertyBlock mpb = new MaterialPropertyBlock();
        mr.GetPropertyBlock(mpb);

        Color color = mpb.GetColor("_Color");
        color.a = 0f; // fade-in alkaa näkymättömänä
        mpb.SetColor("_Color", color);
        mr.SetPropertyBlock(mpb);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            transform.position = Vector3.Lerp(initialPos, startPosition, t);
            transform.rotation = Quaternion.Slerp(initialRot, startRotation, t);

            color.a = Mathf.Lerp(0f, 1f, t); // fade-in
            mpb.SetColor("_Color", color);
            mr.SetPropertyBlock(mpb);

            yield return null;
        }

        transform.position = startPosition;
        transform.rotation = startRotation;

        color.a = 1f;
        mpb.SetColor("_Color", color);
        mr.SetPropertyBlock(mpb);
    }
}

// --- Helper extension ---
public static class MeshExtensions
{
    public static Vector2[] ConvertToVector2(this List<Vector3> verts)
    {
        Vector2[] result = new Vector2[verts.Count];
        for (int i = 0; i < verts.Count; i++)
            result[i] = new Vector2(verts[i].x, verts[i].y);
        return result;
    }
}
