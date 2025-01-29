using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRigSeuraaAlustaController : MonoBehaviour
{
    public GameObject followObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public float speed = 5f; // Speed at which this object follows the target

    // Update is called once per frame
    void Update()
    {
        if (followObject != null)
        {
            // Move this object towards the followObject's position
            //transform.position = Vector3.MoveTowards(transform.position, followObject.transform.position, speed * Time.deltaTime);

            transform.position = followObject.transform.position;


        }
    }
}
