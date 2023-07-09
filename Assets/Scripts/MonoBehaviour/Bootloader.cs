using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootloader : MonoBehaviour
{
    public static Bootloader Instance { get; private set; }

    public string UIScene;
    public string bootScene;
    public string mainMenuScene;

    public string gameOverUI;
    public event System.Action GameOverEvent;

    public GameObject gameMangerPrefab;
    public GameObject playerPrefab;
    public GameObject pointerPrefab;
    public GameObject LevelPrefab;

    private void Awake()
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
        StartCoroutine(Boot());
    }

    IEnumerator Boot()
    {
        SceneManager.LoadSceneAsync(UIScene, LoadSceneMode.Additive);

        yield return null;
    }

    public IEnumerator GameOver()
    {

        Debug.Log("Game Over - Bootloader");

        SceneManager.LoadSceneAsync(gameOverUI, LoadSceneMode.Additive);
        
        GameOverEvent?.Invoke(); // not in use currently, but exists

        GameOverManager.Instance.ResetGameEvent += ResetGame;
        GameOverManager.Instance.LoadMainMenuEvent += LoadMainMenu;
        GameOverManager.Instance.QuitGameEvent += QuitGame;

        yield return null;
    }

    public void ResetGame()
    {
        SceneManager.LoadSceneAsync(bootScene, LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(mainMenuScene, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
