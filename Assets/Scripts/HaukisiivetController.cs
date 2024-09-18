using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaukisiivetController : BaseController, IExplodable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //mit‰ tapahtuu kun hauen siipiin osuu?
        //siivet r‰j‰ht‰‰ ja painovoimaa alkaa vaikuttamaan
        //hauki tippuu
    }
    public void Explode()
    {
     //   transform.parent.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        Destroy(gameObject);
        RajaytaSprite(gameObject, 10, 10, 10.0f, 0.3f);
    }
}
