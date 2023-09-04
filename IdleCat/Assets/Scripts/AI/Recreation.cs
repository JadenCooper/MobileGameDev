using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Recreation : MonoBehaviour
{
    public string RecreationName;
    public Vector2 Location; // X is X / Y Is Level

    public abstract void Initialize(); // Activated When Built
    public abstract Vector2 GetLocation();

    public abstract void OnTriggerEnter2D(Collider2D collision);
}
