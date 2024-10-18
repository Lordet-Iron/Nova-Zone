using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Variables
    [SerializeField] public float health;
    private int weapon_Count = 0;

    // References
    [SerializeField] public List<GameObject> weapons = new List<GameObject>();
    [SerializeField] public List<Transform> weapon_Placements = new List<Transform>();
    [SerializeField] public GameObject Health_Bar;
    private PlayerManager playerManager;

    

    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject weapon in weapons)
        {
            //Instantiate(weapon, weapon_Placements[weapon_Count]);
            GameObject newTurret = Instantiate(weapon, weapon_Placements[weapon_Count].transform.position, weapon_Placements[weapon_Count].rotation);
            newTurret.transform.parent = gameObject.transform;
            weapon_Count++;
        }

        // Health bar setup

        Health_Bar.GetComponent<HealthBar>().SetMaxHealth(((int)health));

        // Accessing player manager
        playerManager = transform.parent.GetComponent<PlayerManager>();

        


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage) // Public method used by projectiles to damage this ship
    {
        health -= damage;
        if (health > 0)
        {
            Health_Bar.GetComponent<HealthBar>().SetHealth(((int)health));
        }
        if (health < 0)
        {
            playerManager.Death();
        }
        
    }
}
