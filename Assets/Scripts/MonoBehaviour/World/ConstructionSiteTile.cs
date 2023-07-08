using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Construction Tile")]
public class ConstructionSiteTile : BuildingTile, ICollisionTile
{
    public BuildingTile buildingTile;

    public void SetBuilding(BuildingTile building, Vector3Int position)
    {
        buildingTile = building;
        sprite = buildingTile.sprite;
        Position = position;
        color = Color.gray;
    }

    public override void OnPlace()
    {
        GameManager.Instance.AddConstructionTimer(Position, buildingTile, buildingTile.deliveryTime);
    }

    public void OnCollision()
    {
        // Get the Players currently held Resource
        Debug.Log("Getting Player resouce!");
        var resource = PlayerController.instance.getResource();
        // Check if resource is the same a required
        if (resource != buildingTile.resource) return;
        // if so, Tell the LevelManager to update the Tile
        Debug.Log("Getting Player resouce!");
        LevelManager.Instance.UpdateTile(buildingTile, Position);
        // And the GameManager to remove the Loss Timer;
        Debug.Log("Removing Timer!");
        GameManager.Instance.RemoveTimer(Position);
        PlayerController.instance.UseResource();
        buildingTile.OnPlace();
        Debug.Log("Collided With Construction Tile!");
    }
}
