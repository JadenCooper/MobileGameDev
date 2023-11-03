using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Elevator : Intractable
{
    /// <summary>
    /// This Script Handles The On The Ground Functions Of An Elevator EG Entering
    /// </summary>
    public string ID;
    public ElevatorChain elevatorChain;
    public int Level;
    public GameObject UpButton, DownButton;
    public void Initialize(ElevatorChain elevatorChain)
    {
        this.elevatorChain = elevatorChain;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        VillagerController VC = collision.gameObject.GetComponent<VillagerController>();
        if (VC != null && VC.enabled == true)
        {
            if (VC.VI.currentElevatorGoal == this)
            {
                elevatorChain.Transport(VC);
            }
        }
    }

    public override void InteractAction()
    {
        // Activate Canvas - Buttons To Go Up And Down - Used For Player

        if (elevatorChain.CheckForLevel(Level + 1))
        {
            // Can Go Up
            if (UpButton.activeSelf)
            {
                UpButton.SetActive(false);
            }
            else
            {
                UpButton.SetActive(true);
            }
        }

        if (elevatorChain.CheckForLevel(Level - 1))
        {
            // Can Go Down
            if (UpButton.activeSelf)
            {
                DownButton.SetActive(false);
            }
            else
            {
                DownButton.SetActive(true);
            }
        }
    }
    public void TransportUp()
    {
        Elevator elevator = elevatorChain.GetElevator(Level + 1);
        elevator.Transport(GameObject.FindGameObjectWithTag("Player"));
        InteractAction(); // To Disable Current Elevator Buttons
    }
    public void TransportDown()
    {
        Elevator elevator = elevatorChain.GetElevator(Level - 1);
        elevator.Transport(GameObject.FindGameObjectWithTag("Player")) ;
        InteractAction(); // To Disable Current Elevator Buttons
    }

    public void Transport(GameObject gameObject)
    {
        gameObject.transform.position = new Vector2(gameObject.transform.position.x, Data.FloorHeights[Level]);
    }
}
