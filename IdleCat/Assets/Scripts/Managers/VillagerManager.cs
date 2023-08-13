using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VillagerManager : MonoBehaviour
{
    public List<VillagerController> Villagers = new List<VillagerController>();
    public List<Elevator> Elevators = new List<Elevator>();

    [SerializeField]
    private GameObject villagerPrefab;
    [SerializeField]
    private VillagerInfo defaultVillagerInfo;
    public GameObject VillagerSpawnPoint;
    public House Inn;
    public Job Mason;
    private void Start()
    {
        defaultVillagerInfo.house = Inn;
        defaultVillagerInfo.job = Mason;
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
        VillagerController VC = NewVillager.GetComponent<VillagerController>();
        VC.villagerInfo = defaultVillagerInfo;
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
