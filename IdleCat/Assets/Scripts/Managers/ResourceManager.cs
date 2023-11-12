using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    // Wood, Stone, Food, Gold, Happiness, VillagerCount
    public GameResources CurrentResources;
    public GameResources DailyLosses;
    public GameResources DailyGains;

    [SerializeReference]
    private List<Sprite> resourceSprites = new List<Sprite>();
    [SerializeReference]
    private List<Sprite> happySprites = new List<Sprite>();

    [SerializeField]
    private UIManager uiManager;

    public List<TMP_Text> NotebookResourceTexts = new List<TMP_Text>();
    public List<Image> NotebookResourceIcons = new List<Image>();
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
        CurrentResources = new();
        DailyLosses = new();
        DailyGains = new();

        CurrentResources.Food = 100;
        CurrentResources.Gold = 100;
        CurrentResources.Stone = 100;
        CurrentResources.Wood = 100;

        SaveManager.Instance.SaveCall += OnSave;
        SaveManager.Instance.LoadCall += OnLoad;
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
                CurrentResources.Wood += amountToChange;

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
                CurrentResources.Stone += amountToChange;

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
                CurrentResources.Food += amountToChange;

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
                CurrentResources.Gold += amountToChange;

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
                CurrentResources.VillagerCount += amountToChange;

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

        uiManager.UpdateResources(CurrentResources);
    }

    public void UpdateVillageHappiness(float VillageHappiness)
    {
        // Given And Activated By VillagerManager
        CurrentResources.TownHappiness = VillageHappiness;
        uiManager.UpdateResources(CurrentResources);
    }

    public bool ResourceCheck(Resource resourceToCheck, float requiredAmount)
    {
        // This Method Checks To See If Player Has Enough Of A Certain Resource
        switch (resourceToCheck)
        {
            case Resource.Wood:
                if (CurrentResources.Wood >= -requiredAmount)
                {
                    return true;
                }
                break;

            case Resource.Stone:
                if (CurrentResources.Stone >= -requiredAmount)
                {
                    return true;
                }
                break;

            case Resource.Food:
                if (CurrentResources.Food >= -requiredAmount)
                {
                    return true;
                }
                break;

            case Resource.Gold:
                if (CurrentResources.Gold >= -requiredAmount)
                {
                    return true;
                }
                break;

            case Resource.Villagers:
                if (CurrentResources.VillagerCount >= -requiredAmount)
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
                return resourceSprites[4];

            default:
                return null;
        }
    }

    public Sprite GetHappySprite(Happiness happiness)
    {
        switch (happiness)
        {
            case Happiness.Ecstatic:
                return happySprites[0];

            case Happiness.Happy:
                return happySprites[1];

            case Happiness.Netural:
                return happySprites[2];

            case Happiness.Sad:
                return happySprites[3];

            case Happiness.Miserable:
                return happySprites[4];


            default:
                return null;

        }
    }
    public void SetNotebook()
    {
        NotebookResourceIcons[0].sprite = GetResourceSprite(Resource.Food);
        NotebookResourceTexts[0].text = CurrentResources.Food.ToString();
        NotebookResourceTexts[1].text = DailyGains.Food.ToString();
        NotebookResourceTexts[2].text = DailyLosses.Food.ToString();

        NotebookResourceIcons[1].sprite = GetResourceSprite(Resource.Stone);
        NotebookResourceTexts[3].text = CurrentResources.Stone.ToString();
        NotebookResourceTexts[4].text = DailyGains.Stone.ToString();
        NotebookResourceTexts[5].text = DailyLosses.Stone.ToString();

        NotebookResourceIcons[2].sprite = GetHappySprite(Data.CalculateHappinessState(CurrentResources.TownHappiness));
        NotebookResourceTexts[6].text = CurrentResources.TownHappiness.ToString() + "%";

        NotebookResourceIcons[3].sprite = GetResourceSprite(Resource.Wood);
        NotebookResourceTexts[7].text = CurrentResources.Wood.ToString();
        NotebookResourceTexts[8].text = DailyGains.Wood.ToString();
        NotebookResourceTexts[9].text = DailyLosses.Wood.ToString();

        NotebookResourceIcons[4].sprite = GetResourceSprite(Resource.Gold);
        NotebookResourceTexts[10].text = CurrentResources.Gold.ToString();
        NotebookResourceTexts[11].text = DailyGains.Gold.ToString();
        NotebookResourceTexts[12].text = DailyLosses.Gold.ToString();

        NotebookResourceIcons[5].sprite = GetResourceSprite(Resource.Villagers);
        NotebookResourceTexts[13].text = CurrentResources.VillagerCount.ToString();
        NotebookResourceTexts[14].text = DailyGains.VillagerCount.ToString();
        NotebookResourceTexts[15].text = DailyLosses.VillagerCount.ToString();
    }

    public void OnLoad()
    {
        CurrentResources = SaveData.current.currentResources;
        DailyGains = SaveData.current.dailyGains;
        DailyLosses = SaveData.current.dailyLosses;
    }

    public void OnSave()
    {
        SaveData.current.currentResources = CurrentResources;
        SaveData.current.dailyGains = DailyGains;
        SaveData.current.dailyLosses = DailyLosses;
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
