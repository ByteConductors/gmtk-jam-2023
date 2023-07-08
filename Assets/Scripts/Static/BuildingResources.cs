using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Building Resource")]
public class BuildingResource : ScriptableObject
{
    public string DisplayName;
    public string Description;
    public Sprite Icon;
}
