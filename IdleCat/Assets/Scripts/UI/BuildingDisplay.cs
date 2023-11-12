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
    [SerializeField]
    private Image buildingIcon;

    [SerializeField]
    private GameObject assignmentPannel;
    [SerializeField]
    private GameObject assignVillagerPrefab;
    [SerializeField]
    private ScrollViewExtender assignmentScroll;

    private BuildingType currentType;
    
    [Header("House")]
    [SerializeField]
    private List<TMP_Text> houseDetails = new List<TMP_Text>();
    [SerializeField]
    private ScrollViewExtender inhabitantsGrid;
    [SerializeField]
    private ScrollViewExtender houseUserGrid;

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
    [SerializeField]
    private ScrollViewExtender recreationGrid;
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
        currentBuilding = building;
        CloseWindow();
        Time.timeScale = 0f;
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
        if (currentBuilding.Icon != null)
        {
            buildingIcon.sprite = currentBuilding.Icon;
            buildingIcon.gameObject.SetActive(true);
        }
        else
        {
            buildingIcon.gameObject.SetActive(false);
        }
        Debug.Log(currentBuilding.buildingType);
        switch (currentBuilding.buildingType)
        {
            case BuildingType.Job:
                currentType = BuildingType.Job;
                DisplayJob(currentBuilding.GetComponent<Job>());
                break;

            case BuildingType.House:
                currentType = BuildingType.House;
                DisplayHouse(currentBuilding.GetComponent<House>());
                break;

            case BuildingType.Recreation:
                currentType = BuildingType.Recreation;
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

        jobDetails[3].text = "Employees Lose " + currentJob.restLoss + " Energy Per Hour";

        pages[0].SetActive(true);
    }

    private void DisplayHouse(House currentHouse)
    {
        FillGrid(inhabitantsGrid, currentHouse.Inhabitants);
        FillGrid(houseUserGrid, currentHouse.users);

        jobDetails[0].text = "Inhabitants Gain " + currentHouse.restValue + " Energy Per Hour";
        jobDetails[1].text = "Inhabitant Capacity Is At " + currentHouse.Inhabitants.Count + "/" + currentHouse.Capacity;

        pages[1].SetActive(true);
    }

    private void DisplayRecreation(Recreation currentRecreation)
    {
        FillGrid(recreationGrid, currentRecreation.users);

        recreationDetails[0].text = "Users Gain " + currentRecreation.happinessGain + " Happiness Per Hour";
        jobDetails[0].text = "User Capacity Is At " + currentRecreation.users.Count + "/" + currentRecreation.Capacity;

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

            newVillager.GetComponent<UIVillagerButton>().Initialize(newVillagers[i], null, currentBuilding);

            scrollViewExtender.ObjectsInGrid.Add(newVillager);
            newVillager.transform.localScale = scrollViewExtender.GridObjectPrefab.transform.localScale;
        }
    }

    public void AlterAssignment(bool opening)
    {
        if (opening)
        {
            assignmentPannel.SetActive(true);

            for (int i = 0; i < pages.Count; i++)
            {
                // Close All Pages
                pages[i].SetActive(false);
            }

            List<VillagerController> villagers = VillagerManager.Instance.Villagers;
            List<VillagerInfo> villagerVIs = new List<VillagerInfo>();

            for (int i = 0; i < villagers.Count; i++)
            {
                switch (currentType)
                {
                    case BuildingType.Job:
                        if (villagers[i].VI.job != currentBuilding.GetComponent<Job>())
                        {
                            villagerVIs.Add(villagers[i].VI);
                        }
                        break;

                    case BuildingType.House:
                        if (villagers[i].VI.house != currentBuilding.GetComponent<House>())
                        {
                            villagerVIs.Add(villagers[i].VI);
                        }
                        break;

                    default:
                        break;
                }
            }

            FillGrid(assignmentScroll, villagerVIs);
        }
        else
        {
            // Close Assignment Window Then Open Previous Display
            assignmentPannel.SetActive(false);

            switch (currentType)
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
    }
}
