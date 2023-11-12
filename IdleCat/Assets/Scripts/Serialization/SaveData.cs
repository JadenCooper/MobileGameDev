using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData 
{
    private static SaveData _current;

    public static SaveData current
    {
        get
        {
            if (_current == null)
            {
                _current = new SaveData();
            }
            return _current;
        }
        set
        {
            if (value != null)
            {
                _current = value;
            }
        }
    }

    public VillagerManagerSaveDate VMSD;
    public Vector3 CurrentTime;
    public DaySaveData DSD; 
    public BuildingManagerData BMD;
    public PlayerSaveData PSD;
    public GameResources currentResources;
    public GameResources dailyGains;
    public GameResources dailyLosses;
}
