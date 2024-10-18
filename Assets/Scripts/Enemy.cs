using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    // Variables
    [SerializeField] public int health;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float offset;
    [SerializeField] private float speed;
    [SerializeField] private float topSpeed;
    [SerializeField] private float topRotationSpeed = 1;
    [SerializeField] private float minimumDistance;
    [SerializeField] private float engageRange;
    [SerializeField] private LayerMask whatIsPlayer;
    public GameObject reset_pos;
    public Transform reset_pos_pos;
    public float area_distance;
    private float exhaust_clock = 0;
    private bool exhaust_switch = false;
    private bool can_see_player;
    private float originalMinimumDistance;
    private float originalSpeed;
    


    // References
    [SerializeField] private GameObject parent;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private List<Transform> deathPositions = new List<Transform>();
    [SerializeField] private GameObject exhaust;
    [SerializeField] private Sprite exhaust1;
    [SerializeField] private Sprite exhaust2;
    [SerializeField] private GameObject Health_Bar;
    [SerializeField] private GameObject Score_Counter;
    [SerializeField] private int Score_Value;
    [SerializeField] public List<GameObject> turrets;
    private Score Score_Interface;
    private SpriteRenderer exhaust_sprite;
    [SerializeField] private GameObject player;
    private Transform current_target;
    private Transform player_target;
    private SpriteRenderer sr;

    private Rigidbody2D rb;
    private Transform trans;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Select the target and keep track of its position, this will be used to have the enemy hunt the target.
        current_target = player.GetComponent<Transform>();
        player_target = player.GetComponent<Transform>();

        rb = GetComponent<Rigidbody2D>();
        exhaust_sprite = exhaust.GetComponent<SpriteRenderer>();
        originalMinimumDistance = minimumDistance;
        originalSpeed = speed;
        Health_Bar.GetComponent<HealthBar>().SetMaxHealth(((int)health)); // health bar setup

        // Score Setup
        Score_Counter = GameObject.FindGameObjectWithTag("Score");
        Score_Interface = Score_Counter.GetComponent<Score>();

        // Reset Position Setup
        area_distance = reset_pos.GetComponent<ResetPos>().distance;

        // Sprite Renderer Setup
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Fix for new enemies locking on to destoryed player. This will cause a problem later:
        if (player == null) 
        {
            player = GameObject.FindGameObjectWithTag("Player"); // Select the target and keep track of its position, this will be used to have the enemy hunt the target.
            current_target = player.GetComponent<Transform>();
            player_target = player.GetComponent<Transform>();
        }


        if (health <= 0) // If they run out of health kill them
        {
            foreach (Transform pos in deathPositions)
            {
                Instantiate(deathEffect, pos.transform.position, Quaternion.identity);
            }

            Score_Interface.RewardScore(Score_Value); // Increases the player's score
            
            Destroy(parent);
        }

        // Rotate towards target:
        // Rotation V1
        /*Vector2 targetDirection = (current_target.position - transform.position).normalized; // Express direction from enemy to target as a vector 2 by subtrating the enemy's position from the target's.
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg; // Convert this expression to radions then degrees
        angle += offset; // Apply rotation offset
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward); // Convert this to a Quaternion roation
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime)); // Apply this rotation at a rate defined in our rotation speed.*/

        // Rotation V2
        float torqueForce = rotationSpeed;

        Vector2 targetDirection = current_target.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg + offset;

        // Calculate the difference between the current angle and the target angle
        float angleDiff = Mathf.DeltaAngle(rb.rotation, targetAngle);

        // Clamp the angleDiff to maintain speeds so it's not too fast if the difference is very high
        float clampedAngleDiff = Mathf.Clamp(angleDiff, -10f, 10f);

        // Apply a torque to the rigidbody to rotate towards the target angle
        rb.AddTorque(clampedAngleDiff * torqueForce * Time.fixedDeltaTime);

        // Cap to top rotation speed
        float clampedRot = Mathf.Clamp(rb.angularVelocity, -topRotationSpeed, topRotationSpeed);
        rb.angularVelocity = clampedRot;

        // Move towards target if they're not within the minimum distance
        if (Vector2.Distance(current_target.position, transform.position) > minimumDistance)
        {
            Quaternion rotation = transform.rotation; // Find our current rotation
            Vector2 direction = rotation * Vector2.up; // Work out the direction we are moving from our rotation

            Vector2 newPosition = rb.position + direction * speed * Time.deltaTime; // Move forward in the direction

            //rb.MovePosition(newPosition); // Apply the movement

            rb.AddForce(transform.up * speed, ForceMode2D.Impulse);

            // Cap to top speed
            Vector2 clampedVel = Vector2.ClampMagnitude(rb.velocity, topSpeed);
            rb.velocity = clampedVel;

        }

        // Check to see if the enemy has line of sight on the player
        Vector2 playerDirection = (player_target.position - transform.position).normalized;
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, playerDirection, engageRange, whatIsPlayer); // Beam towards
        
        if (hitInfo.collider != null)
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

        
        float resetDistance = Vector2.Distance(transform.position, reset_pos_pos.position);
        //Debug.Log(resetDistance);
        if (resetDistance > area_distance) // If they're too far from the reset position, move towards the reset position.
        {
            sr.color = Color.blue;
            current_target = reset_pos_pos;
            minimumDistance = 0.5f;
            speed = originalSpeed / 10;
        }
        else if (can_see_player) // If they can see the player engage
        {
            sr.color = Color.white;
            current_target = player.GetComponent<Transform>();
            minimumDistance = originalMinimumDistance;
            speed = originalSpeed;
        }
        else // If they cannot see the player hold position
        {
            sr.color = Color.red;
            current_target = player.GetComponent<Transform>();
            minimumDistance = 0.5f;
            speed = 0f;
        }
        


        // Exhaust Toggle

        if (exhaust_clock >= 0.5)
        {
            if (exhaust_switch)
            {
                exhaust_sprite.sprite = exhaust1;
                exhaust_switch = false;
            }

            else
            {
                exhaust_sprite.sprite = exhaust2;
                exhaust_switch = true;
            }
            exhaust_clock = 0;
        }
        exhaust_clock += Time.deltaTime;

        //rb.velocity = Vector2.zero; // Stop anomalous movement
        //rb.angularVelocity = 0f; // Stop anomalous rotation

    }

    public void TakeDamage(int damage) // Public method used by projectiles to damage this ship
    {
        health -= damage;
        Health_Bar.GetComponent<HealthBar>().SetHealth(((int)health));
    }
}
