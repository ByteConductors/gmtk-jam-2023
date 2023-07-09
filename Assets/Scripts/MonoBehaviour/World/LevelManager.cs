using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }
    public readonly Vector3 OFFSET = new Vector3(0.5f, 0.5f);

    public List<Vector3Int> roadPositions = new List<Vector3Int>();
    public List<Vector3Int> buildingPositions = new List<Vector3Int>();

    [SerializeField]
    Tilemap buildingMap;

    [SerializeField]
    BuildingTile[] buildingTiles = new BuildingTile[0];

    [SerializeField]
    ConstructionSiteTile constructionSiteTile;

    [SerializeField]
    TileBase roadTile;

    [SerializeField]
    StorageTile[] storageTiles;
    public StorageTile[] StorageTiles { get => storageTiles; }

    public Tilemap BuildingMap { get => buildingMap; private set=> buildingMap = value; }
    [SerializeField]
    Tilemap roadMap;
    public Tilemap RoadMap { get => roadMap; private set => roadMap = value; }

    private void Awake()
    {
        instance = this;
        roadMap.SetTile(new Vector3Int(0, 0), roadTile);
        roadPositions.Add(new Vector3Int());
        var tile = ScriptableObject.CreateInstance<StorageTile>().CloneContents(storageTiles[0]);
        tile.Position = new Vector3Int(0, 1);
        buildingMap.SetTile(new Vector3Int(0, 1), tile);
        buildingPositions.Add(new Vector3Int(0, 1));
        buildingPositions.Add(new Vector3Int(0, 2));
        tile.OnPlace();
    }


    public void BuildrandomBuilding(Vector3Int position)
    {
        var tile = Random.Range(0, buildingTiles.Length);
        BuildBuilding(buildingTiles[tile], position);
    }

    public void BuildBuilding(BuildingTile building, Vector3Int position)
    {
        buildingPositions.Add(position);
        building.Position = position;
        var constrution = ScriptableObject.CreateInstance<ConstructionSiteTile>();
        constrution.SetBuilding(building, position);
        var actions = new List<PointerAction>
        {
            new PointerActionMove(() => {
                buildingMap.SetTile(position, constrution);
                constrution.OnPlace();
            }, 
                OFFSET + position)
        };
        PointerController.Instance.QueueActions(actions);
    }

    public void BuildRoad(Vector3Int position)
    {
        roadPositions.Add(position);
        var actions = new List<PointerAction>
        {
            new PointerActionMove(() =>  {
                RoadMap.SetTile(position, roadTile);
            }, 
            OFFSET + position)
        };
        PointerController.Instance.QueueActions(actions);
    }

    public void UpdateTile(BuildingTile tile, Vector3Int position)
    {
        tile.OnPlace();
        buildingMap.SetTile(position, tile);
    }

    public bool TryGetCollisionTile(Vector3 position, out ICollisionTile tile)
    {
        Debug.Log("Trying to get Collision tile at Position: " + position.ToString());
        var _t = GetTileAtPosition(position);
        Debug.Log($"Tile was {(_t == null ? "found" : "not found")}!");
        if (_t != null && typeof(ICollisionTile).IsAssignableFrom(_t.GetType()))
        {
            Debug.Log("Found Collision Tile");
            tile = (ICollisionTile) _t;
            return true;
        }
        Debug.Log("Did not find Collision Tile");
        tile = null;
        return false;
    }

    Vector3 lastConversion;

    public TileBase GetTileAtPosition(Vector3 position)
    {
        Debug.Log(v3tov3i(position));
        lastConversion = v3tov3i(position);
        return buildingMap.GetTile(v3tov3i(position));
    }
    Vector3Int v3tov3i(Vector3 position)
    {
        int x = position.x < 0 ? (int)position.x - 1 : (int)position.x;
        int y = position.y < 0 ? (int)position.y - 1 : (int)position.y;

        return new Vector3Int( x, y, 0);
    }

    public Vector3Int GetRandomRoad()
    {
        return roadPositions[Random.Range(0,roadPositions.Count)];
    }

    public Vector3Int GetAvailableNeighbour(Vector3Int road)
    {
        if (GetAvailable(road + Vector3Int.left)) return road + Vector3Int.left; 
        if (GetAvailable(road + Vector3Int.right)) return road + Vector3Int.right;
        if (GetAvailable(road + Vector3Int.up)) return road + Vector3Int.up;
        if (GetAvailable(road + Vector3Int.down)) return road + Vector3Int.down;
        return GetAvailableNeighbour(GetRandomRoad());
    }

    public bool GetAvailable(Vector3Int position) => 
        !buildingPositions.Contains(position) && 
        !roadPositions.Contains(position);
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(lastConversion + Vector3.one * 0.5f, Vector3.one);
    }
   
}
