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
    void OnEnable()
    {
        uIDocument = GetComponent<UIDocument>();
        highScoreCount = uIDocument.rootVisualElement.Q<Label>("HighscoreCount");
        scoreCount = uIDocument.rootVisualElement.Q<Label>("ScoreCount");
        onTruck = uIDocument.rootVisualElement.Q<VisualElement>("OnTruck");
        PlayerController.instance.PlayerResourceUpdated += onPlayerResourceUpdate;


    }

    public void onPlayerResourceUpdate((BuildingResource, int) ressource)
    {
        onTruck.style.backgroundImage = ressource.Item1.Icon.texture;
    }


}
