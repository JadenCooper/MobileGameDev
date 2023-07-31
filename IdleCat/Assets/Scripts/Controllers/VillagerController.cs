using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VillagerController : MonoBehaviour
{
    public UnityEvent<Vector2> OnMovementInput;
    public Schedule schedule;
    public string CurrentLocation;
    public VillagerState CurrentState;
    public Vector2 CurrentGoal;
    private int CurrentTimeGoal;
    void Start()
    {
        
    }
    void Update()
    {
        if (CurrentState == VillagerState.Travelling)
        {
            OnMovementInput?.Invoke(DetermineMovement());
        }
    }
    public Vector2 DetermineMovement()
    {
        return
    }
    public void CheckForLocation(int CurrentTime)
    {
        if (schedule.LocationNames[CurrentTime] != CurrentLocation)
        {
            CurrentTimeGoal = CurrentTime;
        }
    }

    public void ReachedLocation()
    {
        CurrentLocation = schedule.LocationNames[CurrentTimeGoal];
        CurrentState = schedule.VillagerStates[CurrentTimeGoal];
    }
}
