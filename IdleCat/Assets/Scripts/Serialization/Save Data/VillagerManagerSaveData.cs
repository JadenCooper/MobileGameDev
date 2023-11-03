using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VillagerManagerSaveDate
{
    public List<VillagerSaveData> Villagers;
    public List<string> PostponedVillagerPetitionsIDS;
    public List<VillagerSaveData> NonVillagers;
    public List<VillagerSaveData> PostponedNonVillagerPetitions;
    public List<VillagerSaveData> QueuedVillagers;
}
