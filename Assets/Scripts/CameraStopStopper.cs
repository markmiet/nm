using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStopStopper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Contains("alus"))
        {
            Kamera k =
            Camera.main.GetComponent<Kamera>();
            if (k!=null)
            {
                if (k.cameraInfo!=null)
                {
                    k.cameraInfo.GetComponent<CameraInfoController>().stop = false;
                    k.paljonkopitaavielaodottaa = -1;


                }
            }
        }
    }
}

