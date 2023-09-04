using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class VillagerController : Intractable
{
    /// <summary>
    /// This Script Handles The Functionality Of The Villager From Moment To Interactions
    /// </summary>

    public VillagerInfo villagerInfo;
    private Navigation navigation;

    public UnityEvent<Vector2> OnMovementInput;
    public int CurrentLevel = 0;
    public bool moving = true;
    public TimeOut timeOut;
    public Elevator currentElevatorGoal;
    public void Initialize(VillagerInfo villagerInfo)
    {
        this.villagerInfo = villagerInfo;
        GetComponent<SpriteRenderer>().sprite = villagerInfo.Species.sprite;

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
    public void ChangeState()
    {
        villagerInfo.currentState = villagerInfo.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x - 6];
    }
    public void GetNewLocationGoal()
    {
        navigation.GetLocationGoal(this, villagerInfo);
    }
    public Vector2 DetermineMovement()
    {
        // Gets Direction For Movement Based On Current Goal
        Vector2 Goal = new Vector2(villagerInfo.CurrentGoal.x, transform.position.y);
        return (Goal - (Vector2)transform.position).normalized;
    }

    public void ReachedLocation()
    {
        villagerInfo.currentState = villagerInfo.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x - Data.TimeIndexIncrement];
        moving = false;
        OnMovementInput?.Invoke(Vector2.zero);
        DayNightManager.Instance.NewHour -= GetNewLocationGoal;

        bool StillCurrentState = true;
        int i = 0;
        switch (villagerInfo.currentState)
        {
            case VillagerState.Home:
                while (StillCurrentState)
                {
                    if (villagerInfo.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x - Data.TimeIndexIncrement + i] == villagerInfo.currentState)
                    {
                        i++;
                    }
                    else
                    {
                        StillCurrentState = false;
                    }
                }
                timeOut.Disable(i);
                break;

            case VillagerState.Work:
                while (StillCurrentState)
                {
                    if (villagerInfo.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x - Data.TimeIndexIncrement + i] == villagerInfo.currentState)
                    {
                        i++;
                    }
                    else
                    {
                        StillCurrentState = false;
                    }
                }
                timeOut.Disable(i);
                break;

            case VillagerState.Recreation:
                while (StillCurrentState)
                {
                    if (villagerInfo.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x - Data.TimeIndexIncrement + i] == villagerInfo.currentState)
                    {
                        i++;
                    }
                    else
                    {
                        StillCurrentState = false;
                    }
                }
                timeOut.Disable(i);
                break;

            //case VillagerState.Petitioning:
            //    break;

            default:
                break;
        }
    }

    public override void InteractAction()
    {
        throw new System.NotImplementedException();
    }

    public void OutOfTimeOut()
    {
        moving = true;
        DayNightManager.Instance.NewHour += GetNewLocationGoal;
        GetNewLocationGoal();
    }
}
