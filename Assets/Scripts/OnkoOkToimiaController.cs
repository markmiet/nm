using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnkoOkToimiaController : BaseController
{

    public bool estaChilderienTormayksetToisiinsa = true;
    // Start is called before the first frame update
    void Start()
    {
        if (estaChilderienTormayksetToisiinsa)
        {
            IgnoraaChildienCollisiot();
        }
        lastVoiko = Voikotoimia();
        Tee(lastVoiko);
    }

    /*

    void Updatevanha()
    {
        voikotoimia = OnkoOkToimiaUusiKasitteleMyosChildienRigidBodyt(gameObject);
        if (toimiviiveella)
        {
            if (voikotoimia)
            {
                viivelaskuri += Time.deltaTime;
            }
            if (voikotoimia && viivelaskuri < viive)
            {

                Rigidbody2D[] rigidbodies = gameObject.GetComponentsInChildren<Rigidbody2D>();
                foreach (Rigidbody2D r in rigidbodies)
                {
                    if (r != null)
                    {
                        r.constraints |= RigidbodyConstraints2D.FreezePositionY;//freeze y
                    }
                }
            }
            else if (voikotoimia && viivelaskuri >= viive)
            {
                Rigidbody2D[] rigidbodies = gameObject.GetComponentsInChildren<Rigidbody2D>();
                foreach (Rigidbody2D r in rigidbodies)
                {
                    if (r != null)
                    {
                        // Unlock Y position
                        r.constraints &= ~RigidbodyConstraints2D.FreezePositionY;//unlock freeze y
                    }
                }
            }

        }



    }
    */


    public bool voikotoimia = false;

    // Update is called once per frame
    public float viive = 0.5f;//@todoo randomisoi
    public bool toimiviiveella = false;


    public bool freezeYposition = false;

    public float checkInterval = 0.5f; // kuinka usein tarkistus tehdään (sekunteina)

    private float nextCheckTime = 0f;

    public void Update()
    {
        // tarkista vain jos riittävästi aikaa on kulunut
        if (Time.time >= nextCheckTime)
        {
            nextCheckTime = Time.time + checkInterval; // ajasta seuraava tarkistus

            if (toimiviiveella)
            {
                bool tmp = Voikotoimia();

                if (tmp != lastVoiko) // arvo muuttui
                {
                    if (teeCoroutine != null) StopCoroutine(teeCoroutine);
                    teeCoroutine = StartCoroutine(StartTeeDelayed(tmp, viive));
                    lastVoiko = tmp;
                }
            }
            else
            {
                bool tmp = Voikotoimia();
                if (tmp != lastVoiko) // optimointi ettei turhaan kutsu Tee()
                {
                    Tee(tmp);
                    lastVoiko = tmp;
                }
            }

            // tämä tarkistus voi edelleen olla joka framella
            TuhoaJosOllaanSiirrettyReilustiKameranVasemmallePuolenSalliPieniAlitusJaYlitys(gameObject);
        }

    }

    private bool lastVoiko;
    private Coroutine teeCoroutine;
    private IEnumerator StartTeeDelayed(bool voiko, float delay)
    {
        Rigidbody2D[] rigidbodies = GetComponentsInChildren<Rigidbody2D>();

        foreach (Rigidbody2D r in rigidbodies)
        {
            if (r != null)
            {
                if (freezeYposition)
                {
                    r.simulated = voiko;
                }
            }
        }

        yield return new WaitForSeconds(delay);
        Tee(voiko);
        teeCoroutine = null;
    }


    private void Tee(bool voiko)
    {
        Rigidbody2D[] rigidbodies = GetComponentsInChildren<Rigidbody2D>();
        foreach (Rigidbody2D r in rigidbodies)
        {
            if (r != null)
            {
                
                r.simulated = voiko;

                if (freezeYposition)
                {
                    if (voiko)
                    {
                        r.constraints &= ~RigidbodyConstraints2D.FreezePositionY;//unlock freeze y
                        r.gravityScale = 2 * r.gravityScale;
                    }
                    else
                    {
                        r.constraints |= RigidbodyConstraints2D.FreezePositionY;//freeze y
                    }
                }

            }
        }
        voikotoimia = voiko;
    }



    public bool tutkiVainChildreneista = false;
    private bool Voikotoimia()
    {
        if (tutkiVainChildreneista)
        {
            return AnyChildVisibleUseMargin(gameObject);

        }
        else
        {
            return IsVisibleUseMargin(gameObject);

        }

    }



}
