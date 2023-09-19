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

    public static Happiness CalculateHappinessState(float happiness)
    {
        if (happiness >= 90)
        {
            return Happiness.Ecstatic;
        }
        else if (happiness >= 60)
        {
            return Happiness.Happy;
        }
        else if(happiness >= 40)
        {
            return Happiness.Netural;
        }
        else if (happiness >= 11)
        {
            return Happiness.Sad;
        }
        else
        {
            return Happiness.Miserable;
        }

    }
    public static int TimeIndexIncrement = 6;
}

public enum Resource
{
    Wood,
    Stone,
    Gold,
    Food,
    Happiness,
    Villagers,
    None
}

public enum Happiness
{
    Ecstatic,
    Happy,
    Netural,
    Sad,
    Miserable
}
