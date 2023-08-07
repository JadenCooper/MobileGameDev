using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VillagerManager : MonoBehaviour
{
    public List<VillagerController> Villagers = new List<VillagerController>();
    public List<Elevator> Elevators = new List<Elevator>();
    private void Start()
    {
        foreach (VillagerController Villager in Villagers)
        {
            Villager.Initialize(this);
        }
    }
    public void TriggerVillagerSchedules(int CurrentTime)
    {
        // Triggers All Villager Schedule Changes Each Hour
        foreach (VillagerController Villager in Villagers)
        {
            Villager.CheckForLocation(CurrentTime);
        }
    }

    public Vector2 GetClosestElevator(float CurrentLevel, float GoalLevel, Vector2 CurrentLocation)
    {
        Debug.Log("CurrentLevel " + CurrentLevel);
        Debug.Log("GoalLevel " + GoalLevel);
        bool CurrentLevelEntranceFound = false;
        bool GoalLevelEntranceFound = false;
        List<Vector2> PossibleElevators = new List<Vector2>();

        for (int e = 0; e < Elevators.Count; e++)
        {
            Debug.Log("Elevator");
            for (int f = 0; f < Elevators[e].Floors.Count; f++)
            {
                if (Elevators[e].Floors[f] == CurrentLevel)
                {
                    Debug.Log("Current Level Found");
                    CurrentLevelEntranceFound = true;
                }
                else if(Elevators[e].Floors[f] == GoalLevel)
                {
                    Debug.Log("Goal Level Found");
                    GoalLevelEntranceFound = true;
                }
            }

            if (CurrentLevelEntranceFound && GoalLevelEntranceFound)
            {
                PossibleElevators.Add(Elevators[e].ElevatorLocations[e]);

            }

            CurrentLevelEntranceFound = false;
            GoalLevelEntranceFound = false;
        }

        if (PossibleElevators.Count == 0)
        {
            Debug.Log("No Elevators Found");
            return Vector2.zero;
        }
        else
        {
            Vector2 currentChoice = PossibleElevators[0];
            for (int i = 1; i < PossibleElevators.Count; i++)
            {
                if (Vector2.Distance(CurrentLocation, currentChoice) > Vector2.Distance(PossibleElevators[i], currentChoice))
                {
                    currentChoice = PossibleElevators[i];
                }
            }

            return currentChoice;
        }
    }
}
