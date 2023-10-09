using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOut : MonoBehaviour
{
    /// <summary>
    /// This Script Handles And Disable And Enabling The  Villager When They Enter And Leave A Building
    /// </summary>

    public GameObject GameObjectToDisable;
    private int HoursToEnable = 0;
    public VillagerController VC;
    [SerializeField]
    private float TimeOutDelayTime = 0.5f;
    [SerializeField]
    private Vector2 ComingBackDelayRange;

    private Building building;
    public void Disable(int HoursToEnable, Building buildingNowIn)
    {
        this.HoursToEnable = HoursToEnable;
        building = buildingNowIn;

        if (building.CheckCapacity())
        {
            building.users.Add(VC);
            StartCoroutine(TimeOutDelay());
        }
        else
        {

        }
    }

    public void Decrement()
    {
        HoursToEnable--;
        building.BuildingAction(VC.villagerInfo);
        if (HoursToEnable <= 0)
        {
            DayNightManager.Instance.NewHour -= Decrement;
            StartCoroutine(ComingBackDelay());
            building.users.Remove(VC);
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
        VC.OutOfTimeOut();
    }
}
