using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TMP_Text clockText;

    public void UpdateClock(float hour)
    {
        clockText.text = hour.ToString() + ":00";
    }
}
