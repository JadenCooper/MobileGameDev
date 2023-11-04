using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Data
{
    public const int HOURSINDAY = 24;
    public const int STARTTIME = 0;
    public const int ENDTIME = 24;

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
    public static float Wrap(float index, float count)
    {
        count--;
        if (index > count)
        {
            // Outside Array Above
            index = 0;
        }
        else if(index < 0)
        {
            // Outside Array Below
            index = count;
        }
        return index;
    }
}

[System.Serializable]
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

[System.Serializable]
public enum Happiness
{
    Ecstatic,
    Happy,
    Netural,
    Sad,
    Miserable
}

[System.Serializable]
public enum BuildingType
{
    Job,
    House,
    Recreation,
}
