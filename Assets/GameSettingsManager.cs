using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSettingsManager : MonoBehaviour
{

    public static GameSettingsManager Instance { get; private set; }
    private AudioListener audioListenerM;

    // Variables
    [SerializeField] private float Volume;

    private void Awake()
    {
        // Make sure only one instance of the GameSettings exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        // Subscribe to the SceneManager's sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Unsubscribe from the SceneManager's sceneLoaded event
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"{SceneManager.GetActiveScene().name}");

        if (SceneManager.GetActiveScene().name == "Game")
        {
            //AudioListener.volume = 0f;
            Debug.Log($"{Volume}");
        }
    }
    public void VolumneUpdate(float volume)
    {
        Volume = volume;
        AudioListener.volume = volume;
    }
}
