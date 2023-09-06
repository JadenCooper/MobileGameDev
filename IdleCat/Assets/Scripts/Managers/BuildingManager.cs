using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    public List<Recreation> RecreationBuildings = new List<Recreation>();

    public Recreation GetRecreationBuilding()
    {
        return RecreationBuildings[Random.Range(0, RecreationBuildings.Count)];
    }
}
