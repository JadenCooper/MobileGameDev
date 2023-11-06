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
    [SerializeField]
    private GameObject uIVillagerButtonPrefab;
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
    private Image lossResourceType;
    [SerializeField]
    private List<Sprite> resourceTypeSprites = new List<Sprite>();

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


}
