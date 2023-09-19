using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSchedule", menuName = "Data/Schedule")]
public class Schedule : ScriptableObject
{
    public VillagerState[] VillagerStates;
}

public enum VillagerState
{
    Home,
    Work,
    Recreation, // EG Parks, Market, Inn 
    Traveling, // Is An Inbetween State
    Petitioning,
    Anything
}
