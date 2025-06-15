using UnityEngine;

public class JointBreakHandler : MonoBehaviour
{
    public float breakForce = 300f;
    public float breakTorque = 300f;

    public float alkuaika = 0.2f;
    // public float alunForce = 500f;
    private bool tehtyExplodeksi = false;
    private Rigidbody2D rb;
    public bool oneBreaksAllBreaks = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        aikalaskuri = 0;
        FixedJoint2D[] fixedJoint = GetComponents<FixedJoint2D>();
        foreach (FixedJoint2D f in fixedJoint)
        {
            f.breakForce = Mathf.Infinity;// alunForce;
            f.breakTorque = Mathf.Infinity;// alunForce;
        }
    }

    private float aikalaskuri = 0f;
   // private bool asetettu = false;

    private bool breakEnabled = false;
    public float topSpeedThreshold = 10f;
    void Update()
    {
        CheckIfHasAnyJoints();
        if (breakEnabled)
        {
            return;
        }

        /*

        if (aikalaskuri>= alkuaika  && Time.timeScale==1.0f && Time.timeSinceLevelLoad>=0.4f)
        {
            asetettu = true;
            FixedJoint2D[] fixedJoint = GetComponents<FixedJoint2D>();
            foreach (FixedJoint2D f in fixedJoint)
            {
                f.breakForce = breakForce;
                f.breakTorque = breakTorque;
                
            }
        }
       

        */

       // Debug.Log("rb.velocity.magnitude ="+ rb.velocity.magnitude);
        if (aikalaskuri >= alkuaika && !breakEnabled && rb.velocity.magnitude >= topSpeedThreshold && Time.timeScale==1.0f && Time.timeSinceLevelLoad>= alkuaika)
        {
            // Enable joint breaking once top speed is reached
            //fixedJoint.breakForce = breakForce;
            //fixedJoint.breakTorque = breakTorque;
            FixedJoint2D[] fixedJoint = GetComponents<FixedJoint2D>();
            foreach (FixedJoint2D f in fixedJoint)
            {
                f.breakForce = breakForce;
                f.breakTorque = breakTorque;

            }

            breakEnabled = true;
            Debug.Log("Top speed reached. Joint is now breakable.");
        }
        aikalaskuri += Time.deltaTime;

    }


    void OnJointBreak2D(Joint2D brokenJoint)
    {
      //  Debug.Log("Joint broken: " + brokenJoint);
        // Call your custom action here
        MyCustomBreakAction(brokenJoint);
        CheckIfHasAnyJoints();
    }
    public GameObject explosion;


    public void BreakAllJoints()
    {
        Joint2D[] jointit =
        this.GetComponents<Joint2D>();
        foreach(Joint2D j in jointit)
        {
            MyCustomBreakAction(j);
        }
    }
    void MyCustomBreakAction(Joint2D joint)
    {
        // Your custom logic when joint breaks
        // Example: play sound, spawn particles, notify game manager, etc.
     //   Debug.Log("Custom break action triggered for: " + joint.name);


        if (oneBreaksAllBreaks)
        {
            FixedJoint2D[] fixedJoint = GetComponents<FixedJoint2D>();
            foreach (FixedJoint2D jointtu in fixedJoint)
            {
                jointtu.connectedBody.transform.gameObject.tag = "makitavihollinenexplodetag";
                jointtu.connectedBody.transform.gameObject.layer = LayerMask.NameToLayer("Default");
                //joint.connectedBody.transform.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;


                KiviIExplodaple c = jointtu.connectedBody.gameObject.GetComponent<KiviIExplodaple>();
                if (c != null)
                {
                    c.prefabExplosion = explosion;
                    c.explode = true;
                }
                Destroy(jointtu);

            }
        }
        else
        {
            joint.connectedBody.transform.gameObject.tag = "makitavihollinenexplodetag";
            joint.connectedBody.transform.gameObject.layer = LayerMask.NameToLayer("Default");
            //joint.connectedBody.transform.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;


            KiviIExplodaple c = joint.connectedBody.gameObject.GetComponent<KiviIExplodaple>();
            if (c != null)
            {
                c.prefabExplosion = explosion;
                c.explode = true;
            }
            Destroy(joint);
        }
        
        
    }

    public void CheckIfHasAnyJoints()
    {
        if (tehtyExplodeksi)
        {
            return;
        }

        bool joints = false;

        FixedJoint2D[] fixedJoint = GetComponents<FixedJoint2D>();
        foreach (FixedJoint2D f in fixedJoint)
        {
            joints = true;

        }
        if (!joints)
        {
            KiviIExplodaple olemassaOleva = transform.gameObject.GetComponent<KiviIExplodaple>();
            if (olemassaOleva!=null)
            {
                transform.gameObject.tag = "makitavihollinenexplodetag";
                transform.gameObject.layer = LayerMask.NameToLayer("Default");
                //transform.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0.5f;
                KiviIExplodaple c = transform.gameObject.GetComponent<KiviIExplodaple>();
                c.prefabExplosion = explosion;
                c.explode = true;
                tehtyExplodeksi = true;
            }
        }
    }
}
