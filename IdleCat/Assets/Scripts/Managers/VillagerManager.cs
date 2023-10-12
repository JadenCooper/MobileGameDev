using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerManager : MonoBehaviour
{
    public static VillagerManager Instance { get; private set; }

    public List<VillagerController> Villagers = new List<VillagerController>();
    public List<VillagerPetitionAI> PostponedVillagerPetitions = new List<VillagerPetitionAI>();
    public List<VillagerPetitionAI> PostponedNonVillagerPetitions = new List<VillagerPetitionAI>();
    public List<VillagerPetitionAI> QueuedVillagers = new List<VillagerPetitionAI>();

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

    public PetitionManager petitionManager;
    public Transform LeaveTrigger;

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
        petitionManager.RemoveAll();

        // Some PetitionSlots Will Later Be Taken Up By Quests And Unlocks
        int currentSlots = BuildingManager.Instance.PetitionSlots;


        List<VillagerPetitionAI> TempList = new List<VillagerPetitionAI>();

        // Check For Postponed Decisions First
        foreach (VillagerPetitionAI villager in PostponedVillagerPetitions)
        {
            // Postponed Villagers Get First Priority
            villager.PostponeTime--;
            if (villager.PostponeTime <= 0)
            {
                QueuedVillagers.Add(villager);
                PostponedVillagerPetitions.Remove(villager);
            }
        }

        if (QueuedVillagers.Count < currentSlots)
        {
            foreach (VillagerPetitionAI villager in PostponedNonVillagerPetitions)
            {
                // Postponed Villagers Get First Priority
                villager.PostponeTime--;
                if (villager.PostponeTime <= 0)
                {
                    QueuedVillagers.Add(villager);
                    PostponedVillagerPetitions.Remove(villager);
                }
            }
        }


        int i = 0;
        while (QueuedVillagers.Count > 0 && currentSlots > 0)
        {
            QueuedVillagers[i].gameObject.transform.parent.gameObject.SetActive(true);
            QueuedVillagers[i].GetComponent<VillagerController>().enabled = false;
            QueuedVillagers[i].enabled = true;
            petitionManager.SetPetitionSlot(QueuedVillagers[i]);

            QueuedVillagers.RemoveAt(i);
            i++;
            currentSlots--;
        }

        QueuedVillagers.RemoveRange(0, i);

        // If Slots Leftover Generate New Villagers
        if (currentSlots > 0)
        {
            StartCoroutine(SpawnVillagerTimer(currentSlots));
        }
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

        VillagerInfo tempVI = Instantiate(defaultVillagerInfo);
        tempVI.house = Inn;
        tempVI.job = Mason;
        tempVI.Species = UnlockedSpecies[Random.Range(0, UnlockedSpecies.Count)];
        tempVI.schedule = Instantiate(defaultSchedule);
        tempVI.gameObject = NewVillager;

        VillagerPetitionAI VPAI = NewVillager.GetComponentInChildren<VillagerPetitionAI>();
        VPAI.Initialize(tempVI);
        petitionManager.SetPetitionSlot(VPAI);
    }

    public void VillagerJoinsVillage(VillagerPetitionAI VPAI)
    {
        VillagerController VC = VPAI.gameObject.GetComponent<VillagerController>();

        if (!VPAI.VillageInhabitant)
        {
            VC.Initialize(VPAI.VI);
            Villagers.Add(VC);
            ResourceManager.Instance.ResourceChange(Resource.Villagers, 1);
            VPAI.VillageInhabitant = true;
        }

        VC.enabled = true;
        VPAI.enabled = false;
    }

    public Vector2 VillagerLeavesVillage(VillagerPetitionAI VPAI)
    {
        petitionManager.RemoveFromSlots(VPAI);
        return new Vector2(LeaveTrigger.position.x, 0);
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
