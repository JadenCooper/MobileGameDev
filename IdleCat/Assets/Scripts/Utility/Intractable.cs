using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Intractable : MonoBehaviour
{
    public float InteractionDistance = 2.3f;
    public void Interact(Transform agent)
    {
        Debug.Log(Vector2.Distance(agent.position, gameObject.transform.position));
        if ((Vector2.Distance(agent.position, gameObject.transform.position) < InteractionDistance))
        {
            InteractAction();
        }
    }

    public abstract void InteractAction();
}
