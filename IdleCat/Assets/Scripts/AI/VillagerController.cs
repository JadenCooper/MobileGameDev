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
    private bool moving = true;
    public TimeOut timeOut;
    public void Initialize(VillagerInfo villagerInfo)
    {
        this.villagerInfo = villagerInfo;
        //GetComponent<SpriteRenderer>().sprite = villagerInfo.Species.Sprite;
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
        villagerInfo.currentState = villagerInfo.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x];
    }
    public void GetNewLocationGoal()
    {
        navigation.CheckSchedule(villagerInfo);
    }
    public Vector2 DetermineMovement()
    {
        // Gets Direction For Movement Based On Current Goal
        Vector2 Goal = new Vector2(villagerInfo.CurrentGoal.x, transform.position.y);
        return (Goal - (Vector2)transform.position).normalized;
    }

    public void ReachedLocation()
    {
        villagerInfo.currentState = villagerInfo.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x];
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
                    if ((int)DayNightManager.Instance.CurrentTime.x + i > (villagerInfo.schedule.VillagerStates.Length - 1))
                    {
                        StillCurrentState = false;
                    }
                    else
                    {
                        if (villagerInfo.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x + i] == villagerInfo.currentState)
                        {
                            i++;
                        }
                        else
                        {
                            StillCurrentState = false;
                        }
                    }
                }
                timeOut.Disable(i, villagerInfo.house);
                break;

            case VillagerState.Work:
                while (StillCurrentState)
                {
                    if ((int)DayNightManager.Instance.CurrentTime.x + i > (villagerInfo.schedule.VillagerStates.Length - 1))
                    {
                        StillCurrentState = false;
                    }
                    else
                    {
                        if (villagerInfo.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x + i] == villagerInfo.currentState)
                        {
                            i++;
                        }
                        else
                        {
                            StillCurrentState = false;
                        }
                    }
                }
                timeOut.Disable(i, villagerInfo.job);
                break;

            case VillagerState.Recreation:
                while (StillCurrentState)
                {
                    if ((int)DayNightManager.Instance.CurrentTime.x + i > (villagerInfo.schedule.VillagerStates.Length - 1))
                    {
                        StillCurrentState = false;
                    }
                    else
                    {
                        if (villagerInfo.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x + i] == villagerInfo.currentState)
                        {
                            i++;
                        }
                        else
                        {
                            StillCurrentState = false;
                        }
                    }
                }
                timeOut.Disable(i, villagerInfo.recreationGoal);
                villagerInfo.recreationGoal = null;
                break;

            //case VillagerState.Petitioning:
            //    break;

            default:
                break;
        }
    }
    public void OutOfTimeOut()
    {
        moving = true;
        DayNightManager.Instance.NewHour += GetNewLocationGoal;
        GetNewLocationGoal();
    }

    public override void InteractAction()
    {

    }
}
