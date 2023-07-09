using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "BuildingTile")]
public class BuildingTile : Tile
{
    public string BuildingName;
    public string BuildingType;
    public BuildingResource resource;
    public float deliveryTime;
    public Vector2Int size;
    public int baseScore;
    Vector3Int position;
    public Vector3Int Position { get => position; set => position = value; }

    public virtual void OnPlace() { }
    private void Awake()
    {
    }
}
