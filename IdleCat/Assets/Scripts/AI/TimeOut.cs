using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOut : MonoBehaviour
{
    /// <summary>
    /// This Script Handles And Disable And Enabling The  Villager When They Enter And Leave A Building
    /// </summary>

    public GameObject GameObjectToDisable;
    [SerializeField]
    private VillagerController vc;
    [SerializeField]
    private float TimeOutDelayTime = 0.5f;
    [SerializeField]
    private Vector2 ComingBackDelayRange;

    private Building building;
    private VillagerState buildingState;
    public void Disable(Building buildingNowIn, VillagerState currentState)
    {
        building = buildingNowIn;
        buildingState = currentState;

        if (building.CheckCapacity())
        {
            building.users.Add(vc.VI);
            StartCoroutine(TimeOutDelay());
        }
    }

    public void Decrement()
    {
        building.BuildingAction(vc.VI);
        if (vc.VI.schedule.VillagerStates[(int)DayNightManager.Instance.CurrentTime.x] != buildingState)
        {
            DayNightManager.Instance.NewHour -= Decrement;
            StartCoroutine(ComingBackDelay());
            building.users.Remove(vc.VI);
        }
    }

    private IEnumerator TimeOutDelay()
    {
        // Play Building Entry Animation Here At Somepoint
        yield return new WaitForSeconds(TimeOutDelayTime);
        DayNightManager.Instance.NewHour += Decrement;
        GameObjectToDisable.SetActive(false);
    }

    private IEnumerator ComingBackDelay()
    {
        // Play Building Exit Animation Here At Somepoint
        yield return new WaitForSeconds(Random.Range(ComingBackDelayRange.x, ComingBackDelayRange.y));
        GameObjectToDisable.SetActive(true);
        vc.OutOfTimeOut();
    }
}
