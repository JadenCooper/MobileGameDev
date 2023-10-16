using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class VillagerPetitionAI : Intractable
{
    public VillagerInfo VI;
    [SerializeField]
    private bool moving = true;
    [SerializeField]
    private Navigation navigation;
    public PetitionSlot petitionSlot;
    public UnityEvent<Vector2> OnMovementInput;
    public Vector2 Goal;

    public bool VillageInhabitant = false;
    public int PostponeTime = 0;
    public void Initialize(VillagerInfo villagerInfo)
    {
        VI = villagerInfo;
        VI.gameObject = gameObject;
        GetComponent<SpriteRenderer>().sprite = VI.Species.Sprite;
    }

    void Update()
    {
        if (moving)
        {
            OnMovementInput?.Invoke(DetermineMovement());
        }
    }

    public Vector2 DetermineMovement()
    {
        // Gets Direction For Movement Based On Current Goal
         Goal = new Vector2(VI.CurrentGoal.x, transform.position.y);
        return (Goal - (Vector2)transform.position).normalized;
    }

    public void AssignPetitionSlot(PetitionSlot petitionSlot)
    {
        this.petitionSlot = petitionSlot;
        moving = true;
        navigation.GetLocationGoal(VI, petitionSlot.Location);
    }

    public override void InteractAction()
    {
        if (moving == false)
        {
            // At Slot So Can Accept Or Decline
            if (!VillageInhabitant)
            {
                // Asking To Join Village
                string fullName = VI.FirstName + " " + VI.LastName;
                UIManager.Instance.ModelWindow.ShowAsPrompt(fullName, VI.Species.Sprite, "Test", "Yay", "Nay", "Postpone", JoinVillage, LeaveVillage, PostponeDecision);
            }
        }
    }

    public void JoinVillage()
    {
        VillagerManager.Instance.VillagerJoinsVillage(this);
        if (petitionSlot != null)
        {
            VillagerManager.Instance.petitionManager.RemoveFromSlots(this);
        }
    }

    public void LeaveVillage()
    {
        navigation.GetLocationGoal(VI, VillagerManager.Instance.VillagerLeavesVillage(this));
        gameObject.tag = "Destroy";
        moving = true;
        if (petitionSlot != null)
        {

            VillagerManager.Instance.petitionManager.RemoveFromSlots(this);
        }
    }

    public void PostponeDecision()
    {
        moving = true;
        // Postpone Petition Decision For 1-3 Days
        PostponeTime = Random.Range(1, 4);
        VillagerManager.Instance.petitionManager.RemoveFromSlots(this);
        if (VillageInhabitant)
        {
            GetComponent<VillagerController>().enabled = true;
            this.enabled = false;
        }
        else
        {
            // Leave Screen
            navigation.GetLocationGoal(VI, VillagerManager.Instance.VillagerLeavesVillage(this));
            gameObject.tag = "Save";
        }
    }

    public void ReachedLocation()
    {
        moving = false;
        OnMovementInput?.Invoke(Vector2.zero);
    }
}
