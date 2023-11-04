using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingSaveData
{
    public string BuildingID;
    public int BuildingTypeID;
    public BuildingType buildingType;
    public Vector3 location;
}
