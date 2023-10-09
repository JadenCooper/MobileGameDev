using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public List<Recreation> RecreationBuildings = new List<Recreation>();
    public List<Job> JobBuildings = new List<Job>();
    public List<House> HouseBuildings = new List<House>();
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


}
