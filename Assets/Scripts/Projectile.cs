using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private float hitDistance;
    [SerializeField] public int damage;
    [SerializeField] private LayerMask whatIsSolid;
    [SerializeField] private int team;


    private void Start()
    {
        Invoke("DestroyProjectile", lifeTime);
    }
    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, transform.up, hitDistance, whatIsSolid); // Detect if there is something within our layermask list infront of our bullet
        if (hitInfo.collider!= null)
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
            if (hitInfo.collider.CompareTag("Projectile"))
            {

                if (hitInfo.collider.GetComponent<Missile>().team != team)
                {
                    hitInfo.collider.GetComponent<Missile>().DestroyProjectile();
                    DestroyProjectile();
                }
            }
            if (hitInfo.collider.CompareTag("Environment"))
            {
                DestroyProjectile();
            }



        }

        transform.Translate(Vector2.up * speed * Time.deltaTime); // Move forward
    }

    void DestroyProjectile()
    {
        Instantiate(destroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
