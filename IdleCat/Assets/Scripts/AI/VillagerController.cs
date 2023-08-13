using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VillagerController : Intractable
{
    public VillagerInfo villagerInfo;
    private Navigation navigation;

    public UnityEvent<Vector2> OnMovementInput;
    private int CurrentLevel = 0;
    public bool moving = true;
    public void Initialize(VillagerInfo villagerInfo)
    {
        this.villagerInfo = villagerInfo;
    }
    private void Start()
    {
        navigation = GetComponent<Navigation>();
        DayNightManager.Instance.NewHour += GetNewLocationGoal;
        GetNewLocationGoal();
    }

    private void Update()
    {
        if (moving)
        {
            OnMovementInput?.Invoke(DetermineMovement());
        }
    }

    public void GetNewLocationGoal()
    {
        Debug.Log("New Location");
        navigation.GetLocationGoal(villagerInfo, CurrentLevel);
    }
    public Vector2 DetermineMovement()
    {
        // Gets Direction For Movement Based On Current Goal
        Vector2 Goal = new Vector2(villagerInfo.CurrentGoal.x, transform.position.y);
        return (Goal - (Vector2)transform.position).normalized;
    }

    public void ReachedLocation()
    {
        Debug.Log("Reached");

        villagerInfo.currentState = villagerInfo.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x];

        switch (villagerInfo.currentState)
        {
            case VillagerState.Home:
                break;
            case VillagerState.Work:
                break;
            //case VillagerState.Recreation:
            //    break;
            //case VillagerState.Petitioning:
            //    break;
            default:
                break;
        }

        moving = false;
        OnMovementInput?.Invoke(Vector2.zero);
    }

    public override void InteractAction()
    {
        throw new System.NotImplementedException();
    }

}
