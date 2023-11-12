using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Job : Building
{
    public List<VillagerInfo> Employees = new List<VillagerInfo>();

    public Resource resourceToGain;
    public float resourceGain;
    public float restLoss;
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
        // Villager Cant Work If Not Happy Enough Or Too Tired
        if (currentUser.happiness >= happinessLoss && currentUser.rest >= restLoss)
        {
            currentUser.happiness -= happinessLoss;
            currentUser.happiness = Mathf.Clamp(currentUser.happiness, 0, 100);
            currentUser.rest -= restLoss;
            currentUser.rest = Mathf.Clamp(currentUser.rest, 0, 100);

            VillagerManager.Instance.CalculateVillageHappiness();
            Happiness currentUserHappiness = Data.CalculateHappinessState(currentUser.happiness);

            // Villager Gets A Output Multiplier Based On Their Happiness
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
                    ResourceMultiplier = 0.25f;
                    break;
            }

            ResourceManager.Instance.ResourceChange(resourceToGain, resourceGain * ResourceMultiplier);
        }
    }

    public void AssignEmployee(VillagerInfo VI)
    {
        // Remove From Current Job If Exists Before Employing At This Job
        if (VI.job != null)
        {
            VI.job.RemoveEmployee(VI);
        }

        Employees.Add(VI);
        VI.job = this;
    }

    public void RemoveEmployee(VillagerInfo VI)
    {
        // Fire Employee From Job
        Employees.Remove(VI);
        VI.job = null;
    }
}
