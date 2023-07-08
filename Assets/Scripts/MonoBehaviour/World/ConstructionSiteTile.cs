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
        GameManager.Instance.AddConstructionTimer(position, buildingTile.deliveryTime);
    }

    public void OnCollision()
    {
        // Get the Players currently held Resource
        var resource = PlayerController.instance.getResource();
        // Check if resource is the same a required
        if (resource != buildingTile.resource) return;
        // if so, Tell the LevelManager to update the Tile
        LevelManager.Instance.UpdateTile(buildingTile, position);
        // And the GameManager to remove the Loss Timer;
        GameManager.Instance.RemoveTimer(position);
        Debug.Log("Collided With Construction Tile!");
    }
}
