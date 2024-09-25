using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TahdetController : MonoBehaviour
{
    public float scrollSpeed = 0.1f;
    private Vector2 offset;
    private Material material;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        offset = Vector2.zero;

    }

    // Update is called once per frame
    void Update()
    {
        offset.x += scrollSpeed * Time.deltaTime;
        material.mainTextureOffset = offset;
    }
}
