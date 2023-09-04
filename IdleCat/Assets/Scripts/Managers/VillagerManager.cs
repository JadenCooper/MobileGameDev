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
    private void Start()
    {
        //foreach (VillagerController Villager in Villagers)
        //{
        //    Villager.Initialize(this);
        //}
    }
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
        tempVI.schedule = GenerateSchedule(tempVI);
        VC.Initialize(tempVI);
        Villagers.Add(VC);
    }
    public Schedule GenerateSchedule(VillagerInfo VI)
    {
        Schedule schedule = VI.schedule;

        bool WorkDone = false;
        if (VI.job == null)
        {
            WorkDone = true;
        }
        for (int i = 0; i < 18; i++)
        {

            if (!WorkDone)
            {
                if (i == VI.job.WorkTimes.x)
                {
                    do
                    {
                        schedule.VillagerStates[i] = VillagerState.Work;
                        i++;
                    } while (i != VI.job.WorkTimes.y);
                    WorkDone = true;
                }

            }
            else
            {
                int temp = Random.Range(0, 2);
                if (temp == 1)
                {
                    schedule.VillagerStates[i] = VillagerState.Home;
                }
                else
                {
                    schedule.VillagerStates[i] = VillagerState.Recreation;
                }
            }
        }

        return schedule;
    }

    //public void GetClosestElevator(VillagerController villager, float currentLevel)
    //{
    //    ////                                                                                          /////
    //    //  This Method Gets The Closest Elevator That Goes At least One Floor Closer To The Goal Floor ///
    //    ////                                                                                          /////
    //    float currentGoal = villager.CurrentGoal.y;

    //    if (currentGoal > currentLevel)
    //    {
    //        // Going Up
    //        currentGoal = currentLevel += 1;
    //    }
    //    else
    //    {
    //        //Going Down
    //        currentGoal = currentLevel -= 1;
    //    }

    //    List<Elevator> CanGetToElevators = new List<Elevator>();
    //    for (int e = 0; e < Elevators.Count; e++)
    //    {
    //        for (int l = 0; l < Elevators[e].Levels.Count; l++)
    //        {
    //            if (Elevators[e].Levels[l] == currentLevel)
    //            {
    //                // Can Get To
    //                CanGetToElevators.Add(Elevators[e]);
    //                break;
    //            }
    //        }
    //    }

    //    List<Elevator> CanGetOutOffElevators = new List<Elevator>();

    //    for (int e = 0; e < CanGetToElevators.Count; e++)
    //    {
    //        for (int l = 0; l < CanGetToElevators[e].Levels.Count; l++)
    //        {
    //            if (CanGetToElevators[e].Levels[l] == currentGoal)
    //            {
    //                // Can Get Off
    //                CanGetOutOffElevators.Add(Elevators[e]);
    //                break;
    //            }
    //        }
    //    }

    //    Elevator finalElevator = CanGetOutOffElevators[0];
    //    Vector2 currentLocation = new Vector2(villager.transform.position.x, 0);
    //    for (int i = 1; i < CanGetOutOffElevators.Count; i++)
    //    {
    //        Vector2 current = new Vector2(finalElevator.x, 0);
    //        Vector2 check = new Vector2(CanGetOutOffElevators[i].x, 0);

    //        if (Vector2.Distance(current, currentLocation) > Vector2.Distance(check, currentLocation))
    //        {
    //            finalElevator = CanGetOutOffElevators[i];
    //        }
    //    }

    //    villager.SetElevator(finalElevator, finalElevator.x, (int)currentGoal);
    //}
}
