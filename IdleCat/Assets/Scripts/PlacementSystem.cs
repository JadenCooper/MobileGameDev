using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject pressIndicator, cellIndicator;
    [SerializeField]
    private Grid grid;
    public void MoveIndicator(Vector2 pos)
    {
        Debug.Log("Move");
        Vector3Int gridPosition = grid.WorldToCell(pos);
        pressIndicator.transform.position = pos;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}
