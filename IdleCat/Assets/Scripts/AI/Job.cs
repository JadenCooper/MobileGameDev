using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Job : MonoBehaviour
{
    [SerializeField]
    private string JobName;
    [SerializeField]
    private Vector2 Location; // X is X / Y Is Level
    [SerializeField]
    private Vector2 WorkTimes; // Start Time / End Time

    public List<VillagerController> Employees = new List<VillagerController>();
    public abstract void Initialize(); // Activated When Built

    public abstract void Work();

    public abstract Vector2 GetLocation();

}
