using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaArvonVaihtelijaController : MonoBehaviour
{
    public float minAlpha = 0.2f;
    public float maxAlpha = 1f;
    public float speed = 1f;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, Mathf.PingPong(Time.time * speed, 1f));
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
