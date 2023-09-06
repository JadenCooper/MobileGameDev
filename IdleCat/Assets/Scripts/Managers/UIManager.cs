using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TMP_Text clockText;
    public bool longHourTime = true; // 24 hour time
    public void UpdateClock(float hour)
    {
        string backEnd;
        if (!longHourTime)
        {
            // Convert To 12 Hour Time
            if (hour > 12)
            {
                hour -= 12;
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
        clockText.text = hour.ToString() + backEnd;
    }

    public void UpdateResources(List<float> Resources)
    {
        // Change Resource Displays

    }
}
