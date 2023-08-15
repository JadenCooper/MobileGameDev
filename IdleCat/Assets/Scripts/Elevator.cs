using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Elevator : Intractable
{
    /// <summary>
    /// This Script Handles The On The Ground Functions Of An Elevator EG Entering
    /// </summary>

    public ElevatorChain elevatorChain;
    public int Level;
    public GameObject MovementCanvas;
    public void Initialize(ElevatorChain elevatorChain)
    {
        this.elevatorChain = elevatorChain;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        VillagerController VC = collision.gameObject.GetComponent<VillagerController>();
        if (VC != null)
        {
            if (VC.currentElevatorGoal == this)
            {
                elevatorChain.Transport(VC);
            }
        }
    }

    public override void InteractAction()
    {
        // Activate Canvas - Buttons To Go Up And Down - Used For Player
        MovementCanvas.SetActive(!MovementCanvas.activeSelf);
    }

    public void TransportUp()
    {

    }
    public void TransportDown()
    {

    }
}
