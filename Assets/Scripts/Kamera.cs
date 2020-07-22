using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamera : MonoBehaviour
{
	float y = 0f;

	public GameObject alus;
    // Start is called before the first frame update
    void Start()
    {
		y = alus.transform.position.y; 
    }

    // Update is called once per frame
    void Update()
    {
       // gameObject.transform.position

    }
}
