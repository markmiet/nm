﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrolliScripti : MonoBehaviour
{


	public float skrollimaara = 0.5f;
	// Start is called before the first frame update
	void Start ()
	{


	}

	// Update is called once per frame
	void Update ()
	{
		//transform.position.x = transform.position.x - skrollimaara;

		transform.position = new Vector2 (transform.position.x - 0.01f, 0);

	}
}