using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerNotebook : GridNotebook
{
    public override void SetupGrid(int GridTypeIndex)
    {
        base.SetupGrid(GridTypeIndex);

        FillGrid(VillagerManager.Instance.Villagers);
    }

    private void FillGrid(List<VillagerController> newVillagers)
    {
        for (int i = 0; i < newVillagers.Count; i++)
        {
            GameObject newVillager = Instantiate(GridObjectPrefab);
            newVillager.transform.parent = GridParent.transform;

            newVillager.GetComponent<UIVillagerButton>().Initialize(newVillagers[i].VI);

            ObjectsInGrid.Add(newVillager);
        }
    }
}
