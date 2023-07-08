using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "StorageTile")]
public class StorageTile : BuildingTile,ICollisionTile 
{
    [SerializeField]
    public BuildingResource storedResource;

    public void OnCollision() //change ressource that the player is carrying
    {
        if (PlayerController.instance == null) return;

        // if player is already carrying a resource, do nothing
        if (PlayerController.instance.getResource() != null) return;
        PlayerController.instance.setResource(storedResource);
        Debug.Log("Player is now carrying " + storedResource.name);
    }

}
