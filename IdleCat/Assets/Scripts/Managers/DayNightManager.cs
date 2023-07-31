using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class DayNightManager : MonoBehaviour
{
    private const int HOURSINDAY = 18;
    private const int STARTTIME = 6;
    private const int ENDTIME = 24;
    public VillagerManager villagerManager;

    [Header("Length of Full Day In Seconds.")]
    public float fullDayLength;

    // X = Current Hour, Y = Inbettween
    private Vector2 CurrentTime;
    private float TimeRate;
    private float LightIncreaceRate;
    private float LightDecreaceRate;


    public Light2D Sun;
    private void Start()
    {
        // Day Starts At 6am Ends At 12am
        // This splits the full day equally from 0 to 1.
        // 0.0 being 6, 0.5 being midnight, and 1.0 being midday.
        TimeRate = 1.0f / (fullDayLength / HOURSINDAY); // About 33 Seconds Per Hour

        CurrentTime.x = STARTTIME; // Start At 6AM

        LightIncreaceRate = ((1 - 0.65f) / 6);
        LightDecreaceRate = ((1 - 0.2f) / 12);
        UpdateLight();
    }
    private void Update()
    {
        //slowly increases the day from 0.0 to 1.0
        CurrentTime.y += TimeRate * Time.deltaTime;

        //When it hits 1.0, it resets the timer back to 0.0, resetting the day.
        if (CurrentTime.y >= 1.0f)
        {
            // New Hour
            CurrentTime.y = 0.0f;
            CurrentTime.x += 1f;
            if (CurrentTime.x >= ENDTIME)
            {
                // New Day
                CurrentTime.x = STARTTIME;
                CurrentTime.y = 0;
            }
            // Change Sunlight Every Hour
            UpdateLight();
        }

    }

    private void UpdateLight()
    {
        Debug.Log("Update Light");
        if (CurrentTime.x == 6)
        {
            // Start Of Day
            Sun.intensity = 0.65f;
        }
        else if (CurrentTime.x < 13)
        {
            // Before Midday
            Sun.intensity += LightIncreaceRate;
        }
        else
        {
            // After Midday
            Sun.intensity -= LightDecreaceRate;
        }
    }
}
