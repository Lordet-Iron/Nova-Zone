using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatic : MonoBehaviour
{
    // Variables
    [SerializeField] private int health;

    // References
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private List<Transform> deathPositions = new List<Transform>();
    [SerializeField] private GameObject Health_Bar;
    [SerializeField] private GameObject Score_Counter;
    [SerializeField] private int Score_Value;
    [SerializeField] public List<GameObject> turrets;
    private Score Score_Interface;

    // Start is called before the first frame update
    void Start()
    {
        Health_Bar.GetComponent<HealthBar>().SetMaxHealth(((int)health)); // health bar setup

        // Score Setup
        Score_Counter = GameObject.FindGameObjectWithTag("Score");
        Score_Interface = Score_Counter.GetComponent<Score>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0) // If they run out of health kill them
        {
            foreach (Transform pos in deathPositions)
            {
                Instantiate(deathEffect, pos.transform.position, Quaternion.identity);
            }

            Score_Interface.RewardScore(Score_Value); // Increases the player's score

            Destroy(gameObject);
        }
    }

    public void TakeDamage(int damage) // Public method used by projectiles to damage this ship
    {
        health -= damage;
        Health_Bar.GetComponent<HealthBar>().SetHealth(((int)health));
    }
}
