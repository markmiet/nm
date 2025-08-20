using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeSlice : MonoBehaviour
{
    private SpriteRenderer sr;
    private float aliveTime;
    private float fadeDuration;
    private GameObject explosion;

    public void Init(SpriteRenderer spriteRenderer, float alive, float fade,GameObject p_explosion)
    {
        sr = spriteRenderer;
        aliveTime = alive;
        fadeDuration = fade;
        explosion = p_explosion;
        StartCoroutine(FadeAndDestroy());
    }
    //tänne vois tehdä räjäytyksetkin....
    private IEnumerator FadeAndDestroy()
    {
        yield return new WaitForSeconds(Mathf.Max(0f, aliveTime - fadeDuration));

        Color originalColor = sr.color;
        float elapsed = 0f;


        if (explosion == null)
        {
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
                sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
        }
        else
        {
            GameObject instannsI = Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(instannsI, fadeDuration);
        }
        Destroy(gameObject); // Destroy the entire slice GameObject
        
    }
}
