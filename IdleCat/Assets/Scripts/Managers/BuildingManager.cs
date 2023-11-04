using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public List<Recreation> RecreationBuildings = new List<Recreation>();
    public List<Job> JobBuildings = new List<Job>();
    public List<House> HouseBuildings = new List<House>();

    public List<GameObject> ListOfBuildingPrefabs = new List<GameObject>();

    public int PetitionSlots = 4;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public Recreation GetRecreationBuilding()
    {
        Recreation selectedRecreation = RecreationBuildings[Random.Range(0, RecreationBuildings.Count)];
        if (selectedRecreation.CheckCapacity())
        {
            return selectedRecreation;
        }

        return RecreationBuildings[0];
    }

    public void SaveBuildingData()
    {

    }

    public void LoadBuildingSaveData(List<VillagerInfo> allVillagerInfos, List<VillagerSaveData> allVSD)
    {
        // Need To Add Clear All Functionality
        List<BuildingSaveData> newBuildingData = SaveData.current.Buildings;

        for (int i = 0; i < newBuildingData.Count; i++)
        {
            GameObject newBuilding = Instantiate(ListOfBuildingPrefabs[newBuildingData[i].BuildingTypeID]);
            newBuilding.transform.position = new Vector2(newBuildingData[i].location.x, Data.FloorHeights[(int)newBuildingData[i].location.y]);

            switch (newBuildingData[i].buildingType)
            {
                case BuildingType.Job:
                    Job newJob = newBuilding.GetComponent<Job>();
                    for (int t = 0; t < allVSD.Count; t++)
                    {
                        if (newBuildingData[i].BuildingID == allVSD[t].JobID)
                        {
                            allVillagerInfos[t].job = newJob;
                            newJob.Employees.Add(allVillagerInfos[t]);
                        }
                    }
                    newJob.ID = newBuildingData[i].BuildingID;
                    JobBuildings.Add(newJob);
                    break;

                case BuildingType.House:
                    House newHouse = newBuilding.GetComponent<House>();

                    for (int t = 0; t < allVSD.Count; t++)
                    {
                        if (newBuildingData[i].BuildingID == allVSD[t].HouseID)
                        {
                            allVillagerInfos[t].house = newHouse;
                            newHouse.Inhabitants.Add(allVillagerInfos[t]);
                        }
                    }
                    newHouse.ID = newBuildingData[i].BuildingID;
                    HouseBuildings.Add(newHouse);
                    break;

                case BuildingType.Recreation:
                    Recreation newRecreation = newBuilding.GetComponent<Recreation>();
                    newRecreation.ID = newBuildingData[i].BuildingID;
                    RecreationBuildings.Add(newRecreation);
                    break;

                default:
                    break;
            }
        }
    }

}
