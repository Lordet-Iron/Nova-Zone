using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    // Variables
    [SerializeField] private float offset;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shotPoint;
    [SerializeField] private float startTimeBtwShots;
    [SerializeField] private float burstAmount = 1;
    [SerializeField] private float startTimeBtwBurst = 1;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float range;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private bool isVisible = true;
    private AudioClip soundEffect;
    private float burstRemaining;
    private float timeBtwBurst;


    // References
    private GameObject player;
    private Transform target;
    private Rigidbody2D rb;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<Transform> shotPoints= new List<Transform>();

    private float timeBtwShots;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.GetComponent<Transform>();

        if (!isVisible)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        // Fix for new enemies locking on to destoryed player. This will cause a problem later:
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            target = player.GetComponent<Transform>();
        }

        Vector3 difference = target.position - transform.position; // Calculate the direction between weapon and mouse. By subtracting the two positions representing the direction
                                                                                                       // as a vector

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);
        float targetDistance = Vector2.Distance(target.position, transform.position);

        // Burst handler
        
        
        if (timeBtwBurst <= 0 && burstRemaining > 0)
        {
                burstRemaining--;
                foreach (Transform shotPoint in shotPoints)
                {
                    Instantiate(projectile, shotPoint.position, transform.rotation); // Create the projectile at the end of our weapon facing the same way as our weapon
                }
            timeBtwBurst = startTimeBtwBurst;
        }
        else
        {
            timeBtwBurst -= 1;
        }
        // Shot handler
        if (timeBtwShots <= 0 && targetDistance < range) // Weapon cooldown
        {
            Vector2 playerDirection = (target.position - transform.position).normalized; // Work out direction of target as a Vector2
            RaycastHit2D hitInfo = Physics2D.Raycast(shotPoint.position, playerDirection, range, whatIsPlayer); // Beam towards the player

            bool can_see_player = false;
            if (hitInfo.collider != null) // If it hits the player allow the weapon to fire
            {
                if (hitInfo.collider.CompareTag("Player"))
                {
                    can_see_player = true;
                }

                else if (hitInfo.collider.CompareTag("Environment"))
                {
                    can_see_player = false;
                }
            }

            float dotProduct = Vector2.Dot(playerDirection, transform.up);
            if (dotProduct > 0.5f && can_see_player)
            {
                foreach (Transform shotPoint in shotPoints)
                {
                    Instantiate(projectile, shotPoint.position, transform.rotation); // Create the projectile at the end of our weapon facing the same way as our weapon
                }
                burstRemaining = burstAmount - 1; // Begin burst
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
