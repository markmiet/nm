using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

//[RequireComponent(typeof(SpriteSkin))]
public class SpriteMeshExploderReverse : HitCounter
{
    [Header("Setup")]
    public SpriteSkin spriteSkin;
    public Material meshMaterial;

    [Header("Explosion Grid")]
    public int rows = 10;
    public int cols = 10;

    [Header("Explosion Physics")]
    public float explodeForce = 1f;
    public float maxAngularVelocity = 360f;

    [Header("Timing")]
    public float recordDuration = 5f; // How long to record explosion motion
    public float implodeWait = 1f;    // Delay before implosion starts
    public float implodeSpeed = 1f;
    public float fadeOutDuration = 1f; // Fade-out time for pieces



    private SpriteRenderer sr;
    private List<GameObject> pieces = new List<GameObject>();
    private bool isRecording = false;

    [Header("Hit Settings")]
    public int hitsBeforeDestruction = 1;
    private int hitCountTotal = 0;
    private int hitCountti = 0;
    public bool disableCollisions = false;

    private void Start()
    {
        if (spriteSkin == null) spriteSkin = GetComponent<SpriteSkin>();
        sr = spriteSkin.GetComponent<SpriteRenderer>();

        haulikot =
        GetComponentsInChildren<HaulikkoPiippuController>();


        materiaali = sr.material;
        //sr.material = materiaali;
        if (disableCollisions)
        {
            DisableInternalCollisions(gameObject);
        }
    }

    public static void DisableInternalCollisions(GameObject root)
    {
        // Get all colliders under this object
        Collider2D[] colliders = root.GetComponentsInChildren<Collider2D>();

        // Loop through all combinations and disable collision between them
        for (int i = 0; i < colliders.Length; i++)
        {
            for (int j = i + 1; j < colliders.Length; j++)
            {
                Physics2D.IgnoreCollision(colliders[i], colliders[j], true);
            }
        }
    }

    public Material materiaali;

    private HaulikkoPiippuController[] haulikot;

    public float piecerigidbodygravityscale = 1;

    public override bool RegisterHit()
    {
        if (!sr.enabled)
        {
            return false;

        }

        hitCountTotal++;
        hitCountti++;

        if (hitCountTotal >= hitThreshold)
        {
            if (hitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan!=null)
            {
                
                //GameObject raj=Instantiate(hitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan,
                  //  transform.position,Quaternion.identity);
                //Destroy(raj, kestoaikahitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan);
                explodeForce = explodeForce / 10;
                maxAngularVelocity = maxAngularVelocity /10;
                piecerigidbodygravityscale = 0f;
                //sr.enabled = false;
                Collider2D[] cc =
                GetComponentsInChildren<Collider2D>();
                foreach(Collider2D c in cc)
                {
                    c.enabled = false;
                }

                SpriteRenderer[] ccs =
                GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer c in ccs)
                {
                    c.enabled = false;
                }
                HaulikkoPiippuController[] ccsk =
               GetComponentsInChildren<HaulikkoPiippuController>();
                foreach (HaulikkoPiippuController c in ccsk)
                {
                    c.enabled = false;
                }


                StartCoroutine(ExplodeRoutine());
               
                StartCoroutine(TuhoaViiveella());
               
            }
            //BaseDestroy();
            return true;
        }
        else if (hitCountti >= hitsBeforeDestruction)
        {
            hitCountti = 0;
            DisableMainRenderers();
            StartCoroutine(ExplodeSequence());
            return false;
        }

        return false;
    }

    public IEnumerator TuhoaViiveella()
    {
        yield return 0.1f;
        int maara = 0;
        foreach (GameObject p in pieces)
        {
  if (explosion!=null)
                {
                    GameObject raj = Instantiate(explosion,
                            p.transform.position, Quaternion.identity);
                    Destroy(raj, kestoaikahitcounterinRajaytaObjektiJokaInstantioidaanKunThreadSoldYlitetaan);
                }


            Destroy(p);
            maara++;
            yield return null;
        }

        BaseDestroy();

    }


    private void DisableMainRenderers()
    {
        foreach (var c in GetComponentsInChildren<Collider2D>())
        {
            if (!c.gameObject.name.Contains("Haulikko"))
                c.enabled = false;
            else
            {
                Debug.Log("haullikko");


            }

        }


        sr.enabled = false;

        foreach (var rb in GetComponentsInChildren<Rigidbody2D>())
        {
            if (!rb.gameObject.name.Contains("Haulikko"))
            {
                rb.simulated = false;
            }
            else
            {

                //rb.constraints = RigidbodyConstraints2D.FreezePositionX;

                rb.constraints = RigidbodyConstraints2D.FreezePositionX |
                     RigidbodyConstraints2D.FreezePositionY |
                     RigidbodyConstraints2D.FreezeRotation;

                /*
                rb.constraints = RigidbodyConstraints2D.FreezePositionX |
                    RigidbodyConstraints2D.FreezePositionY;
                */
                //   rb.gravityScale = 0.15f;
                //todoo arvot talteen ja
                foreach (HaulikkoPiippuController h in haulikot)
                {
                    /*
                    h.gameObject.GetComponent<ApplyForces2D>().boostedSpeed = 5;
                    h.gameObject.GetComponent<ApplyForces2D>().force = 0.5f;
                    h.gameObject.GetComponent<ApplyForces2D>().maxAngularSpeed = 2.5f;
                    */

                    // h.gameObject.GetComponent<ApplyForces2D>().PuolitaArvot();

                    h.ignorekulmalimit = true;

                }

            }

        }

    }

    private IEnumerator ExplodeSequence()
    {
        yield return StartCoroutine(ExplodeRoutine());

        // Start recording all piece motion
        isRecording = true;
        foreach (var p in pieces)
        {
            var rec = p.GetComponent<MeshPieceRecorder>();
            if (rec != null)
                rec.StartRecording();
        }

        // Start fade-out for all pieces
        foreach (var p in pieces)
        {
            var rec = p.GetComponent<MeshPieceRecorder>();
            if (rec != null)
                rec.StartFadeOut(fadeOutDuration);
        }

        yield return new WaitForSeconds(recordDuration);
        isRecording = false;

        foreach (var p in pieces)
        {
            var rec = p.GetComponent<MeshPieceRecorder>();
            if (rec != null)
                rec.StopRecording();
        }



        yield return new WaitForSeconds(implodeWait);

        yield return StartCoroutine(ImplodeAll());


        yield return new WaitForSeconds(0.1f);
        yield return StartCoroutine(Alpha());

        EnabloiSpriteSkintaas();

    }

    private void EnabloiSpriteSkintaas()
    {
        sr.enabled = true;

        foreach (var c in GetComponentsInChildren<Collider2D>())
            c.enabled = true;

        foreach (var rb in GetComponentsInChildren<Rigidbody2D>())
        {
            if (!rb.gameObject.name.Contains("Haulikko"))
            {
                rb.simulated = true;
            }
            else
            {
                rb.constraints = RigidbodyConstraints2D.None;
                //  rb.gravityScale = 0.0f;
            }
        }



        //todoo arvot talteen ja
        foreach (HaulikkoPiippuController h in haulikot)
        {
            /*
            h.gameObject.GetComponent<ApplyForces2D>().boostedSpeed = 5;
            h.gameObject.GetComponent<ApplyForces2D>().force = 0.5f;
            h.gameObject.GetComponent<ApplyForces2D>().maxAngularSpeed = 2.5f;
            */

            // h.gameObject.GetComponent<ApplyForces2D>().PalautaArvot();

            h.ignorekulmalimit = false;

        }


    }



    private IEnumerator Alpha()
    {
        if (sr == null)
            yield break;

        SpriteRenderer k = sr;

        // Store original color and alpha
        Color baseColor = materiaali.color;
        float originalAlpha = baseColor.a;

        float blinkDuration = 1.5f; // total blink time
        float blinkSpeed = 5f;     // higher = faster blinking
        float elapsed = 0f;

        while (elapsed < blinkDuration)
        {
            // PingPong alpha smoothly between 0 and originalAlpha
            float alpha = Mathf.PingPong(Time.time * blinkSpeed, originalAlpha);
            materiaali.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure sprite returns to its original visibility
        materiaali.color = new Color(baseColor.r, baseColor.g, baseColor.b, originalAlpha);
    }




    private IEnumerator ExplodeRoutine()
    {
        Sprite sprite = sr.sprite;
        if (!spriteSkin.HasCurrentDeformedVertices())
        {
            Debug.LogWarning("No deformed vertex data from SpriteSkin.");
            yield break;
        }

        Vector3[] verts = spriteSkin.GetDeformedVertexPositionData().ToArray();
        ushort[] uTris = sprite.triangles;
        int[] tris = uTris.Select(x => (int)x).ToArray();
        Vector2[] uvs = sprite.uv;

        Bounds bounds = sprite.bounds;
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;
        float stepX = (max.x - min.x) / cols;
        float stepY = (max.y - min.y) / rows;

        List<int>[] triGroups = new List<int>[rows * cols];
        for (int i = 0; i < triGroups.Length; i++) triGroups[i] = new List<int>();

        for (int i = 0; i < tris.Length; i += 3)
        {
            Vector3 avg = (verts[tris[i]] + verts[tris[i + 1]] + verts[tris[i + 2]]) / 3f;
            int col = Mathf.Clamp(Mathf.FloorToInt((avg.x - min.x) / stepX), 0, cols - 1);
            int row = Mathf.Clamp(Mathf.FloorToInt((avg.y - min.y) / stepY), 0, rows - 1);
            triGroups[row * cols + col].AddRange(new[] { tris[i], tris[i + 1], tris[i + 2] });
        }

        foreach (var group in triGroups)
        {
            if (group.Count == 0) continue;
            CreatePiece(group, verts, uvs);
        }

        yield return null;
    }

    private void CreatePiece(List<int> group, Vector3[] verts, Vector2[] uvs)
    {
        List<Vector3> newVerts = new List<Vector3>();
        List<Vector2> newUVs = new List<Vector2>();
        List<int> newTris = new List<int>();
        Dictionary<int, int> map = new Dictionary<int, int>();

        foreach (int tri in group)
        {
            if (!map.TryGetValue(tri, out int idx))
            {
                idx = newVerts.Count;
                map[tri] = idx;
                newVerts.Add(verts[tri]);
                newUVs.Add(uvs[tri]);
            }
            newTris.Add(map[tri]);
        }

        GameObject piece = new GameObject("MeshPiece");
        piece.layer = LayerMask.NameToLayer("AlusammusLayer");
        // SetLayerRecursively(piece, pieceLayer);
        piece.transform.position = spriteSkin.transform.position;
        piece.transform.rotation = spriteSkin.transform.rotation;
        piece.transform.localScale = spriteSkin.transform.lossyScale;



        Mesh mesh = new Mesh();
        mesh.SetVertices(newVerts);
        mesh.SetTriangles(newTris, 0);
        mesh.SetUVs(0, newUVs);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        MeshFilter mf = piece.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        MeshRenderer mr = piece.AddComponent<MeshRenderer>();
        mr.material = meshMaterial != null ? meshMaterial : sr.sharedMaterial;

        PolygonCollider2D col = piece.AddComponent<PolygonCollider2D>();
        col.pathCount = newTris.Count / 3;
        for (int t = 0; t < newTris.Count; t += 3)
        {
            Vector2[] path = new Vector2[3];
            for (int v = 0; v < 3; v++)
                path[v] = newVerts[newTris[t + v]];
            col.SetPath(t / 3, path);
        }

        Rigidbody2D rb = piece.AddComponent<Rigidbody2D>();
        rb.gravityScale = piecerigidbodygravityscale;
        Vector2 dir = (Random.insideUnitCircle + Vector2.up * 0.3f).normalized;
        rb.AddForce(dir * explodeForce, ForceMode2D.Impulse);
        rb.angularVelocity = Random.Range(-maxAngularVelocity, maxAngularVelocity);


        foreach (HaulikkoPiippuController h in haulikot)
        {
            IgnoraaCollisiotVihollistenValillaALakasittelevihollinen(h.gameObject, piece);

        }
        piece.AddComponent<MeshPieceRecorder>(); // record motion and handle fade

        pieces.Add(piece);
    }

    
    private IEnumerator ImplodeAll()
    {
        foreach (var p in pieces)
        {
            if (p == null) continue;
            var rb = p.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.simulated = false;
            }
        }

        List<Coroutine> running = new List<Coroutine>();
        foreach (var p in pieces)
        {
            if (p == null) continue;
            var rec = p.GetComponent<MeshPieceRecorder>();
            if (rec != null)
                running.Add(StartCoroutine(rec.PlayReverseWithFade(implodeSpeed)));
        }

        foreach (var c in running)
            yield return c;

        foreach (var p in pieces)
            if (p != null) Destroy(p);
        pieces.Clear();

        sr.enabled = true;



    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }
}

// ----------------------------------------------------------------
// Records motion, handles fade-in and fade-out for each piece
// ----------------------------------------------------------------
public class MeshPieceRecorder : MonoBehaviour
{
    private List<Vector3> positions = new List<Vector3>();
    private List<Quaternion> rotations = new List<Quaternion>();
    private bool recording = false;
    private MeshRenderer mr;

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
    }

    public void StartRecording()
    {
        positions.Clear();
        rotations.Clear();
        recording = true;
        StartCoroutine(RecordRoutine());
    }

    public void StopRecording()
    {
        recording = false;
    }

    private IEnumerator RecordRoutine()
    {
        while (recording)
        {
            positions.Add(transform.position);
            rotations.Add(transform.rotation);
            yield return null;
        }
    }

    public void StartFadeOut(float duration)
    {
        if (mr != null)
            StartCoroutine(FadeOutRoutine(duration));
    }

    private IEnumerator FadeOutRoutine(float duration)
    {
        if (mr == null) yield break;

        Color c = mr.material.color;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1f, fadenminimi, t / duration);

            Debug.Log("arvo=" + c.a);
            mr.material.color = c;
            yield return null;
        }

        c.a = fadenminimi;
        mr.material.color = c;
    }

    private float fadenminimi = 0.3f;

    public IEnumerator PlayReverseWithFade(float speed = 30f)
    {
        if (mr == null || positions == null || positions.Count < 2)
            yield break;

        // Cache material (avoid creating new instances repeatedly)
        Material mat = mr.material;

        // Start faded out
        Color c = mat.color;
        c.a = fadenminimi;
        mat.color = c;

        int lastIndex = positions.Count - 1;
        float duration = positions.Count / speed; // total playback time based on speed

        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;       // normalized time (0 → 1)
            float reverseT = 1f - t;            // reverse playback progress

            // Map normalized time to index range
            float indexF = reverseT * (positions.Count - 1);
            int indexA = Mathf.FloorToInt(indexF);
            int indexB = Mathf.Min(indexA + 1, lastIndex);
            float lerpT = indexF - indexA;

            // Interpolate position & rotation smoothly
            transform.position = Vector3.Lerp(positions[indexA], positions[indexB], lerpT);
            transform.rotation = Quaternion.Slerp(rotations[indexA], rotations[indexB], lerpT);

            // Fade in (alpha increases from fadenminimi → 1)
            c.a = Mathf.Lerp(fadenminimi, 1f, t);
            mat.color = c;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure final state (fully visible and at the first recorded position)
        transform.position = positions[0];
        transform.rotation = rotations[0];
        c.a = 1f;
        mat.color = c;
    }

}
