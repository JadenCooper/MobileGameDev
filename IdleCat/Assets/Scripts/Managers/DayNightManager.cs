using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class DayNightManager : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    
    public static DayNightManager Instance { get; private set; }

    public event Action NewHour;

    public VillagerManager villagerManager;

    [Header("Length of Full Day In Seconds")]
    public float fullDayLength;

    // X = Current Hour, Y = Inbettween
    public Vector2 CurrentTime;

    private float TimeRate;

    private float LightIncreaceRate;
    private float LightDecreaceRate;
    private float LightChangeOverTime;

    public UIManager uiManager;
    public Light2D Sun;

    public List<SeasonLightValues> SeasonLightValues = new List<SeasonLightValues>();
    private SeasonLightValues currentSeasonLightValue;
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
    private void Start()
    {
        // Day Starts And Ends At 12am
        // This splits the full day equally from 0 to 1.
        // 0.0 being 6, 0.5 being midnight, and 1.0 being midday.
        TimeRate = 1.0f / (fullDayLength / Data.HOURSINDAY);
        currentSeasonLightValue = SeasonLightValues[0]; // Set Current Season To Spring
        CalculateLightRates(currentSeasonLightValue);
        Sun.intensity = SeasonLightValues[0].LightDecreaseGoal;
        SetTime( new Vector2(Data.STARTTIME, 0)); // ADD Check For Tutorial, If So Start At 6
        uiManager.UpdateClockTime(CurrentTime.x);
    }
    private void Update()
    {
        //slowly increases the day from 0.0 to 1.0
        CurrentTime.y += TimeRate * Time.deltaTime;

        //When it hits 1.0, it resets the timer back to 0.0, resetting the hour.
        if (CurrentTime.y >= 1.0f)
        {
            // New Hour
            CurrentTime.y = 0.0f;
            CurrentTime.x += 1f;
            if (CurrentTime.x >= Data.ENDTIME)
            {
                // New Day
                CurrentTime.x = Data.STARTTIME;
                CurrentTime.y = 0;
                //Time.timeScale = 0.0f; // Slow Down Time For End Of Day Report
            }

            // Change Sunlight Every Hour
            UpdateLight();
            uiManager.UpdateClockTime(CurrentTime.x);

            if (NewHour != null)
            {
                NewHour(); // Trigger New Hour Event If It Has Subscribers
            }
        }

    }

    private void UpdateLight()
    {
        if (CurrentTime.x < LightChangeOverTime)
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

    public void SetTime(Vector2 TimeToSetTo)
    {
        // Used To Set And Load Game Time
       float increments = TimeToSetTo.x - CurrentTime.x;
        for (int i = 0; i < increments; i++)
        {
            UpdateLight(); // Get Light Upto Set Time
        }

        CurrentTime = TimeToSetTo;
    }

    public void CalculateLightRates(SeasonLightValues season)
    {
        // Second Number Is Goal Of Setting - Third Number Is The Amount Of Time To Do It
        float intialValue = season.LightDecreaseGoal;
        LightChangeOverTime = season.LightChangeOverTime;
        LightIncreaceRate = (season.LightIncreaseGoal - intialValue) / LightChangeOverTime;
        LightDecreaceRate = (season.LightIncreaseGoal - season.LightDecreaseGoal) / (24 - LightChangeOverTime);
    }
}
