using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public TMP_Text ClockText;
    public List<TMP_Text> ResourceTexts = new List<TMP_Text>(); // Happiness, Wood, Stone, Food, Gold, VillagerCount
    private bool longHourTime = true; // 24 hour time
    private float currentHour = 6;
    public ModelWindowPanel ModelWindow;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
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
        ClockText.text = currentHour.ToString() + backEnd;
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
