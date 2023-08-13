using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    public void GetLocationGoal(VillagerInfo villagerInfo, float currentLevel)
    {
        int currentTime = (int)DayNightManager.Instance.CurrentTime.x - 6;
        Vector2 Location = new Vector3(villagerInfo.CurrentGoal.x, 0); // Default To Current Goal / Current Floor
        if (villagerInfo.currentState != VillagerState.Traveling) // If Not Already Traveling To Elevator
        {
            switch (villagerInfo.schedule.VillagerStates[currentTime])
            {
                case VillagerState.Home:
                    if (villagerInfo.currentState != VillagerState.Home)
                    {
                        Location = villagerInfo.house.GetLocation();
                        villagerInfo.currentState = VillagerState.Home;
                    }
                    break;

                case VillagerState.Work:
                    if (villagerInfo.currentState != VillagerState.Work)
                    {
                        Location = villagerInfo.job.GetLocation();
                        villagerInfo.currentState = VillagerState.Work;
                    }
                    break;

                //case VillagerState.Recreation:
                //    break;

                //case VillagerState.Petitioning:
                //    break;

                default:
                    Debug.Log("Navigation Broke");
                    break;
            }

            if (currentLevel != Location.y)
            {
                // Get Nearest Elevator
                villagerInfo.currentState = VillagerState.Traveling;

                if (Location.y > currentLevel)
                {
                    // Needs To Go Up
                    villagerInfo.CurrentGoal.y = 1;
                }
                else
                {
                    // Needs To Go Down
                    villagerInfo.CurrentGoal.y = -1;
                }
            }

            villagerInfo.CurrentGoal.x = Location.x;
        }
    }
}
