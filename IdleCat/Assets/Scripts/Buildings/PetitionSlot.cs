using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetitionSlot : MonoBehaviour
{
    public Vector2 Location; // X is X / Y Is Level
    public VillagerPetitionAI VPAI;
    public bool Occupied = false;
    public int slotNumber;
    private void Start()
    {
        Location.x = gameObject.transform.position.x;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        VillagerPetitionAI VPAI = collision.GetComponent<VillagerPetitionAI>();
        if (VPAI != null)
        {
            if (VPAI.petitionSlot == this)
            {
                VPAI.ReachedLocation();
                this.VPAI = VPAI;
            }
        }
    }
}
