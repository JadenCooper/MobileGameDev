using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UIElements;

public class DayNightManager : MonoBehaviour
{
    private const int HOURSINDAY = 18;
    public VillagerManager villagerManager;

    [Header("Length of Full Day In Seconds.")]
    public float fullDayLength;

    // X = Current Hour, Y = Inbettween
    private Vector2 CurrentTime;
    private float TimeRate;

    private void Start()
    {
        // Day Starts At 6am Ends At 12am
        // This splits the full day equally from 0 to 1.
        // 0.0 being 6, 0.5 being midnight, and 1.0 being midday.
        TimeRate = 1.0f / (fullDayLength / HOURSINDAY); // About 33 Seconds Per Hour
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
            if (CurrentTime.x >= HOURSINDAY)
            {
                // New Day
                CurrentTime = Vector2.zero;
            }
        }
    }
}
