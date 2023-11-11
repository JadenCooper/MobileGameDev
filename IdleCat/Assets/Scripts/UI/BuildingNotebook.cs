using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNotebook : ScrollViewExtender
{
    public override void SetupGrid(int GridTypeIndex)
    {
        base.SetupGrid(GridTypeIndex);

        List<Building> newBuildings = new List<Building>(); // Create List To Hold Buildings
        switch (GridTypeIndex)
        {
            case 0:
                // Anything
                newBuildings.AddRange(BuildingManager.Instance.JobBuildings);
                newBuildings.AddRange(BuildingManager.Instance.HouseBuildings);
                newBuildings.AddRange(BuildingManager.Instance.RecreationBuildings);
                FillGrid(newBuildings);
                break;

            case 1:
                // Jobs
                newBuildings.AddRange(BuildingManager.Instance.JobBuildings);
                FillGrid(newBuildings);
                break;

            case 2:
                // Houses
                newBuildings.AddRange(BuildingManager.Instance.HouseBuildings);
                FillGrid(newBuildings);
                break;

            case 3:
                // Recreation
                newBuildings.AddRange(BuildingManager.Instance.RecreationBuildings);
                FillGrid(newBuildings);
                break;

            default:
                break;
        }
    }

    private void FillGrid(List<Building> newBuildings)
    {
        if (newBuildings.Count > ObjectsPerPage)
        {
            ExpandGrid(newBuildings.Count);
        }

        for (int i = 0; i < newBuildings.Count; i++)
        {
            GameObject newBuilding = Instantiate(GridObjectPrefab);
            newBuilding.transform.parent = GridParent.transform;

            newBuilding.GetComponent<UIBuildingButton>().Initialize(newBuildings[i]);

            ObjectsInGrid.Add(newBuilding);
        }
    }
}
