using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Settings", menuName = "Settings", order = 51)]
public class Settings : ScriptableObject
{   
    public Color highLightColor;

    [Range(20,65)]
    public int difficulty;
}
