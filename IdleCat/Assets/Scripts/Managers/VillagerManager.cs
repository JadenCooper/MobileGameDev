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
            }
        }

        PostponedVillagerPetitions = RemoveFromList(PostponedVillagerPetitions);

        if (QueuedVillagers.Count < currentSlots)
        {
            foreach (VillagerPetitionAI villager in PostponedNonVillagerPetitions)
            {
                // Postponed Villagers Get First Priority
                villager.PostponeTime--;
                if (villager.PostponeTime <= 0)
                {
                    QueuedVillagers.Add(villager);
                }
            }

            PostponedNonVillagerPetitions = RemoveFromList(PostponedNonVillagerPetitions);
        }

        StartCoroutine(SpawnVillagerTimer(currentSlots));
    }

    private List<VillagerPetitionAI> RemoveFromList(List<VillagerPetitionAI> compareList)
    {
        for (int i = 0; i < QueuedVillagers.Count; i++)
        {
            for (int t = 0; t < compareList.Count; t++)
            {
                if (QueuedVillagers[i] == compareList[t])
                {
                    compareList.RemoveAt(t);
                    t--;
                }
            }
        }

        return compareList;
    }
    private IEnumerator SpawnVillagerTimer(int i)
    {
        // Iteratively Spawn Villagers Until I Is 0 With A Second Gap Between
        if (i <= 0)
        {
            yield break;
        }

        if (QueuedVillagers.Count > 0)
        {
            QueuedVillagers[0].tag = "Intractable";
            GameObject parent = QueuedVillagers[0].gameObject.transform.parent.gameObject;
            parent.transform.position = VillagerSpawnPoint.transform.position;
            parent.SetActive(true);

            QueuedVillagers[0].GetComponent<VillagerController>().enabled = false;
            QueuedVillagers[0].enabled = true;
            petitionManager.SetPetitionSlot(QueuedVillagers[0]);
            QueuedVillagers.RemoveAt(0);
        }

        else
        {
            GenerateVillager();
        }

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
        return new Vector2(LeaveTrigger.position.x, 0);
    }

    public void CalculateVillageHappiness()
    {
        float happinessTotal = 0;

        foreach (VillagerController vc in Villagers)
        {
            happinessTotal += vc.VI.happiness;
        }

        happinessTotal = happinessTotal / Villagers.Count;
        happinessTotal = Mathf.Clamp(happinessTotal, 0, 100);

        ResourceManager.Instance.UpdateVillageHappiness(happinessTotal);
    }
}
