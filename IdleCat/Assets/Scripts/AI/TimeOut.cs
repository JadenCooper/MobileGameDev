using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOut : MonoBehaviour
{
    public GameObject GameObjectToDisable;
    private int HoursToEnable = 0;
    public VillagerController VC;
    private float TimeOutDelayTime = 0.5f;
    public void Disable(int HoursToEnable)
    {
        this.HoursToEnable = HoursToEnable;
        StartCoroutine(TimeOutDelay()); ;
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

    private IEnumerator TimeOutDelay()
    {
        // Play Building Entry Animation Here At Somepoint
        yield return new WaitForSeconds(TimeOutDelayTime);
        DayNightManager.Instance.NewHour += Decrement;
        GameObjectToDisable.SetActive(false);
    }
}
