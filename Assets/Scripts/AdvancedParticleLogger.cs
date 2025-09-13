using UnityEngine;

public class AdvancedParticleLogger : MonoBehaviour
{
    [Tooltip("Check all children recursively for Particle Systems")]
    public bool checkChildren = true;

    [Tooltip("Only log once at Start")]
    public bool logOnce = true;

    void Start()
    {
        LogParticles();
    }

    void Update()
    {
        if (!logOnce)
            LogParticles();
    }

    void LogParticles()
    {
        ParticleSystem[] particleSystems;

        if (checkChildren)
            particleSystems = GetComponentsInChildren<ParticleSystem>();
        else
            particleSystems = GetComponents<ParticleSystem>();

        foreach (var ps in particleSystems)
        {
            if (ps == null)
            {
                Debug.LogWarning("[AdvancedParticleLogger] Null ParticleSystem reference found!");
                continue;
            }

            var renderer = ps.GetComponent<ParticleSystemRenderer>();

            // Material info
            string matName = "None";
            string shaderName = "None";
            bool shaderMissing = false;
            bool textureMissing = false;

            if (renderer != null && renderer.sharedMaterial != null)
            {
                var mat = renderer.sharedMaterial;
                matName = mat.name;
                shaderName = mat.shader != null ? mat.shader.name : "None";

                // Check shader vs mobile
                if (!mat.shader.isSupported)
                    shaderMissing = true;

                // Check main texture
                if (mat.HasProperty("_MainTex") && mat.mainTexture == null)
                    textureMissing = true;
            }

            // Log basic info
            Debug.Log($"[AdvancedParticleLogger] ParticleSystem: {ps.name}, Active Particles: {ps.particleCount}, Material: {matName}, Shader: {shaderName}");

            // Warnings
            if (renderer == null)
                Debug.LogWarning($"[AdvancedParticleLogger] ParticleSystem {ps.name} has no renderer!");
            else if (renderer.sharedMaterial == null)
                Debug.LogWarning($"[AdvancedParticleLogger] ParticleSystem {ps.name} has no material assigned!");
            else
            {
                if (shaderMissing)
                    Debug.LogWarning($"[AdvancedParticleLogger] Shader {shaderName} is not supported on this device!");

                if (textureMissing)
                    Debug.LogWarning($"[AdvancedParticleLogger] Material {matName} has no main texture assigned!");
            }

            if (ps.particleCount == 0)
                Debug.LogWarning($"[AdvancedParticleLogger] ParticleSystem {ps.name} has zero active particles. Check emission settings.");
        }
    }
}
