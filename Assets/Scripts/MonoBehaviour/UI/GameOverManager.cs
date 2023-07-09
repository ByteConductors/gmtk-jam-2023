using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        ResetGameEvent?.Invoke();
    }

    public void LoadMainMenu()
    {
        LoadMainMenuEvent?.Invoke();
    }

    public void QuitGame()
    {
        QuitGameEvent?.Invoke();
    }
}
