using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class ParticleSizeChanger : MonoBehaviour
{
    public float newSize = 1.0f; // Set this to your desired size

    void Start()
    {
        ChangeParticleSizes(transform, newSize);
    }


    void ChangeParticleSizes(Transform parent, float size)
    {
        foreach (Transform child in parent)
        {
            ParticleSystem ps = child.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                var main = ps.main;
                main.startSize = size; // Change start size
            }
            // Recursively check for children
            ChangeParticleSizes(child, size);
        }
    }
}
