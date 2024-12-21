using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BonusButtonController : MonoBehaviour
{
    public bool selected;
    //public bool used;//usedcount
                     //usedcount max

    public int usedcount = 0;
    public int maxusedCount ;

    public string text;

    [SerializeField]
    private TMP_Text _title;


    public enum Bonusbuttontype
    {
        Speed,
        MissileDown,
        MissileUp,
        Option
    }
    //taka-ammus jolla voi siis tukkia pallon tulemisen
    //se force field
    //laser




    public Bonusbuttontype bonusbuttontype;

    public GameObject alus;


    public int order;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            GetComponent<Image>().color = Color.yellow;

            if (usedcount>= maxusedCount)
            {
                //keltainen väri, ei tekstiä
                // text = "";

                    _title.text = "";
                

            }
            else
            {
                //keltainen väri tekstillä
                _title.text = text;
            }
        }
        else
        {
            GetComponent<Image>().color = Color.blue;

            if (usedcount >= maxusedCount)
            {
                //sininen väri ei tekstiä
                _title.text = "";
            }
            else
            {
                //sininen väri tekstillä
                _title.text = text;
            }

        }

    }

    void FixedUpdate()
    {

    }

}
