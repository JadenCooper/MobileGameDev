using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VillagerManagerSaveDate
{
    public List<VillagerSaveData> Villagers = new List<VillagerSaveData>();
    public List<string> PostponedVillagerPetitionsIDS;
    public List<VillagerSaveData> NonVillagers = new List<VillagerSaveData>();
    public List<VillagerSaveData> PostponedNonVillagerPetitions = new List<VillagerSaveData>();
    public List<VillagerSaveData> QueuedVillagers = new List<VillagerSaveData>();
}
