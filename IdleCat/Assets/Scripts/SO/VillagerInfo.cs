using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVillagerInfo", menuName = "Data/VillagerInfo")]
[System.Serializable]
public class VillagerInfo : ScriptableObject
{
    public string ID;
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

    public float happiness = 100;
    public float rest = 100;

    public Vector2 CurrentGoal;
    public Elevator currentElevatorGoal;
    public GameObject gameObject;
    public int CurrentLevel = 0;


    public Vector2 Age; // X is current age Y is the next life stage
    public LifeStages LifeStage;
    public string Sex;
    public VillagerInfo Father;
    public VillagerInfo Mother;
    public VillagerInfo Partner;
    public List<VillagerInfo> Children = new List<VillagerInfo>();
    public bool Moving = true;
}
