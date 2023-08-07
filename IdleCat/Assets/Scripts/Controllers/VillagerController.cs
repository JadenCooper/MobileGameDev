using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class VillagerController : Intractable
{
    public UnityEvent<Vector2> OnMovementInput;
    public Schedule schedule;
    public string CurrentLocation;
    public VillagerState CurrentState;
    public Vector2 CurrentGoal;
    private int CurrentTimeGoal;

    private VillagerManager villagerManager;
    private float CurrentLevel = 0;
    public void Initialize(VillagerManager vM)
    {
        villagerManager = vM;
    }
    void Update()
    {
        if (CurrentState == VillagerState.Travelling)
        {
            if (CurrentGoal == (Vector2)transform.position)
            {
                ReachedLocation();
            }
            else
            {
                OnMovementInput?.Invoke(DetermineMovement());
            }
        }
    }
    public Vector2 DetermineMovement()
    {
        // Gets Direction For Movement Based On Current Goal
        Vector2 direction;
        if (CurrentGoal.y != CurrentLevel)
        {
            // Need To Find Elevator
            CurrentGoal = villagerManager.GetClosestElevator(CurrentLevel, CurrentGoal.y, (Vector2)transform.position);
        }
        direction = (CurrentGoal - (Vector2)transform.position).normalized;
        return direction;
    }
    public void CheckForLocation(int CurrentTime)
    {
        // Will Be Triggered By Villager Manager Every Hour
        if (schedule.LocationNames[CurrentTime] != CurrentLocation)
        {
            CurrentTimeGoal = CurrentTime;
            CurrentGoal = schedule.Locations[CurrentTime];
            CurrentState = VillagerState.Travelling;
        }
    }

    public void ReachedLocation()
    {
        CurrentLocation = schedule.LocationNames[CurrentTimeGoal];
        CurrentState = schedule.VillagerStates[CurrentTimeGoal];
    }

    public override void InteractAction()
    {
        throw new System.NotImplementedException();
    }
}
