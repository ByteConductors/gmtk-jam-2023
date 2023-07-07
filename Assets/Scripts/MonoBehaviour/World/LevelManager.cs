using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    [SerializeField]
    Tilemap buildingMap;
    public Tilemap BuildingMap { get; private set; }

    private void Awake()
    {
        instance = this;
    }


    /*public void BuildBuilding(Building building)
    {

    }*/
}
