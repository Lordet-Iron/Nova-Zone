using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    // Variables
    [SerializeField] float scale;
    [SerializeField] float zoomMin;
    [SerializeField] float zoomMax;
    [SerializeField] float zoomSpeed; // How fast the zoom changes
    [SerializeField] float zoomStep; // Rough distance from the player and zoom target
    [SerializeField] float zoomTolerance; // How much of a difference in distance is required before changing zoom target
    [SerializeField] private float range;
    float targetZoom;

    // References
    private GameObject player;
    private Camera mainCamera;
    private GameObject closestEnemy;

    // Start is called before the first frame update
    void Start()
    {
        
        mainCamera = Camera.main;

        AudioListener.volume = 0.5f;

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z); // Lock camera to the target
            mainCamera.orthographicSize -= Input.mouseScrollDelta.y * scale; // Increase size of camera based on scroll wheel
                                                                             // Lock the camera within min and max values


            // Dynamic Zoom
            List<GameObject> enemies = new List<GameObject>();
            enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy")); // Select all enemies
            enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy Turret"));
            //float nearestDistance = Mathf.Infinity; // No longer used.
            float nearestDistance = range;
            targetZoom = 4;

            foreach (GameObject enemy in enemies) // Work out which enemy is closest to the missile
            {
                float selectedDistance = Vector2.Distance(transform.position, enemy.transform.position);

                if (selectedDistance < nearestDistance)
                {
                    nearestDistance = selectedDistance;
                    closestEnemy = enemy;
                }
            }
            if (closestEnemy != null)
            {
                Transform closestEnemy_transform = closestEnemy.GetComponent<Transform>();

                //Debug.Log(nearestDistance);

/*                if (nearestDistance <= 1)
                {
                    targetZoom = 2;
                }
                else if (nearestDistance <= 2)
                {
                    targetZoom = 4;
                }
                else if (nearestDistance <= 3)
                {
                    targetZoom = 6;
                }
                else if (nearestDistance <= 4)
                {
                    targetZoom = 8;
                }*/

                float zoomdifference = Mathf.Abs(targetZoom - nearestDistance * zoomStep);
                if (zoomdifference > 0.1f)
                {
                    targetZoom = nearestDistance * zoomStep;
                }
            }

            float zoom = mainCamera.orthographicSize;

            // Apply Dynamic Zoom
/*            if (Mathf.Abs(zoom - targetZoom) < zoomSpeed)
            {
                zoom = targetZoom;
            }
            else if (Mathf.Abs(targetZoom + zoom) < zoomSpeed)
            {
                zoom = targetZoom;
            }*/
            if (zoom > targetZoom)
            {
                zoom -= zoomSpeed * Time.deltaTime;
                if (zoom < targetZoom)
                {
                    zoom = targetZoom;
                }
            }
            else if (zoom < targetZoom)
            {
                zoom += zoomSpeed * Time.deltaTime;
                if (zoom > targetZoom)
                {
                    zoom = targetZoom;
                }
            }
            
            // Clamp zoom within boundaries
            if (zoom < zoomMin)
            {
                zoom = zoomMin;
            }
            if (zoom > zoomMax)
            {
                zoom = zoomMax;
            }

            // Set Zoom
            mainCamera.orthographicSize = zoom;


        } 
    }

    public void SetPlayer(GameObject input)
    {
        player = input;
    }
}
