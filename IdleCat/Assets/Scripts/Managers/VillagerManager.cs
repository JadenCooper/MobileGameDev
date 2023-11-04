using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerManager : MonoBehaviour
{
    private Dictionary<string, VillagerInfo> villagerIDToVillagerInfo = new Dictionary<string, VillagerInfo>();

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
    public Job Workhouse;

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
        tempVI.job = Workhouse;
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
        VillagerController VC = NewVillager.GetComponentInChildren<VillagerController>();
        VC.Initialize(tempVI);
        VC.enabled = false;
        petitionManager.SetPetitionSlot(VPAI);
        NonVillagers.Add(VPAI);
    }

    public void VillagerJoinsVillage(VillagerPetitionAI VPAI)
    {
        VillagerController VC = VPAI.gameObject.GetComponent<VillagerController>();
        NonVillagers.Remove(VPAI);
        if (!VPAI.VillageInhabitant)
        {
            Villagers.Add(VC);
            ResourceManager.Instance.ResourceChange(Resource.Villagers, 1);
            VPAI.VillageInhabitant = true;
        }

        VC.enabled = true;
        VPAI.enabled = false;
    }

    public Vector2 VillagerLeavesVillage()
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
        villagerIDToVillagerInfo.Clear();
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

        SaveData.current.VMSD = VMSD;
    }

    public void LoadVillagerData()
    {
        // Need To Add Clear All Functionality
        foreach (VillagerController villager in Villagers)
        {
            Destroy(villager.gameObject.transform.parent.gameObject);
        }
        Villagers.Clear();
        PostponedVillagerPetitions.Clear();

        foreach (VillagerPetitionAI villager in PostponedVillagerPetitions)
        {
            Destroy(villager.gameObject.transform.parent.gameObject);
        }

        PostponedVillagerPetitions.Clear();

        foreach (VillagerPetitionAI villager in NonVillagers)
        {
            Destroy(villager.gameObject.transform.parent.gameObject);
        }

        NonVillagers.Clear();

        VillagerManagerSaveDate VMSD = SaveData.current.VMSD;

        for (int i = 0; i < VMSD.Villagers.Count; i++)
        {
            VillagerSaveData VSD = VMSD.Villagers[i];
            GameObject newVillager = LoadInVillagerSaveData(VSD);
            newVillager.GetComponentInChildren<VillagerPetitionAI>().enabled = false;
            Villagers.Add(newVillager.GetComponentInChildren<VillagerController>());
        }

        if (VMSD.PostponedVillagerPetitionsIDS != null)
        {
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
        AssignVillagerReferences(VMSD);

        for (int i = 0; i < NonVillagers.Count; i++)
        {
            string tag = NonVillagers[i].gameObject.transform.parent.tag;
            if (tag == "Save" || tag == "Destroy")
            {
                // Send To Leave Trigger
               NonVillagers[i].Navigation.GetLocationGoal(NonVillagers[i].VI, VillagerLeavesVillage());
            }
            else
            {
                // Needs To Be Sent To Petition
                petitionManager.SetPetitionSlot(NonVillagers[i]);
            }
        }
    }

    public VillagerSaveData FillInVillagerSaveData(VillagerController vc)
    {
        VillagerSaveData VSD = new VillagerSaveData();
        VillagerInfo tempVI = vc.VI;
        VSD.ID = tempVI.ID;
        VSD.VillagerPosition = vc.gameObject.transform.position;
        VSD.VillageInhabitant = vc.GetComponent<VillagerPetitionAI>().VillageInhabitant;

        VSD.FirstName = tempVI.FirstName;
        VSD.LastName = tempVI.LastName;
        VSD.currentState = tempVI.currentState;

        VSD.JobID = tempVI.job.ID;
        VSD.HouseID = tempVI.house.ID;

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

        if (tempVI.Mother != null)
        {
            VSD.MotherID = tempVI.Mother.ID;
        }
        if (tempVI.Mother != null)
        {
            VSD.FatherID = tempVI.Father.ID;
        }
        if (tempVI.Mother != null)
        {
            VSD.PartnerID = tempVI.Partner.ID;
        }

        for (int c = 0; c < tempVI.Children.Count; c++)
        {
            VSD.ChildrenID.Add(tempVI.Children[c].ID);
        }

        VSD.VillagerStates = tempVI.schedule.VillagerStates;
        VSD.Moving = tempVI.Moving;

        VSD.VillagerPosition = vc.gameObject.transform.position;

        VSD.Tag = vc.gameObject.transform.parent.tag;

        VSD.Species = tempVI.Species.Species;

        villagerIDToVillagerInfo[VSD.ID] = vc.VI;
        return VSD;
    }

    public GameObject LoadInVillagerSaveData(VillagerSaveData VSD)
    {
        GameObject newVillager = Instantiate(villagerPrefab);
        newVillager.transform.position = VSD.VillagerPosition;
        newVillager.transform.parent = null;
        newVillager.transform.parent = this.transform;
        newVillager.tag = VSD.Tag;

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

        for (int i = 0; i < UnlockedSpecies.Count; i++)
        {
            if (VSD.Species == UnlockedSpecies[i].Species)
            {
                tempVI.Species = UnlockedSpecies[i];
                break;
            }
        }

        VillagerPetitionAI VPAI = newVillager.GetComponentInChildren<VillagerPetitionAI>();
        VPAI.enabled = true;
        VPAI.Initialize(tempVI);

        VillagerController VC = newVillager.GetComponentInChildren<VillagerController>();
        VC.enabled = true;
        VC.Initialize(tempVI);

        return newVillager;
    }

    private void AssignVillagerReferences(VillagerManagerSaveDate VMSD)
    {
        List<VillagerInfo> allVillagerInfos = new List<VillagerInfo>();
        List<VillagerSaveData> allVSD = new List<VillagerSaveData>();

        // Combine All Villager Infos
        for (int i = 0; i < Villagers.Count; i++)
        {
            allVillagerInfos.Add(Villagers[i].VI);
        }
        for (int i = 0; i < PostponedVillagerPetitions.Count; i++)
        {
            allVillagerInfos.Add(PostponedVillagerPetitions[i].VI);
        }
        for (int i = 0; i < NonVillagers.Count; i++)
        {
            allVillagerInfos.Add(NonVillagers[i].VI);
        }

        // Combine All VSD
        allVSD.AddRange(VMSD.Villagers);
        allVSD.AddRange(VMSD.PostponedNonVillagerPetitions);
        allVSD.AddRange(VMSD.NonVillagers);


        for (int i = 0; i < allVSD.Count; i++)
        {
            if (allVSD[i].MotherID != null && villagerIDToVillagerInfo.ContainsKey(allVSD[i].MotherID))
            {
                allVillagerInfos[i].Mother = villagerIDToVillagerInfo[allVSD[i].MotherID];
            }

            if (allVSD[i].MotherID != null && villagerIDToVillagerInfo.ContainsKey(allVSD[i].FatherID))
            {
                allVillagerInfos[i].Father = villagerIDToVillagerInfo[allVSD[i].FatherID];
            }

            if (allVSD[i].MotherID != null && villagerIDToVillagerInfo.ContainsKey(allVSD[i].PartnerID))
            {
                allVillagerInfos[i].Partner = villagerIDToVillagerInfo[allVSD[i].PartnerID];
            }

            foreach (string ID in allVSD[i].ChildrenID)
            {
                if (villagerIDToVillagerInfo.ContainsKey(ID))
                {
                    allVillagerInfos[i].Children.Add(villagerIDToVillagerInfo[ID]);
                }
            }

        }

        BuildingManager.Instance.LoadBuildingSaveData(allVillagerInfos, allVSD);
    }

    public void TriggerNavigation()
    {
        for (int i = 0; i < Villagers.Count; i++)
        {
            Villagers[i].GetNewLocationGoal();
        }
    }
}
