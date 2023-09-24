using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSeasonsLightValues", menuName = "Data/SeasonsLightValues")]
public class SeasonLightValues : ScriptableObject
{
    public float LightIncreaseGoal;
    public float LightDecreaseGoal;
    public float LightChangeOverTime;
}
