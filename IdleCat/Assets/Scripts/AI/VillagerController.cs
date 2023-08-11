using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VillagerController : Intractable
{
    public VillagerInfo villagerInfo;
    public float CurrentGoal;
    private Navigation navigation;
    private void Start()
    {
        navigation = GetComponent<Navigation>();
    }
    public override void InteractAction()
    {
        throw new System.NotImplementedException();
    }
}
