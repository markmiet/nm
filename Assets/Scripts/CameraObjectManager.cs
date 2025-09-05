using System.Collections.Generic;
using UnityEngine;

public class CameraObjectManager : MonoBehaviour
{
    public float activationMargin = 2f;
    public float destroyMargin = 5f;

    private Camera cam;
    private readonly List<ManagedObject> objects = new();

    private struct ManagedObject
    {
        public GameObject go;
        public Rigidbody2D rb;
        public SpriteRenderer sr;
        public LineRenderer lr;

        public ManagedObject(GameObject obj)
        {
            go = obj;
            rb = obj.GetComponent<Rigidbody2D>();
            sr = obj.GetComponent<SpriteRenderer>();
            lr = obj.GetComponent<LineRenderer>();
        }

        public void SetActive(bool active)
        {
            /*
            if (rb != null) rb.simulated = active;
            if (sr != null) sr.enabled = active;
            if (lr != null) lr.enabled = active;
            */

            go.SetActive(active);
        }
    }

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (cam == null) return;

        // Kamera-alue
        float camHeight = 2f * cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float left = cam.transform.position.x - camWidth / 2f;
        float right = cam.transform.position.x + camWidth / 2f;
        float bottom = cam.transform.position.y - camHeight / 2f;
        float top = cam.transform.position.y + camHeight / 2f;

        // Käydään läpi kaikki objektit
        for (int i = objects.Count - 1; i >= 0; i--)
        {
            ManagedObject mo = objects[i];
            if (mo.go == null)
            {
                objects.RemoveAt(i);
                continue;
            }
            if (mo.go.GetComponent<BaseController>()!=null)
            {
                if (mo.go.GetComponent<BaseController>().IsGoingToBeDestroyed())
                {
                    objects.RemoveAt(i);
                    continue;
                }
            }

            Vector2 pos = mo.go.transform.position;

            // --- Tuhotaan jos tarpeeksi vasemmalla
            if (pos.x < left - destroyMargin)
            {
                if (mo.go.GetComponent<BaseController>() != null)
                {
                    mo.go.GetComponent<BaseController>().BaseDestroy();
                }
                else
                {
                    Destroy(mo.go);
                }
                objects.RemoveAt(i);
                continue;
            }

            // --- Tarkistetaan onko lähellä kameraa
            bool isNear =
                pos.x > left - activationMargin &&
                pos.x < right + activationMargin &&
                pos.y > bottom - activationMargin &&
                pos.y < top + activationMargin;

            mo.SetActive(isNear);
        }
    }

    // --- API objekteille ---
    public void Register(GameObject obj)
    {
        objects.Add(new ManagedObject(obj));
    }

    public void Unregister(GameObject obj)
    {
        objects.RemoveAll(mo => mo.go == obj);
    }
}
