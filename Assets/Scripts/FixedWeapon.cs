using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedWeapon : MonoBehaviour
{

    [SerializeField] private float offset;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private float startTimeBtwShots;
    [SerializeField] private int bloomStrength;

    private float timeBtwShots;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Audio
    }

    // Update is called once per frame
    void Update()
    {

        if (timeBtwShots <= 0) // Weapon cooldown
        {
            if (Input.GetKey(KeyCode.Space))
            {
                float bloom = Random.Range(-bloomStrength, bloomStrength);
                Quaternion zRotation = Quaternion.Euler(0f, 0f, bloom);
                Quaternion bloomedRotation = transform.rotation * zRotation; // Give the bullet the same z rotaiton as the ship but with a slight amount of bloom
                Instantiate(projectile, shotPoint.position, bloomedRotation); // Create the projectile at the end of our weapon facing the same way as our weapon
                audioSource.Play();
                timeBtwShots = startTimeBtwShots; // Set it to cooldown when fired

            }
        }

        else
        {
            timeBtwShots -= Time.deltaTime; // Decrease cooldown if weapon is on cooldown
        }


    }
}
