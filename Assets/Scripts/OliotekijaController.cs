using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OliotekijaController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject go;
    public float sykli = 15.0f;

    void Start()
    {
        
    }
    private float delta = 0.0f;
    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;
        if (delta>=sykli)
        {
            GameObject instanssi = Instantiate(go,transform.position, Quaternion.identity);
            delta = 0.0f;
        }



    }
}
