using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AdvancedPolygonColliderManager : MonoBehaviour
{
    //ei nämä vaaaan toimiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiiii
    private int width;
    private int height;
    private byte[] solids;
    private int solidsLength;

    // ========================
    // PUBLIC ENTRYPOINT: vain GameObject annetaan
    // ========================
    public void StartRebuild(GameObject go, int alphaTolerance = 10)
    {
        StartCoroutine(RebuildPipelineCo(go, alphaTolerance));
    }

    // ========================
    // Pipeline: detect -> rebuild collider
    // ========================
    private IEnumerator RebuildPipelineCo(GameObject go, int alphaTolerance)
    {
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        if (sr == null || sr.sprite == null)
        {
            Debug.LogError("StartRebuild: GameObjectilla ei SpriteRendereriä tai Spriteä!");
            yield break;
        }

        Texture2D tex = sr.sprite.texture;
        Color32[] pixels = tex.GetPixels32(); // turvallinen kopio

        PolygonCollider2D poly = go.GetComponent<PolygonCollider2D>();
        if (poly == null)
            poly = go.AddComponent<PolygonCollider2D>();

        List<Vertices> detectedPolygons = new List<Vertices>();
        yield return StartCoroutine(DetectVertices2Co(pixels, alphaTolerance, detectedPolygons));

        if (detectedPolygons.Count == 0)
        {
            Debug.LogError("RebuildPipelineCo: Ei löydetty verteksipolygoneja.");
            yield break;
        }

        yield return StartCoroutine(RebuildColliderCo(detectedPolygons, poly));
    }

    // ========================
    // Rebuild collider coroutine
    // ========================
    private IEnumerator RebuildColliderCo(List<Vertices> detectedPolygons, PolygonCollider2D poly)
    {
        poly.pathCount = 0;
        yield return null;

        poly.pathCount = detectedPolygons.Count;
        yield return null;

        for (int pathIndex = 0; pathIndex < detectedPolygons.Count; pathIndex++)
        {
            Vertices polygon = detectedPolygons[pathIndex];
            Vector2[] path = new Vector2[polygon.Count];

            for (int i = 0; i < polygon.Count; i++)
            {
                path[i] = polygon[i];
                if (i % 100 == 0)
                    yield return null;
            }

            poly.SetPath(pathIndex, path);

            if (pathIndex % 5 == 0)
                yield return null;
        }

        Debug.Log($"PolygonCollider2D päivitetty {detectedPolygons.Count} pathilla");
    }

    // ========================
    // DetectVertices2 coroutine (Color32[] versio)
    // ========================
    private IEnumerator DetectVertices2Co(Color32[] colors, int alphaTolerance, List<Vertices> ret)
    {
        width = Mathf.RoundToInt(Mathf.Sqrt(colors.Length));
        height = colors.Length / width;

        solids = new byte[colors.Length];

        int n, s;
        for (int i = 0; i < solids.Length; i++)
        {
            n = alphaTolerance - colors[i].a;
            s = (int)((n & 0x80000000) >> 31) - 1; // sign multiplier
            n = n * s * s;
            solids[i] = (byte)n;

            if (i % 1000 == 0)
                yield return null;
        }

        List<Vertices> detectedPolygons = new List<Vertices>();
        Vector2? holeEntrance = null;
        Vector2? polygonEntrance = null;
        List<Vector2> blackList = new List<Vector2>();

        bool searchOn;
        int laskuri = 0;

        do
        {
            laskuri++;
            if (laskuri >= 500)
            {
                laskuri = 0;
                yield return null;
            }

            Vertices polygon;
            if (detectedPolygons.Count == 0)
            {
                polygon = new Vertices(CreateSimplePolygon(Vector2.zero, Vector2.zero));
                if (polygon.Count > 2)
                    polygonEntrance = GetTopMostVertex(polygon);
            }
            else if (polygonEntrance.HasValue)
            {
                polygon = new Vertices(CreateSimplePolygon(polygonEntrance.Value, new Vector2(polygonEntrance.Value.x - 1f, polygonEntrance.Value.y)));
            }
            else
                break;

            searchOn = false;

            if (polygon.Count > 2)
            {
                do
                {
                    holeEntrance = SearchHoleEntrance(polygon, holeEntrance);
                    if (holeEntrance.HasValue)
                    {
                        if (!blackList.Contains(holeEntrance.Value))
                        {
                            blackList.Add(holeEntrance.Value);
                            Vertices holePolygon = new Vertices(CreateSimplePolygon(holeEntrance.Value, new Vector2(holeEntrance.Value.x + 1f, holeEntrance.Value.y)));
                            if (holePolygon != null && holePolygon.Count > 2)
                            {
                                holePolygon.Add(holePolygon[0]);
                                int vertex1Index, vertex2Index;
                                if (SplitPolygonEdge(polygon, holeEntrance.Value, out vertex1Index, out vertex2Index))
                                {
                                    polygon.InsertRange(vertex2Index, holePolygon);
                                }
                                break;
                            }
                        }
                        else
                            break;
                    }
                    else
                        break;
                } while (true);

                detectedPolygons.Add(polygon);
            }

            if (polygonEntrance.HasValue &&
                SearchNextHullEntrance(detectedPolygons, polygonEntrance.Value, out polygonEntrance))
            {
                searchOn = true;
            }

        } while (searchOn);

        if (detectedPolygons == null || detectedPolygons.Count == 0)
            throw new Exception("Couldn't detect any vertices.");

        ret.AddRange(detectedPolygons);
        yield return null;
    }

    // ========================
    // HELPERS (Vector2 versio)
    // ========================
    private List<Vector2> CreateSimplePolygon(Vector2 start, Vector2 end)
    {
        return new List<Vector2>()
        {
            new Vector2(start.x, start.y),
            new Vector2(start.x, end.y),
            new Vector2(end.x, end.y),
            new Vector2(end.x, start.y)
        };
    }

    private Vector2? GetTopMostVertex(Vertices polygon)
    {
        Vector2 top = polygon[0];
        foreach (var v in polygon)
            if (v.y > top.y) top = v;
        return top;
    }

    private Vector2? SearchHoleEntrance(Vertices polygon, Vector2? prev)
    {
        return null; // pixel-perfect hole detection placeholder
    }

    private bool SplitPolygonEdge(Vertices polygon, Vector2 holeEntrance, out int vertex1Index, out int vertex2Index)
    {
        vertex1Index = 0;
        vertex2Index = polygon.Count / 2;
        return true;
    }

    private bool SearchNextHullEntrance(List<Vertices> detectedPolygons, Vector2 prev, out Vector2? next)
    {
        next = null;
        return false;
    }
}

// ========================
// Vertices helper class
// ========================
public class Vertices : List<Vector2>
{
    public Vertices() : base() { }
    public Vertices(IEnumerable<Vector2> collection) : base(collection) { }
}
