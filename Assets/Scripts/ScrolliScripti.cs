using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrolliScripti : MonoBehaviour
{


    public float skrollimaara = 0.01f;
	// Start is called before the first frame update
	void Start ()
	{


	}

	// Update is called once per frame
	void Update ()
	{
        //transform.position.x = transform.position.x - skrollimaara;
        float deltaAika = Time.deltaTime;
        float maara = deltaAika * skrollimaara;

		transform.position = new Vector2 (transform.position.x - maara, transform.position.y);


	}
}
