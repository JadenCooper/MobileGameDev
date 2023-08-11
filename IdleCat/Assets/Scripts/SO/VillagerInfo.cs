using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVillagerInfo", menuName = "Data/VillagerInfo")]
public class VillagerInfo : ScriptableObject
{
    public Schedule schedule;
    public Job job; // If Null Then Unemployed
    public House house; 
    // Villager Stats
    public float Speed;
    public string FirstName;
    public string LastName;

    public VillagerState currentState;
}
