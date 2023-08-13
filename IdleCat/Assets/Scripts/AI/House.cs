using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField]
    private Vector2 Location; // X is X / Y Is Level

    public List<VillagerController> Inhabitants = new List<VillagerController>();
    public  Vector2 GetLocation()
    {
        return Location;
    }
}
