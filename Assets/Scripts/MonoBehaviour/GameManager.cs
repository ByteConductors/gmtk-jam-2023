using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    const float ROADTHRESHOLD = .9f;
    const int MAX_TRIES = 3;
    public float ROAD_ACTION_TIME = 10;
    public float BUILDING_ACTION_TIME = 2f;
    const int STOREHOUSE_BUILD_ITTERATIONS = 6;
    int itterations;
    const int STOREHOUSE_MATERIAL_ITTERATIONS = 4;
    int matIttertations;
    int materials;
    bool first = true;

    int score;
    public event System.Action<int> OnScoreUpdate;
    static GameManager instance;
    public static GameManager Instance { get => instance; }

    [SerializeField]
    StorageTile[] stores;

    public float actionTime;
    public event System.Action<int> OnLiveUpdate;
    int lives = 10;
    public int Lives { get { return lives; } }

    float phaseTimer;
    public float phaseTime;
    int tries;

    Dictionary<string,(BuildingTile,float)> timers = new Dictionary<string, (BuildingTile, float)>();

    [SerializeField]  int skillPoints = 1000;
    public event System.Action<int> OnSkillPointUpdate;

    public event System.Action<int> CarryCapacityUpgrade;
    public event System.Action<int> BuildSpeedUpgrade;
    public event System.Action<int> SpeedUpgrade;

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

    private int curCarryCapacityUpgrades = 1;
    private int curBuildSpeedUpgrades = 1;
    private int curSpeedUpgrades = 1;

    public bool isGameOver;

    private void Awake()
    {
        phaseTimer = Time.deltaTime;
        phaseTime = Time.time;
        instance = this;
        actionTime = Time.time + 5f;

        isGameOver = false;

        lives = 0;

    }

    private void FixedUpdate()
    {
        CheckTimer();
        CheckActionTime();

        if (!isGameOver && lives <= 0)
        {
            isGameOver = true;
            Debug.Log("Game Over - GameManager Awake");
            StartCoroutine(Bootloader.Instance.GameOver());
        }
    }

    private void CheckActionTime()
    {
        if (actionTime > Time.time) return;
        itterations++;
        if (itterations == STOREHOUSE_BUILD_ITTERATIONS)
        {
            itterations = 0;
            var store = stores[Random.Range(0, Mathf.Min(materials, stores.Length))];
            var instance = ScriptableObject.CreateInstance<StorageTile>().CloneContents(store);
            BuildBuildings(instance);
            matIttertations++;
        }
        if (matIttertations == STOREHOUSE_MATERIAL_ITTERATIONS) {
            matIttertations = 0;
            materials++; 
        }
        if (first)
        {
            BuildRoads();
            first = false;
        }
        var num = Random.Range(0f, 1f);
        if (tries >= MAX_TRIES || num > ROADTHRESHOLD)
        {
            tries = 0;
            BuildRoads();
        }else
        {
            tries++;
            BuildBuildings();
        }
    }

    public void BuildBuildings(BuildingTile building = null)
    {
        actionTime = Time.time + BUILDING_ACTION_TIME;
        var pos = LevelManager.Instance.GetAvailableNeighbour(LevelManager.Instance.GetRandomRoad());
        if (!building) LevelManager.Instance.BuildrandomBuilding(pos);
        else LevelManager.Instance.BuildBuilding(building, pos);
    }

    public void BuildRoads()
    {
        actionTime = Time.time + ROAD_ACTION_TIME;
        var position = LevelManager.Instance.GetAvailableNeighbour(LevelManager.Instance.GetRandomRoad());
        if (position == null) return;
        Vector3Int[] directions = new Vector3Int[] { Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right };
        int offset = Random.Range(0, directions.Length);
        Vector3Int direction = Vector3Int.right;
        for (int i = 0; i < directions.Length; i++)
        {
            var _dir = directions[(i + offset) % directions.Length];
            if (LevelManager.Instance.GetAvailable(position + _dir))
            {
                direction = _dir;
                break;
            };
        }
        int roadCount = Random.Range(4, 7);
        for (int i = 0; i < roadCount; i++)
        {
            if (!LevelManager.Instance.GetAvailable(position + direction * i)) break;
            Debug.Log(position + direction * i);
            Debug.Log(LevelManager.Instance.GetAvailable(position + direction * i));
            LevelManager.Instance.BuildRoad(position + direction * i);
        }
    }


    public void AddConstructionTimer(Vector3Int position,BuildingTile tile, float deliveryTime)
    {
        Debug.Log("Adding timer at:" + v3i2key(position));
        float starttime = Time.time;
        timers.Add(v3i2key(position), (tile, starttime + deliveryTime));
        SliderScriptManager.instance.AddSlider(v3i2key(position), position, starttime);
        Debug.Log("Timer time: " + timers[v3i2key(position)].Item2);
    }
    public (BuildingTile, float) GetTimer(Vector3Int position)
    {
        (BuildingTile, float) time;
        timers.TryGetValue(v3i2key(position), out time);
        return time;
    }
    void CheckTimer()
    {
        List<string> flagForRemoval = new List<string>();
        foreach (var key in timers.Keys)
        {
            SliderScriptManager.instance.UpdateSlidert(key, timers[key].Item2);
            if (timers[key].Item2 > Time.time) continue;
            lives -= 1;
            OnLiveUpdate?.Invoke(lives);
            flagForRemoval.Add(key.ToString());
        }
        foreach (string key in flagForRemoval)
        {
            timers.Remove(key);
            SliderScriptManager.instance.SliderRemove(key);
        }
    }

    public bool RemoveTimer(Vector3Int position)
    {
        string key = v3i2key(position);
        if (!timers.ContainsKey(key))
        {
            Debug.Log($"Couldn't find Timer {key}, aborting.");
            return false;
        }
        float timeLeft = timers[key].Item2 - Time.time;
        uint score = (uint)(timers[key].Item1.baseScore * (timers[key].Item1.deliveryTime / timeLeft));
        Debug.Log("Adding score: " + score);
        AddScore(score);
        SliderScriptManager.instance.SliderRemove(key);
        var success = timers.Remove(key);
        return success;
    }

    public static string v3i2key(Vector3Int position)
    {
        return $"{position.x};{position.y};{position.z}";
    }

    public void AddScore(uint points)
    {
        score += (int)points;
        OnScoreUpdate?.Invoke(score);
    }

    public (int, int) UpgardeCapayity()
    {
        int kosten = carryCapacityUpgradeBasicKost + (carryCapacityUpgradeKostIncreasement * (curCarryCapacityUpgrades -1));
        if (curCarryCapacityUpgrades < maxCarryCapacityUpgrades && skillPoints >= kosten)
        {
            skillPoints -= kosten;
            OnSkillPointUpdate?.Invoke(skillPoints);
            curCarryCapacityUpgrades++;
            CarryCapacityUpgrade?.Invoke(curCarryCapacityUpgrades);
        }
        return (curCarryCapacityUpgrades, maxCarryCapacityUpgrades);
    }
    public (int, int) UpgardeSpeed()
    {
        int kosten = speedUpgradeBasicKost + (speedUpgradesKostIncreasement * (curSpeedUpgrades - 1));
        if (curSpeedUpgrades < maxSpeedUpgrades && skillPoints >= kosten)
        {
            skillPoints -= kosten;
            OnSkillPointUpdate?.Invoke(skillPoints);
            curSpeedUpgrades++;
            SpeedUpgrade?.Invoke(curCarryCapacityUpgrades);
        }
        return (curSpeedUpgrades, maxSpeedUpgrades);
    }
    public (int, int) UpgardeTime()
    {
        int kosten = BuildSpeedUpgradeBasicKost + (buildSpeedUpgradesKostIncreasement * (curBuildSpeedUpgrades - 1));
        if (curBuildSpeedUpgrades < maxBuildSpeedUpgrades && skillPoints >= kosten)
        {
            skillPoints -= kosten;
            OnSkillPointUpdate?.Invoke(skillPoints);
            curBuildSpeedUpgrades++;
            BuildSpeedUpgrade?.Invoke(curCarryCapacityUpgrades);
        }
        return (curBuildSpeedUpgrades, maxBuildSpeedUpgrades);
    }
    public (int, int) GetUpgardeTime()
    {
        return (curBuildSpeedUpgrades, BuildSpeedUpgradeBasicKost + (buildSpeedUpgradesKostIncreasement * (curBuildSpeedUpgrades - 1)));
    }
    public (int, int) GetUpgardeSpeed()
    {
        return (curSpeedUpgrades, speedUpgradeBasicKost + (speedUpgradesKostIncreasement * (curSpeedUpgrades - 1)));
    }
    public (int, int) GetUpgradeCapasyity()
    {
        return (curCarryCapacityUpgrades, carryCapacityUpgradeBasicKost + (carryCapacityUpgradeKostIncreasement * (curCarryCapacityUpgrades - 1)));
    }
}
