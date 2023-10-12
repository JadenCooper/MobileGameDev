using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Destroy"))
        {
            Destroy(collision.gameObject.transform.parent.gameObject);
        }
        else if(collision.CompareTag("Save"))
        {
            // For Villagers Leaving The Map But Will Return
            Debug.Log("Save");
        }
    }
}
