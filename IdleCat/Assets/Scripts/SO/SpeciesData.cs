using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSpeciesData", menuName = "Data/SpeciesData")]
public class SpeciesData : ScriptableObject
{
    public string Species;
    public Sprite Sprite;
    public int FoodUsage;
}
