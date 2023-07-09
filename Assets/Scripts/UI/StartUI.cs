using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class StartUI : MonoBehaviour
{
    private UIDocument uIDocument;
    private Button settings;
    private Button start;
    private Button exit;
    void OnEnable()
    {
        uIDocument = GetComponent<UIDocument>();
        start = uIDocument.rootVisualElement.Q<Button>("StartButton");
        exit = uIDocument.rootVisualElement.Q<Button>("ExitButton");
        settings = uIDocument.rootVisualElement.Q<Button>("OptionButton");
        settings.RegisterCallback<ClickEvent>(OnOptionsButtonClick);
        start.RegisterCallback<ClickEvent>(OnStartButtonClick);
        exit.RegisterCallback<ClickEvent>(OnExitButtonClick);
    }

    private void OnStartButtonClick(ClickEvent env)
    {
        SceneManager.LoadScene("_boot");
    }
    void OnOptionsButtonClick(ClickEvent env)
    {
        SceneManager.LoadSceneAsync("OptionsUI", LoadSceneMode.Additive);
    }
    private void OnExitButtonClick(ClickEvent env)
    {
        Application.Quit();
    }
}
