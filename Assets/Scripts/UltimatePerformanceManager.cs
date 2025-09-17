using UnityEngine;
using System.Collections.Generic;

public class UltimatePerformanceManager : MonoBehaviour
{
    [Header("FPS Settings")]
    public int pcTargetFPS = 60;
    public bool pcVSync = false;
    public int mobileTargetFPS = 60;
    public bool mobileDynamicFPS = true;
    public int minFPS = 30;
    public int maxFPS = 60;
    public float cpuCheckInterval = 1f;
    public float highCPULoadThreshold = 0.75f;
    public float lowCPULoadThreshold = 0.4f;

    [Header("Visibility Optimization")]
    public Camera mainCamera;
    public float visibilityMargin = 0.5f;
    public List<GameObject> managedObjects = new List<GameObject>();

    private float fpsTimer = 0f;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            Application.targetFrameRate = mobileTargetFPS;
        }
        else
        {
            Application.targetFrameRate = pcTargetFPS;
            if (pcVSync) QualitySettings.vSyncCount = 1;
        }

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        fpsTimer += Time.unscaledDeltaTime;
        if (mobileDynamicFPS && fpsTimer >= cpuCheckInterval)
        {
            AdjustFPS();
            fpsTimer = 0f;
        }

        // Update visibility for managed objects
        if (mainCamera != null)
        {
            foreach (var go in managedObjects)
            {
                if (go == null) continue;
                bool visible = IsVisible(go, mainCamera, visibilityMargin);
                if (go.activeSelf != visible)
                    go.SetActive(visible); // Activate only if visible
            }
        }
    }

    private void AdjustFPS()
    {
        float targetFrameTime = 1f / maxFPS;
        float cpuLoad = Mathf.Clamp01(Time.unscaledDeltaTime / targetFrameTime);

        int currentFPS = Application.targetFrameRate;

        if (cpuLoad > highCPULoadThreshold && currentFPS > minFPS)
        {
            Application.targetFrameRate = Mathf.Max(minFPS, currentFPS - 5);
            Debug.Log($"[FPS Manager] High CPU load ({cpuLoad:F2}), lowering FPS to {Application.targetFrameRate}");
        }
        else if (cpuLoad < lowCPULoadThreshold && currentFPS < maxFPS)
        {
            Application.targetFrameRate = Mathf.Min(maxFPS, currentFPS + 5);
            Debug.Log($"[FPS Manager] Low CPU load ({cpuLoad:F2}), raising FPS to {Application.targetFrameRate}");
        }
    }

    private bool IsVisible(GameObject go, Camera cam, float margin)
    {
        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        if (sr == null) return false;

        Vector3 pos = go.transform.position;
        Vector3 scale = go.transform.localScale;
        Vector2 spriteSize = sr.sprite.bounds.size;
        Vector2 extents = new Vector2(spriteSize.x * 0.5f * scale.x, spriteSize.y * 0.5f * scale.y);

        Vector2[] corners =
        {
            new Vector2(pos.x - extents.x, pos.y - extents.y),
            new Vector2(pos.x - extents.x, pos.y + extents.y),
            new Vector2(pos.x + extents.x, pos.y - extents.y),
            new Vector2(pos.x + extents.x, pos.y + extents.y)
        };

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;
        Vector3 camPos = cam.transform.position;

        foreach (var c in corners)
        {
            if (c.x < camPos.x - halfWidth - margin ||
                c.x > camPos.x + halfWidth + margin ||
                c.y < camPos.y - halfHeight - margin ||
                c.y > camPos.y + halfHeight + margin)
                return false;
        }

        return true;
    }
}
