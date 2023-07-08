using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class BaseUI : MonoBehaviour
{
    private UIDocument uIDocument;
    private Label scoreCount;
    private Label highScoreCount;
    private VisualElement onTruck;
    private PlayerController playerController;
    private GameManager gameManager;
    void OnEnable()
    {
        playerController = PlayerController.instance;
        gameManager = GameManager.Instance;
        uIDocument = GetComponent<UIDocument>();
        highScoreCount = uIDocument.rootVisualElement.Q<Label>("HighscoreCount");
        scoreCount = uIDocument.rootVisualElement.Q<Label>("ScoreCount");
        onTruck = uIDocument.rootVisualElement.Q<VisualElement>("OnTruck");
        playerController.PlayerResourceUpdated += OnPlayerResourceUpdate;
        gameManager.OnScoreUpdate += OnScoreUpdate;
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScoreCount.text = PlayerPrefs.GetInt("HighScore").ToString();
        }



    }

    public void OnPlayerResourceUpdate((BuildingResource, int) ressource)
    {
        onTruck.style.backgroundImage = ressource.Item1.Icon.texture;
    }

    public void OnScoreUpdate(int count)
    {
        scoreCount.text = count.ToString();
        PlayerPrefs.SetInt("HighScore", count);
        PlayerPrefs.Save();
    }

}
