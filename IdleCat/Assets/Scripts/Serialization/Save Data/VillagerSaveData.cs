using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VillagerSaveData
{
    public string ID;

    public Transform VillagerTransform;
    public bool VillageInhabitant = false;
    public int PostponeTime = 0;


    // Villager Info Data
    public string FirstName;
    public string LastName;
    public VillagerState currentState = VillagerState.Home;

    public string JobID;
    public string HouseID;
    public string RecreationID;
    public string ElevatorID;
    public int CurrentLevel = 0;
    public Vector2 CurrentGoal;


    public Vector2 Age; // X is current age Y is the next life stage
    public LifeStages LifeStage;
    public string Sex;
    public float happiness;
    public float rest;

    public string MotherID;
    public string FatherID;
    public string PartnerID;
    public List<string> ChildrenID = new List<string>();


    // Schedule
    public VillagerState[] VillagerStates;
    public bool Moving = true;

    public Transform transform;
}
