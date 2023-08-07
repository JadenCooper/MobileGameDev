using System.Collections;
using System.Collections.Generic;
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
        return (CurrentGoal - (Vector2)transform.position).normalized;
    }
    public void CheckForLocation(int CurrentTime)
    {
        // Will Be Triggered By Villager Manager Every Hour
        if (schedule.LocationNames[CurrentTime] != CurrentLocation)
        {
            CurrentTimeGoal = CurrentTime;
            CurrentGoal = schedule.Locations[CurrentTime];
            CurrentState = schedule.VillagerStates[CurrentTime];
        }
    }

    public void ReachedLocation()
    {
        CurrentLocation = schedule.LocationNames[CurrentTimeGoal];
        CurrentState = schedule.VillagerStates[CurrentTimeGoal];
    }

    public override void Interact()
    {
        throw new System.NotImplementedException();
    }
}
