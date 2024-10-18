using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPos : MonoBehaviour
{
    [SerializeField] private SpriteRenderer SR;
    [SerializeField] public float distance = 10f;
    [SerializeField] public float spawnDistance;
    [SerializeField] private List<GameObject> spawners = new List<GameObject>();
    private bool hasSpawned = false;
    private bool isSpawning = false;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        SR.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player = null fix it
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if(isSpawning && !hasSpawned)
        {
            
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < spawnDistance) 
            {
                foreach (GameObject spawner in spawners)
                {
                    spawner.GetComponent<Spawn_Enemy>().Spawn();
                    hasSpawned = true;
                    isSpawning = false;
                }
                    
            }
            
        }
        if (hasSpawned)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance > spawnDistance * 5)
            {
                foreach (GameObject spawner in spawners)
                {
                    spawner.GetComponent<Spawn_Enemy>().Sleep();
                    hasSpawned = false;
                    isSpawning = true;
                }
            }
        }
    }

    public void ActivateSpawners()
    {
        player = null;
        player = GameObject.FindGameObjectWithTag("Player");
        foreach (GameObject spawner in spawners)
        {
            
            spawner.GetComponent<Spawn_Enemy>().Despawn();
            isSpawning = true;
            hasSpawned = false;
            
        }
    }

    public void Despawn()
    {
        foreach (GameObject spawner in spawners)
        {
            spawner.GetComponent<Spawn_Enemy>().Sleep();
        }
        player= null;
        isSpawning = false;
        hasSpawned = false;
        
    }
}
