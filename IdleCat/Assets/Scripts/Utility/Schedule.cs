using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Schedule : ScriptableObject
{
    public Vector2[] Locations;
    public string[] LocationNames;
    public VillagerState[] VillagerStates;
}

public enum VillagerState
{
    Idle,
    Travelling,
    Working,
    Sleeping,
    Petitioning
}
