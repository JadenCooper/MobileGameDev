using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    public float GetLocationGoal(VillagerInfo villagerInfo, float currentLevel)
    {
        int currentTime = (int)DayNightManager.Instance.CurrentTime.x;
        Vector2 Location;
        switch (villagerInfo.schedule.VillagerStates[currentTime])
        {
            case VillagerState.Home:
                Location = villagerInfo.house.GetLocation();
                villagerInfo.currentState = VillagerState.Home;
                break;
            case VillagerState.Work:
                Location = villagerInfo.job.GetLocation();
                villagerInfo.currentState = VillagerState.Work;
                break;
            //case VillagerState.Recreation:
            //    break;
            //case VillagerState.Petitioning:
            //    break;
            default:
                Debug.Log("Navigation Broke");
                return 0;
        }

        if (currentLevel != Location.y)
        {
            // Get Nearest Elevator
            villagerInfo.currentState = VillagerState.Traveling;

            if (Location.y > currentLevel)
            {
                // Needs To Go Up
            }
            else
            {
                // Needs To Go Down
            }
        }

        return Location.x;
    }
}
