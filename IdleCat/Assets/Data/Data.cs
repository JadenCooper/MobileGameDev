using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public static readonly Dictionary<int, float> FloorHeights = new Dictionary<int, float>()
    {
        {0, 0.3f },
        {1, 7.3f }
    };

    public static int TimeIndexIncrement = 6;
}
