using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject cellIndicator;
    [SerializeField]
    private SpriteRenderer cellIndicatorRenderer;
    [SerializeField]
    private Grid grid;
    private int selectedObjectIndex = 3;

    [SerializeField]
    private GameObject gridVisulalizations; // Parent Of The Different Grids
    private GridData buildingData;

    [SerializeField]
    private Color validPlacement;
    [SerializeField]
    private Color nonValidPlacement;
    [SerializeField]
    private GameObject constructionMenu;

    private void Start()
    {
        buildingData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = ID;

        Building building = BuildingManager.Instance.ListOfBuildingPrefabs[selectedObjectIndex].GetComponentInChildren<Building>();

        if (ResourceManager.Instance.ResourceCheck(building.ResourceToBuild, building.ResourceCostToBuild))
        {
            Time.timeScale = 0;
            BuildingManager.Instance.BuildMode = true;
            ResourceManager.Instance.ResourceChange(building.ResourceToBuild, building.ResourceCostToBuild);
            gridVisulalizations.SetActive(true);
            cellIndicator.SetActive(true);
            grid.gameObject.SetActive(true);
            constructionMenu.SetActive(false);
        }
    }

    private void PlaceStructure(Vector3Int gridPosition)
    {
        GameObject buildingObject = Instantiate(BuildingManager.Instance.ListOfBuildingPrefabs[selectedObjectIndex]);
        buildingObject.transform.position = grid.CellToWorld(gridPosition);

        Building building = buildingObject.GetComponentInChildren<Building>();
        System.Guid newGuid = System.Guid.NewGuid();
        building.ID = newGuid.ToString();
        building.GridPos = gridPosition;

        if (gridPosition.y > 0)
        {
            building.Location = new Vector2(buildingObject.transform.position.x, 1);
        }
        else
        {
            building.Location = new Vector2(buildingObject.transform.position.x, 0);
        }
        Debug.Log(building.Location);
        buildingData.AddBuildingAt(gridPosition, building.buildingWidth, building.ID);
        BuildingManager.Instance.AddBuilding(building);
        StopPlacement();
    }

    private void StopPlacement()
    {
        gridVisulalizations.SetActive(false);
        cellIndicator.SetActive(false);
        grid.gameObject.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        BuildingManager.Instance.BuildMode = true;
        constructionMenu.SetActive(true);
    }

    public void MoveIndicator(Vector2 pos)
    {
        Vector3Int gridPosition = grid.WorldToCell(pos);
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
        bool placementValidity = CheckPlacementValidity(gridPosition);
        if (placementValidity == false)
        {
            cellIndicatorRenderer.color = nonValidPlacement;
            return;
        }

        cellIndicatorRenderer.color = validPlacement;
        PlaceStructure(gridPosition);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition)
    {
        return buildingData.CanPlaceBuildingAt(gridPosition, BuildingManager.Instance.ListOfBuildingPrefabs[selectedObjectIndex].GetComponentInChildren<Building>().buildingWidth);
    }

    public void ImportSaveData(List<Building> importedBuildings)
    {
        buildingData.ClearData();
        foreach (var building in importedBuildings)
        {
            buildingData.AddBuildingAt(building.GridPos, building.buildingWidth, building.ID);
        }
    }
}
