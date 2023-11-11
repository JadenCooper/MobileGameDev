using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public List<float> Resources = new List<float>(); // Wood, Stone, Food, Gold, Happiness, VillagerCount
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
    public void ResourceChange(Resource resourceToChange, float amountToChange)
    {
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

        Resources[ResourceIndex] += amountToChange;
        Resources[ResourceIndex] = Mathf.Ceil(Resources[ResourceIndex]);

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
