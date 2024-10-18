using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Missile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private float hitDistance;
    [SerializeField] public int damage;
    [SerializeField] private LayerMask whatIsSolid;
    [SerializeField] public int team;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float offset;
    private GameObject target;

    private Transform targetPos;

    // Start is called before the first frame update
    void Start()
    {
        LockOntoTarget();

        Invoke("DestroyProjectile", lifeTime);

        
    }

    // Update is called once per frame
    void Update()
    {
        

        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, hitDistance, whatIsSolid); // Detect if there is something within our layermask list infront of our bullet
        if (hitInfo.collider != null)
        {
            if (team == 0)
            {
                if (hitInfo.collider.CompareTag("Enemy"))
                {
                    hitInfo.collider.GetComponent<Enemy>().TakeDamage(damage); // If its an enemy call the damage function in the enemy script inside the targetPos.
                    DestroyProjectile();
                }
                if (hitInfo.collider.CompareTag("Enemy Turret"))
                {
                    hitInfo.collider.GetComponent<EnemyStatic>().TakeDamage(damage); // If its an enemy call the damage function in the enemy script inside the targetPos.
                    DestroyProjectile();
                }


            }
            if (team == 1)
            {
                if (hitInfo.collider.CompareTag("Player"))
                {
                    hitInfo.collider.GetComponent<Player>().TakeDamage(damage); // If its an enemy call the damage function in the enemy script inside the targetPos.
                    DestroyProjectile();
                }
            }
            if (hitInfo.collider.CompareTag("Environment"))
            {
                DestroyProjectile();
            }


        }
    }

    private void LockOntoTarget()
    {
        if (team == 1) // If its an enemy missile lock onto the target
        {
            target = GameObject.FindGameObjectWithTag("Player");
            if (target == null)
            {
                DestroyProjectile();
            }
            //targetPos = target.GetComponent<Transform>();
            try
            {
                targetPos = target.GetComponent<Transform>();
            }
            catch (NullReferenceException)
            {
                DestroyProjectile();
            }
        }
        if (team == 0) // If the missile is a target lock onto the nearest enemy
        {
            List<GameObject> enemies = new List<GameObject>();
            enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy")); // Select all enemies
            enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy Turret"));
            float nearestDistance = Mathf.Infinity;

            foreach (GameObject enemy in enemies) // Work out which enemy is closest to the missile
            {
                float selectedDistance = Vector2.Distance(transform.position, enemy.transform.position);

                if (selectedDistance < nearestDistance)
                {
                    nearestDistance = selectedDistance;
                    target = enemy;
                }
            }
            if (target != null)
            {
                targetPos = target.GetComponent<Transform>();
            }

            

        }
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            LockOntoTarget();
        }

        transform.Translate(Vector2.up * speed * Time.deltaTime); // Move forward

        // Rotation
        /*        Quaternion currentRotation = transform.rotation;

                Vector3 difference = targetPos.position - transform.position; // Calculate the direction between the targetPos and the missile. By subtracting the two positions representing the direction
                                                                           // as a vector

                float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg + rotationSpeed; // Calculate where we're rotating
                transform.rotation = Quaternion.RotateTowards(currentRotation, Quaternion.Euler(0, 0, rotZ), Time.deltaTime * rotationSpeed);*/
        if (target != null)
        {
            Vector3 dir = targetPos.position - transform.position; // calculate direciton
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + offset; // Calculate angle of said direction
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // Apply rotation
        }

        
    }
    public void DestroyProjectile()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
