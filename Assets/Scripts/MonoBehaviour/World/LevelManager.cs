using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }
    public readonly Vector3 OFFSET = new Vector3(0.5f, 0.5f);

    [SerializeField]
    Tilemap buildingMap;

    [SerializeField]
    BuildingTile[] buildingTiles = new BuildingTile[0];

    [SerializeField]
    ConstructionSiteTile constructionSiteTile;

    public Tilemap BuildingMap { get; private set; }

    private void Awake()
    {
        instance = this;
        BuildBuilding(buildingTiles[0], new Vector3Int(1, 0, 0));
        BuildBuilding(buildingTiles[0], new Vector3Int(4, 4, 0));
        BuildBuilding(buildingTiles[0], new Vector3Int(2, 0, 0));
        BuildBuilding(buildingTiles[0], new Vector3Int(3, 0, 0));
        BuildBuilding(buildingTiles[0], new Vector3Int(4, 0, 0));
    }


    public void BuildBuilding(BuildingTile building, Vector3Int position)
    {
        var constrution = ScriptableObject.CreateInstance<ConstructionSiteTile>();
        constrution.SetBuilding(building, position);
        var actions = new List<PointerAction>
        {
            new PointerActionMove(() => buildingMap.SetTile(position, constrution), OFFSET + position)
        };
        PointerController.Instance.QueueActions(actions);
    }

    public void UpdateTile(TileBase tile, Vector3Int position)
    {
        
    }

    public TileBase GetTileAtPosition(Vector3 position)
    {
        return buildingMap.GetTile(v3tov3i(position));
    }
    Vector3Int v3tov3i(Vector3 position)
    {
        return new Vector3Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y), Mathf.RoundToInt(position.z));
    }
}
