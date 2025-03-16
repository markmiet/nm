using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal; // Required for Light2D
public class AlusLightController : MonoBehaviour
{
    public float lightIntensity = 1.5f;
    public float explosionRadius = 2.0f;
    public float originalRadius = 0.5f;
    public float originalIntensity = 0.8f; // Keep some base intensity
    public float lengthOfExplosionLights = 0.5f;

    private float valonintensiteettikunsuojaaeiole = 1.5f;
    private float valonintensiteetinvahennyskunosaumatuli = 0.1f;


    private Light2D light2D;
    void Start()
    {
        light2D = GetComponent<Light2D>();
        light2D.enabled = true; // Ensure light is enabled at start
        light2D.intensity = originalIntensity; // Set initial intensity
        light2D.pointLightOuterRadius = originalRadius; // Set initial radius
    }

    public void SetExplosionLights()
    {
        originalIntensity -= valonintensiteetinvahennyskunosaumatuli;
        //StartCoroutine(ExplosionLightEffect());
        light2D.intensity = originalIntensity;

    }

    private IEnumerator ExplosionLightEffect()
    {
        float elapsedTime = 0f;

        while (elapsedTime < lengthOfExplosionLights)
        {
            float t = elapsedTime / lengthOfExplosionLights; // Normalize time

            // Expand light and increase intensity
            light2D.intensity = Mathf.Lerp(originalIntensity, lightIntensity, t);
            light2D.pointLightOuterRadius = Mathf.Lerp(originalRadius, explosionRadius, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f; // Reset time for fade-out

        while (elapsedTime < lengthOfExplosionLights)
        {
            float t = elapsedTime / lengthOfExplosionLights;

            // Fade back to original state
            light2D.intensity = Mathf.Lerp(lightIntensity, originalIntensity, t);
            light2D.pointLightOuterRadius = Mathf.Lerp(explosionRadius, originalRadius, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure final values are set correctly
        light2D.intensity = originalIntensity;
        light2D.pointLightOuterRadius = originalRadius;
    }
}
