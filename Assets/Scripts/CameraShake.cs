using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;

    private void Awake()
    {
        Instance = this;
       
    }

    public void ShakeYourAssBaby()
    {
        StartCoroutine(CameraShake.Instance.Shake(1, 2f,0.5f));
    }


    public IEnumerator Shake(float duration, float magnitude, float burstStrength = 0.5f)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        // --- 1) Burst-efekti heti alussa ---
        Vector3 burstOffset = new Vector3(
            Random.Range(-1f, 1f) * burstStrength,
            Random.Range(-1f, 1f) * burstStrength,
            0f
        );
        transform.localPosition = originalPos + burstOffset;

        // Varmistetaan, ett‰ burst ei j‰‰ p‰‰lle
        yield return new WaitForSeconds(0.05f);

        // --- 2) Perlin noise shake ---
        float randomStartX = Random.Range(0f, 100f);
        float randomStartY = Random.Range(0f, 100f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percentComplete = elapsed / duration;

            // Mit‰ pidemm‰lle menn‰‰n, sit‰ heikompi shake
            float damper = 1f - Mathf.Clamp01(percentComplete);

            float x = (Mathf.PerlinNoise(randomStartX + elapsed * 10f, 0f) - 0.5f) * 2f;
            float y = (Mathf.PerlinNoise(0f, randomStartY + elapsed * 10f) - 0.5f) * 2f;

            x *= magnitude * damper;
            y *= magnitude * damper;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    public IEnumerator Shakessssssssssssss(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        // Satunnaiset siemenet, jotta jokainen shake on erilainen
        float randomStartX = Random.Range(0f, 100f);
        float randomStartY = Random.Range(0f, 100f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percentComplete = elapsed / duration;

            // Mit‰ pidemm‰lle shake etenee, sit‰ pienempi voimakkuus
            float damper = 1f - Mathf.Clamp01(percentComplete);

            // K‰ytet‰‰n Perlin Noisea x ja y siirtymiin
            float x = (Mathf.PerlinNoise(randomStartX + elapsed * 10f, 0f) - 0.5f) * 2f;
            float y = (Mathf.PerlinNoise(0f, randomStartY + elapsed * 10f) - 0.5f) * 2f;

            x *= magnitude * damper;
            y *= magnitude * damper;

            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);

            yield return null;
        }

        // Palautetaan kamera takaisin alkuper‰iseen paikkaan
        transform.localPosition = originalPos;
    }
}
