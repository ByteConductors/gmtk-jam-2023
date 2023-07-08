using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
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
    Vector3Int position;
    public Vector3Int Position { get => position; set => position = value; }



    private void Awake()
    {
    }
}
