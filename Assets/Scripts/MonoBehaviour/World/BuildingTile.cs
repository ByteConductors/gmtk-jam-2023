using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
           
    }
}
