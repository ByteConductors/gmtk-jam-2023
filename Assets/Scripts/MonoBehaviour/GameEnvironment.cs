using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnvironment : MonoBehaviour
{
    private static GameEnvironment instance;
    public static GameEnvironment Instance { get => instance; private set => instance = value; }

    private void Awake()
    {
        instance = this;
    }
}
