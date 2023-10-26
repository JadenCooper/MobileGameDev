using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNotebook : MonoBehaviour
{
    public GameObject GridObjectPrefab;
    public GameObject GridParent;
    public List<GameObject> ObjectsInGrid = new List<GameObject>();
    public RectTransform ContentRect;
    private Vector2 intialRect;
    public int ObjectsPerPage = 20;
    public int ObjectsPerLine = 4;
    public float GridLineExpansion;
    private void Start()
    {
        intialRect = ContentRect.sizeDelta;
    }
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
        ContentRect.sizeDelta = intialRect;
    }

    public void ExpandGrid(float count)
    {
        count -= ObjectsPerPage;

        count /= ObjectsPerLine;

        count = Mathf.Ceil(count);

        ContentRect.sizeDelta = new Vector2(ContentRect.sizeDelta.x, ContentRect.sizeDelta.y + (count * GridLineExpansion));
    }
}
