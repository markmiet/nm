using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NappulaScripti : MonoBehaviour
{
    // Start is called before the first frame update
    public bool selected = false;
    public bool used = false;
    public string text;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            if (used)
            {
                //keltainen väri, ei tekstiä
                // text = "";
            }
            else
            {
                //keltainen väri tekstillä
            }
        }
        else
        {
            if (used)
            {
                //sininen väri ei tekstiä
            }
            else
            {
                //sininen väri tekstillä
            }

        }

    }

    void FixedUpdate()
    {

    }
}