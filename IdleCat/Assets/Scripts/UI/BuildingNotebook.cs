using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingNotebook : MonoBehaviour
{
    [SerializeField]
    private GameObject uIBuildingPrefab;
    [SerializeField]
    private GameObject gridParent;
    [SerializeField]
    private List<GameObject> buildingsInGrid = new List<GameObject>();

    private const int BUILDINGSPERPAGE = 20;
    public void FillBuildings(int GridTypeIndex)
    {
        if (buildingsInGrid.Count != 0)
        {
            ClearGrid();
        }

        List<Building> newBuildings = new List<Building>(); // Create List To Hold Buildings
        switch (GridTypeIndex)
        {
            case 0:
                // Anything
                newBuildings.AddRange(BuildingManager.Instance.JobBuildings);
                FillGrid(newBuildings);
                newBuildings.Clear();
                newBuildings.AddRange(BuildingManager.Instance.HouseBuildings);
                FillGrid(newBuildings);
                newBuildings.Clear();
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
        for (int i = 0; i < newBuildings.Count; i++)
        {
            GameObject newBuilding = Instantiate(uIBuildingPrefab);
            newBuilding.transform.parent = gridParent.transform;

            newBuilding.GetComponent<UIBuildingButton>().Initialize(newBuildings[i]);

            buildingsInGrid.Add(newBuilding);
        }
    }

    public void ClearGrid()
    {
        for (int i = 0; i < buildingsInGrid.Count; i++)
        {
            Destroy(buildingsInGrid[i]);
        }

        buildingsInGrid.Clear();
    }
}
