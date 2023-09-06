using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Navigation : MonoBehaviour
{
    /// <summary>
    /// This Script Handles Navigation Of The Villagers Based On Their Schedules And The Current Time
    /// </summary>

    /// <param name="VC"></param>
    /// <param name="villagerInfo"></param>
    public void GetLocationGoal(VillagerController VC ,VillagerInfo villagerInfo)
    {
        int currentTime = (int)DayNightManager.Instance.CurrentTime.x - 6;
        Vector2 Location = new Vector3(villagerInfo.CurrentGoal.x, 0); // Default To Current Goal / Current Floor
        if (villagerInfo.currentState != VillagerState.Traveling) // If Not Already Traveling To Elevator
        {
            switch (villagerInfo.schedule.VillagerStates[currentTime])
            {
                case VillagerState.Home:
                    Location = villagerInfo.house.Location;
                    villagerInfo.currentState = VillagerState.Home;
                    break;

                case VillagerState.Work:
                    Location = villagerInfo.job.Location;
                    villagerInfo.currentState = VillagerState.Work;
                    break;

                case VillagerState.Recreation:
                    Location = BuildingManager.Instance.GetRecreationBuilding().Location;
                    villagerInfo.currentState = VillagerState.Recreation;
                    break;

                //case VillagerState.Petitioning:
                //    break;

                default:
                    Debug.Log("Navigation Broke");
                    break;
            }

            if (VC.CurrentLevel != Location.y)
            {
                // Get Nearest Elevator
                villagerInfo.currentState = VillagerState.Traveling;

                if (Location.y > VC.CurrentLevel)
                {
                    // Needs To Go Up
                    villagerInfo.CurrentGoal.y = 1;
                }
                else
                {
                    // Needs To Go Down
                    villagerInfo.CurrentGoal.y = -1;
                }

                VC.currentElevatorGoal = ElevatorManager.Instance.GetClosestElevator((int)VC.CurrentLevel, (int)villagerInfo.CurrentGoal.y, VC.gameObject.transform.position.x);
                Location.x = VC.currentElevatorGoal.elevatorChain.X;
            }

            villagerInfo.CurrentGoal.x = Location.x;
        }
    }
}
