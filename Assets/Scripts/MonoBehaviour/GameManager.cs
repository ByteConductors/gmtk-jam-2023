using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

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
            SwitchPhase();
            phaseTimer = Time.time + phaseTime;
        }
    }
    void SwitchPhase()
    {

    }
}
