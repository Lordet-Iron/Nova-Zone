using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRigidMovement : MonoBehaviour
{
    // Variables
    [SerializeField] private float acceleration;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float topSpeed;
    [SerializeField] private float topRotationSpeed;

    // References
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W)) // Move forwards
        {
            rb.AddForce(transform.up * acceleration, ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.A)) // Rotate left
        {
            rb.AddTorque(rotationSpeed * Time.deltaTime);
            //rb.rotation += rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) // Rotate right
        {
            rb.AddTorque(-rotationSpeed * Time.deltaTime);
            //rb.rotation -= rotationSpeed * Time.deltaTime;
        }
        // Clamping speeds
        Vector2 vel = rb.velocity;
        float rotation = rb.rotation;

        vel = Vector2.ClampMagnitude(vel, topSpeed);
        rotation = Mathf.Clamp(rotation, -4f, 4f);

        rb.velocity = vel;
        //rb.rotation = rotation;


        // Clamp rotation to top rotation speed
        float currentRotationSpeed = Mathf.Abs(rb.angularVelocity);
        if (currentRotationSpeed > topRotationSpeed)
        {
            float brakeTorque = (currentRotationSpeed - topRotationSpeed) * rb.inertia;
            rb.AddTorque(-Mathf.Sign(rb.angularVelocity) * brakeTorque);




        }
    }
    private void Move()
    {
        
    }
}
