using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job : MonoBehaviour
{
    public string JobName;
    public Vector2 Location; // X is X / Y Is Level
    public Vector2 WorkTimes; // Start Time / End Time

    public List<VillagerController> Employees = new List<VillagerController>();
    public abstract void Initialize(); // Activated When Built

    public abstract void Work();

    public abstract Vector2 GetLocation();

    public abstract void OnTriggerEnter2D(Collider2D collision);

}
