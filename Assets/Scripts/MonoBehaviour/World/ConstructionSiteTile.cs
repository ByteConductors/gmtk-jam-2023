using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConstructionSiteTile : Tile
{
    BuildingTile buildingTile;

    public void SetBuilding(BuildingTile tile)
    {
        buildingTile = tile;
    }

    private void Awake()
    {
        
    }
}
