using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job : Building
{
    public List<VillagerInfo> Employees = new List<VillagerInfo>();

    public Resource resourceToGain;
    public float resourceGain;
    public Resource resourceToCost;
    public float resourceCost;

    public float happinessLoss;
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        VillagerController vc = collision.gameObject.GetComponent<VillagerController>();
        if (vc != null && vc.enabled == true)
        {
            if (vc.VI.currentState == VillagerState.Work && vc.VI.job == this)
            {
                vc.ReachedLocation();
            }
        }
    }

    public override void BuildingAction(VillagerInfo currentUser)
    {
        if (resourceToCost == Resource.None || ResourceManager.Instance.ResourceCheck(resourceToCost, resourceCost)) // If Can Afford The Cost
        {
            currentUser.happiness -= happinessLoss;
            currentUser.happiness = Mathf.Clamp(currentUser.happiness, 0, 100);


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

            if (resourceToCost != Resource.None)
            {
                ResourceManager.Instance.ResourceChange(resourceToCost, resourceCost);
            }
        }
    }

    public override void InteractAction()
    {
        throw new System.NotImplementedException();
    }
}
