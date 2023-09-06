using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recreation : Building
{
    public float happinessGain;

    public override void BuildingAction(VillagerInfo currentUser)
    {
        currentUser.happiness += happinessGain;
        currentUser.happiness = Mathf.Clamp(currentUser.happiness, 0, 100);
    }

    public override void InteractAction()
    {
        throw new System.NotImplementedException();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        VillagerController vc = collision.gameObject.GetComponent<VillagerController>();
        if (vc != null)
        {
            if (vc.villagerInfo.currentState == VillagerState.Recreation && vc.villagerInfo.recreationGoal == this)
            {
                vc.ReachedLocation();
            }
        }
    }
}
