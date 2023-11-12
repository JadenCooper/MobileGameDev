using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    // Wood, Stone, Food, Gold, Happiness, VillagerCount
    public List<float> Resources = new List<float>(); // Wood, Stone, Food, Gold, Happiness, VillagerCount
    public List<Vector2> dailyResourceChange = new List<Vector2>(); // X Is Gain Y Is Loss

    [SerializeReference]
    private List<Sprite> resourceSprites = new List<Sprite>();

    [SerializeField]
    private UIManager uiManager;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Reset Daily losses And Gain At Start Of New Day
        DayNightManager.Instance.NewDay += ResetDailyResources;
    }

    private void ResetDailyResources()
    {
        dailyResourceChange.Clear();
    }

    public void ResourceChange(Resource resourceToChange, float amountToChange)
    {
        // Change Resource Based On Its Type
        int ResourceIndex;
        switch (resourceToChange)
        {
            case Resource.Wood:
                ResourceIndex = 0;
                break;

            case Resource.Stone:
                ResourceIndex = 1;
                break;

            case Resource.Food:
                ResourceIndex = 2;
                break;

            case Resource.Gold:
                ResourceIndex = 3;
                break;

            case Resource.Villagers:
                ResourceIndex = 4;
                break;

            case Resource.None:
                return;

            default:
                Debug.Log("Resource Manager Broke In Resource Change Method");
                return;
        }
        amountToChange = Mathf.Ceil(amountToChange);
        Resources[ResourceIndex] += amountToChange;

        if (amountToChange < 0)
        {
            // So Lose Resource
            dailyResourceChange[ResourceIndex] += new Vector2(0, amountToChange);
        }
        else
        {
            dailyResourceChange[ResourceIndex] += new Vector2(amountToChange, 0);
        }

        uiManager.UpdateResources(Resources);
    }

    public void UpdateVillageHappiness(float VillageHappiness)
    {
        // Given And Activated By VillagerManager
        Resources[0] = VillageHappiness;
        uiManager.UpdateResources(Resources);
    }

    public bool ResourceCheck(Resource resourceToCheck, float requiredAmount)
    {
        // This Method Checks To See If Player Has Enough Of A Certain Resource
        switch (resourceToCheck)
        {
            case Resource.Wood:
                if (Resources[1] >= requiredAmount)
                {
                    return true;
                }
                break;

            case Resource.Stone:
                if (Resources[2] >= requiredAmount)
                {
                    return true;
                }
                break;

            case Resource.Food:
                if (Resources[3] >= requiredAmount)
                {
                    return true;
                }
                break;

            case Resource.Gold:
                if (Resources[4] >= requiredAmount)
                {
                    return true;
                }
                break;

            case Resource.Villagers:
                if (Resources[5] >= requiredAmount)
                {
                    return true;
                }
                break;

            case Resource.None:
                return true;


            default:
                Debug.Log("Resource Manager Broke In Resource Check Method");
                break;
        }

        return false;
    }

    public Sprite GetResourceSprite(Resource resourceToGet)
    {
        // Spits Out Resource Sprite
        switch (resourceToGet)
        {
            case Resource.Wood:
                return resourceSprites[0];

            case Resource.Stone:
                return resourceSprites[1];

            case Resource.Gold:
                return resourceSprites[2];

            case Resource.Food:
                return resourceSprites[3];

            case Resource.Villagers:
                return resourceSprites[5];

            default:
                return null;
        }
    }
}
