using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{

    public List<VillagerController> Inhabitants = new List<VillagerController>();
    public float restValue;
    public override void BuildingAction(VillagerInfo currentUser)
    {
        currentUser.rest += restValue;
        currentUser.rest = Mathf.Clamp(currentUser.rest, 0, 100);
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
            if (vc.villagerInfo.currentState == VillagerState.Home && vc.villagerInfo.house == this)
            {
                vc.ReachedLocation();
            }
        }
    }
}
