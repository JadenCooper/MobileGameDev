using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : Intractable
{
    public string Name;
    public Vector2 Location; // X is X / Y Is Level
    public List<VillagerController> users = new List<VillagerController>(); // People Currently Inside, Used For Display Purposes
    public Sprite Icon;
    public int Capacity;
    public abstract void OnTriggerEnter2D(Collider2D collision);

    public abstract void BuildingAction(VillagerInfo currentUser);

    public bool CheckCapacity()
    {
        if (users.Count < Capacity)
        {
            return true;
        }
        return false;
    }
}
