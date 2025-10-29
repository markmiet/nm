using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class UusiPallerotController : BaseController, RegisterHitCallable
{
    public GameObject palleroPrefab;
    public GameObject bonusprefab;
    public int palleromaara = 5;
    public int bonusmaara = 1;
    public float speed = 2f;

    // How many recorded head positions to delay each segment by
    public int positionDelayStep = 35;
    public float sinAmplitude = 0.5f;    // "width" of the sinusoidal movement (vertical amplitude)


    // --- Movement pattern setup ---
    public enum MovementPhase { MoveLeft, MoveRight, MoveDown, MoveUp, Stop, MoveLeftUp, MoveLeftDown, Sin }

    [System.Serializable]
    public struct PhaseStep
    {
        public MovementPhase phase;
        public float duration;
    }

    public PhaseStep[] pattern;

    private int currentIndex = 0;
    private float phaseTimer = 0f;
    private MovementPhase currentPhase;
    private float sinTime = 0f;

    // --- Internal data ---
    private List<Vector3> headPositions = new List<Vector3>();
    private List<GameObject> gos = new List<GameObject>();

    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        // Spawn head and body
        for (int i = 0; i < palleromaara; i++)
        {
            GameObject ins = Instantiate(palleroPrefab, transform.position, Quaternion.identity);
            ins.GetComponent<HitCounter>().registerHitCallable = this;
            gos.Add(ins);
        }

        if (pattern.Length > 0)
            currentPhase = pattern[0].phase;
    }

    void Update()
    {
        if (gos.Count == 0 || pattern.Length == 0) return;

        // --- Phase timing ---
        phaseTimer += Time.deltaTime;
        if (phaseTimer > pattern[currentIndex].duration)
        {
            phaseTimer = 0f;
            currentIndex = (currentIndex + 1) % pattern.Length;
            currentPhase = pattern[currentIndex].phase;
        }

        // --- Move head ---
        Vector3 moveDir = GetMovementDirection(currentPhase);
        GameObject head = gos[0];
        head.transform.position += moveDir * speed * Time.deltaTime;

        // --- Record head position every frame ---
        headPositions.Insert(0, head.transform.position);

        // --- Move the rest of the body ---
        for (int i = 1; i < gos.Count; i++)
        {
            int index = i * positionDelayStep;
            if (index < headPositions.Count)
                gos[i].transform.position = headPositions[index];
        }

        // --- Trim position history to avoid unbounded growth ---
        int maxPositions = palleromaara * positionDelayStep + 1;
        if (headPositions.Count > maxPositions)
            headPositions.RemoveRange(maxPositions, headPositions.Count - maxPositions);
    }

    private Vector3 GetMovementDirection(MovementPhase phase)
    {
        switch (phase)
        {
            case MovementPhase.MoveLeft: return Vector3.left;
            case MovementPhase.MoveRight: return Vector3.right;
            case MovementPhase.MoveDown: return Vector3.down;
            case MovementPhase.MoveUp: return Vector3.up;
            case MovementPhase.Stop: return Vector3.zero;
            case MovementPhase.MoveLeftUp: return (Vector3.left + Vector3.up).normalized;
            case MovementPhase.MoveLeftDown: return (Vector3.left + Vector3.down).normalized;
            case MovementPhase.Sin:
                sinTime += Time.deltaTime * 2f;
                return new Vector3(-1f, Mathf.Sin(sinTime) * sinAmplitude, 0f).normalized;
            default: return Vector3.zero;
        }
    }

    public void AfterRegisterHit(GameObject go)
    {
        if (go.GetComponent<HitCounter>() != null)
        {
            go.GetComponent<HitCounter>().RajaytaSpriteNoDestroy();

        }
        ParticleSystem p = go.GetComponentInChildren<ParticleSystem>();
        if (p != null)
        {

            p.Stop();

        }


        Renderer[] sr = go.GetComponents<Renderer>();
        foreach (Renderer r in sr)
        {
            r.enabled = false;
        }

        Collider2D[] cc = go.
        GetComponents<Collider2D>();
        foreach (Collider2D c in cc)
        {
            c.enabled = false;
        }
        if (!OnkoElossaKetaan())
        {
            if (bonusprefab != null)
            {
                //GameObject bonus = Instantiate(bonusprefab, go.transform.position, Quaternion.identity);
                TeeBonusUusiKaytaTata(go.transform.position, bonusprefab, bonusmaara);
            }

            foreach (GameObject g in gos)
            {
                Destroy(g);
                Destroy(gameObject);
            }
        }

    }

    private bool OnkoElossaKetaan()
    {
        foreach (GameObject g in gos)
        {
            if (g.GetComponent<Renderer>().enabled)
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR

        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 10;
        labelStyle.normal.textColor = Color.red;



        Handles.Label(transform.position + Vector3.up * 0.2f, $"{gameObject.name}", labelStyle);


        //uusi = new Vector2(transform.position.x, transform.position.y);
        //tulos = !onkoTagiaBoxissaTransformPositionArvoonLisataanBoxLocation("vihollinen", boxsizealhaalla, uusi, layerMask);

#endif
    }


}
