using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public List<int> Levels = new List<int>(); // X = X, Y = Floor Level
    public float x;
    public void Transport()
    {
        Debug.Log("Transport");
    }
}
