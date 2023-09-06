using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public List<float> Resources = new List<float>(); // Happiness, Wood, Stone, Food, Gold, VillagerCount

    [SerializeField]
    private UIManager uiManager;
    public void ResourceChange(Resource resourceToChange, float amountToChange)
    {
        switch (resourceToChange)
        {
            case Resource.Wood:
                Resources[1] += amountToChange;
                break;

            case Resource.Stone:
                Resources[2] += amountToChange;
                break;

            case Resource.Food:
                Resources[3] += amountToChange;
                break;

            case Resource.Gold:
                Resources[4] += amountToChange;
                break;

            case Resource.Villagers:
                Resources[5] += amountToChange;
                break;

            default:
                Debug.Log("Resource Change Broke In Resource Change Method");
                return;
        }

        uiManager.UpdateResources(Resources);
    }

    public void UpdateVillageHappiness(float VillageHappiness)
    {
        // Given And Activated By VillagerManager
        Resources[0] = VillageHappiness;
        uiManager.UpdateResources(Resources);
    }
}
