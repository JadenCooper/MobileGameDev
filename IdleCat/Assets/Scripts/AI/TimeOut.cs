using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOut : MonoBehaviour
{
    public GameObject GameObjectToDisable;
    private int HoursToEnable = 0;
    public VillagerController VC;
    public void Disable(int HoursToEnable)
    {
        this.HoursToEnable = HoursToEnable;
        DayNightManager.Instance.NewHour += Decrement;
        GameObjectToDisable.SetActive(false);
    }

    public void Decrement()
    {
        HoursToEnable--;
        if (HoursToEnable <= 0)
        {
            DayNightManager.Instance.NewHour -= Decrement;
            GameObjectToDisable.SetActive(true);
            VC.OutOfTimeOut();
        }
    }
}
