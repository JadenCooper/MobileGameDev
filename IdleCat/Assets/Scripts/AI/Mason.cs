using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mason : Job
{
    public override Vector2 GetLocation()
    {
        return Location;
    }

    public override void Initialize()
    {
        throw new System.NotImplementedException();
    }

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

    public override void Work()
    {
        throw new System.NotImplementedException();
    }
}
