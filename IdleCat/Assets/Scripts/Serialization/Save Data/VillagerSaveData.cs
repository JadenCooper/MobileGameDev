using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VillagerSaveData
{
    public string ID;
    public string Tag;

    public Vector3 VillagerPosition;
    public bool VillageInhabitant = false;
    public int PostponeTime = 0;


    // Villager Info Data
    public string FirstName;
    public string LastName;
    public VillagerState currentState = VillagerState.Home;

    public string JobID;
    public string HouseID;
    public string ElevatorID;
    public int CurrentLevel = 0;
    public Vector3 CurrentGoal;


    public Vector3 Age; // X is current age Y is the next life stage
    public LifeStages LifeStage;
    public string Sex;
    public float happiness;
    public float rest;

    public string MotherID;
    public string FatherID;
    public string PartnerID;
    public List<string> ChildrenID = new List<string>();

    public string Species;

    // Schedule
    public VillagerState[] VillagerStates;
    public bool Moving = true;
}
