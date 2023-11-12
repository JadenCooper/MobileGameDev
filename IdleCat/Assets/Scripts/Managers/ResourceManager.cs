using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    // Wood, Stone, Food, Gold, Happiness, VillagerCount
    public GameResources Resources;
    public GameResources DailyLosses;
    public GameResources DailyGains;

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
        Resources = new();
        DailyLosses = new();
        DailyGains = new();
    }

    private void ResetDailyResources()
    {
        DailyLosses.Clear();
        DailyGains.Clear();
    }

    public void ResourceChange(Resource resourceToChange, float amountToChange)
    {
        // Change Resource Based On Its Type
        amountToChange = Mathf.Ceil(amountToChange);
        bool loss;
        if (amountToChange < 0)
        {
            // So Lose Resource
            loss = true;
        }
        else
        {
            loss = false;
        }

        switch (resourceToChange)
        {
            case Resource.Wood:
                Resources.Wood += amountToChange;

                if (loss)
                {
                    DailyLosses.Wood += amountToChange;
                }
                else
                {
                    DailyGains.Wood += amountToChange;
                }

                break;

            case Resource.Stone:
                Resources.Stone += amountToChange;

                if (loss)
                {
                    DailyLosses.Stone += amountToChange;
                }
                else
                {
                    DailyGains.Stone += amountToChange;
                }

                break;

            case Resource.Food:
                Resources.Food += amountToChange;

                if (loss)
                {
                    DailyLosses.Food += amountToChange;
                }
                else
                {
                    DailyGains.Food += amountToChange;
                }

                break;

            case Resource.Gold:
                Resources.Gold += amountToChange;

                if (loss)
                {
                    DailyLosses.Gold += amountToChange;
                }
                else
                {
                    DailyGains.Gold += amountToChange;
                }

                break;

            case Resource.Villagers:
                Resources.VillagerCount += amountToChange;

                if (loss)
                {
                    DailyLosses.VillagerCount += amountToChange;
                }
                else
                {
                    DailyGains.VillagerCount += amountToChange;
                }

                break;

            case Resource.None:
                return;

            default:
                Debug.Log("Resource Manager Broke In Resource Change Method");
                return;
        }

        uiManager.UpdateResources(Resources);
    }

    public void UpdateVillageHappiness(float VillageHappiness)
    {
        // Given And Activated By VillagerManager
        Resources.TownHappiness = VillageHappiness;
        uiManager.UpdateResources(Resources);
    }

    public bool ResourceCheck(Resource resourceToCheck, float requiredAmount)
    {
        // This Method Checks To See If Player Has Enough Of A Certain Resource
        switch (resourceToCheck)
        {
            case Resource.Wood:
                if (Resources.Wood >= requiredAmount)
                {
                    return true;
                }
                break;

            case Resource.Stone:
                if (Resources.Stone >= requiredAmount)
                {
                    return true;
                }
                break;

            case Resource.Food:
                if (Resources.Food >= requiredAmount)
                {
                    return true;
                }
                break;

            case Resource.Gold:
                if (Resources.Gold >= requiredAmount)
                {
                    return true;
                }
                break;

            case Resource.Villagers:
                if (Resources.VillagerCount >= requiredAmount)
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

[System.Serializable]
public class GameResources
{
    [field: SerializeField]
    public float Food;
    [field: SerializeField]
    public float Stone;
    [field: SerializeField]
    public float Wood;
    [field: SerializeField]
    public float TownHappiness;
    [field: SerializeField]
    public float Gold;
    [field: SerializeField]
    public float VillagerCount;

    public void Clear()
    {
        Food = 0;
        Stone = 0;
        Wood = 0;
        Gold = 0;
        VillagerCount = 0;
    }
}
