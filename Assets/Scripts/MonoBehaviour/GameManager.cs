using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    const float ROADTHRESHOLD = .9f;
    const int MAX_TRIES = 5;
    const float ROAD_ACTION_TIME = 10;
    const float BUILDING_ACTION_TIME = 2f;
    const int STOREHOUSE_BUILD_ITTERATIONS = 6;
    int itterations;
    const int STOREHOUSE_MATERIAL_ITTERATIONS = 4;
    int matIttertations;
    int materials;
    bool first = true;

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

    Dictionary<string,float> timers = new Dictionary<string,float>();

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


    public void AddConstructionTimer(Vector3Int position, float deliveryTime)
    {
        timers.Add(position.ToString(), Time.time + deliveryTime);
    }
    void CheckTimer()
    {
        List<string> flagForRemoval = new List<string>();
        foreach (string key in timers.Keys)
        {
            if (timers[key] < Time.time) lives -= 1;
            flagForRemoval.Add(key);
        }
        foreach (var key in flagForRemoval) timers.Remove(key);
    }

    public bool RemoveTimer(Vector3Int position)
    {
        return timers.Remove(position.ToString());
    }

}
