using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{

    public List<VillagerInfo> Inhabitants = new List<VillagerInfo>();
    public float restValue;
    public override void BuildingAction(VillagerInfo currentUser)
    {
        currentUser.rest += restValue;
        currentUser.rest = Mathf.Clamp(currentUser.rest, 0, 100);
    }

    public override void InteractAction()
    {

    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        VillagerController vc = collision.gameObject.GetComponent<VillagerController>();
        if (vc != null)
        {
            if (vc.VI.currentState == VillagerState.Home && vc.VI.house == this)
            {
                vc.ReachedLocation();
            }
        }
    }

    public void AssignInhabitant(VillagerInfo VI)
    {
        Inhabitants.Add(VI);
        VI.house = this;
    }

    public void RemoveInhabitant(VillagerInfo VI)
    {
        Inhabitants.Remove(VI);
        VI.house = null;
    }
}
