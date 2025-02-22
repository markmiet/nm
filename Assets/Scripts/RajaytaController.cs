using UnityEngine;
using UnityEngine.U2D.Animation;

public class RajaytaController : BaseController, IExplodable
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public int rajaytysrows = 2;
    public int rajaytyscols = 2;
    public bool rajaytaspriteexplodenjalkeenpistatamatrueksi = true;
    public float rajaytysvoima = 1.0f;
    public float rajaytyskestoaika = 1.0f;



    public void Explode()
    {
       // GetComponentInParent<RajaytaController>().ExplodeParentti();

        transform.parent.GetComponent<RajaytaController>().ExplodeParentti();
    }



    public void ExplodeParentti()
    {

        if (rajaytaspriteexplodenjalkeenpistatamatrueksi)
        {
            RajaytaSprite(gameObject, rajaytysrows, rajaytyscols, rajaytysvoima, rajaytyskestoaika);
            //RajaytaUudellaTavalla();

        }

        //ammuksen massa oli 0.06
        Destroy(gameObject);

    }
}
