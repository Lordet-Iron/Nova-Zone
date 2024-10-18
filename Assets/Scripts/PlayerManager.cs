using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    // Variables
    GameObject created_player;
    GameObject created_HUD;

    // Player Variable with Defaults
    public int defaultHealth = 90;

    // Reference
    [SerializeField] public GameObject player_prefab;   // Mostly depricated at this point as we are parsing the ship
                                                        // via the function arguments
    [SerializeField] public GameObject player_canvas;
    [SerializeField] public Camera MainCamera;

    private GameObject UI;
    private GameObject UI_Death;

    // Start is called before the first frame update
    void Start()
    {
        // UI Accessing
        UI = GameObject.FindGameObjectWithTag("UI");
        UI_Death = UI.transform.Find("Death").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void SpawnPlayer(GameObject Ship ,int health, List<GameObject> weapons) // Load the player template into memory and apply any selected upgrades to it before spawning it in
    {
        Debug.Log(weapons.Count);
        player_prefab = Ship;
        GameObject new_player = player_prefab;
        GameObject new_player_canvas = player_canvas;
        
        if (created_player != null)
        {
            Object.Destroy(created_player);
            Object.Destroy(created_HUD);
        }

        // Spawn objects as inactive
        created_player = Instantiate(new_player, gameObject.transform); // Spawn in ship
        created_HUD = Instantiate(new_player_canvas, gameObject.transform); // Spawn in HUD

        // Player Component
        Player created_Player_Player = created_player.GetComponent<Player>();
        created_Player_Player.health = health * defaultHealth;
        created_Player_Player.Health_Bar = created_HUD.transform.GetChild(0).gameObject;

        // Player Weapons
        created_Player_Player.weapons.AddRange(weapons);

        // PLayer HUD

        // Character HUD component
        CharacterHUD Created_HUD = created_HUD.GetComponent<CharacterHUD>();
        Created_HUD.character = created_player.transform;

        // Update other gameObjects to target the new player
        CameraController CamCont = MainCamera.GetComponent<CameraController>();
        CamCont.SetPlayer(created_player);

        //Debug.Log("Spawning");

    }

    /*public void SpawnEnemies() Depricated by a better system that uses reset positions 
     *                          to manage spawning when the player is within a set distance
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");

        foreach (GameObject spawner in spawners)
        {
            spawner.GetComponent<Spawn_Enemy>().Spawn();
            //Debug.Log(spawner.name);
        }
    }*/

    public void SpawnEnemies()
    {
        GameObject[] resetPoses = GameObject.FindGameObjectsWithTag("ResetPos");

        foreach (GameObject resetPos in resetPoses)
        {
            resetPos.GetComponent<ResetPos>().ActivateSpawners();
        }
    }

    public void Death()
    {
        // Destroy all enemies
        GameObject[] resetPoses = GameObject.FindGameObjectsWithTag("ResetPos");

        foreach (GameObject resetPos in resetPoses)
        {
            resetPos.GetComponent<ResetPos>().Despawn();
        }

        UI_Death.GetComponent<TextMeshProUGUI>().enabled = true;

        // Destroy the player
        Object.Destroy(created_player);
        Object.Destroy(created_HUD);
    }
}
