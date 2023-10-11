using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class VillagerPetitionAI : Intractable
{
    public VillagerInfo VI;
    private bool moving = true;
    [SerializeField]
    private Navigation navigation;
    public PetitionSlot petitionSlot;
    public UnityEvent<Vector2> OnMovementInput;
    public Vector2 Goal;
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
            string fullName = VI.FirstName + " " + VI.LastName;
            UIManager.Instance.ModelWindow.ShowAsPrompt(fullName, VI.Species.Sprite, "Test", null);
        }
    }

    public void ReachedLocation()
    {
        moving = false;
        OnMovementInput?.Invoke(Vector2.zero);
    }
}
