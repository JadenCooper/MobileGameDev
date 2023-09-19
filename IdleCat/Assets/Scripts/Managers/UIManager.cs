using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TMP_Text clockText;
    private bool longHourTime = true; // 24 hour time
    private float currentHour = 6;
    public void UpdateClockTime(float hour)
    {
        currentHour = hour;
        UpdateClock();
    }

    private void UpdateClock()
    {
        string backEnd;
        if (!longHourTime)
        {
            // Convert To 12 Hour Time
            if (currentHour > 12)
            {
                currentHour -= 12;
                backEnd = ":00 pm";
            }
            else
            {
                backEnd = ":00 am";
            }
        }
        else
        {
            backEnd = ":00";
        }
        clockText.text = currentHour.ToString() + backEnd;
    }

    [ContextMenu("Toggle Time Setting")]
    public void ChangeTimeSetting()
    {
        longHourTime = !longHourTime;
        UpdateClock();
    }
    public void UpdateResources(List<float> Resources)
    {
        // Change Resource Displays

    }
}
