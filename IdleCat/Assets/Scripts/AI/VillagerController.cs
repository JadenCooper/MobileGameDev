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
    public TimeOut timeOut;
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

            //case VillagerState.Recreation:
            //    break;

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
