using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseUI : MonoBehaviour
{
    public static BaseUI instance;
    private UIDocument uIDocument;

    private Label scoreCount;
    private Label highScoreCount;
    private Label elementCount;
    private Label skillPoint;
    private VisualElement onTruck;
    private Button speedUp;
    private Button carryCapacity;
    private Button buildSpeedUp; 

    private PlayerController playerController;
    private GameManager gameManager;

    private int curRessources = 0;

    void OnEnable()
    {

        playerController = PlayerController.instance;
        gameManager = GameManager.Instance;

        uIDocument = GetComponent<UIDocument>();

        highScoreCount = uIDocument.rootVisualElement.Q<Label>("HighscoreCount");
        scoreCount = uIDocument.rootVisualElement.Q<Label>("ScoreCount");
        elementCount = uIDocument.rootVisualElement.Q<Label>("ElementCount");
        skillPoint = uIDocument.rootVisualElement.Q<Label>("SkillPoint");
        onTruck = uIDocument.rootVisualElement.Q<VisualElement>("OnTruck");
        carryCapacity = uIDocument.rootVisualElement.Q<Button>("CarryCapacityUp");
        buildSpeedUp = uIDocument.rootVisualElement.Q<Button>("BuildSpeedButton");
        speedUp = uIDocument.rootVisualElement.Q<Button>("SpeedUp");

        (int, int) res = gameManager.GetUpgardeSpeed();
        speedUp.text = "Speed " + res.Item1 + " (" + res.Item2 + "SP)";
        res = gameManager.GetUpgardeTime();
        buildSpeedUp.text = "Time " + res.Item1 + " (" + res.Item2 + "SP)";
        res = gameManager.GetUpgradeCapasyity();
        carryCapacity.text = "Capacity " + res.Item1 + " (" + res.Item2 + "SP)";
        elementCount.text = curRessources + "/" + res.Item1;

        carryCapacity.RegisterCallback<ClickEvent>(CarryCapacityOnClick);
        buildSpeedUp.RegisterCallback<ClickEvent>(BuildSpeedUpOnClick);
        speedUp.RegisterCallback<ClickEvent>(SpeedUpOnClick);


        playerController.PlayerResourceUpdated += OnPlayerResourceUpdate;
        gameManager.OnSkillPointUpdate += OnSkillPointUpdate;
        gameManager.OnScoreUpdate += OnScoreUpdate;

        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScoreCount.text = PlayerPrefs.GetInt("HighScore").ToString();
        }



    }
    public void OnSkillPointUpdate(int count)
    {
        skillPoint.text = count + "SP";
    }
    public void CarryCapacityOnClick(ClickEvent env)
    {
        (int, int) res = gameManager.UpgardeCapayity();
        (int, int) curr = gameManager.GetUpgradeCapasyity();
        if (res.Item1 != res.Item2)
        {
            carryCapacity.text = "Capacity " + curr.Item1 + " (" + curr.Item2 + "SP)";
        }
        else
        {
            carryCapacity.text = "Max Capacity";
        }
        elementCount.text = curRessources + "/" + res.Item1;
    }
    public void BuildSpeedUpOnClick(ClickEvent env)
    {
        (int, int) res = gameManager.UpgardeTime();
        (int, int) curr = gameManager.GetUpgardeTime();
        if (res.Item1 != res.Item2)
        {
            buildSpeedUp.text = "Time " + curr.Item1 + " (" + curr.Item2 + "SP)";
        }
        else
        {
            buildSpeedUp.text = "Max Time";
        }

    }
    public void SpeedUpOnClick(ClickEvent env)
    {
        (int, int) res = gameManager.UpgardeSpeed();
        (int, int) curr = gameManager.GetUpgardeSpeed();
        if (res.Item1 != res.Item2)
        {
            speedUp.text = "Speed " + curr.Item1 + " (" + curr.Item2 + "SP)";
        }
        else
        {
            speedUp.text = "Max Speed";
        }
    }

    public void OnPlayerResourceUpdate((BuildingResource, int) ressource)
    {
        if(ressource.Item1 != null)
        {
            onTruck.style.backgroundImage = new StyleBackground(ressource.Item1.Icon);
        }
        else
        {
            onTruck.style.backgroundImage = null;
        }
        (int, int) res = gameManager.GetUpgradeCapasyity();
        curRessources = ressource.Item2;
        elementCount.text = curRessources + "/" + res.Item1;
        
    }

    public void OnScoreUpdate(int count)
    {
        scoreCount.text = count.ToString();
        if(Int32.Parse(highScoreCount.text) < count) {
            highScoreCount.text = count.ToString();
            PlayerPrefs.SetInt("HighScore", count);
            PlayerPrefs.Save();
        }
       
    }
    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
}
