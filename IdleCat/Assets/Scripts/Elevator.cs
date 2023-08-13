using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Elevator : MonoBehaviour
{
    public List<int> Levels = new List<int>(); // X = X, Y = Floor Level
    public float x;
    //public void TransportVillager(VillagerController villager)
    //{
    //    villager.gameObject.transform.position = new Vector2(x, Data.FloorHeights[villager.goalFloor]);
    //    villager.currentElevator = null;
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    VillagerController villager = collision.GetComponent<VillagerController>();
    //    if (villager != null)
    //    {
    //        if (villager.currentElevator == this)
    //        {
    //            TransportVillager(villager);
    //        }
    //    }
    //}

}
