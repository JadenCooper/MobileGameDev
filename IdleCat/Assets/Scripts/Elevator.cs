using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Elevator : MonoBehaviour
{
    public ElevatorChain elevatorChain;
    public int Level;
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
}
