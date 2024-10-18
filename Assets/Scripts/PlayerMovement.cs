using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Variables
    [SerializeField] private float speed;
    private float exhaust_clock = 0;
    private bool exhaust_switch = false;
    [SerializeField] private float team;
    [SerializeField] private float rotationSpeed;

    // References
    private Rigidbody2D rb;
    private SpriteRenderer exhaust_sprite;
    [SerializeField] private GameObject exhaust;
    [SerializeField] private Sprite exhaust1;
    [SerializeField] private Sprite exhaust2;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        exhaust_sprite = exhaust.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

/*        Quaternion rotation = transform.rotation;
        Vector2 direction = rotation * Vector2.right;

        Vector2 newPosition = rb.position + direction * speed * Time.deltaTime;

        rb.MovePosition(newPosition);*/

        if (Input.GetKey(KeyCode.W))
        {
            Quaternion rotation = transform.rotation; // Find our current rotation
            Vector2 direction = rotation * Vector2.up; // Work out the direction we are moving from our rotation

            Vector2 newPosition = rb.position + direction * speed * Time.deltaTime; // Move forward in the direction

            rb.MovePosition(newPosition); // Apply the movement
        }
        if (Input.GetKey(KeyCode.A)) // Rotate left
        {
            rb.rotation += rotationSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D)) // Rotate right
        {
            rb.rotation -= rotationSpeed * Time.deltaTime;
        }



        // Change the exhaust sprite every half second
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

        rb.velocity = Vector2.zero; // Stop anomalous movement
        rb.angularVelocity = 0f;

    }

    void Movement()
    {

    }

    void OnForward(InputValue value) 
    {
        
    }
}
