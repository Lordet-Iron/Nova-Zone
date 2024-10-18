using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Enemy : MonoBehaviour
{
    // Variables
    GameObject Spawned;

    // References
    [SerializeField] GameObject Enemy;
    [SerializeField] GameObject Reset_Pos;
    [SerializeField] SpriteRenderer SR;
    Transform Reset_Pos_Pos;

    // Start is called before the first frame update
    void Start()
    {
        SR.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Spawn()
    {
        // How tf do I disable spawner if the instantiated enemy ship has been destroyed. But be reset once the level has been restarted.
        if (Spawned != null) 
        {
            Object.Destroy(Spawned);
        }

        Spawned = Instantiate(Enemy, transform.position, Quaternion.identity);

        if (Spawned.tag != "Enemy Turret") // If the enemy is mobile pass in its reset position
        {
            Reset_Pos_Pos = Reset_Pos.GetComponent<Transform>();
            GameObject Spawned_Ship = Spawned.transform.Find("Ship").gameObject;
            Enemy Spawned_Ship_Enemy = Spawned_Ship.GetComponent<Enemy>();
            Spawned_Ship_Enemy.reset_pos = Reset_Pos;
            Spawned_Ship_Enemy.reset_pos_pos = Reset_Pos_Pos;
            //Spawned_Ship_Enemy.area_distance = Reset_Pos.GetComponent<ResetPos>().distance; // Now managed inside the Enemy component
        }
        

    }

    public void Despawn()
    {
        Destroy(Spawned);
    }
    public void Sleep() // Turn off the AI
    {
        if (Spawned != null)
        {
            if (Spawned.tag == "Enemy Turret")
            {
                foreach (GameObject turret in Spawned.GetComponent<EnemyStatic>().turrets)
                {
                    turret.GetComponent<EnemyTurret>().enabled = false;
                }
            }
            else
            {
                foreach (GameObject turret in Spawned.transform.Find("Ship").GetComponent<Enemy>().turrets)
                {
                    turret.GetComponent<EnemyTurret>().enabled = false;
                }
                Spawned.transform.Find("Ship").GetComponent<Enemy>().enabled = false;
            }
        }
        
    }
}
