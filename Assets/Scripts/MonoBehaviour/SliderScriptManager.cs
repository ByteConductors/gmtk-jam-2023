using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SliderScriptManager : MonoBehaviour
{
    public static SliderScriptManager instance;

    [SerializeField]
    private GameObject sliderTime;
    private List<Vector3> postionsremove;

    private Dictionary<Vector3, (GameObject, float, float, Vector3)> sliderTimes = new Dictionary<Vector3, (GameObject, float, float, Vector3)>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        postionsremove = new List<Vector3>();
    }

    public void AddSlider(Vector3 position,float start, float deliverytime)
    {
        sliderTimes[position] = (Instantiate(sliderTime, new Vector3(((float)(position.x)), ((float)(position.y)+0.2f), 1), Quaternion.identity), start, deliverytime, position);
       
    }
    void Update()
    {
        GameObject child;
        float per;
        List<Vector3> newlist = postionsremove;
        postionsremove.Clear();
        foreach(Vector3 position in newlist) {
            Destroy(sliderTimes[position].Item1);
            sliderTimes.Remove(position);
        }
        Dictionary<Vector3, (GameObject, float, float, Vector3)> newsliderTimes = new Dictionary<Vector3, (GameObject, float, float, Vector3)>(); ;
        foreach ((GameObject, float, float, Vector3) slider in sliderTimes.Values)
        {

            child = slider.Item1.transform.GetChild(0).gameObject;
            per = (slider.Item3 + slider.Item2) - slider.Item2;
            per = 1 / per;
            if (child.transform.localScale.x > 0f && per * Time.deltaTime >=0 && per * Time.deltaTime <= 1)
            {
                slider.Item1.transform.GetChild(0).transform.localScale -= new Vector3(per * Time.deltaTime, 0, 0);
                newsliderTimes[slider.Item4] = slider;
            } 
        }
        sliderTimes = newsliderTimes;   
    }
    public void SliderRemove(Vector3 pos)
    {
        postionsremove.Add(pos);
    }
}
