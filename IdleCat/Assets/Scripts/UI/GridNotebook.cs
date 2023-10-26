using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNotebook : MonoBehaviour
{
    public GameObject GridObjectPrefab;
    public GameObject GridParent;
    public List<GameObject> ObjectsInGrid = new List<GameObject>();

    public int ObjectsPerPage = 20;
    public virtual void SetupGrid(int GridTypeIndex)
    {
        if (ObjectsInGrid.Count != 0)
        {
            ClearGrid();
        }
    }
    public void ClearGrid()
    {
        for (int i = 0; i < ObjectsInGrid.Count; i++)
        {
            Destroy(ObjectsInGrid[i]);
        }

        ObjectsInGrid.Clear();
    }
}
