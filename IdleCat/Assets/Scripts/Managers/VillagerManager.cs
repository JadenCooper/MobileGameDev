using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerManager : MonoBehaviour
{
    public List<VillagerController> Villagers = new List<VillagerController>();
    public void TriggerVillagerSchedules(int CurrentTime)
    {
        // Triggers All Villager Schedule Changes Each Hour
        foreach (VillagerController Villager in Villagers)
        {
            Villager.CheckForLocation(CurrentTime);
        }
    }
}
