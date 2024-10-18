using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretWeapon : MonoBehaviour
{

    [SerializeField] private float offset;
    [SerializeField] public GameObject projectile;
    [SerializeField] private List<Transform> shotPoints;
    [SerializeField] public float startTimeBtwShots;
    [SerializeField] private float bloomStrength = 0;

    private AudioSource audioSource;
    private float timeBtwShots;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Audio
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; // Calculate the direction between weapon and mouse. By subtracting the two positions representing the direction
                                                                                                       // as a vector

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if (timeBtwShots <= 0) // Weapon cooldown
        {
            if (Input.GetMouseButton(0))
            {
                foreach (Transform shotPoint in shotPoints)
                {
                    float bloom = Random.Range(-bloomStrength, bloomStrength);
                    Quaternion zRotation = Quaternion.Euler(0f, 0f, bloom);
                    Quaternion bloomedRotation = transform.rotation * zRotation; // Give the bullet the same z rotaiton as the ship but with a slight amount of bloom
                    Instantiate(projectile, shotPoint.position, bloomedRotation); // Create the projectile at the end of our weapon facing the same way as our weapon
                }
                
                timeBtwShots = startTimeBtwShots; // Set it to cooldown when fired
                audioSource.Play();

            }
        }

        else
        {
            timeBtwShots -= Time.deltaTime; // Decrease cooldown if weapon is on cooldown
        }

        
    }
}
