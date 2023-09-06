using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VillagerManager : MonoBehaviour
{
    public static VillagerManager Instance { get; private set; }

    public List<VillagerController> Villagers = new List<VillagerController>();
    public List<SpeciesData> UnlockedSpecies = new List<SpeciesData>();
    public List<SpeciesData> AllSpecies = new List<SpeciesData>();

    [SerializeField]
    private GameObject villagerPrefab;
    [SerializeField]
    private VillagerInfo defaultVillagerInfo;
    [SerializeField]
    private Schedule defaultSchedule;
    public GameObject VillagerSpawnPoint;
    public House Inn;
    public Job Mason;

    [ContextMenu("Generate Villager")]
    public void GenerateVillager()
    {
        GameObject NewVillager = Instantiate(villagerPrefab, VillagerSpawnPoint.transform);
        NewVillager.transform.parent = null;
        NewVillager.transform.parent = this.transform;
        VillagerController VC = NewVillager.GetComponentInChildren<VillagerController>();
        VillagerInfo tempVI = Instantiate(defaultVillagerInfo);
        tempVI.house = Inn;
        tempVI.Species = UnlockedSpecies[Random.Range(0, UnlockedSpecies.Count)];
        tempVI.schedule = Instantiate(defaultSchedule);
        //tempVI.schedule = GenerateSchedule(tempVI);
        VC.Initialize(tempVI);
        Villagers.Add(VC);
        ResourceManager.Instance.ResourceChange(Resource.Villagers, 1);
    }

    public void CalculateVillageHappiness()
    {
        float happinessTotal = 0;

        foreach (VillagerController vc in Villagers)
        {
            happinessTotal += vc.villagerInfo.happiness;
        }

        happinessTotal = happinessTotal / Villagers.Count;
        happinessTotal = Mathf.Clamp(happinessTotal, 0, 100);

        ResourceManager.Instance.UpdateVillageHappiness(happinessTotal);
    }
}
