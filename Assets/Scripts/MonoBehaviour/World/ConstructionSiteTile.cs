using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Construction Tile")]
public class ConstructionSiteTile : Tile, ICollisionTile
{
    public BuildingTile buildingTile;
    public Vector3Int position;

    public void SetBuilding(BuildingTile building, Vector3Int position)
    {
        buildingTile = building;
        this.sprite = buildingTile.sprite;
        this.position = position;
        this.color = Color.gray;
    }

    public void OnPlace()
    {
        GameManager.Instance.AddConstructionTimer(buildingTile.resource, buildingTile.deliveryTime, this);
    }

    public void OnCollision()
    {
        Debug.Log("Collided With Construction Tile!");
    }
}
