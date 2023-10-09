using System.Collections;
using System.Collections.Generic;
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
        DayNightManager.Instance.NewDay += SpawnVillagers;
        SpawnVillagers();
    }

    public void SpawnVillagers()
    {
        // Some PetitionSlots Will Later Be Taken Up By Quests And Unlocks
        StartCoroutine(SpawnVillagerTimer(BuildingManager.Instance.PetitionSlots));
    }

    private IEnumerator SpawnVillagerTimer(int i)
    {
        // Iteratively Spawn Villagers Until I Is 0 With A Second Gap Between
        if (i <= 0)
        {
            yield break;
        }
        GenerateVillager();
        i--;
        yield return new WaitForSeconds(1f);

        StartCoroutine(SpawnVillagerTimer(i));
    }

    [ContextMenu("Generate Villager")]
    public void GenerateVillager()
    {
        GameObject NewVillager = Instantiate(villagerPrefab, VillagerSpawnPoint.transform);
        NewVillager.transform.parent = null;
        NewVillager.transform.parent = this.transform;
        VillagerController VC = NewVillager.GetComponentInChildren<VillagerController>();
        VillagerInfo tempVI = Instantiate(defaultVillagerInfo);
        tempVI.house = Inn;
        tempVI.job = Mason;
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
