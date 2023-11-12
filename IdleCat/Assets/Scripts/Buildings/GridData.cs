using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData : MonoBehaviour
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddBuildingAt(Vector3Int gridPosition, float buildingWidth, string ID)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, buildingWidth);
        PlacementData data = new PlacementData(positionsToOccupy, ID);
        foreach (var pos in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains this cell position {pos} ");
            }
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, float buildingWidth)
    {
        List<Vector3Int> returnValues = new();
        returnValues.Add(gridPosition);
        for (int i = 1; i < buildingWidth; i++)
        {
            returnValues.Add(new Vector3Int(gridPosition.x + i, gridPosition.y , 0));
        }

        return returnValues;
    }

    public bool CanPlaceBuildingAt(Vector3Int gridPosition, float buildingWidth)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, buildingWidth);
        foreach (var pos in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                return false;
            }
        }
        return true;
    }

    public void ClearData()
    {
        placedObjects.Clear();
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public string ID { get; private set;}

    public PlacementData(List<Vector3Int> occupiedPositions, string iD)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
    }

}