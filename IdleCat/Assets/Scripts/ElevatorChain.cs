using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorChain : MonoBehaviour
{
    public List<Elevator> Elevators;
    public float X;
    private void Start()
    {
        X = gameObject.transform.position.x;
        foreach (Elevator elevator in Elevators)
        {
            elevator.Initialize(this);
        }
    }
    public void Transport(VillagerController VC)
    {
        int newFloor = VC.CurrentLevel + (int)VC.villagerInfo.CurrentGoal.y;
        VC.gameObject.transform.position = new Vector2(VC.gameObject.transform.position.x, Data.FloorHeights[newFloor]);
        VC.CurrentLevel = newFloor;
        VC.currentElevatorGoal = null;
        VC.ChangeState();
        VC.GetNewLocationGoal();
    }

    public bool CheckForLevel(int LevelToCheck)
    {
        for (int i = 0; i < Elevators.Count; i++)
        {
            if (LevelToCheck == Elevators[i].Level)
            {
                return true;
            }
        }

        return false;
    }

    public Elevator GetElevator(int LevelToCheck)
    {
        for (int i = 0; i < Elevators.Count; i++)
        {
            if (LevelToCheck == Elevators[i].Level)
            {
                return Elevators[i];
            }
        }

        return null;
    }
}
