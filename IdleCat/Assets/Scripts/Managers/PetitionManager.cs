using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetitionManager : MonoBehaviour
{
    public List<PetitionSlot> PetitionSlots = new List<PetitionSlot>();

    public void SetPetitionSlot(VillagerPetitionAI VPAI)
    {
        for (int i = 0; i < PetitionSlots.Count; i++)
        {
            if (!PetitionSlots[i].Occupied)
            {
                VPAI.AssignPetitionSlot(PetitionSlots[i]);
                PetitionSlots[i].Occupied = true;
                return;
            }
        }
    }

    public void ShuffleSlots(int index)
    {
        // Goes In At The Slot Before The Removed Slot
        for (int i = (index + 1); i < PetitionSlots.Count; i++)
        {
            if (PetitionSlots[i].Occupied)
            {
                // Setting To Previous Slot
                PetitionSlots[i].VPAI.AssignPetitionSlot(PetitionSlots[i - 1]);
                PetitionSlots[i - 1].Occupied = true;

                // Clearing Current Slot
                PetitionSlots[i].VPAI = null;
                PetitionSlots[i].Occupied = false;

            }
            else
            {
                return;
            }
        }
    }

    public void RemoveFromSlots(VillagerPetitionAI removedVillager)
    {
        for (int i = 0; i < PetitionSlots.Count; i++)
        {
            if (PetitionSlots[i].VPAI == removedVillager)
            {
                PetitionSlots[i].VPAI = null;
                PetitionSlots[i].Occupied = false;
                ShuffleSlots(i);
                return;
            }
        }
    }

    [ContextMenu("Test")]
    public void TestRemove()
    {
        PetitionSlots[0].VPAI = null;
        PetitionSlots[0].Occupied = false;
        ShuffleSlots(0);
    }

    public void RemoveAll()
    {
        foreach (PetitionSlot slot in PetitionSlots)
        {
            if (slot.Occupied == true)
            {
                // If Current Villager Return To Normal Villager Behaviors Otherwise Leave
                if (slot.VPAI.VillageInhabitant)
                {
                    slot.VPAI.JoinVillage();
                }
                else
                {
                    slot.VPAI.LeaveVillage();
                }
                slot.VPAI = null;
                slot.Occupied = false;
            }
        }
    }
}
