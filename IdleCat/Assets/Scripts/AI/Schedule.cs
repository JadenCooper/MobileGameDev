using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSchedule", menuName = "Data/Schedule")]
[System.Serializable]
public class Schedule : ScriptableObject
{
    public VillagerState[] VillagerStates;
}

[System.Serializable]
public enum VillagerState
{
    Home,
    Work,
    Recreation, // EG Parks, Market, Inn 
    Traveling, // Is An Inbetween State
    Petitioning,
    Anything
}
