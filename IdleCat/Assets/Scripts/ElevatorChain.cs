using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorChain : MonoBehaviour
{
    /// <summary>
    /// This Script Handles The Overhead Of A Chain Of Elevators
    /// </summary>
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
        int newFloor = VC.VI.CurrentLevel + (int)VC.VI.CurrentGoal.y;
        VC.gameObject.transform.position = new Vector2(VC.gameObject.transform.position.x, Data.FloorHeights[newFloor]);
        VC.VI.CurrentLevel = newFloor;
        VC.VI.currentElevatorGoal = null;
        VC.ChangeState();
        VC.GetNewLocationGoal();
    }

    public bool CheckForLevel(int LevelToCheck)
    {
        // Checks To See If The Level Is Present In The Chain
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
        // Gets The Specific Elevator Based On The Level
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
