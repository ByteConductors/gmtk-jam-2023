using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance { get => instance; }

    float phaseTimer;
    public float phaseTime;

    private void Awake()
    {
        phaseTimer = Time.deltaTime;
        phaseTime = Time.time;
        instance = this;
    }

    private void FixedUpdate()
    {

        if (phaseTimer < Time.time)
        {
            phaseTimer = Time.time + phaseTime;
        }
    }

    public void AddConstructionTimer(BuildingResource resource, float deliveryTime, ConstructionSiteTile tile)
    {

    }

    public void OnTileCollision(TileBase tile)
    {
        ((ICollisionTile)tile).OnCollision();
    }

}
