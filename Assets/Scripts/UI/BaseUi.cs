using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class BaseUI : MonoBehaviour
{
    private UIDocument uIDocument;
    private Label scoreCount;
    private Label highScoreCount;
    void OnEnable()
    {
        uIDocument = GetComponent<UIDocument>();
        Label highScoreCount = uIDocument.rootVisualElement.Q<Label>("HighscoreCount");
        Label scoreCount = uIDocument.rootVisualElement.Q<Label>("ScoreCount");
        

    }

}
