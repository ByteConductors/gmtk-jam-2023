using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUpController : MonoBehaviour
{

    public PowerUpController instance;

    public int upgradePoints = 0;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        upgradePoints = 0;

        BaseUI.instance.SpeedUpgrade += SpeedUp;
        BaseUI.instance.CarryCapacityUpgrade += CarryCapacityUp;
        BaseUI.instance.BuildSpeedUpgrade += BuildSpeedUp;
    }

    void SpeedUp(int price)
    {
        if (upgradePoints >= price){
            upgradePoints -= price;
            PlayerController.instance.speed += 1;
        }
    }

    void CarryCapacityUp(int price)
    {
        if (upgradePoints >= price){
            upgradePoints -= price;
            PlayerController.instance.maxCarry += 1;
        }
    }

    void BuildSpeedUp(int price)
    {
        if (upgradePoints >= price){
            upgradePoints -= price;
            GameManager.Instance.ROAD_ACTION_TIME += 1f;
            GameManager.Instance.BUILDING_ACTION_TIME += 1f;
        }
    }


}
