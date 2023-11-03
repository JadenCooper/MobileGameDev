using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerManager : MonoBehaviour
{
    public static VillagerManager Instance { get; private set; }

    public List<VillagerController> Villagers = new List<VillagerController>();
    public List<VillagerPetitionAI> NonVillagers = new List<VillagerPetitionAI>();
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
        SaveManager.Instance.SaveCall += SaveVillagerData;
        SaveManager.Instance.LoadCall += LoadVillagerData;
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
        System.Guid newGuid = System.Guid.NewGuid();
        tempVI.ID = newGuid.ToString();
        // 25 -- 75 odds for adult or young adult
        int RandomNumber = Random.Range(0, 3);
        if (RandomNumber == 0)
        {
            tempVI.LifeStage = LifeStages.Adult;
        }
        else
        {
            tempVI.LifeStage = LifeStages.YoungAdult;
        }

        RandomNumber = Random.Range(0,2);

        if (RandomNumber == 0)
        {
            tempVI.Sex = "Male";
        }
        else
        {
            tempVI.Sex = "Female";
        }

        VillagerPetitionAI VPAI = NewVillager.GetComponentInChildren<VillagerPetitionAI>();
        VPAI.Initialize(tempVI);
        petitionManager.SetPetitionSlot(VPAI);
        NonVillagers.Add(VPAI);
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

    public void SaveVillagerData()
    {
        VillagerManagerSaveDate VMSD = new VillagerManagerSaveDate();

        for (int i = 0; i < Villagers.Count; i++)
        {
            VMSD.Villagers.Add(FillInVillagerSaveData(Villagers[i]));
        }

        for (int i = 0; i < PostponedVillagerPetitions.Count; i++)
        {
            string ID = PostponedVillagerPetitions[i].VI.ID;

            for (int t = 0; t < VMSD.Villagers.Count; t++)
            {
                if (ID == VMSD.Villagers[t].ID)
                {
                    VMSD.Villagers[t].PostponeTime = PostponedVillagerPetitions[i].PostponeTime;
                }
            }
            VMSD.PostponedVillagerPetitionsIDS.Add(PostponedVillagerPetitions[i].VI.ID);
        }

        for (int i = 0; i < NonVillagers.Count; i++)
        {
            VMSD.NonVillagers.Add(FillInVillagerSaveData(NonVillagers[i].GetComponent<VillagerController>()));
        }

        for (int i = 0; i < PostponedNonVillagerPetitions.Count; i++)
        {
            VMSD.PostponedNonVillagerPetitions.Add(FillInVillagerSaveData(PostponedNonVillagerPetitions[i].GetComponent<VillagerController>()));
        }

        for (int i = 0; i < QueuedVillagers.Count; i++)
        {
            VMSD.QueuedVillagers.Add(FillInVillagerSaveData(QueuedVillagers[i].GetComponent<VillagerController>()));
        }

    }

    public void LoadVillagerData()
    {
        VillagerManagerSaveDate VMSD = SaveData.current.VMSD;

        for (int i = 0; i < VMSD.Villagers.Count; i++)
        {
            VillagerSaveData VSD = VMSD.Villagers[i];
            GameObject newVillager = LoadInVillagerSaveData(VSD);
            newVillager.GetComponentInChildren<VillagerPetitionAI>().enabled = false;
            Villagers.Add(newVillager.GetComponentInChildren<VillagerController>());
        }

        for (int i = 0; i < VMSD.PostponedVillagerPetitionsIDS.Count; i++)
        {
            string ID = PostponedVillagerPetitions[i].VI.ID;

            for (int t = 0; t < Villagers.Count; t++)
            {
                if (ID == Villagers[t].VI.ID)
                {
                    PostponedVillagerPetitions.Add(Villagers[t].GetComponent<VillagerPetitionAI>());
                }
            }
        }

        for (int i = 0; i < VMSD.PostponedNonVillagerPetitions.Count; i++)
        {
            VillagerSaveData VSD = VMSD.PostponedNonVillagerPetitions[i];
            GameObject newVillager = LoadInVillagerSaveData(VSD);
            newVillager.GetComponentInChildren<VillagerController>().enabled = false;
            PostponedNonVillagerPetitions.Add(newVillager.GetComponentInChildren<VillagerPetitionAI>());
            newVillager.SetActive(false);
        }

        for (int i = 0; i < VMSD.QueuedVillagers.Count; i++)
        {
            VillagerSaveData VSD = VMSD.Villagers[i];
            GameObject newVillager = LoadInVillagerSaveData(VSD);
            newVillager.GetComponentInChildren<VillagerController>().enabled = false;
            QueuedVillagers.Add(newVillager.GetComponentInChildren<VillagerPetitionAI>());
            newVillager.SetActive(false);
        }

        // Use IDS To Reference Family And Buildings
    }

    public VillagerSaveData FillInVillagerSaveData(VillagerController vc)
    {
        VillagerSaveData VSD = new VillagerSaveData();
        VillagerInfo tempVI = vc.VI;

        VSD.ID = tempVI.ID;
        VSD.VillagerTransform = vc.gameObject.transform;
        VSD.VillageInhabitant = vc.GetComponent<VillagerPetitionAI>().VillageInhabitant;

        VSD.FirstName = tempVI.FirstName;
        VSD.LastName = tempVI.LastName;
        VSD.currentState = tempVI.currentState;

        VSD.JobID = tempVI.job.ID;
        VSD.HouseID = tempVI.house.ID;
        if (tempVI.recreationGoal != null)
        {
            VSD.RecreationID = tempVI.recreationGoal.ID;
        }
        if (tempVI.currentElevatorGoal != null)
        {
            VSD.ElevatorID = tempVI.currentElevatorGoal.ID;
        }
        VSD.CurrentLevel = tempVI.CurrentLevel;
        VSD.CurrentGoal = tempVI.CurrentGoal;

        VSD.Age = tempVI.Age;
        VSD.LifeStage = tempVI.LifeStage;
        VSD.Sex = tempVI.Sex;
        VSD.happiness = tempVI.happiness;
        VSD.rest = tempVI.rest;

        VSD.MotherID = tempVI.Mother.ID;
        VSD.FatherID = tempVI.Father.ID;
        VSD.PartnerID = tempVI.Partner.ID;

        for (int c = 0; c < tempVI.Children.Count; c++)
        {
            VSD.ChildrenID.Add(tempVI.Children[c].ID);
        }

        VSD.VillagerStates = tempVI.schedule.VillagerStates;
        VSD.Moving = tempVI.Moving;

        VSD.transform = vc.gameObject.transform;

        return VSD;
    }

    public GameObject LoadInVillagerSaveData(VillagerSaveData VSD)
    {
        GameObject newVillager = Instantiate(villagerPrefab);
        newVillager.transform.position = VSD.transform.position;
        newVillager.transform.rotation = VSD.transform.rotation;
        newVillager.transform.localScale = VSD.transform.localScale;
        newVillager.transform.parent = null;
        newVillager.transform.parent = this.transform;

        VillagerInfo tempVI = Instantiate(defaultVillagerInfo);
        tempVI.FirstName = VSD.FirstName;
        tempVI.LastName = VSD.LastName;
        tempVI.currentState = VSD.currentState;
        tempVI.CurrentLevel = VSD.CurrentLevel;
        tempVI.CurrentGoal = VSD.CurrentGoal;

        tempVI.Age = VSD.Age;
        tempVI.LifeStage = VSD.LifeStage;
        tempVI.Sex = VSD.Sex;
        tempVI.happiness = VSD.happiness;
        tempVI.rest = VSD.rest;

        tempVI.schedule.VillagerStates = VSD.VillagerStates;
        tempVI.Moving = VSD.Moving;


        VillagerPetitionAI VPAI = newVillager.GetComponentInChildren<VillagerPetitionAI>();
        VPAI.enabled = true;
        VPAI.Initialize(tempVI);

        VillagerController VC = newVillager.GetComponentInChildren<VillagerController>();
        VC.enabled = true;
        VC.Initialize(tempVI);

        return newVillager;
    }
}
