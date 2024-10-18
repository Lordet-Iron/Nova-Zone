using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void Quit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
        SceneManager.UnloadScene(0);
    }
    public void Options()
    {
        SceneManager.LoadScene(2);
        SceneManager.UnloadScene(0);
    }
}
