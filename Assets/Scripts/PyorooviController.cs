using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyorooviController : MonoBehaviour
{

    private SpriteRenderer m_SpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    public float rotationSpeed = 30.0f;
    // Update is called once per frame
    void Update()
    {
        if (!OnkoOkLiikkua())
        {
            return;
        }
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Apply the rotation around the Y-axis (you can change the axis as needed)
        transform.Rotate(0, 0, rotationAmount);
    }

    private bool OnkoOkLiikkua()
    {
        return m_SpriteRenderer.isVisible;
    }

}
