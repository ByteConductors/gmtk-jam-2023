using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseUi : MonoBehaviour
{
    UIDocument baseUI;
    ProgressBar deliverTimeBar;
    public float speed;

    void OnEnable()
    {
        baseUI = GetComponent<UIDocument>();
        if (baseUI == null )
        {
            Debug.Log("ERROR");
        }
        deliverTimeBar = baseUI.rootVisualElement.Q("DeliverTimeBar") as ProgressBar;
        if (deliverTimeBar != null)
        {
            Debug.Log("Bar found");
        }
    }
    void Update() {
        deliverTimeBar.value += speed * Time.deltaTime;
    }
}
