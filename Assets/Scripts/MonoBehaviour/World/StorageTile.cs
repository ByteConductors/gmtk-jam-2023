using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "StorageTile")]
public class StorageTile : BuildingTile,ICollisionTile 
{
    [SerializeField]
    public BuildingResource storedResource;


    public override void OnPlace()
    {
        if (Position == null || Position == new Vector3Int(0,0,0)) return;
        Debug.Log("Storage Position: " + Position);
        ContextBoubbleManager.instance.showContextBoubble(Position, storedResource.Icon);
    }

    public void OnCollision() //change ressource that the player is carrying
    {
        if (PlayerController.instance == null) return;

        // if player is already carrying a resource, do nothing
        //if (PlayerController.instance.getResource() != null) return;
        PlayerController.instance.setResource(storedResource);
        Debug.Log("Player is now carrying " + storedResource.name);
    }

}
