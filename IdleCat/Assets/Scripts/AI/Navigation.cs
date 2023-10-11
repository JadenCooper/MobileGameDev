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
    public void CheckSchedule(VillagerInfo villagerInfo)
    {
        int currentTime = (int)DayNightManager.Instance.CurrentTime.x;
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
                    villagerInfo.recreationGoal = BuildingManager.Instance.GetRecreationBuilding();
                    Location = villagerInfo.recreationGoal.Location;
                    villagerInfo.currentState = VillagerState.Recreation;
                    break;

                //case VillagerState.Petitioning:
                //    break;

                default:
                    Debug.Log("Navigation Broke");
                    break;
            }
        }

        GetLocationGoal(villagerInfo, Location);
    }
    public void GetLocationGoal(VillagerInfo VI, Vector2 Location)
    {
        if (VI.CurrentLevel != Location.y)
        {
            GetNearestElevator(VI, Location);
        }
        else
        {
            VI.CurrentGoal.x = Location.x;
        }
    }

    public void GetNearestElevator(VillagerInfo VI, Vector2 Location)
    {
        VI.currentState = VillagerState.Traveling;

        if (Location.y > VI.CurrentLevel)
        {
            // Needs To Go Up
            VI.CurrentGoal.y = 1;
        }
        else
        {
            // Needs To Go Down
            VI.CurrentGoal.y = -1;
        }

        VI.currentElevatorGoal = ElevatorManager.Instance.GetClosestElevator((int)VI.CurrentLevel, (int)VI.CurrentGoal.y, VI.gameObject.transform.position.x);
        Location.x = VI.currentElevatorGoal.elevatorChain.X;

        VI.CurrentGoal.x = Location.x;
    }
}
