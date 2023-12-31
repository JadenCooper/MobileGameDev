using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Intractable : MonoBehaviour
{
    public float InteractionDistance = 2.3f;
    public void Interact(Transform agent)
    {
        // If Player Is Within Interaction Distance Then Interaction Can Take Place
        if ((Vector2.Distance(agent.position, gameObject.transform.position) < InteractionDistance))
        {
            InteractAction();
        }
    }

    public abstract void InteractAction();
}
