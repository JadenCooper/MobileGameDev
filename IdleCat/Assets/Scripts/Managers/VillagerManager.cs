using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VillagerManager : MonoBehaviour
{
    public List<VillagerController> Villagers = new List<VillagerController>();
    public List<SpeciesData> UnlockedSpecies = new List<SpeciesData>();
    public List<SpeciesData> AllSpecies = new List<SpeciesData>();

    [SerializeField]
    private GameObject villagerPrefab;
    [SerializeField]
    private VillagerInfo defaultVillagerInfo;
    [SerializeField]
    private Schedule defaultSchedule;
    public GameObject VillagerSpawnPoint;
    public House Inn;
    public Job Mason;

    [ContextMenu("Generate Villager")]
    public void GenerateVillager()
    {
        GameObject NewVillager = Instantiate(villagerPrefab, VillagerSpawnPoint.transform);
        NewVillager.transform.parent = null;
        NewVillager.transform.parent = this.transform;
        VillagerController VC = NewVillager.GetComponentInChildren<VillagerController>();
        VillagerInfo tempVI = Instantiate(defaultVillagerInfo);
        tempVI.house = Inn;
        tempVI.Species = UnlockedSpecies[Random.Range(0, UnlockedSpecies.Count)];
        tempVI.schedule = Instantiate(defaultSchedule);
        //tempVI.schedule = GenerateSchedule(tempVI);
        VC.Initialize(tempVI);
        Villagers.Add(VC);
    }
    //public Schedule GenerateSchedule(VillagerInfo VI)
    //{
    //    Schedule schedule = VI.schedule;

    //    bool WorkDone = false;
    //    if (VI.job == null)
    //    {
    //        WorkDone = true;
    //    }
    //    for (int i = 0; i < 18; i++)
    //    {

    //        if (!WorkDone)
    //        {
    //            if (i == VI.job.WorkTimes.x)
    //            {
    //                do
    //                {
    //                    schedule.VillagerStates[i] = VillagerState.Work;
    //                    i++;
    //                } while (i != VI.job.WorkTimes.y);
    //                WorkDone = true;
    //            }

    //        }
    //        else
    //        {
    //            int temp = Random.Range(0, 2);
    //            if (temp == 1)
    //            {
    //                schedule.VillagerStates[i] = VillagerState.Home;
    //            }
    //            else
    //            {
    //                schedule.VillagerStates[i] = VillagerState.Recreation;
    //            }
    //        }
    //    }

    //    return schedule;
    //}
}
