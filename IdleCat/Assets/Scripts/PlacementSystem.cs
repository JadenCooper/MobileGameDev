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

    private void Start()
    {
        StopPlacement();
        buildingData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = ID;
        gridVisulalizations.SetActive(true);
        cellIndicator.SetActive(true);
        grid.gameObject.SetActive(true);
    }

    private void PlaceStructure(Vector3Int gridPosition)
    {
        GameObject buildingObject = Instantiate(BuildingManager.Instance.ListOfBuildingPrefabs[selectedObjectIndex]);
        buildingObject.transform.position = grid.CellToWorld(gridPosition);

        Building building = buildingObject.GetComponentInChildren<Building>();
        System.Guid newGuid = System.Guid.NewGuid();
        building.ID = newGuid.ToString();

        buildingData.AddBuildingAt(gridPosition, building.buildingWidth, building.ID);
        StopPlacement();
    }

    private void StopPlacement()
    {
        gridVisulalizations.SetActive(false);
        cellIndicator.SetActive(false);
        grid.gameObject.SetActive(false);
    }

    public void MoveIndicator(Vector2 pos)
    {
        Vector3Int gridPosition = grid.WorldToCell(pos);
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
        Debug.Log(gridPosition);
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
}
