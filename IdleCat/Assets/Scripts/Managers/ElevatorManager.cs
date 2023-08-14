using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    public static ElevatorManager Instance { get; private set; }
    public List<ElevatorChain> ElevatorsChains = new List<ElevatorChain>();
    private void Awake()
    {
        // Singleton
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

    public Elevator GetClosestElevator(int CurrentLevel, int GoalLevel, float currentX)
    {
        List<ElevatorChain> PossibleChains = new List<ElevatorChain>();
        for (int i = 0; i < ElevatorsChains.Count; i++)
        {
            if (ElevatorsChains[i].CheckForLevel(CurrentLevel) && ElevatorsChains[i].CheckForLevel(CurrentLevel))
            {
                // Elevator Chain Has Both The Current Level And The Goal Level
                PossibleChains.Add(ElevatorsChains[i]);
            }
        }

        if (PossibleChains != null)
        {
            ElevatorChain ClosestElevatorChain = PossibleChains[0];
            for (int i = 1; i < PossibleChains.Count; i++)
            {
                if (Vector2.Distance(new Vector2(ClosestElevatorChain.X, 0), new Vector2(currentX, 0)) > Vector2.Distance(new Vector2(ClosestElevatorChain.X, 0), new Vector2(currentX, 0)))
                {
                    ClosestElevatorChain = PossibleChains[i];
                }
            }
            return ClosestElevatorChain.GetElevator(CurrentLevel);
        }
        else
        {
            return null;
        }
    }


}
