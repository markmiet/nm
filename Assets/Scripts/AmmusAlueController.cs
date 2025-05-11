using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmusAlueController : BaseController
{
    public GameObject ammusPrefab;
    private GameObject alus;
    public float ampumakertojenvalinenviive = 5.0f;
    private float ammuttulaskuri = 0.0f;
    public float ampumisenkokonaisvoima = 5.0f;

    public float xoffset =-0.5f;
    public float yoffset = 1.0f;
    public int ammuksiaPerLaukaus = 5;  // Number of bullets per shot
    public float spreadAngle = 15f;  // Angle between bullets in degrees

    void Start()
    {
        alus = GameObject.FindGameObjectWithTag("alustag");
    }

    void Update()
    {
        if (IsOffScreen()) return;

        ammuttulaskuri += Time.deltaTime;

        if (ammuttulaskuri >= ampumakertojenvalinenviive)
        {
            FireProjectiles();
            ammuttulaskuri = 0.0f;
        }
    }

    void FireProjectiles()
    {

        for (float i = 0; i < ammuksiaPerLaukaus; i++)
        {
            Vector3 spawnPosition = new Vector3(transform.position.x + xoffset+(i/2), transform.position.y + yoffset, transform.position.z);

            GameObject ammusinstanssi = Instantiate(ammusPrefab, spawnPosition, Quaternion.identity);

            // Calculate spread angle
            float angleOffset = (i - (ammuksiaPerLaukaus - 1) / 2.0f) * spreadAngle;
            Vector2 baseVelocity = palautaAmmuksellaVelocityVector(alus, ampumisenkokonaisvoima, spawnPosition);
            Vector2 rotatedVelocity = Quaternion.Euler(0, 0, angleOffset) * baseVelocity;

            ammusinstanssi.GetComponent<Rigidbody2D>().velocity = rotatedVelocity;
        }
    }
}
