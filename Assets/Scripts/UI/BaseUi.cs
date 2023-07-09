using System;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseUI : MonoBehaviour
{
    public static BaseUI instance;
    private UIDocument uIDocument;

    private Label scoreCount;
    private Label highScoreCount;
    private Label elementCount;
    private VisualElement onTruck;
    private Button speedUp;
    private Button carryCapacity;
    private Button buildSpeedUp; 

    private PlayerController playerController;
    private GameManager gameManager;

    public event Action<int> CarryCapacityUpgrade;
    public event Action<int> BuildSpeedUpgrade;
    public event Action<int> SpeedUpgrade;

    // MaxUpgarde
    public int maxCarryCapacityUpgrades;
    public int maxBuildSpeedUpgrades;
    public int maxSpeedUpgrades;
    
    //Increasement
    public int carryCapacityUpgradeKostIncreasement;
    public int buildSpeedUpgradesKostIncreasement;
    public int speedUpgradesKostIncreasement;

    //Basic Kost
    public int carryCapacityUpgradeBasicKost;
    public int BuildSpeedUpgradeBasicKost;
    public int speedUpgradeBasicKost;

    private int curCarryCapacityUpgrades;
    private int curBuildSpeedUpgrades;
    private int curSpeedUpgrades;
    private int curRessources;


    void OnEnable()
    {
        curCarryCapacityUpgrades = 1;
        curBuildSpeedUpgrades = 0;
        curSpeedUpgrades = 0;
        curRessources = 0;

        playerController = PlayerController.instance;
        gameManager = GameManager.Instance;

        uIDocument = GetComponent<UIDocument>();

        highScoreCount = uIDocument.rootVisualElement.Q<Label>("HighscoreCount");
        scoreCount = uIDocument.rootVisualElement.Q<Label>("ScoreCount");
        elementCount = uIDocument.rootVisualElement.Q<Label>("ElementCount");
        onTruck = uIDocument.rootVisualElement.Q<VisualElement>("OnTruck");
        carryCapacity = uIDocument.rootVisualElement.Q<Button>("CarryCapacityUp");
        buildSpeedUp = uIDocument.rootVisualElement.Q<Button>("BuildSpeedButton");
        speedUp = uIDocument.rootVisualElement.Q<Button>("SpeedUp");

        carryCapacity.text = "Capacity " + carryCapacityUpgradeBasicKost;
        buildSpeedUp.text = "Time " + BuildSpeedUpgradeBasicKost;
        speedUp.text = "Speed " + speedUpgradeBasicKost;
        elementCount.text = curRessources + "/" + curCarryCapacityUpgrades;

        carryCapacity.RegisterCallback<ClickEvent>(CarryCapacityOnClick);
        buildSpeedUp.RegisterCallback<ClickEvent>(BuildSpeedUpOnClick);
        speedUp.RegisterCallback<ClickEvent>(SpeedUpOnClick);


        playerController.PlayerResourceUpdated += OnPlayerResourceUpdate;
        gameManager.OnScoreUpdate += OnScoreUpdate;

        if (PlayerPrefs.HasKey("HighScore"))
        {
            highScoreCount.text = PlayerPrefs.GetInt("HighScore").ToString();
        }



    }
    public void CarryCapacityOnClick(ClickEvent env)
    {
        if (curCarryCapacityUpgrades < maxCarryCapacityUpgrades)
        {
            carryCapacity.text = "Capacity " + (carryCapacityUpgradeBasicKost + (carryCapacityUpgradeKostIncreasement * curCarryCapacityUpgrades));
            curCarryCapacityUpgrades++;
            CarryCapacityUpgrade?.Invoke(curCarryCapacityUpgrades);
            elementCount.text = curRessources + "/" + curCarryCapacityUpgrades;
        }
        if (curCarryCapacityUpgrades == maxCarryCapacityUpgrades)
        {
            carryCapacity.text = "Max Capacity";
        }
    }
    public void BuildSpeedUpOnClick(ClickEvent env)
    {
        if(curBuildSpeedUpgrades < maxBuildSpeedUpgrades) 
        {
            buildSpeedUp.text = "Time " + (BuildSpeedUpgradeBasicKost + (buildSpeedUpgradesKostIncreasement * curBuildSpeedUpgrades));
            curBuildSpeedUpgrades++;
            BuildSpeedUpgrade?.Invoke(curBuildSpeedUpgrades);
        }
        if (curBuildSpeedUpgrades == maxCarryCapacityUpgrades)
        {
            buildSpeedUp.text = "Max Time";
        }

    }
    public void SpeedUpOnClick(ClickEvent env)
    {
        if(curSpeedUpgrades < maxSpeedUpgrades) 
        {
            speedUp.text = "Speed " + (speedUpgradeBasicKost + (speedUpgradesKostIncreasement * curSpeedUpgrades));
            curSpeedUpgrades++;
            SpeedUpgrade?.Invoke(curSpeedUpgrades);
        }
        if(curSpeedUpgrades == maxSpeedUpgrades)
        {
            speedUp.text = "Max Speed";
        }
    }

    public void OnPlayerResourceUpdate((BuildingResource, int) ressource)
    {
        onTruck.style.backgroundImage = ressource.Item1.Icon.texture;
        curRessources = ressource.Item2;
        elementCount.text = curRessources + "/" + curCarryCapacityUpgrades;
        
    }

    public void OnScoreUpdate(int count)
    {
        scoreCount.text = count.ToString();
        PlayerPrefs.SetInt("HighScore", count);
        PlayerPrefs.Save();
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
