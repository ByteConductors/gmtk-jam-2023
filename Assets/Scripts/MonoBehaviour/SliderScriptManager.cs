using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SliderScriptManager : MonoBehaviour
{
    public static SliderScriptManager instance;
    private GameManager gameManager;

    [SerializeField]
    private GameObject sliderTime;

    private Dictionary<string, (GameObject, Vector3, float)> sliderTimes = new Dictionary<string, (GameObject, Vector3, float)>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        gameManager = GameManager.Instance;
    }

    public void AddSlider(string key, Vector3Int position,float start)
    {
        GameObject gameObject = Instantiate(sliderTime, new Vector3(((float)(position.x) + 0.5f), ((float)(position.y) + 1f), 0), Quaternion.identity);
        sliderTimes[key] = (gameObject, position, start);
       
    }
    public void UpdateSlidert(string key, float time)
    {
        float per = time - sliderTimes[key].Item3;
        per = 1 / per;
        sliderTimes[key].Item1.transform.GetChild(0).localScale -= new Vector3(per * Time.deltaTime, 0, 0);
    }
    public void SliderRemove(string key)
    {
        //Destroy(sliderTimes[key].Item1.transform.GetChild(0).gameObject);
        Destroy(sliderTimes[key].Item1);
        sliderTimes.Remove(key);
    }
}
 