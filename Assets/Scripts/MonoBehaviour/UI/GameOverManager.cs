using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }

    public System.Action ResetGameEvent;
    public System.Action LoadMainMenuEvent;
    public System.Action QuitGameEvent;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadSceneAsync(Bootloader.Instance.bootScene, LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(Bootloader.Instance.mainMenuScene, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
