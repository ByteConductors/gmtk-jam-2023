using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class PowerUpController : MonoBehaviour
{
    public Button speedButton;
    public Button carryCapacityButton;
    public Button buildSpeedButton;

    void Start()
    {
        speedButton.onClick.AddListener(SpeedUp);
        carryCapacityButton.onClick.AddListener(CarryCapacityUp);
        buildSpeedButton.onClick.AddListener(BuildSpeedUp);
    }

    void SpeedUp()
    {
        // <env>.instance.speed += 1;
    }

    void CarryCapacityUp()
    {
        // <env>.instance.maxCarry += 1;
    }

    void BuildSpeedUp()
    {
        // <env>.instance.buildSpeed += 1;
    }


}
