using UnityEngine;

public class ParticleAttractor : BaseController
{
    public ParticleSystem particleSystem;  // Reference to your particle system
    private Transform target;               // The target object

    private ParticleSystem.Particle[] particles;
    public float speed = 2f; // Adjust speed here

    public void Start()
    {
        target = PalautaAlus().transform;
    }

    void LateUpdate()
    {

            if (particleSystem == null || target == null) return;

            // Get all particles
            int particleCount = particleSystem.particleCount;
            if (particles == null || particles.Length < particleCount)
            {
                particles = new ParticleSystem.Particle[particleCount];
            }

            particleSystem.GetParticles(particles);

            // Move particles toward the target
            for (int i = 0; i < particleCount; i++)
            {
                Vector3 direction = (target.position - particles[i].position).normalized;

                particles[i].velocity = direction * speed;
            }

            // Apply modified particles back to the system
            particleSystem.SetParticles(particles, particleCount);
        

    }
}
