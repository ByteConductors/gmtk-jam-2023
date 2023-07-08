using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextBoubbleManager : MonoBehaviour
{
    public static ContextBoubbleManager instance;

    [SerializeField]
    private GameObject contextBoubblePrefab;

    private Dictionary<Vector3Int, GameObject> contextBoubbles = new Dictionary<Vector3Int, GameObject>();

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public void showContextBoubble(Vector3Int position, Sprite icon)
    {
        contextBoubbles[position] = Instantiate(contextBoubblePrefab, new Vector3(((float)(position.x)+0.5f), ((float)(position.y)+1f),0), Quaternion.identity);
        contextBoubbles[position].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = icon;
    }

    public void hideContextBoubble(Vector3Int position)
    {
        if (contextBoubbles.ContainsKey(position))
        {
            Destroy(contextBoubbles[position]);
            contextBoubbles.Remove(position);
        }
    }
}
