using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    const float ROADTHRESHOLD = .9f;
    const int MAX_TRIES = 5;
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

    int lives = 10;
    public int Lives { get { return lives; } }

    float phaseTimer;
    public float phaseTime;
    int tries;

    Dictionary<string,(BuildingTile,float)> timers = new Dictionary<string, (BuildingTile, float)>();

    private void Awake()
    {
        phaseTimer = Time.deltaTime;
        phaseTime = Time.time;
        instance = this;
        actionTime = Time.time + 5f;
    }

    private void FixedUpdate()
    {
        CheckTimer();
        CheckActionTime();
    }

    private void CheckActionTime()
    {
        if (actionTime > Time.time) return;
        itterations++;
        if (itterations == STOREHOUSE_BUILD_ITTERATIONS)
        {
            itterations = 0;
            BuildBuildings(stores[Random.Range(0,Mathf.Min(materials,stores.Length))]);
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
        timers.Add(v3i2key(position), (tile, Time.time + deliveryTime));
        Debug.Log("Timer time: " + timers[v3i2key(position)].Item2);
    }
    void CheckTimer()
    {
        List<string> flagForRemoval = new List<string>();
        foreach (var key in timers.Keys)
        {
            if (timers[key].Item2 > Time.time) continue;
            lives -= 1;
            flagForRemoval.Add(key.ToString());
        }
        foreach (string key in flagForRemoval) timers.Remove(key);
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

}
