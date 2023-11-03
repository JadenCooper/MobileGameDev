using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        VillagerManager.Instance.NonVillagers.Remove(collision.GetComponent<VillagerPetitionAI>());

        if (collision.CompareTag("Destroy"))
        {
            Destroy(collision.gameObject.transform.parent.gameObject);
        }
        else if(collision.CompareTag("Save"))
        {
            // For Villagers Leaving The Map But Will Return
            collision.gameObject.transform.parent.gameObject.SetActive(false);
            VillagerPetitionAI VPAI = collision.GetComponent<VillagerPetitionAI>();
            if (VPAI.VillageInhabitant)
            {
                VillagerManager.Instance.PostponedVillagerPetitions.Add(collision.GetComponent<VillagerPetitionAI>());
            }
            else
            {
                VillagerManager.Instance.PostponedNonVillagerPetitions.Add(collision.GetComponent<VillagerPetitionAI>());
            }
        }
    }
}
