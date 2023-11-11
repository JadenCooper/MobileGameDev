using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDisplay : MonoBehaviour
{
    [Header("General")]
    [SerializeField]
    private List<GameObject> pages = new List<GameObject>();
    [SerializeField]
    private TweenScale tweenScale;
    [SerializeField]
    private Vector3 openScale;
    private bool alreadyInMenu = false;
    private Building currentBuilding;
    [SerializeField]
    private TMP_Text title;

    [Header("House")]
    [SerializeField]
    private List<TMP_Text> houseDetails = new List<TMP_Text>();

    [Header("Job")]
    [SerializeField]
    private List<TMP_Text> jobDetails = new List<TMP_Text>();
    [SerializeField]
    private Image gainResourceType;
    [SerializeField]
    private ScrollViewExtender employeeGrid;
    [SerializeField]
    private ScrollViewExtender jobUserGrid;

    [Header("Recreation")]
    [SerializeField]
    private List<TMP_Text> recreationDetails = new List<TMP_Text>();
    public void OpenWindow(Building building)
    {
        if (Time.timeScale == 0)
        {
            alreadyInMenu = true;
        }
        else
        {
            alreadyInMenu = false;
        }

        CloseWindow();
        Time.timeScale = 0f;
        currentBuilding = building;
    }

    public void CloseWindow()
    {
        if (!alreadyInMenu)
        {
            Time.timeScale = 1f;
        }

        foreach (GameObject page in pages)
        {
            page.SetActive(false);
        }

        tweenScale.ScaleDown(null, SetBuildingScreen);
    }

    private void SetBuildingScreen()
    {
        SetBuildingDetails();
        tweenScale.ScaleUp(openScale);
    }
    private void SetBuildingDetails()
    {
        title.text = currentBuilding.Name;

        switch (currentBuilding.buildingType)
        {
            case BuildingType.Job:
                DisplayJob(currentBuilding.GetComponent<Job>());
                break;

            case BuildingType.House:
                DisplayHouse(currentBuilding.GetComponent<House>());
                break;

            case BuildingType.Recreation:
                DisplayRecreation(currentBuilding.GetComponent<Recreation>());
                break;

            default:
                break;
        }
    }

    private void DisplayJob(Job currentJob)
    {
        FillGrid(employeeGrid, currentJob.Employees);
        FillGrid(jobUserGrid, currentJob.users);

        jobDetails[0].text = "Gain " + currentJob.resourceGain + " Per Hour";
        gainResourceType.sprite = ResourceManager.Instance.GetResourceSprite(currentJob.resourceToGain);

        jobDetails[1].text = "Employee Capacity Is At " + currentJob.Employees.Count + "/" + currentJob.Capacity;

        jobDetails[2].text = "Employees Lose " + currentJob.happinessLoss + " Happiness Per Hour";

        jobDetails[3].text = "Employees Lose " + currentJob.restLoss + " Rest Per Hour";

        pages[0].SetActive(true);
    }

    private void DisplayHouse(House currentHouse)
    {
        pages[1].SetActive(true);
    }

    private void DisplayRecreation(Recreation currentRecreation)
    {
        pages[2].SetActive(true);
    }

    private void FillGrid(ScrollViewExtender scrollViewExtender, List<VillagerInfo> newVillagers)
    {
        scrollViewExtender.ClearGrid();

        if (newVillagers.Count > scrollViewExtender.ObjectsPerPage)
        {
            scrollViewExtender.ExpandGrid(newVillagers.Count);
        }

        for (int i = 0; i < newVillagers.Count; i++)
        {
            GameObject newVillager = Instantiate(scrollViewExtender.GridObjectPrefab);
            newVillager.transform.parent = scrollViewExtender.GridParent.transform;

            newVillager.GetComponent<UIVillagerButton>().Initialize(newVillagers[i]);

            scrollViewExtender.ObjectsInGrid.Add(newVillager);
            newVillager.transform.localScale = scrollViewExtender.GridObjectPrefab.transform.localScale;
        }
    }


}
