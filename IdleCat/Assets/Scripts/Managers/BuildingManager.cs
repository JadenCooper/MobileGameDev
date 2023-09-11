using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public List<Recreation> RecreationBuildings = new List<Recreation>();

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
        return RecreationBuildings[Random.Range(0, RecreationBuildings.Count)];
    }
}
