using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Job : Building
{
    public List<VillagerController> Employees = new List<VillagerController>();

    public Resource resourceToGain;
    public float resourceGain;
    public Resource resourceToCost;
    public float resourceCost;

    public float happinessLoss;
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        VillagerController vc = collision.gameObject.GetComponent<VillagerController>();
        if (vc != null)
        {
            if (vc.villagerInfo.currentState == VillagerState.Work && vc.villagerInfo.job == this)
            {
                vc.ReachedLocation();
            }
        }
    }

    public override void BuildingAction(VillagerInfo currentUser)
    {
        if (ResourceManager.Instance.ResourceCheck(resourceToCost, resourceCost)) // If Can Afford The Cost
        {
            currentUser.happiness -= happinessLoss;
            currentUser.happiness = Mathf.Clamp(currentUser.happiness, 0, 100);
            currentUser.happiness = Mathf.Ceil(currentUser.happiness);

            VillagerManager.Instance.CalculateVillageHappiness();
            Happiness currentUserHappiness = Data.CalculateHappinessState(currentUser.happiness);

            float ResourceMultiplier = 1;

            switch (currentUserHappiness)
            {
                case Happiness.Ecstatic:
                    ResourceMultiplier = 1.5f;
                    break;

                case Happiness.Sad:
                    ResourceMultiplier = 0.5f;
                    break;

                case Happiness.Miserable:
                    ResourceMultiplier = 0;
                    break;
            }

            ResourceManager.Instance.ResourceChange(resourceToGain, resourceGain * ResourceMultiplier);
            ResourceManager.Instance.ResourceChange(resourceToCost, resourceCost);
        }
    }

    public override void InteractAction()
    {
        throw new System.NotImplementedException();
    }
}
