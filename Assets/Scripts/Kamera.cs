﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamera : MonoBehaviour
{
	float y = 0f;

	public GameObject alus;
    public float skrollimaara;
    // Start is called before the first frame update
    void Start()
    {
		y = alus.transform.position.y; 
    }

    // Update is called once per frame
    void Update()
    {
        //   gameObject.transform.position.Set(alus.transform.position.x, alus.transform.position.y, gameObject.transform.position.z);


        Vector3 skrolli = new Vector3(skrollimaara, 0f, 0f) * Time.deltaTime;

        transform.position += skrolli;
        if (alus!=null)
        {
            alus.transform.position += skrolli;
            
        }
       

    }
}
