using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class VillagerController : Intractable
{
    /// <summary>
    /// This Script Handles The Functionality Of The Villager From Moment To Interactions
    /// </summary>

    public VillagerInfo VI;
    public Navigation navigation;

    public UnityEvent<Vector2> OnMovementInput;
    public TimeOut timeOut;
    public void Initialize(VillagerInfo villagerInfo)
    {
        this.VI = villagerInfo;
        GetComponent<SpriteRenderer>().sprite = villagerInfo.Species.Sprite;
    }
    private void Start()
    {
        navigation = GetComponent<Navigation>();
        DayNightManager.Instance.NewHour += GetNewLocationGoal;
        GetNewLocationGoal();
    }

    private void Update()
    {
        if (VI.Moving)
        {
            OnMovementInput?.Invoke(DetermineMovement());
        }
    }
    public void ChangeState()
    {
        VI.currentState = VI.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x];
    }
    public void GetNewLocationGoal()
    {
        navigation.CheckSchedule(VI);
    }
    public Vector2 DetermineMovement()
    {
        // Gets Direction For Movement Based On Current Goal
        Vector2 Goal = new Vector2(VI.CurrentGoal.x, transform.position.y);
        return (Goal - (Vector2)transform.position).normalized;
    }

    public void ReachedLocation()
    {
        VI.currentState = VI.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x];
        VI.Moving = false;
        OnMovementInput?.Invoke(Vector2.zero);
        DayNightManager.Instance.NewHour -= GetNewLocationGoal;
        switch (VI.currentState)
        {
            case VillagerState.Home:
                timeOut.Disable(VI.house, VI.currentState);
                break;

            case VillagerState.Work:
                timeOut.Disable(VI.job, VI.currentState);
                break;

            case VillagerState.Recreation:
                timeOut.Disable(VI.recreationGoal, VI.currentState);
                break;

            case VillagerState.Anything:
                break;

            default:
                break;
        }
    }
    public void OutOfTimeOut()
    {
        VI.Moving = true;
        DayNightManager.Instance.NewHour += GetNewLocationGoal;
        GetNewLocationGoal();
    }

    public override void InteractAction()
    {
        UIManager.Instance.VillagerDisplayWindow.OpenWindow(VI);
    }
}
