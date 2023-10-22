using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVillagerInfo", menuName = "Data/VillagerInfo")]
public class VillagerInfo : ScriptableObject
{
    public Schedule schedule;
    public Job job; // If Null Then Unemployed
    public House house;
    public Recreation recreationGoal;
    // Villager Stats
    public float Speed;
    public string FirstName;
    public string LastName;
    public VillagerState currentState = VillagerState.Home;

    public SpeciesData Species;

    public float happiness;
    public float rest;

    public Vector2 CurrentGoal;
    public Elevator currentElevatorGoal;
    public GameObject gameObject;
    public int CurrentLevel = 0;


    public int Age;
    public string Sex;
    public VillagerInfo Father;
    public VillagerInfo Mother;
    public VillagerInfo Partner;
    public List<VillagerInfo> Children = new List<VillagerInfo>();
}
