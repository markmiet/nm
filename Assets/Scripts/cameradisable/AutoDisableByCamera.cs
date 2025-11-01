using UnityEngine;
using System.Collections.Generic;

public class AutoDisableByCamera : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Multiplier for how far beyond the camera view to disable (1.0 = exactly at edge, 1.2 = 20% beyond).")]
    public float disableMultiplier = 1.2f;
    [Tooltip("Multiplier for how close before re-enabling (usually smaller to prevent flicker).")]
    public float enableMultiplier = 1.0f;
   // public bool useFrustumCheck = true;
    public float checkInterval = 0.5f;

    private Camera mainCam;
    private bool isActive = true;
    private float nextCheckTime = 0f;

    // Cache renderers to avoid GC allocations every frame
    private List<Renderer> cachedRenderers = new List<Renderer>();
    public bool useOnlySpriteRenderers = false;
    public bool debugBreakPoint = false;

    private void Start()
    {
        mainCam = Camera.main;
        CacheRenderers();
    }

    private void CacheRenderers()
    {
        cachedRenderers.Clear();
        foreach (var r in GetComponentsInChildren<Renderer>())
        {

            if (useOnlySpriteRenderers)
            {
                if (r is SpriteRenderer && r.enabled)
                {
                    cachedRenderers.Add(r);
                }
                continue;
            }
            // Skip particle renderers entirely
            if (r is ParticleSystemRenderer)
                continue;

            if (r.enabled)
                cachedRenderers.Add(r);
        }
    }

    private void Update()
    {
        if (Time.time < nextCheckTime) return;
        nextCheckTime = Time.time + checkInterval;
        if (debugBreakPoint)
        {
            Debug.Log(gameObject.name);
        }

        if (mainCam == null) mainCam = Camera.main;
        if (mainCam == null) return;

        // --- Calculate dynamic distances ---
        float disableDist = GetCameraViewRadius() * disableMultiplier;
        float enableDist = GetCameraViewRadius() * enableMultiplier;

        // --- Compute check position based on visible renderers ---
        Vector3 checkPos3D = GetRendererCenter();

        //float dist = Vector2.Distance(checkPos, mainCam.transform.position);

        Vector2 checkPos = new Vector2(checkPos3D.x, checkPos3D.y);
        Vector2 camPos = new Vector2(mainCam.transform.position.x, mainCam.transform.position.y);

        // Measure pure 2D distance
        float dist = Vector2.Distance(checkPos, camPos);

        //  bool inFrustum = !useFrustumCheck || IsInCameraView(checkPos);

        // --- Disable logic ---
        if (isActive && ( /*!inFrustum ||*/ dist > disableDist))
        {
            gameObject.SetActive(false);
            isActive = false;
            AutoDisableManager.RegisterDisabledObject(this);
        }
    }

    public void TryEnable(Camera cam)
    {
        if (cam == null) return;

        float enableDist = GetCameraViewRadius(cam) * enableMultiplier;
        Vector3 checkPos3d = GetRendererCenter();
        Vector3 checkposilman = CalculateCenterOfSpriteChildren();

        Vector2 checkPos = new Vector2(checkPos3d.x, checkPos3d.y);

        float dist = Vector2.Distance(checkPos, cam.transform.position);

        if (dist < enableDist)
        {
            gameObject.SetActive(true);
            isActive = true;
        }
    }

    public bool OnkoKamerassa(Camera cam)
    {
        if (cam == null) return false;

        float enableDist = GetCameraViewRadius(cam) * enableMultiplier;
        Vector3 checkPos3d = GetRendererCenter();
        Vector3 checkposilman = CalculateCenterOfSpriteChildren();

        Vector2 checkPos = new Vector2(checkPos3d.x, checkPos3d.y);

        float dist = Vector2.Distance(checkPos, cam.transform.position);

        if (dist < enableDist)
        {
            return true;
        }
        return false;
    }
    

    


    // --- Helper: Finds the average or bounding box center of all renderers ---
    private Vector3 GetRendererCenterfddf()
    {
        if (cachedRenderers.Count == 0)
        {
            CacheRenderers();
            if (cachedRenderers.Count == 0)
                return transform.position; // fallback
        }
        if (cachedRenderers[0] == null)
        {
            Debug.Log("nulli nimi="+gameObject.name);
            return transform.position;
        }
        Bounds b = cachedRenderers[0].bounds;
        foreach (var r in cachedRenderers)
        {
            if (r == null || !r.enabled) continue;
            b.Encapsulate(r.bounds);
        }

        return b.center;
    }


    public Vector3 CalculateCenterOfSpriteChildren()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        // The above line gets all SpriteRenderers in the hierarchy, including the parent if it has one.
        // If you don't want to include the parent, you can use the following line:
        // List<SpriteRenderer> childrenRenderers = new List<SpriteRenderer>();
        // foreach (Transform child in transform)
        // {
        //     SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
        //     if (sr != null)
        //     {
        //         childrenRenderers.Add(sr);
        //     }
        // }
        // spriteRenderers = childrenRenderers.ToArray();

        // Calculate the average center
        Vector3 totalCenter = Vector3.zero;
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            totalCenter += sr.bounds.center;
        }

        if (spriteRenderers.Length == 0)
        {
            return Vector3.zero; // Or handle error appropriately
        }

        Vector3 averageCenter = totalCenter / spriteRenderers.Length;
        return averageCenter;
    }

    public Vector3 GetRendererCenter()
    {

        if (cachedRenderers.Count == 0)
        {
            CacheRenderers();
            if (cachedRenderers.Count == 0)
                return transform.position; // fallback
        }
        /*
        if (cachedRenderers[0] == null)
        {
            Debug.Log("nulli nimi=" + gameObject.name);
            return transform.position;
        }
        */

        //SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        // The above line gets all SpriteRenderers in the hierarchy, including the parent if it has one.
        // If you don't want to include the parent, you can use the following line:
        // List<SpriteRenderer> childrenRenderers = new List<SpriteRenderer>();
        // foreach (Transform child in transform)
        // {
        //     SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
        //     if (sr != null)
        //     {
        //         childrenRenderers.Add(sr);
        //     }
        // }
        // spriteRenderers = childrenRenderers.ToArray();

        // Calculate the average center
        Vector3 totalCenter = Vector3.zero;
        int maara = 0;
        foreach (Renderer sr in cachedRenderers)
        {
            if (sr!=null)
            {
                totalCenter += sr.bounds.center;
                maara++;
            }
           
        }
        if (maara==0)
        {
            return transform.position;
        }

        // if (spriteRenderers.Length == 0)
        // {
        //     return Vector3.zero; // Or handle error appropriately
        //}

        Vector3 averageCenter = totalCenter / maara;
        return averageCenter;
    }


    public Vector3 GetRendererCenterworking()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        // The above line gets all SpriteRenderers in the hierarchy, including the parent if it has one.
        // If you don't want to include the parent, you can use the following line:
        // List<SpriteRenderer> childrenRenderers = new List<SpriteRenderer>();
        // foreach (Transform child in transform)
        // {
        //     SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
        //     if (sr != null)
        //     {
        //         childrenRenderers.Add(sr);
        //     }
        // }
        // spriteRenderers = childrenRenderers.ToArray();

        // Calculate the average center
        Vector3 totalCenter = Vector3.zero;
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            totalCenter += sr.bounds.center;
        }

        if (spriteRenderers.Length == 0)
        {
            return Vector3.zero; // Or handle error appropriately
        }

        Vector3 averageCenter = totalCenter / spriteRenderers.Length;
        return averageCenter;
    }

    /*
    private Vector3 GetRendererCenter()
    {
        if (cachedRenderers.Count == 0)
        {
            CacheRenderers();
            if (cachedRenderers.Count == 0)
                return transform.position; // fallback
        }

        if (cachedRenderers[0] == null)
        {
            Debug.Log("nulli nimi=" + gameObject.name);
            return transform.position;
        }

        // Start with first renderer's bounds (always in WORLD space)
        Bounds b = cachedRenderers[0].bounds;

        // Combine all enabled renderers' bounds
        foreach (var r in cachedRenderers)
        {
            if (r == null || !r.enabled) continue;
            b.Encapsulate(r.bounds);
        }

            return b.center;
    }


    
    private Vector3 GetRendererCenterdsdsds()
    {
        // Collect SpriteRenderers if not cached
        if (cachedRenderers == null || cachedRenderers.Count == 0)
        {
            CacheRenderers();
            if (cachedRenderers == null || cachedRenderers.Count == 0)
                return transform.position; // fallback
        }

        // Filter only valid, enabled renderers
        Renderer firstValid = null;
        foreach (var r in cachedRenderers)
        {
            if (r != null && r.enabled)
            {
                firstValid = r;
                break;
            }
        }

        if (firstValid == null)
        {
            Debug.LogWarning($"[{name}] No active SpriteRenderers found.");
            return transform.position;
        }

        // Compute combined bounds
        Bounds b = firstValid.bounds;
        foreach (var r in cachedRenderers)
        {
            if (r == null || !r.enabled) continue;
            b.Encapsulate(r.bounds);
        }

        return b.center;
    }
    */
    /*

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Skip if we don't have renderers
        if (cachedRenderers == null || cachedRenderers.Count == 0)
            CacheRenderers();

        if (cachedRenderers == null || cachedRenderers.Count == 0)
            return;

        // Compute full bounds
        Bounds combined = new Bounds(transform.position, Vector3.zero);
        bool found = false;

        foreach (var r in cachedRenderers)
        {
            if (r == null || !r.enabled) continue;

            if (!found)
            {
                combined = r.bounds;
                found = true;
            }
            else
            {
                combined.Encapsulate(r.bounds);
            }
        }

        if (!found)
            return;

        // Draw wire box
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(combined.center, combined.size);

        // Draw center point
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(combined.center, 0.05f);

        // Draw label
        UnityEditor.Handles.Label(combined.center + Vector3.up * 0.2f, $"{name}\nCenter: {combined.center}");
    }
#endif
    */



    // --- Helper: dynamic camera view radius (based on size + aspect ratio) ---
    private float GetCameraViewRadius(Camera cam = null)
    {
        if (cam == null) cam = mainCam;
        if (cam == null) return 10f;

        if (cam.orthographic)
        {
            float height = cam.orthographicSize * 2f;
            float width = height * cam.aspect;
            return Mathf.Sqrt(width * width + height * height) * 0.5f; // half-diagonal
        }
        else
        {
            // fallback for perspective
            return 10f;
        }
    }

    private bool IsInCameraView(Vector3 worldPos)
    {
        Vector3 viewPos = mainCam.WorldToViewportPoint(worldPos);
        return (viewPos.x > -0.1f && viewPos.x < 1.1f && viewPos.y > -0.1f && viewPos.y < 1.1f && viewPos.z > 0);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (mainCam == null) mainCam = Camera.main;

        Vector3 center = Application.isPlaying ? GetRendererCenter() : transform.position;

        Gizmos.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(center, Vector3.forward, GetCameraViewRadius() * disableMultiplier);

        Gizmos.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(center, Vector3.forward, GetCameraViewRadius() * enableMultiplier);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(center, Vector3.one * 0.1f);
    }
#endif
}
