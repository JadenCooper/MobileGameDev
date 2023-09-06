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
        currentUser.happiness -= happinessLoss;
        currentUser.happiness = Mathf.Clamp(currentUser.happiness, 0, 100);


    }

    public override void InteractAction()
    {
        throw new System.NotImplementedException();
    }
}
