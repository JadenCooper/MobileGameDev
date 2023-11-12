using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : Intractable
{
    public string ID; // Used To Restore Villager References
    public int BuildingTypeID; // Used To Restore Building Type
    public BuildingType buildingType;
    public string Name;
    public Vector2 Location; // X is X / Y Is Level
    public List<VillagerInfo> users = new List<VillagerInfo>(); // People Currently Inside, Used For Display Purposes
    public Sprite Icon;
    public int Capacity;
    public string ActionDescription;
    public int buildingWidth = 1;
    public Vector3Int GridPos;
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

    public override void InteractAction()
    {
        UIManager.Instance.BuildingDisplayWindow.OpenWindow(this);
    }
}
