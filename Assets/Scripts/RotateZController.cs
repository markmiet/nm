using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateZController : BaseController
{

    public float speed = 90f; // astetta sekunnissa

    void Update()
    {
        if (!OnkoOkToimiaUusi(gameObject))
        {
            return;
        }

        transform.Rotate(0f, 0f, speed * Time.deltaTime);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
